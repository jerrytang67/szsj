﻿using System.Collections.Generic;

namespace TTWork.Abp.AppManagement.Apps
{
    public interface IAppValueProviderManager
    {
        List<IAppValueProvider> Providers { get; }
    }
}