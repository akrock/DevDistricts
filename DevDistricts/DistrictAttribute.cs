using System;
using System.Collections.Generic;

namespace DevDistricts
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DistrictAttribute : Attribute
    {
        public Type[]? DependencyInjectionTypes { get; set; }
    }
    
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OccupantAttribute : Attribute
    {
        public string? UserName { get; set; }
        public string? MachineName { get; set; }
    }
}