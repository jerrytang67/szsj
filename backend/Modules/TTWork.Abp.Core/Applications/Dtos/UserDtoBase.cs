using Abp.Application.Services.Dto;

namespace TTWork.Abp.Core.Applications.Dtos
{
    public class UserDtoBase : EntityDto<long>
    {
        public string UserName { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public string Surname { get; set; }
        
        public string Town { get; set; }
    }
}