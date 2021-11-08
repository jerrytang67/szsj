using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace TTWork.Abp.QA.Domains
{
    public interface IQuestionData
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int AnswerIndex { get; set; }
        public List<string> AnswerList { get; set; }
    }


    // ReSharper disable once InconsistentNaming
    public class QAQuestion : FullAuditedEntity<Guid>, IMustHaveTenant, IQuestionData
    {
        public string Title { get; set; }

        public EnumClass.QAQuestionState State { get; set; }

        public long PlanId { get; set; }

        public int AnswerIndex { get; set; }
        public List<string> AnswerList { get; set; } = new();

        public int TenantId { get; set; }

        [ForeignKey("PlanId")] public virtual QAPlan QAPlan { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    public class QAQuestionDto : EntityDto<Guid>
    {
        public string Title { get; set; }

        public int State { get; set; }

        public long PlanId { get; set; }

        public int AnswerIndex { get; set; }

        public List<string> AnswerList { get; set; }
    }

}