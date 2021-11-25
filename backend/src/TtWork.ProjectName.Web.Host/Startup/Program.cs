using System;
using Castle.Core.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TtWork.ProjectName.Web.Host.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger>();

                logger.InfoFormat("Application Start!");
            }

            host.Run();
        }

        public static IWebHostBuilder BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                            .AddEnvironmentVariables();

                        var env = context.HostingEnvironment;
                        context.Configuration = config.Build();
                        // var consulUrl = context.Configuration["Consul_Url"] ?? "http://127.0.0.1:8500";
                        // Console.WriteLine($"Consul Url:{consulUrl}");
                        Console.WriteLine($"ApplicationName:{env.ApplicationName}");
                        Console.WriteLine($"EnvironmentName:{env.EnvironmentName}");
                        //     config.AddConsul(
                        //             $"wjzgh/appsettings.{env.EnvironmentName}.json", options =>
                        //             {
                        //                 options.ConsulConfigurationOptions =
                        //                     cco =>
                        //                     {
                        //                         cco.Address = new Uri(consulUrl);
                        //                         cco.Token = "sadfawefaiSDYFSdf987y21";
                        //                     };
                        //                 options.Optional = true;
                        //                 options.ReloadOnChange = true;
                        //                 options.OnLoadException = exceptionContext =>
                        //                 {
                        //                     Console.WriteLine($"Console OnLoadException:{exceptionContext.Exception.Message}");
                        //                     exceptionContext.Ignore = true;
                        //                 };
                        //                 options.OnWatchException = exceptionContext =>
                        //                 {
                        //                     Console.WriteLine($"Console OnWatchException:{exceptionContext.Exception.Message}, wait for 30 sec to retry.");
                        //                     return TimeSpan.FromSeconds(30);
                        //                 };
                        //             })
                        //         .AddEnvironmentVariables();
                    })
                    .UseStartup<Startup>()
                ;
        }
    }
}