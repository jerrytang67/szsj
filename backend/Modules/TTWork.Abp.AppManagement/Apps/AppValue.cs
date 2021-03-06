using System;
using System.Collections.Generic;
using Abp;

namespace TTWork.Abp.AppManagement.Apps
{
    [Serializable]
    public class AppValue : NameValue<Dictionary<string, string>>
    {
        public AppValue()
        {
        }

        public AppValue(string name, Dictionary<string, string> value)
        {
            Name = name;
            Value = value;
        }
    }
}