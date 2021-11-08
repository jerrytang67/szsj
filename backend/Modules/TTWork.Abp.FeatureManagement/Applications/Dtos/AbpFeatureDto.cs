using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;
using TTWork.Abp.Core.Organizations;
using TTWork.Abp.FeatureManagement.Domain;

namespace TTWork.Abp.FeatureManagement.Applications.Dtos
{
    [AutoMap(typeof(AbpFeature))]
    public class AbpFeatureDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        //This field "G","T","O"
        public string ProviderName { get; set; }

        public string ProviderKey { get; set; }

        public bool Enable { get; set; }

        public DateTime? DateTimeExpired { get; set; }

        public string Value { get; set; }

        public BasicOrganizationInfo OrganizationUnit { get; set; }
    }
}