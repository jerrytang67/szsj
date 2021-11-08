using Xunit;

namespace TtWork.ProjectName.Tests
{
    public sealed class MultiTenantFactAttribute : FactAttribute
    {
        public MultiTenantFactAttribute()
        {
            if (!ProjectNameConsts.MultiTenancyEnabled)
            {
                //Skip = "MultiTenancy is disabled.";
            }
        }
    }
}
