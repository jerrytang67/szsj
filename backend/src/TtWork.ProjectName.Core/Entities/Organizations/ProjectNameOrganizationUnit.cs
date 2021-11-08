using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Organizations;

namespace TtWork.ProjectName.Entities.Organizations
{
    public class ProjectNameOrganizationUnit : OrganizationUnit, IExtendableObject
    {
        public ProjectNameOrganizationUnit()
        {
        }

        public ProjectNameOrganizationUnit(int? tenantId, string displayName, long? parentId = null) : base(tenantId, displayName, parentId)
        {
        }

        public int Status { get; protected set; }


        public OrganizationUnitDetail Detail { get; set; } = new OrganizationUnitDetail();

        public string ExtensionData { get; set; }

        [NotMapped]
        public string RefuseContent
        {
            get => this.GetData<string>("RefuseContent");
            set => this.SetData("RefuseContent", value);
        }

        #region Domain Command

        public void Resfuse(string refuseContent)
        {
            Status = 0;
            RefuseContent = refuseContent;
        }

        public void Approve()
        {
            Status = 1;
            RefuseContent = null;
        }

        #endregion
    }
}