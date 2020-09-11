using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DevDistricts.Internal
{
    internal static class InternalExtensions
    {
        public static IReadOnlyList<(Type Type, DistrictAttribute Config, IReadOnlyList<OccupantAttribute> Occupants)> GetDistricts(this Assembly assembly)
        {
            return assembly.GetTypes()
                .Select(t => new
                {
                    Type = t,
                    Config = t.GetCustomAttribute<DistrictAttribute>(),
                    Occupants = t.GetCustomAttributes<OccupantAttribute>()
                }).Where(x => x.Config != null)
                .Select(x => (x.Type, x.Config!, (IReadOnlyList<OccupantAttribute>) x.Occupants.ToList()))
                .ToList();
        }

        public static (Type Type, DistrictAttribute Config, IReadOnlyList<OccupantAttribute> Occupants)? MatchDistrict(
            this IReadOnlyList<(Type Type, DistrictAttribute Config, IReadOnlyList<OccupantAttribute> Occupants)> districts,
            string userName,
            string machineName)
        {
            var matches = districts.Where(x => x.Occupants.IsMatch(userName, machineName))
                .ToList();

            if (matches.Count == 1)
            {
                return matches.First();
            }

            if (matches.Count == 0)
            {
                return null;
            }
            
            throw new AmbiguousDistrictMatchException(matches.Select(x => x.Type).ToList(), userName, machineName);
        }

        public static bool IsTask(this Type type)
        {
            if (type == typeof(Task))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return true;
            }

            return false;
        }

        private static bool IsMatch(
            this IReadOnlyList<OccupantAttribute> occupants,
            string userName,
            string machineName)
        {
            return occupants.FirstOrDefault(o =>
                string.Equals(o.UserName, userName, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(o.MachineName, machineName, StringComparison.InvariantCultureIgnoreCase)) != null;
        }
        
    }
}