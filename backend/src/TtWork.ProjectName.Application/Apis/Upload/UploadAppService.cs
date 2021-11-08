using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.UI;
using Abp.Web.Models;
using TtWork.ProjectName.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using TTWork.Abp.Core;
using TTWork.Abp.Core.Oss;
using TTWork.Abp.Oss.UpYun;

namespace TtWork.ProjectName.Apis.Upload
{
    public class UploadAppService : AbpAppServiceBase, IUploadAppService
    {
        private readonly ISettingManager _settingManager;
        private readonly IOssClient _ossClient;

        public UploadAppService(SettingManager settingManager, IOssClient ossClient, ISettingManager settingManager1)
        {
            _ossClient = ossClient;
            _settingManager = settingManager1;
        }


        static string GetMd5(string str)
        {
            //创建MD5哈稀算法的默认实现的实例
            MD5 md5 = MD5.Create();
            //将指定字符串的所有字符编码为一个字节序列
            byte[] buffer = Encoding.Default.GetBytes(str);
            //计算指定字节数组的哈稀值
            byte[] bufferMd5 = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bufferMd5.Length; i++)
            {
                //x:表示将十进制转换成十六进制
                sb.Append(bufferMd5[i].ToString("x2"));
            }

            return sb.ToString();
        }

        [DontWrapResult]
        public async Task<object> GetSignature(string data)
        {
            var password = GetMd5(await _settingManager.GetSettingValueAsync(OssSetting.Upyun.Password));

            var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(password));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));

            return await Task.FromResult(new {signature = Convert.ToBase64String(hashBytes)});
        }

        [Authorize]
        public async Task<string> Upload(int? id, [Microsoft.AspNetCore.Mvc.FromForm] IFormFile file)
        {
            //todo:vue-admin cookies
            var tenantId = id ?? 2; //for test

            if (file == null || file.Length == 0)
                return "file not selected";
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var buffer = memoryStream.ToArray();
            var url = $"/{_ossClient.BucketName}/{DateTime.Now:yyyy/MM/HHmmss_}{file.FileName}";
            //Do whatever you want with filename and its binary data.
            var b = await _ossClient.writeFile(url, buffer, true);
            if (!b)
                throw new UserFriendlyException("Upload Fail");

            return $"{_ossClient.Domain}{url}";
        }
    }
}