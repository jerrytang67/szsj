using System;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Castle.Core.Internal;

namespace TTWork.Abp.AuditManagement.Applications.Dtos
{
    public class AuditRequestDto : PagedResultRequestDto, IShouldNormalize
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public string Sorting { get; set; }

        public DateTime? From { get; set; } // javascript date within timezone
        public DateTime? To { get; set; } // javascript date within timezone

        public void Normalize()
        {
            if (Sorting.IsNullOrEmpty())
                Sorting = "Id desc";
        }
    }
}