using System.Collections.Generic;
using AutoMapper;
using Newtonsoft.Json;
using TTWork.Abp.LaborUnion.Applications;
using TTWork.Abp.LaborUnion.Applications.Dtos;
using TTWork.Abp.LaborUnion.Domains;

namespace TTWork.Abp.LaborUnion
{
    public static class LaborUnionMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<CraftsmanRecommendCreateOrUpdateDto, CraftsmanRecommend>()
                .ReverseMap();

            cfg.CreateMap<CraftsmanRecommendDto, CraftsmanRecommend>()
                .ReverseMap();

            cfg.CreateMap<CraftsmanCreateOrUpdateDto, Craftsman>()
                .ForMember(x=>x.RedpacketRecived,opt=>opt.Ignore())
                .ForMember(x=>x.Redpacket,opt=>opt.Ignore())
                .ReverseMap();

            cfg.CreateMap<CraftsmanDto, Craftsman>()
                .ReverseMap();
        }
    }
}