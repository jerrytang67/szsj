using System.Threading.Tasks;

namespace TTWork.Abp.Core.Security
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}