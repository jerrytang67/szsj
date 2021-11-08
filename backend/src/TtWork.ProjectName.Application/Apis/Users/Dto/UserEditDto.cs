﻿using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using TtWork.ProjectName.Authorization.Users;
using TTWork.Abp.Core.Authorization.Users;

namespace TtWork.ProjectName.Users.Dto
{
    //Mapped to/from User in CustomDtoMapper
    [AutoMapTo(typeof(User))]
    public class UserEditDto : IPassivable
    {
        /// <summary>
        /// Set null to create a new user. Set user's Id to update a user
        /// </summary>
        public long? Id { get; set; }


        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        public string HeadImgUrl { get; set; }


        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        // Not used "Required" attribute since empty value is used to 'not change password'
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        //public bool ShouldChangePasswordOnNextLogin { get; set; }

        //public virtual bool IsTwoFactorEnabled { get; set; }

        //public virtual bool IsLockoutEnabled { get; set; }
    }
}