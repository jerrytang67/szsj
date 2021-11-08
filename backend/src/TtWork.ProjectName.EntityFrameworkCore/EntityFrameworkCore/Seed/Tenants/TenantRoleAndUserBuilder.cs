using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using TtWork.ProjectName.Authorization;
using TtWork.ProjectName.Authorization.Roles;
using TtWork.ProjectName.Authorization.Users;
using TTWork.Abp.Core.Authorization;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TTWork.Abp.Core.Definitions;
using TtWork.ProjectName.Entities.Cms;

namespace TtWork.ProjectName.EntityFrameworkCore.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly AbpDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(AbpDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
            // Create_CmsCategory();
        }

        private void Create_CmsCategory()
        {
            bool needsave = false;
            if (!_context.CmsCategory.Any(x => x.Name == "寻访动态" && x.TenantId == _tenantId))
            {
                _context.CmsCategory.Add(new CmsCategory()
                {
                    Name = "寻访动态",
                    TenantId = _tenantId
                });
                needsave = true;
            }

            if (!_context.CmsCategory.Any(x => x.Name == "首届回顾" && x.TenantId == _tenantId))
            {
                _context.CmsCategory.Add(new CmsCategory()
                {
                    Name = "首届回顾",
                    TenantId = _tenantId
                });
                needsave = true;
            }

            if (needsave)
                _context.SaveChanges();
        }


        private void CreateRolesAndUsers()
        {
            #region admin角色初始化

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) {IsStatic = true}).Entity;
                _context.SaveChanges();
            }

            // Grant all permissions to admin role
            // 数据库中已有权限
            var grantedPermissions = _context.Permissions.IgnoreQueryFilters()
                .OfType<RolePermissionSetting>()
                .Where(p => p.TenantId == _tenantId
                    // && p.RoleId == adminRole.Id
                )
                .Select(p => p.Name)
                .ToList();

            // 和数据库的差集
            var permissions = PermissionFinder
                .GetAllPermissions(new AbpAuthorizationProvider())
                .Where(p => p.MultiTenancySides.HasFlag(MultiTenancySides.Tenant) &&
                            !grantedPermissions.Contains(p.Name))
                .ToList();

            if (permissions.Any())
            {
                _context.Permissions.AddRange(
                    permissions.Select(permission => new RolePermissionSetting
                    {
                        TenantId = _tenantId,
                        Name = permission.Name,
                        IsGranted = true,
                        RoleId = adminRole.Id
                    })
                );
                _context.SaveChanges();
            }


            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@wujiangapp.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "321ewq");
                adminUser.IsEmailConfirmed = true;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                // Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();
            }

            #endregion

            #region 机构角色初始化

            var organizeRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Organize);
            if (organizeRole == null)
            {
                organizeRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Organize, "商户角色") {IsStatic = true}).Entity;
                _context.SaveChanges();
                foreach (var p in new[]
                {
                    AppPermissions.Pages.Default,
                    AppPermissions.Pages.Organization.Default
                })

                    _context.Permissions.Add(
                        new RolePermissionSetting()
                        {
                            TenantId = _tenantId,
                            Name = p,
                            IsGranted = true,
                            RoleId = organizeRole.Id
                        });
                _context.SaveChanges();
            }

            #endregion

            #region User角色初始化

            var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.User);
            if (userRole == null)
            {
                userRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.User, "用户角色") {IsStatic = true, IsDefault = true}).Entity;
                _context.SaveChanges();

                foreach (var p in new[]
                {
                    AppPermissions.Pages.Default
                })
                {
                    _context.Permissions.Add(
                        new RolePermissionSetting()
                        {
                            TenantId = _tenantId,
                            Name = p,
                            IsGranted = true,
                            RoleId = userRole.Id
                        });
                }

                _context.SaveChanges();
            }
            else
            {
                if (!userRole.IsDefault)
                {
                    userRole.IsDefault = true;
                    _context.SaveChanges();
                }
            }

            #endregion
        }
    }
}