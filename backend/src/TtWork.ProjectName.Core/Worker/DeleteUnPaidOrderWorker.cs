using System;
using System.Data.SqlClient;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Microsoft.EntityFrameworkCore;

namespace TtWork.ProjectName.Worker
{
    public class DeleteUnPaidOrderWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public DeleteUnPaidOrderWorker(AbpTimer timer)
            : base(timer)
        {
            Timer.Period = 15 * 60 * 1000;
            //timer.RunOnStart = true; 
        }

        [UnitOfWork]
        protected override void DoWork()
        {
            var date = new SqlParameter("date", DateTime.Now.AddMinutes(-30));
            var date2 = new SqlParameter("date2", DateTime.Now.AddHours(-6));

            var command = "Delete FROM dbo.PayOrders WHERE IsSuccessPay = 0 and CreationTime < @date";

            var res = CurrentUnitOfWork.GetDbContext<AbpDbContext>().Database
                .ExecuteSqlRaw(command, date);
            if (res > 0)
                Logger.Warn($"[Worker] DeleteUnPaidOrderWorker 删除{res}条,每15分钟执行一次");


            // 清除报名列表中超时的待付款
            var command2 = "Delete FROM dbo.activity_apply WHERE status = 5 and CreationTime < @date2";
            var res2 = CurrentUnitOfWork.GetDbContext<AbpDbContext>().Database
                .ExecuteSqlRaw(command2, date2);
            if (res2 > 0)
                Logger.Warn($"[Worker] DeleteUnPaidApplyWorker 删除{res2}条,每15分钟执行一次");
        }
    }
}