using System;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace TTWork.Abp.Core.Applications.Dtos
{
    //custom PagedResultRequestDto
    public class AppResultRequestDto : PagedResultRequestDto, IShouldNormalize, ISortedResultRequest
    {
        public long? OrganizationUnitId { get; set; }
        public int? Status { get; set; }
        
        public long? UserId { get; set; }
        public int? Pid { get; set; }
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public string Sorting { get; set; }
        
        public DateTime? From { get; set; } // javascript date within timezone

        public DateTime? To { get; set; } // javascript date within timezone

        public virtual void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
                Sorting = "Id descending";
        }
    }
}