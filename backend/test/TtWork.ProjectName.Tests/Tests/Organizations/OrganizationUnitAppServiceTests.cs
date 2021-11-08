using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using TtWork.ProjectName.Apis.Organizations;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Definitions;
using TtWork.ProjectName.Apis.Organizations.Dtos;
using TtWork.ProjectName.Apis.Users;
using TtWork.ProjectName.Apis.Users.Dto;
using Xunit;

namespace TtWork.ProjectName.Tests.Tests.Organizations
{
    public class OrganizationUnitAppServiceTests : AbpTestBase
    {
        public readonly IOrganizationUnitAppService _appService;
        public readonly IOrganizationApplyAppService _applyService;
        public readonly IUserAppService _userAppService;

        public OrganizationUnitAppServiceTests()
        {
            _appService = Resolve<IOrganizationUnitAppService>();
            _applyService = Resolve<IOrganizationApplyAppService>();
            _userAppService = Resolve<IUserAppService>();
        }

        [Fact]
        public async Task CreateDefault()
        {
            AbpSession.TenantId = 1;
            AbpSession.UserId = 2;
            var thisTestNewUser = await _userAppService.CreateAsync(new CreateUserDto()
            {
                EmailAddress = "tt@tt.com",
                IsActive = true,
                Name = "JiaWei",
                Password = "123qwe",
                Surname = "Tang",
                UserName = "TT",
                RoleNames = new[] {"User"}
            });

            var getUser = await _userAppService.GetAsync(new EntityDto<long>(thisTestNewUser.Id));

            getUser.UserName.ShouldBe("TT");
            getUser.Name.ShouldBe("JiaWei");

            //登录为新建的用户
            AbpSession.UserId = thisTestNewUser.Id;

            await UsingDbContextAsync(async context =>
            {
                var users = await context.Users.Include(x => x.Roles).ToListAsync();
                var roles = await context.Roles.ToListAsync();
                var permission = await context.Permissions.Where(x => x.Name == AppPermissions.Pages.Administration.Default).ToListAsync();
            });


            var applyOu = await _applyService.CreateAsync(new CreatorOrUpdateOrganizationApplyDto
            {
                DisplayName = "TTOU",
                Detail = new OrganizationUnitDetailCreateDto()
                {
                    Desc = "desc",
                    LogoImgUrl = "d",
                    Address = "address",
                    HeadmanRealName = "1",
                    HeadmanPhone = "1",
                },
                Type = 1,
            });


            AbpSession.UserId = 2;

            var ou1 = await _applyService.GetAllAsync(new AppResultRequestDto());

            // AbpSession.UserId = 2;
            // await _appService.ApproveApply(new ApplyInputDto {Id = applyOu.Id});

            ou1 = await _applyService.GetAllAsync(new AppResultRequestDto() {Status = 1});

            ou1.Items.Count.ShouldBe(1);
            ou1.Items[0].CreatorUserId.ShouldBe(thisTestNewUser.Id);
            ou1.Items[0].DisplayName.ShouldBe("TTOU");
            //到这里,DB中有一个机构,机构所有者thisTestNewUserr的ID.权限为User
        }
    }
}