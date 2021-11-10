using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Newtonsoft.Json;
using TT.Extensions;
using TTWork.Abp.Activity.Applications;
using TTWork.Abp.Activity.Domains;

namespace TTWork.Abp.Activity
{
    public static class ActivityMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<PointActivityDto, PointActivity>()
                .ForMember(x => x.Settings, opt => opt.MapFrom(z => JsonConvert.SerializeObject(z.Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })))
                .ReverseMap()
                .ForMember(x => x.Settings,
                    opt => opt.MapFrom(z => JsonConvert.DeserializeObject<Dictionary<string, string>>(z.Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })))
                .ForMember(x => x.IsEnd, opt => opt.MapFrom(z => z.DatetimeEnd < DateTime.Now))
                ;


            cfg.CreateMap<PointActivityCreateOrUpdateDto, PointActivity>()
                .ForMember(x => x.Settings, opt => opt.MapFrom(z => JsonConvert.SerializeObject(z.Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })))
                .ReverseMap()
                .ForMember(x => x.Settings,
                    opt => opt.MapFrom(z => JsonConvert.DeserializeObject<Dictionary<string, string>>(z.Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })))
                ;


            cfg.CreateMap<PointActivityUserLog, PointActivityUserLogDto>();


            cfg.CreateMap<LuckDraw, LuckDrawBaseDto>();


            cfg.CreateMap<LuckDrawDto, LuckDraw>()
                .ForMember(x => x.Settings, opt => opt.MapFrom(z => JsonConvert.SerializeObject(z.Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })))
                .ReverseMap()
                .ForMember(x => x.Settings,
                    opt => opt.MapFrom(z => JsonConvert.DeserializeObject<Dictionary<string, string>>(z.Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })))
                ;


            cfg.CreateMap<LuckDrawCreateOrUpdateDto, LuckDraw>()
                .ForMember(x => x.Settings, opt => opt.MapFrom(z => JsonConvert.SerializeObject(z.Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })))
                .ForMember(x => x.CheckCodes, opt => opt.MapFrom(z => JsonConvert.SerializeObject(z.CheckCodes)))
                .ReverseMap()
                .ForMember(x => x.Settings,
                    opt => opt.MapFrom(z => JsonConvert.DeserializeObject<Dictionary<string, string>>(z.Settings, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })))
                .ForMember(x => x.CheckCodes,
                    opt =>
                    {
                        opt.Condition(src => !src.CheckCodes.IsNullOrEmptyOrWhiteSpace());
                        opt.MapFrom(z => JsonConvert.DeserializeObject<List<string>>(z.CheckCodes, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                    })
                ;

            cfg.CreateMap<LuckDrawPrizeCreateOrUpdateDto, LuckDrawPrize>()
                .ReverseMap();

            cfg.CreateMap<LuckDrawPrize, LuckDrawPrizeDto>()
                .ReverseMap();

            cfg.CreateMap<UserPrize, UserPrizeDto>()
                .ReverseMap();

            cfg.CreateMap<UserLuckTime, UserLuckTimeDto>();
        }
    }
}