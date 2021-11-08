using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using FluentValidation;
using TtWork.ProjectName.Entities.Cms;

namespace TtWork.ProjectName.Apis.Cms.Dtos
{
    [AutoMap(typeof(CmsCategory))]
    public class CmsCategoryCreateOrUpdateDto : EntityDto<int>
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CmsCategoryCreateOrUpdateDtoValidater : AbstractValidator<CmsCategoryCreateOrUpdateDto>
    {
        public CmsCategoryCreateOrUpdateDtoValidater()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(64).WithName("标题");

            RuleFor(x => x.Name).MaximumLength(256).WithName("图片");
        }
    }
}