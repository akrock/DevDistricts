using System;
using System.Collections.Generic;
using System.Linq;

namespace DevDistricts
{
    public class AmbiguousDistrictMatchException : Exception
    {
        public IReadOnlyList<Type> DistrictTypes { get; }
        public string UserName { get; }
        public string MachineName { get; }

        public AmbiguousDistrictMatchException(
            IReadOnlyList<Type> districtTypes,
            string userName,
            string machineName) : base($"The userName [{userName}] and machineName [{machineName}] combination resolves to more than district. Matching Types: [{string.Join(", ", districtTypes.Select(x => x.FullName))}")
        {
            DistrictTypes = districtTypes;
            UserName = userName;
            MachineName = machineName;
        }
    }
}