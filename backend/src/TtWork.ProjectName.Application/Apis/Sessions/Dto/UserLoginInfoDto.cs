using Abp.Application.Services.Dto;

namespace TtWork.ProjectName.Apis.Sessions.Dto
{
    //Mapped to/from User in CustomDtoMapper
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string HeadImgUrl { get; set; }

        public int Jf { get; set; }

        public bool IsSubscribe { get; set; }

        public string PhoneNumber { get; set; }

        public string Town { get; set; }
    }
}