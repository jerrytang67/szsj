using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using TTWork.Abp.Core.Applications.Dtos;

namespace TtWork.ProjectName.Apis.Dtos
{
    public class CommentRequestDto : AppResultRequestDto
    {
        public int? ActivityId { get; set; }
    }

    public class CmsRequestDto : AppResultRequestDto
    {
        public int? CategoryId { get; set; }
    }

    public class Filter
    {
        public string key { get; set; }
        public List<String> values { get; set; }
    }
}