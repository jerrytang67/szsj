using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using FluentValidation;

namespace TTWork.Abp.QA.Domains
{
    // ReSharper disable once InconsistentNaming
    public class QAQuestionCreateOrUpdateDto : EntityDto<Guid>
    {
        public long? PlanId { get; set; }

        public string Title { get; set; }

        public int State { get; set; } = 1;

        public int? AnswerIndex { get; set; }

        public List<string> AnswerList { get; set; } = new();
    }

    // ReSharper disable once InconsistentNaming
    public class QAQuestionCreateOrUpdateDtoValidater : AbstractValidator<QAQuestionCreateOrUpdateDto>
    {
        public QAQuestionCreateOrUpdateDtoValidater()
        {
            RuleFor(x => x.PlanId)
                .NotNull()
                .WithName("答题活动");
            
            RuleFor(x => x.Title)
                .NotNull()
                .NotEmpty()
                .WithName("题目");

            RuleFor(x => x.AnswerList)
                .NotNull()
                .NotEmpty()
                .WithName("答案列表");

            RuleFor(x => x.AnswerIndex)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .WithName("正确答案");
        }
    }
}