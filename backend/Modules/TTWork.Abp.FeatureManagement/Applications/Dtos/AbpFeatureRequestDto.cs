using System;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using TT.Extensions;

namespace TTWork.Abp.FeatureManagement.Applications.Dtos
{
    public class AbpFeatureRequestDto : PagedResultRequestDto, IShouldNormalize
    {
        public string Keyword { get; set; }
        public bool? IsActive { get; set; }
        public string Sorting { get; set; }

        public DateTime? From { get; set; } // javascript date within timezone
        public DateTime? To { get; set; } // javascript date within timezone

        public void Normalize()
        {
            if (Sorting.IsNullOrEmptyOrWhiteSpace())
                Sorting = "Id desc";
        }
    }
}