﻿using System;

namespace TTWork.Abp.Core.Applications.Dtos
{
    public class AccessTokeCacheItem
    {
        public string AccessToken { get; set; }

        public string Appid { get; set; }

        public string AppSecret { get; set; }

        public DateTimeOffset TimeCreated { get; set; }

        public DateTimeOffset TimeExpired { get; set; }
    }
}