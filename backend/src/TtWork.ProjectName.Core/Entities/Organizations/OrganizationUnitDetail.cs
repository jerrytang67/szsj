using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TtWork.ProjectName.Entities.Organizations
{
    [Owned]
    public class OrganizationUnitDetail : IExtendableObject
    {
        [StringLength(512)] public string Desc { get; set; }

        [StringLength(256)] public string LogoImgUrl { get; set; }

        [StringLength(128)] public string Address { get; set; }

        [StringLength(32)] public string HeadmanRealName { get; set; }

        [StringLength(32)] public string HeadmanPhone { get; set; }

        /// <summary>
        /// 所属省份
        /// </summary>
        [StringLength(24)]
        public string Province { get; set; }

        /// <summary>
        /// 所属城市
        /// </summary>
        [StringLength(24)]
        public string City { get; set; }

        /// <summary>
        /// 所属城市ID
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// 所属区/县
        /// </summary>
        [StringLength(24)]
        public string District { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double? Lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double? Lat { get; set; }


        [NotMapped]
        public string unionid
        {
            get => this.GetData<string>("unionid") ?? "";
            set => this.SetData("unionid", value);
        }

        public string ExtensionData { get; set; }
    }
}