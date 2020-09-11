using System;
using System.Collections.Generic;

namespace DevDistricts
{
    public class DevDistrictsOptions
    {
        public static readonly DevDistrictsOptions Default =
            new DevDistrictsOptions(Environment.UserName, Environment.MachineName, null);
        
        public DevDistrictsOptions(string userName, string machineName, Action<IReadOnlyList<Type>>? dependencyInjectionCallback)
        {
            UserName = userName;
            MachineName = machineName;
            DependencyInjectionCallback = dependencyInjectionCallback;
        }

        public string UserName { get; }
        
        public string MachineName { get; }
        
        public Action<IReadOnlyList<Type>>? DependencyInjectionCallback { get; }

        public DevDistrictsOptions WithDependencyInjectionCallback(Action<IReadOnlyList<Type>> dependencyInjectionCallback)
        {
            return new DevDistrictsOptions(UserName,MachineName, dependencyInjectionCallback);
        }

        public DevDistrictsOptions WithUserName(string userName)
        {
            return new DevDistrictsOptions(userName, MachineName, DependencyInjectionCallback);
        }

        public DevDistrictsOptions WithMachineName(string machineName)
        {
            return new DevDistrictsOptions(UserName, machineName, DependencyInjectionCallback);
        }
    }
}