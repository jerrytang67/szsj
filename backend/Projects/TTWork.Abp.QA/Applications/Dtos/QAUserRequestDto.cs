using FluentValidation;

namespace TTWork.Abp.QA.Applications.Dtos
{
    public class QAUserRequestDto
    {
        public string Surname { get; set; }
        public string Town { get; set; }
        public string PhoneNumber { get; set; }
    }
    
    
    public class QAUserRequestDtoValidater : AbstractValidator<QAUserRequestDto>
    {
        public QAUserRequestDtoValidater()
        {
            RuleFor(x => x.Surname)
                .NotNull().WithMessage("请输入姓名")
                .NotEmpty().WithMessage("请输入姓名");
            RuleFor(x => x.Town)
                .NotNull().WithMessage("请选择所在镇名")
                .NotEmpty().WithMessage("请选择所在镇名");
            RuleFor(x => x.PhoneNumber)
                .NotNull().WithMessage("请输入手机")
                .NotEmpty().WithMessage("请输入手机");
        }
    }
}