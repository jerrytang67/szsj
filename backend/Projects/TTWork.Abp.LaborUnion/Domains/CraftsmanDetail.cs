using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace TTWork.Abp.LaborUnion.Domains
{
    [Owned]
    public class CraftsmanDetail
    {
        [StringLength(32)] public string Realname { get; set; } //姓名
        [StringLength(8)] public string Sex { get; set; } // 性别
        
        [StringLength(256)] public string HeadImgUrl { get; set; } //证件照
        [StringLength(32)] public string Birthday { get; set; } // 生日
        [StringLength(32)] public string NativePlace { get; set; } //籍贯
        
        [StringLength(32)] public string Nation { get; set; } // 名族
        [StringLength(32)] public string PoliticsStatus { get; set; } //政治面貌
        [StringLength(64)] public string EducationBackground { get; set; } //学历
        
        [StringLength(64)] public string SkillLevel { get; set; } //技能等级
        
        public DateTime? TimeOfStartWork { get; set; } //参加工作时间
        
        public DateTime? TimeOfStartWorkLocal { get; set; } //在吴江从事本职业(工种)时间
        
        [StringLength(64)] public string Address { get; set; } //地址,地区
        [StringLength(64)] public string WorkUnit { get; set; } //工作单位
        [StringLength(64)] public string WorkTitle { get; set; } //职务
        [StringLength(32)] public string PhoneNumber { get; set; } //手机号
        public string PersonalResume { get; set; } //个人简历
        public string MainAchievement { get; set; } //主要成果,获奖情况
        public string MainEvent { get; set; } // 主要事迹

        public string OpinionsOfWorkUnit { get; set; } //工作单位意见
        public string OpinionsOfRecommandUnit { get; set; } //推荐单位意见
        public string OpinionsOfLeadingGroup { get; set; } //寻访领导小组意见
    }

    public class CraftsmanDetailValidater : AbstractValidator<CraftsmanDetail>
    {
        public CraftsmanDetailValidater()
        {
            RuleFor(x => x.Realname).NotEmpty().WithName("姓名");
            
            RuleFor(x => x.Sex).NotEmpty().WithName("性别");
            
            RuleFor(x => x.Birthday)
                .NotEmpty().WithMessage("生日不能为空");

            RuleFor(x => x.NativePlace).NotEmpty().WithName("籍贯");
            
            RuleFor(x => x.EducationBackground).NotEmpty().WithName("学历");
            
            RuleFor(x => x.PoliticsStatus).NotEmpty().WithName("政治面貌");
            
            RuleFor(x => x.Address)
                .NotEmpty().MaximumLength(64)
                .WithName("所属区域");

            RuleFor(x => x.WorkUnit).NotEmpty().WithName("工作单位");
            
            RuleFor(x => x.WorkTitle).NotEmpty().WithName("职务");
            
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("手机号不能为空")
                .Matches("^(1\\d{10})$").WithMessage("手机号格式不正确");
            
            // RuleFor(x => x.PersonalResume).NotEmpty().WithName("个人简历");
            
            // RuleFor(x => x.MainAchievement).NotEmpty().WithName("主要成果,获奖情况");
            
            // RuleFor(x => x.MainEvent).NotEmpty().WithName("主要事迹");
        }
    }
}