using Abp.Application.Services;
using TtWork.ProjectName.Apis.AliCloud.Dto;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth.Sts;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Microsoft.AspNetCore.Http.Features;

namespace TtWork.ProjectName.Apis.AliCloud
{
    public class StsAppService:ApplicationService
    {

        public StsResultDto Get()
        {
            var regionid = "cn-hangzhou";
            var endpoint = "sts.aliyuncs.com";
            var accessKeyId = "LTAI4G9JYccP7KozoBdQkhpx";
            var accessSecret = "loNLuDeO5haHVxLA0qyJhhv1xYr2qM";
            var bucketName = "community-tech-vod";

            var defaultProfile = DefaultProfile.GetProfile();
            defaultProfile.AddEndpoint(regionid,regionid,"Sts",endpoint);
            var profile = DefaultProfile.GetProfile(regionid, accessKeyId, accessSecret);

            var client = new DefaultAcsClient(profile);
            var request = new AssumeRoleRequest();
            request.DurationSeconds = 3600;
            request.AcceptFormat = FormatType.JSON;

            request.RoleArn = "acs:ram::1483284917031376:role/ramoss";
            request.RoleSessionName = "oss-session";

            var response = client.GetAcsResponse(request);

            return new StsResultDto()
            {
                AccessKeyId = accessKeyId,
                AccessKeySecret = accessSecret,
                SecurityToken = response.Credentials.SecurityToken,
                Bucket = bucketName,
                Region = regionid
            };

        }
    }
}