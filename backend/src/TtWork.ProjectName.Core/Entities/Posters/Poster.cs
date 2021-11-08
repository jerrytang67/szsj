using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;

namespace TtWork.ProjectName.Entities.Posters
{
    [Table("T_Poster")]
    public class Poster : FullAuditedAggregateRoot<int>, IMustHaveTenant, IMayHaveOrganizationUnit, IExtendableObject
    {
        [StringLength(256)] public virtual string Url { get; set; }

        [StringLength(256)] public virtual string BgImageUrl { get; set; }
        public virtual int BgWidth { get; set; }
        public virtual int BgHeight { get; set; }

        [StringLength(256)] public virtual string MainImageDetail { get; set; } //contont like 200,400,100,100 => x,y,width,height
        [StringLength(256)] public virtual string QrImageDetail { get; set; } //contont like 200,400,100,100 => x,y,width,height
        [StringLength(256)] public virtual string HeadImageDetail { get; set; } //contont like 100,100,100,100 => x,y,width,height
        [StringLength(256)] public virtual string TitleTextDetail { get; set; } //contont like 100,100,14,20 => x,y,fontSize,wordLength
        [StringLength(256)] public virtual string DescTextDetail { get; set; } //contont like 100,100,14,50 => x,y,fontSize

        public virtual int TenantId { get; set; }
        public virtual long? OrganizationUnitId { get; set; }
        public virtual string ExtensionData { get; set; }
    }
}