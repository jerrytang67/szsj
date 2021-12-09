using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Abp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Castle.Facilities.Logging;
using Abp.AspNetCore;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Json;
using Abp.Reflection.Extensions;
using AwsomeApi.DingTalk;
using AwsomeApi.WeixinWork;
using Castle.Services.Logging.SerilogIntegration;
using Microsoft.AspNetCore.Http;
using TtWork.ProjectName.Authentication.JwtBearer;
using TtWork.ProjectName.Configuration;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using TT.Extensions.Redis;
using TT.HttpClient.Weixin;
using TT.HttpClient.Weixin.Signature;
using TTWork.Abp.Activity.Applications;
using TTWork.Abp.Core.Identity;
using TTWork.Abp.Oss.UpYun;
using TTWork.Abp.WorkFlowCore.Services;
using TtWork.ProjectName.Apis.Users;
using TTWork.WeiXinMiddleware;
using TTWork.WeiXinMiddleware.Extensions;
using WorkflowCore.Interface;

namespace TtWork.ProjectName.Web.Host.Startup
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private const string DefaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISignatureGenerator, SignatureGenerator>();

            services.Configure<RedisOptions>(_appConfiguration.GetSection("Redis"));

            // services.Configure<LaborUnionSettings>(_appConfiguration.GetSection(LaborUnionSettings.Position));

            services.AddSingleton<IRedisClient, RedisClient>();


            SetupHttpClient(services);

            services.AddAbpWorkflow(a => { a.UseRedisLocking(_appConfiguration["Redis:ConnectionString"]); });

            // MVC
            services.AddControllersWithViews(opt => { opt.InputFormatters.Add(new XmlSerializerInputFormatter(opt)); })
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.ContractResolver = new AbpMvcContractResolver(IocManager.Instance)
                    {
                        NamingStrategy = new CamelCaseNamingStrategy(),
                    };
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            IdentityRegistrar.Register(services);

            services.ConfigureJwt(_appConfiguration);

            services.AddSignalR();

            // Configure CORS 
            services.AddCors(
                options => options.AddPolicy(
                    DefaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            if (_env.IsDevelopment())
            {
                // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "TtWork ProjectName API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);

                    // Define the BearerAuth scheme that's in use
                    options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme()
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.OpenIdConnect,
                    });

                    var xmlPath = Path.Combine(AppContext.BaseDirectory,
                        $"{typeof(IUserAppService).GetAssembly().GetName().Name}.xml");
                    options.IncludeXmlComments(xmlPath);
                });
                services.AddSwaggerGenNewtonsoftSupport();
            }


            var self = services.AddAbp<ProjectWebHostModule>(ConfigSerilog());
            return self;
        }


        private Action<AbpBootstrapperOptions> ConfigSerilog()
        {
            var config = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(_appConfiguration["ElasticSearch:Uri"]))
                    {
                        IndexFormat = $"{_appConfiguration["ApplicationName"]}-logs-{_env.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 1,
                        NumberOfReplicas = 1
                    }
                )
                .Enrich.WithProperty("Environment", _env.EnvironmentName)
                .ReadFrom.Configuration(_appConfiguration)
                .CreateLogger();


            return options => options.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.LogUsing(new SerilogFactory(config)));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<RealIpMiddleware>();

            // app.UseAllElasticApm(_appConfiguration);

            app.UseAbp(options => { options.UseAbpRequestLocalization = true; }); // Initializes ABP framework.

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(DefaultCorsPolicyName); // Enable CORS!

            app.UseAuthentication();

            app.UseJwtTokenMiddleware();

            app.UseAbpRequestLocalization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
            if (_env.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(_appConfiguration["App:ServerRootAddress"].EnsureEndsWith('/') + "swagger/v1/swagger.json", "Abp API V1");
                    options.IndexStream = () => Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("TtWork.ProjectName.Web.Host.wwwroot.swagger.ui.index.html");
                }); // URL: /swagger
            }

            //微信消息中间件
            app.UseWeiXin(options: new WeiXinOptions() { Path = "/wx", MutilTenant = true });

            // 注册Workflow
            var host = app.ApplicationServices.GetService<IWorkflowHost>();

            host!.Start();
        }


        private void SetupHttpClient(IServiceCollection services)
        {
            // HTTPClient 
            services.AddHttpClient<IUpyunApi, UpyunApi>(cfg => { cfg.BaseAddress = new Uri("https://v0.api.upyun.com/"); })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { Proxy = null, UseProxy = false });

            // HTTPClient
            services.AddHttpClient<IWeixinApi, WeixinApi>(cfg => { cfg.BaseAddress = new Uri("https://api.weixin.qq.com/"); })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { Proxy = null, UseProxy = false });

            services.AddHttpClient<IWeixinWorkApi, WeixinWorkApi>()
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { Proxy = null, UseProxy = false });

            services.AddHttpClient<IPayApi, PayApi>(cfg => { cfg.BaseAddress = new Uri("https://api.mch.weixin.qq.com/"); })
                .ConfigurePrimaryHttpMessageHandler(serviceProvider => new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    Proxy = null,
                    UseProxy = false,
                    ClientCertificates =
                    {
                        serviceProvider.GetService<CertificateProvider>()!.GetCertificate()
                    }
                });

            services.AddHttpClient<DingTalkOApiClient>()
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { Proxy = null, UseProxy = false });

            services.AddHttpClient<LinkunstClient>()
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { Proxy = null, UseProxy = false });

            services.AddHttpClient("qlogoClient", x => { x.BaseAddress = new Uri("https://wx.qlogo.cn/"); })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { Proxy = null, UseProxy = false });

            services.AddHttpClient("imgClient", x => { x.BaseAddress = new Uri("https://img.wujiangapp.com/"); })
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler { Proxy = null, UseProxy = false });
        }
    }


    public class RealIpMiddleware
    {
        private readonly RequestDelegate _next;

        public RealIpMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            if (headers.ContainsKey("X-Forwarded-For"))
            {
                var ip = headers["X-Forwarded-For"].ToString().Split(',', StringSplitOptions.RemoveEmptyEntries)[0];
                context.Connection.RemoteIpAddress = IPAddress.Parse(ip);
            }

            return _next(context);
        }
    }
}