using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using TTWork.Abp.Activity.Domains;
using TTWork.Abp.Core.Applications.Dtos;

namespace TTWork.Abp.Activity.Applications
{
    /// <summary>
    /// <see cref="UserPoint"/>
    /// </summary>
    [AutoMap(typeof(UserPointLog))]
    public class UserPointLogDto : EntityDto<long>, IMustHaveTenant
    {
        public long UserId { get; set; }

        public UserDtoBase User { get; set; }

        public EnumClass.UserPointEventType EventType { get; set; }

        public string EventId { get; set; }

        public int AfterPoints { get; set; }

        public int BeforePoints { get; set; }

        public int ChangePoints { get; set; }

        public string Desc { get; set; }

        public int TenantId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}