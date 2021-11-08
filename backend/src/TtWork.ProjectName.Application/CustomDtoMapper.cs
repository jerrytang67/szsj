using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Organizations;
using TtWork.ProjectName.Apis.Organizations.Dtos;
using TtWork.ProjectName.Apis.Pays;
using TtWork.ProjectName.Apis.Poster;
using TtWork.ProjectName.Apis.Roles.Dto;
using TtWork.ProjectName.Apis.Sessions.Dto;
using TtWork.ProjectName.Entities;
using TtWork.ProjectName.Entities.Organizations;
using TtWork.ProjectName.Entities.Pay;
using TtWork.ProjectName.Roles.Dto;
using TtWork.ProjectName.Users.Dto;
using AutoMapper;
using Newtonsoft.Json;
using TT.Extensions;
using TTWork.Abp.Core.Applications.Dtos;
using TTWork.Abp.Core.Authorization.Roles;
using TTWork.Abp.Core.Authorization.Users;
using TtWork.Lib.Extensions;
using TtWork.ProjectName.Apis.Cms;
using TtWork.ProjectName.Apis.Cms.Dtos;
using TtWork.ProjectName.Apis.Pays.Dtos;
using TtWork.ProjectName.Apis.Users.Dto;
using TtWork.ProjectName.Entities.Cms;
using TtWork.ProjectName.Entities.Posters;

namespace TtWork.ProjectName
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            #region 用户

            // User
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<UserDto, User>()
                .ForMember(x => x.Roles, opt => opt.Ignore())
                .ForMember(x => x.CreationTime, opt => opt.Ignore());
            configuration.CreateMap<CreateUserDto, User>();
            configuration.CreateMap<CreateUserDto, User>().ForMember(x => x.Roles, opt => opt.Ignore());
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(z => z.Password, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(z => z.Password, opt => opt.Ignore());


            // Role and permission
            configuration.CreateMap<Permission, string>().ConvertUsing(r => r.Name);
            configuration.CreateMap<RolePermissionSetting, string>().ConvertUsing(r => r.Name);

            configuration.CreateMap<CreateRoleDto, Role>();

            configuration.CreateMap<RoleDto, Role>();

            configuration.CreateMap<Role, RoleDto>().ForMember(x => x.GrantedPermissions,
                opt => opt.MapFrom(x => x.Permissions.Where(p => p.IsGranted)));

            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<Role, RoleEditDto>();
            configuration.CreateMap<Permission, FlatPermissionDto>();

            #endregion


            #region 机构

            //OrganizationUnit

            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>()
                .ReverseMap();

            configuration.CreateMap<ProjectNameOrganizationUnit, OrganizationUnitDto>()
                .ReverseMap();

            configuration.CreateMap<ProjectNameOrganizationUnit, ProjectNameOrganizationUnitDtoBase>()
                .ForMember(z => z.Desc, opt => opt.MapFrom(x => x.Detail.Desc))
                .ForMember(z => z.LogoImgUrl, opt => opt.MapFrom(x => x.Detail.LogoImgUrl))
                .ForMember(z => z.Address, opt => opt.MapFrom(x => x.Detail.Address))
                .ForMember(z => z.Phone, opt => opt.MapFrom(x => x.Detail.HeadmanPhone))
                ;


            configuration.CreateMap<CreateOrganizationUnitInput, ProjectNameOrganizationUnit>().ReverseMap();
            configuration.CreateMap<UpdateOrganizationUnitInput, ProjectNameOrganizationUnit>().ReverseMap();

            //OrganizationUnit Detail
            configuration.CreateMap<OrganizationUnitDetailDto, OrganizationUnitDetail>()
                // .ForMember(x => x.TeacherUrl, opt => opt.MapFrom(z => JsonConvert.SerializeObject(z.TeacherUrl)))
                .ReverseMap()
                // .ForMember(x => x.TeacherUrl, opt => opt.MapFrom(z => JsonConvert.DeserializeObject<List<string>>(z.TeacherUrl)))
                ;

            configuration.CreateMap<OrganizationUnitDetailCreateDto, OrganizationUnitDetail>()
                // .ForMember(x => x.TeacherUrl, opt => opt.MapFrom(z => JsonConvert.SerializeObject(z.TeacherUrl)))
                .ReverseMap()
                // .ForMember(x => x.TeacherUrl, opt => opt.MapFrom(z => JsonConvert.DeserializeObject<List<string>>(z.TeacherUrl)))
                ;

            #endregion

            #region 支付

            configuration.CreateMap<PayOrder, PayOrderDto>();

            configuration.CreateMap<TenPayNotifyXml, TenPayNotify>()
                .ForMember(z => z.Id, opt => opt.Ignore());

            #endregion

            #region 轮播

            configuration.CreateMap<Swiper, SwiperDto>()
                .ReverseMap();

            #endregion

            configuration.CreateMap<RefundDetail, RefundDetailDto>();
            
            #region 海报

            configuration.CreateMap<Poster, PosterDto>()
                .ForMember(x => x.MainImage, opt => opt.MapFrom(x =>
                    GetBoxDetail(x.MainImageDetail)))
                .ForMember(x => x.QrImage, opt => opt.MapFrom(x =>
                    GetBoxDetail(x.QrImageDetail)))
                .ForMember(x => x.HeadImage, opt => opt.MapFrom(x =>
                    GetBoxDetail(x.HeadImageDetail)))
                .ForMember(x => x.TitleText, opt => opt.MapFrom(x =>
                    GetFontBoxDetail(x.TitleTextDetail)))
                .ForMember(x => x.DescText, opt => opt.MapFrom(x =>
                    GetFontBoxDetail(x.DescTextDetail)))
                .ReverseMap()
                .ForMember(x => x.MainImageDetail, opt => opt.MapFrom(x =>
                    JsonConvert.SerializeObject(x.MainImage)))
                .ForMember(x => x.QrImageDetail, opt => opt.MapFrom(x =>
                    JsonConvert.SerializeObject(x.QrImage)))
                .ForMember(x => x.HeadImageDetail, opt => opt.MapFrom(x =>
                    JsonConvert.SerializeObject(x.HeadImage)))
                .ForMember(x => x.TitleTextDetail, opt => opt.MapFrom(x =>
                    JsonConvert.SerializeObject(x.TitleText)))
                .ForMember(x => x.DescTextDetail, opt => opt.MapFrom(x =>
                    JsonConvert.SerializeObject(x.DescText)))
                ;

            #endregion
            
            #region CMS

            configuration.CreateMap<CmsContent, CmsContentDto>()
                .ForMember(x => x.TitleImageUrl, opt =>
                {
                    opt.PreCondition(x => !x.TitleImageUrl.IsNullOrEmptyOrWhiteSpace());
                    opt.MapFrom(x => Regex.Replace(x.TitleImageUrl, "(.*)!w500$", "$1") + "!w500");
                })
                .ReverseMap();

            configuration.CreateMap<CmsContent, CmsContentCreateOrUpdateDto>()
                .ReverseMap();

            configuration.CreateMap<CmsCategory, CmsCategoryDto>()
                .ForMember(x => x.ImageUrl, opt =>
                {
                    opt.PreCondition(x => !x.ImageUrl.IsNullOrEmptyOrWhiteSpace());
                    opt.MapFrom(x => Regex.Replace(x.ImageUrl, "(.*)!w500$", "$1") + "!w500");
                })
                .ReverseMap();

            configuration.CreateMap<CmsContent, CmsCategoryCreateOrUpdateDto>()
                .ReverseMap();

            #endregion
        }

        private static BoxDetail GetBoxDetail(string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<BoxDetail>(jsonStr);
            }
            catch (Exception)
            {
                return new BoxDetail();
            }
        }

        private static FontBoxDetail GetFontBoxDetail(string jsonStr)
        {
            try
            {
                return JsonConvert.DeserializeObject<FontBoxDetail>(jsonStr);
            }
            catch (Exception)
            {
                return new FontBoxDetail();
            }
        }

        /// <summary>
        /// 取得枚举存放INT的各int位值的LIST  input: 11 = > 1011 =>  output: [8,2,1]
        /// </summary>
        /// <param name="tags"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<int> GetTagsIntList<T>(int tags) where T : Enum
        {
            var result = new List<int>();
            foreach (int v in typeof(T).GetEnumValues())
            {
                if ((v & tags) == v)
                    result.Add(v);
            }

            return result;
        }

        /// <summary>
        /// 取得枚举存放INT的各key位值的LIST  input: 11 = > 1011 =>  output: ["8的Key","2的Key","1的key"]
        /// </summary>
        /// <param name="tags"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<string> GetTagsNameList<T>(int tags)
        {
            var result = new List<string>();
            foreach (int v in typeof(T).GetEnumValues())
            {
                if ((v & tags) == v)
                    result.Add(EnumHelper<T>.GetDisplayValue(EnumHelper<T>.Parse(v.ToString())));
            }

            return result;
        }

        private static int TagsListToInt(List<int> tags)
        {
            return tags.Aggregate(0, (current, v) => current | v);
        }
    }
}