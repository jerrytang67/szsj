using System.Linq;
using AutoMapper;
using TTWork.Abp.QA.Applications;
using TTWork.Abp.QA.Applications.Dtos;
using TTWork.Abp.QA.Domains;

namespace TTWork.Abp.QA
{
    // ReSharper disable once InconsistentNaming
    public static class QAMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<QAPlan, QAPlanDto>()
                .ReverseMap();

            cfg.CreateMap<QAPlan, QAPlanCreateOrUpdateDto>()
                .ForMember(x => x.PointRules, opt => opt.MapFrom(x => x.PointRules.OrderBy(z => z.Count)))
                // .ForMember(x => x.Settings, opt => opt.MapFrom(x => x.Settings))
                .ReverseMap()
                .ForMember(x => x.PointRules, opt => opt.MapFrom(x => x.PointRules.OrderBy(z => z.Count)))

                // .ForMember(x => x.Settings, opt => opt.MapFrom(x => x.Settings))
                ;

            cfg.CreateMap<QAQuestion, QAQuestionDto>().ReverseMap();
            cfg.CreateMap<QAQuestion, QAQuestionCreateOrUpdateDto>().ReverseMap();


            cfg.CreateMap<UserQuestionLog, UserQuestionLogDto>()
                .ReverseMap()
                ;
        }
    }
}