﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTWork.Abp.AppManagement.Domain;

namespace TTWork.Abp.AppManagement.Apps
{
    public interface IAppRepository
    {
        Task<App> FindAsync(string name, string providerName, string providerKey);

        Task<List<App>> GetListAsync(string providerName, string providerKey);

        Task<App> InsertAsync(App app);

        Task<App> UpdateAsync(App app);

        Task DeleteAsync(App app);
    }
}