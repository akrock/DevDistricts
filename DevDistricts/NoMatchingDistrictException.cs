using System;

namespace DevDistricts
{
    public class NoMatchingDistrictException : Exception
    {
        public NoMatchingDistrictException(string userName, string machineName) :
            base($"No matching district found for userName [{userName}] and machineName [{machineName}]")
        {
            
        }
    }
}