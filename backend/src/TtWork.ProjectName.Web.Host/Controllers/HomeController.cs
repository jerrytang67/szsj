using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.AspNetCore.Mvc.Results.Wrapping;
using Abp.Auditing;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using Abp.Web.Models;
using TtWork.ProjectName.Controllers;

namespace TtWork.ProjectName.Web.Host.Controllers
{
    public class HomeController : AbpControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;

        public HomeController(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }

        // public IActionResult Index()
        // {
        //     return Redirect("/swagger");
        // }

        [DisableAuditing]
        [DontWrapResult]
        public string health()
        {
            return "ok";
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }
    }
}
