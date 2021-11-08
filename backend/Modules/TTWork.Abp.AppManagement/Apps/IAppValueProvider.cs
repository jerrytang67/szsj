﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace TTWork.Abp.AppManagement.Apps
{
    public interface IAppValueProvider
    {
        string Name { get; }

        Task<Dictionary<string, string>> GetOrNullAsync([NotNull] AppDefinition app);
    }
}