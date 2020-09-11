using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using DevDistricts.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace DevDistricts
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection WithDevDistricts(
            this IServiceCollection serviceCollection,
            Func<DevDistrictsOptions, DevDistrictsOptions>? optionsBuilder = null)
        {
            var options = DevDistrictsOptions.Default;
            if (optionsBuilder != null)
            {
                options = optionsBuilder(options);
            }

            var district = Assembly.GetCallingAssembly()
                .GetDistricts()
                .MatchDistrict(options.UserName, options.MachineName);

            if (district == null)
            {
                throw new NoMatchingDistrictException(options.UserName, options.MachineName);
            }

            var (type, config, _) = district.Value;

            if (config.DependencyInjectionTypes != null)
            {
                if (options.DependencyInjectionCallback == null)
                {
                    throw new NotSupportedException("DependencyInjectTypes may only be defined if a DependencyInjectionCallback is defined on the DevDistrictsOptions.");
                }

                options.DependencyInjectionCallback(config.DependencyInjectionTypes);
            }

            typeof(ServiceCollectionExtensions)
                .GetMethod(nameof(AddDistrict), BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(type)
                .Invoke(null, new object[] {serviceCollection});
            
            return serviceCollection;
        }
        
        private static void AddDistrict<T>(IServiceCollection services)
            where T : class
        {
            services.AddHostedService<DistrictRunner<T>>();
        }
    }
}