﻿using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using TtWork.ProjectName.EntityFrameworkCore.Seed.Host;
using TtWork.ProjectName.EntityFrameworkCore.Seed.Tenants;
using TtWork.ProjectName.Extensions;

namespace TtWork.ProjectName.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<AbpDbContext>(iocResolver, SeedHostDb);
        }

        public static void SeedHostDb(AbpDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

            // Host seed
            new InitialHostDbBuilder(context).Create();

            // Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();


            var allTenants = context.Tenants.ToList();
            foreach (var t in allTenants)
            {
                new TenantRoleAndUserBuilder(context, t.Id).Create();
            }
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}