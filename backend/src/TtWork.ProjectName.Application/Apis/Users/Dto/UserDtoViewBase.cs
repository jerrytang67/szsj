using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDtoViewBase : EntityDto<long>
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }
    }
}