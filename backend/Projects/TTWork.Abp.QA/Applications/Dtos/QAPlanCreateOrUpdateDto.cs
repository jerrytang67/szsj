using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using FluentValidation;
using JetBrains.Annotations;
using Newtonsoft.Json.Linq;
using TTWork.Abp.QA.Domains;

namespace TTWork.Abp.QA.Applications.Dtos
{
    // ReSharper disable once InconsistentNaming

    public class QAPlanCreateOrUpdateDto : EntityDto<long>
    {
        [StringLength(256)] public string TitleImageUrl { get; set; }

        [StringLength(256)] public string Title { get; set; }

        [StringLength(512)] public string SubTitle { get; set; }

        public EnumClass.QAPlanState State { get; set; }

        public EnumClass.QAPlanType Type { get; set; }
        public long? LuckDrawId { get; set; } //对应Type==2时

        public string HtmlContext { get; set; }

        public int ViewCount { get; init; }

        public int AnswerPerDay { get; set; }
        public int HelpPerDay { get; set; }

        public int SharePoints { get; set; }

        public JObject Settings { get; set; } = new();

        public List<PointRule> PointRules { get; set; } = new();

        public int QuestionCount { get; set; }

        public DateTime DatetimeStart { get; set; } = DateTime.Today.AddDays(2);

        public DateTime DatetimeEnd { get; set; } = DateTime.Today.AddDays(2).AddMonths(1);

        [StringLength(16)] [CanBeNull] public string TimeStart { get; set; }

        [StringLength(16)] [CanBeNull] public string TimeEnd { get; set; }
    }

    public class QAPlanCreateOrUpdateDtoValitater : AbstractValidator<QAPlanCreateOrUpdateDto>
    {
        public QAPlanCreateOrUpdateDtoValitater()
        {
            RuleFor(x => x.Title).NotNull().NotEmpty().WithName("标题");

            When(x => x.Type == EnumClass.QAPlanType.抽奖, () =>
            {
                RuleFor(x => x.LuckDrawId).NotNull().WithMessage("当活动为奖励抽奖时,必须指定抽奖活动");
            });
        }
    }
}