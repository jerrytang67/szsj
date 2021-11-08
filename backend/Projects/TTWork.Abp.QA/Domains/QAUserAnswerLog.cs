using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace TTWork.Abp.QA.Domains
{
    public enum UserQuestionLogEnum
    {
        未完成 = 0,
        完成答题 = 1,
        已领奖 = 2
    }


    public class UserQuestionLogDto : CreationAuditedEntityDto<long>
    {
        public long PlanId { get; set; }

        public UserQuestionLogEnum State { get; set; }

        public int? RightCount { get; private set; } = null;

        public int? Points { get; private set; } = null;
        public long? UserLuckTimeId { get; set; }
        public long? ShareFrom { get; set; }

        public List<QuestionItem> QuestionItems { get; set; }

        public int SpendTime { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    public class UserQuestionLog : CreationAuditedEntity<long>, IMustHaveTenant
    {
        public long PlanId { get; set; }
        public UserQuestionLogEnum State { get; set; }
        public int? RightCount { get; set; } = null;
        public int? Points { get; set; } = null;
        
        public long? UserLuckTimeId { get; set; }
        public List<QuestionItem> QuestionItems { get; set; } = new();
        public long? ShareFrom { get; set; }

        public int TenantId { get; set; }

        public int? SpendTime { get; set; } = null;
    }

    [NotMapped]
    public class QuestionItem : IQuestionData
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int AnswerIndex { get; set; }
        public List<string> AnswerList { get; set; }
        public int? UserSelectIndex { get; set; } = null;
        public DateTime? FinishTime { get; set; } = null;
    }
}