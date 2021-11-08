namespace TTWork.Abp.Core.Organizations
{
    public interface ICurrentOrganizationAccessor
    {
        BasicOrganizationInfo Current { get; set; }
    }
}