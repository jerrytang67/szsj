using System.Security.Cryptography.X509Certificates;
using Abp.Dependency;
using Abp.Runtime.Session;
using Castle.Core.Logging;

namespace TtWork.ProjectName.Web.Host.Startup
{
    public class CertificateProvider : ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly IAbpSession _abpSession;

        public CertificateProvider(
            ILogger logger,
            IAbpSession abpSession)
        {
            _logger = logger;
            _abpSession = abpSession;
        }

        public X509Certificate2 GetCertificate()
        {
            // 这个只有在第一次请求的时候才会执行
            var tenant = _abpSession.TenantId;

            var certPath = @"C:\apiclient_cert.p12";
            var certPwd = "1486627732";
            _logger.WarnFormat($"租户:{tenant} 请求Http证书:{certPath}");
            return new X509Certificate2(certPath, certPwd, X509KeyStorageFlags.MachineKeySet);
        }
    }
}