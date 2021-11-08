using Abp.Domain.Entities;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Activity.EntityFrameworkCore;
using TtWork.ProjectName.Entities;
using TtWork.ProjectName.Entities.Organizations;
using TtWork.ProjectName.Entities.Pay;
using TtWork.ProjectName.Entities.Place;
using TtWork.ProjectName.Entities.Shares;
using TTWork.Abp.AppManagement.Domain;
using TTWork.Abp.AppManagement.EntityFrameworkCore;
using TTWork.Abp.AuditManagement.Domain;
using TTWork.Abp.AuditManagement.EntityFrameworkCore;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Domains.Weixin;
using TTWork.Abp.Core.MultiTenancy;
using TTWork.Abp.FeatureManagement.Domain;
using TTWork.Abp.FeatureManagement.EntityFrameworkCore;
using TTWork.Abp.WorkFlowCore.EntityFrameworkCore;
using TTWork.Abp.WorkFlowCore.Models;
using TtWork.ProjectName.Entities.Cms;
using TtWork.ProjectName.Entities.Posters;

namespace TtWork.ProjectName.EntityFrameworkCore
{
    public class AbpDbContext : AbpZeroDbContext<Tenant, Role, User, AbpDbContext>,
        IAuditDbContext,
        IFeatureDbContext,
        IActivityDbContext,
        IWorkFlowCoreDbContext
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<WechatUserinfo> WechatUserinfo { get; set; }
        public DbSet<ProjectNameOrganizationUnit> ProjectNameOrganizationUnits { get; set; }

        public DbSet<OrganizationApply> OrganizationApply { get; set; }
        public DbSet<OrganizationEvent> OrganizationEvents { get; set; }

        public DbSet<TenPayNotify> TenPayNotify { get; set; }
        public DbSet<PayOrder> PayOrder { get; set; }
        public DbSet<RefundLog> RefundLog { get; set; }

        public DbSet<Place> Place { get; set; }

        public DbSet<Swiper> Swiper { get; set; }

        public DbSet<CmsContent> CmsContent { get; set; }
        public DbSet<CmsCategory> CmsCategory { get; set; }
        public DbSet<CmsContentEvent> CmsContentEvents { get; set; }

        public DbSet<Comment> Comment { get; set; }
        public DbSet<QrDetail> QrDetails { get; set; }

        public DbSet<Poster> Poster { get; set; }
        public DbSet<App> Apps { get; set; }
        
        #region ActivityDbContext

        public DbSet<PointActivity> PointActivities { get; set; }
        public DbSet<PointActivityUserLog> PointActivityUserLogs { get; set; }
        public DbSet<UserPointLog> UserPointLogs { get; set; }
        public DbSet<LuckDraw> LuckDraws { get; set; }
        public DbSet<LuckDrawPrize> LuckDrawPrizes { get; set; }
        public DbSet<UserPrize> UserPrizes { get; set; }
        public DbSet<UserLuckTime> UserLuckTimes { get; set; }

        #endregion

        #region AuditManagement Module

        public DbSet<AuditFlow> AuditFlows { get; set; }
        public DbSet<AuditNode> AuditNodes { get; set; }
        public DbSet<AuditUserLog> AuditUserLogs { get; set; }

        #endregion

        #region AbpFeatureManagement Module

        public DbSet<AbpFeature> AbpFeatures { get; set; }

        #endregion

        #region IWorkFlowCoreDbContext

        public DbSet<PersistedEvent> PersistedEvents { get; set; }
        public DbSet<PersistedExecutionError> PersistedExecutionErrors { get; set; }
        public DbSet<PersistedExecutionPointer> PersistedExecutionPointers { get; set; }
        public DbSet<PersistedExtensionAttribute> PersistedExtensionAttributes { get; set; }
        public DbSet<PersistedSubscription> PersistedSubscriptions { get; set; }
        public DbSet<PersistedWorkflow> PersistedWorkflows { get; set; }
        public DbSet<PersistedWorkflowDefinition> PersistedWorkflowDefinitions { get; set; }

        #endregion
        
        public AbpDbContext(DbContextOptions<AbpDbContext> options)
            : base(options)
        {
            base.SuppressAutoSetTenantId = false;
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDb();

            builder.ConfigureAppManagement();

            builder.ConfigureAuditManagement();

            builder.ConfigureFeatureManagement();

            builder.ConfigurActivity();

            builder.ConfigureWorkFlowCore();
        }


        protected override void CheckAndSetMayHaveTenantIdProperty(object entityAsObj)
        {
            if (SuppressAutoSetTenantId)
            {
                return;
            }

            //Only works for single tenant applications
            // if (MultiTenancyConfig?.IsEnabled ?? false)
            // {
            //     return;
            // }

            //Only set IMayHaveTenant entities
            if (!(entityAsObj is IMayHaveTenant))
            {
                return;
            }

            var entity = entityAsObj.As<IMayHaveTenant>();

            //Don't set if it's already set
            if (entity.TenantId != null)
            {
                return;
            }

            entity.TenantId = GetCurrentTenantIdOrNull();
        }

        protected override void CheckAndSetMustHaveTenantIdProperty(object entityAsObj)
        {
            base.CheckAndSetMustHaveTenantIdProperty(entityAsObj);
        }
    }
}