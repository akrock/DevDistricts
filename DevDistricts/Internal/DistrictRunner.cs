using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevDistricts.Internal
{
    internal class DistrictRunner<T> : BackgroundService
        where T : class
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly IServiceProvider _serviceProvider;

        public DistrictRunner(
            IHostApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider)
        {
            _applicationLifetime = applicationLifetime;
            _serviceProvider = serviceProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var name = typeof(T).Name;
                Console.WriteLine($">>> Running {name} <<<\n");
                await RunDistrict();
                Console.WriteLine($"\n<<< Finished {name} >>>");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled Exception Thrown:\n{ex}");
                throw;
            }
            finally
            {
                _applicationLifetime.StopApplication();
            }
        }

        private async Task RunDistrict()
        {
            var tType = typeof(T);
            var runMethod = tType.GetMethod("Run",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);

            if (runMethod == null)
            {
                throw new MissingMethodException(tType.Name, "Run");
            }
            
            var parameters = runMethod.GetParameters()
                .Select(p => ResolveFromServices(p.ParameterType))
                .ToArray();

            object? instance = null;
            
            if (!runMethod.IsStatic)
            {
                instance = ResolveFromServices(tType);
            }

            if (runMethod.ReturnType.IsTask())
            {
                await (Task) runMethod.Invoke(instance, parameters);
            }
            else
            {
                runMethod.Invoke(instance, parameters);
            }
        }

        private object ResolveFromServices(Type t)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, t);
        }
        
        
    }
}