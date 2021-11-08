using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FluentValidation;
using Newtonsoft.Json.Linq;

namespace TTWork.Abp.Activity.Domains
{
    public class VotePlan : FullAuditedAggregateRoot<long>, IMustHaveTenant
    {
        [StringLength(256)] public string TitleImageUrl { get; set; }
        [StringLength(256)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public EnumClass.VotePlanState State { get; set; } = EnumClass.VotePlanState.开启;

        public int Type { get; set; }

        public string HtmlContext { get; set; }

        public int ViewCount { get; private set; }

        public bool IsUserUpload { get; set; } = false;

        public DateTime? UploadStartTime { get; set; }

        public DateTime? UploadEndTime { get; set; }

        public DateTime VoteStartTime { get; set; }

        public DateTime VoteEndTime { get; set; }

        public JObject Settings { get; set; } = new();

        public int TenantId { get; set; }
        
        public virtual List<VoteItem> VoteItems { get; set; }

        public void Visit()
        {
            ViewCount++;
        }
    }

    public class VotePlanDto : EntityDto<long>
    {
        public string TitleImageUrl { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public EnumClass.VotePlanState State { get; set; }

        public int Type { get; set; }

        public string HtmlContext { get; set; }

        public int ViewCount { get; set; }

        public bool IsUserUpload { get; set; }

        public DateTime? UploadStartTime { get; set; }

        public DateTime? UploadEndTime { get; set; }

        public DateTime VoteStartTime { get; set; }

        public DateTime VoteEndTime { get; set; }

        public JObject Settings { get; set; }
    }

    public class VotePlanCreateOrUpdateDto : EntityDto<long>
    {
        [StringLength(256)] public string TitleImageUrl { get; set; }

        [StringLength(256)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public EnumClass.VotePlanState State { get; set; } = EnumClass.VotePlanState.开启;

        public int Type { get; set; }

        public string HtmlContext { get; set; }

        public bool IsUserUpload { get; set; } = false;

        public DateTime? UploadStartTime { get; set; }

        public DateTime? UploadEndTime { get; set; }

        public DateTime? VoteStartTime { get; set; }

        public DateTime? VoteEndTime { get; set; }

        public JObject Settings { get; set; } = new();
    }

    public class VotePlanCreateOrUpdateValidater : AbstractValidator<VotePlanCreateOrUpdateDto>
    {
        public VotePlanCreateOrUpdateValidater()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(256).WithName("标题");

            RuleFor(x => x.VoteEndTime).NotNull().GreaterThan(x => x.VoteStartTime).WithName("投票结束时间");
            RuleFor(x => x.VoteStartTime).NotNull().WithName("投票开始时间");
            
            When(x => x.UploadEndTime.HasValue && x.UploadStartTime.HasValue, () => { RuleFor(x => x.UploadEndTime).GreaterThan(x => x.UploadStartTime).WithName("上传结束时间"); });
        }
    }
}