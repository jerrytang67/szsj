using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TT.Extensions;
using TTWork.Abp.Timeline.Applications;
using TTWork.Abp.Timeline.Applications.Dtos;
using TTWork.Abp.Timeline.Domains;

namespace TTWork.Abp.Timeline
{
    public static class TimelineMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<TimelineEventDto, TimelineEvent>()
                .ReverseMap();

            cfg.CreateMap<TimelineEventCreateOrUpdateDto, TimelineEvent>()
                .ReverseMap();


            cfg.CreateMap<TimelineCategoryDto, TimelineCategory>()
                .ReverseMap();

            cfg.CreateMap<TimelineCategoryCreateOrUpdateDto, TimelineCategory>()
                .ReverseMap();


            cfg.CreateMap<TimelineFileDto, TimelineFile>()
                .ReverseMap()
                .ForMember(x => x.Data, opt => opt.MapFrom(x => JObject.Parse(x.ExtensionData)))
                ;


            cfg.CreateMap<TimelineFileCreateOrUpdateDto, TimelineFile>()
                .ReverseMap();
        }
    }
}