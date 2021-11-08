using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TT.Extensions;
using TTWork.Abp.AuditManagement.Audits;
using TTWork.Abp.Core.Authorization.Users;

namespace TTWork.Abp.LaborUnion.Domains
{
    public enum RecomandState
    {
        已提交 = 0,
        审核中 = 1,
        推荐成功 = 3,
        推荐失败 = 4
    }


    public class CraftsmanRecommend : FullAuditedEntity<long>, INeedAudit, IMayHaveOrganizationUnit
    {
        public RecomandState State { get; set; } = RecomandState.已提交;

        public bool RedpacketRecived { get; set; } = false;

        public DateTime? RedpacketRecivedTime { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal? Redpacket { get; set; }

        public Guid? SecurityStamp { get; set; } = Guid.NewGuid();

        public virtual CraftsmanRecommendDetail Detail { get; set; }

        [StringLength(64)] public string RejectText { get; set; }

        [ForeignKey("CreatorUserId")] public virtual User CreatorUser { get; set; }

        #region 审核相关

        public Guid? AuditFlowId { get; set; }
        public int? Audit { get; set; }
        public int? AuditStatus { get; set; }

        [NotMapped]
        public bool IsAudited
        {
            get
            {
                switch (Audit)
                {
                    //未初始化
                    case null:
                    case -1:
                        return false;
                    default:
                        return Audit == AuditStatus;
                }
            }
        }

        #endregion

        public long? OrganizationUnitId { get; set; }
    }

    [Owned]
    public class CraftsmanRecommendDetail
    {
        [StringLength(32)] public string Realname { get; set; } //姓名
        [StringLength(8)] public string Sex { get; set; } // 性别
        [StringLength(8)] public string Age { get; set; } //年龄
        [StringLength(32)] public string PoliticsStatus { get; set; } //政治面貌

        [StringLength(64)] public string Address { get; set; } //地址,地区
        [StringLength(64)] public string WorkUnit { get; set; } //工作单位
        [StringLength(64)] public string WorkTitle { get; set; } //职务
        [StringLength(32)] public string PhoneNumber { get; set; } //手机号

        [StringLength(1024)] public string Desc { get; set; } // 推荐理由
    }

    public class CraftsmanRecommendDetailValidater : AbstractValidator<CraftsmanRecommendDetail>
    {
        public CraftsmanRecommendDetailValidater()
        {
            RuleFor(x => x.Realname).NotEmpty()
                .MaximumLength(32)
                .WithName("被推荐人姓名");

            RuleFor(x => x.Sex).NotEmpty()
                .MaximumLength(8)
                .WithName("性别");

            RuleFor(x => x.Age)
                .Matches("^(\\d+)$").When(x => !x.Age.IsNullOrEmptyOrWhiteSpace()).WithMessage("年龄必须为数字")
                .MaximumLength(8)
                .WithName("年龄")
                ;

            RuleFor(x => x.PoliticsStatus)
                .NotEmpty().MaximumLength(32)
                .WithName("政治面貌");

            RuleFor(x => x.Address)
                .NotEmpty().MaximumLength(64)
                .WithName("所属区域");

            RuleFor(x => x.WorkUnit)
                .NotEmpty().MaximumLength(64)
                .WithName("工作单位");

            RuleFor(x => x.WorkTitle)
                .MaximumLength(64)
                .WithName("职务");

            RuleFor(x => x.PhoneNumber)
                .Matches("^(1\\d{10})$").When(x => !x.Age.IsNullOrEmptyOrWhiteSpace()).WithMessage("手机号格式不正确");

            // RuleFor(x => x.PhoneNumber)
            //     .MaximumLength(32)
            //     .NotEmpty().WithMessage("请填写被推荐人手机号")
            //     .WithName("手机号");

            RuleFor(x => x.Desc)
                .NotEmpty().MaximumLength(1024)
                .WithName("推荐理由");
        }
    }
}