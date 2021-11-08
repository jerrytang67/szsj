using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using FluentValidation;
using TtWork.ProjectName.Entities.Cms;

namespace TtWork.ProjectName.Apis.Cms
{
    /// <summary>
    /// <see cref="CmsContent"/>
    /// 新建或编辑新闻内容
    /// </summary>
    [AutoMapTo(typeof(CmsContent))]
    public class CmsContentCreateOrUpdateDto : EntityDto<int>, ICustomValidate
    {
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string TitleImageUrl { get; set; }

        public int LinkType { get; set; } = 0;

        [StringLength(256)] public string LinkUrl { get; set; }
        public string Content { get; set; }

        public EnumClass.CmsContentStatus Status { get; set; }

        public int Sort { get; set; }


        public DateTime? CreationTime { get; set; }


        public void AddValidationErrors(CustomValidationContext context)
        {
        }
    }

    public class CmsContentCreateOrEditDtoValidate : AbstractValidator<CmsContentCreateOrUpdateDto>
    {
        public CmsContentCreateOrEditDtoValidate()
        {
            RuleFor(x => x.CategoryId).NotNull().WithName("分类");
            RuleFor(x => x.Title).Length(2, 256).WithName("标题");
            RuleFor(x => x.TitleImageUrl).Length(2, 256).WithName("图片");
            // RuleFor(x => x.Content).NotNull().NotEmpty().WithName("内容");
        }
    }
}