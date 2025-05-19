using System;
using System.Linq;
using DevDistricts;
using DevDistricts.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

[District(DependencyInjectionTypes = new[] { typeof(SampleService) })]
[Occupant(UserName = "user", MachineName = "machine")]
internal class SampleDistrict
{
}

internal class SampleService
{
}

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void WithDevDistricts_RegistersHostedService_AndInvokesCallback()
    {
        var services = new ServiceCollection();
        Type[]? callbackTypes = null;

        services.WithDevDistricts(o => o
            .WithUserName("user")
            .WithMachineName("machine")
            .WithDependencyInjectionCallback(types => callbackTypes = types.ToArray()));

        var descriptor = services.Single(d => d.ServiceType == typeof(IHostedService));
        Assert.Equal(typeof(DistrictRunner<SampleDistrict>), descriptor.ImplementationType);
        Assert.NotNull(callbackTypes);
        Assert.Contains(typeof(SampleService), callbackTypes);
    }

    [Fact]
    public void WithDevDistricts_Throws_NoMatchingDistrictException_When_No_District()
    {
        var services = new ServiceCollection();
        Assert.Throws<NoMatchingDistrictException>(() =>
            services.WithDevDistricts(o => o
                .WithUserName("none")
                .WithMachineName("none")));
    }

    [Fact]
    public void WithDevDistricts_Throws_NotSupported_When_DITypes_NoCallback()
    {
        var services = new ServiceCollection();
        Assert.Throws<NotSupportedException>(() =>
            services.WithDevDistricts(o => o
                .WithUserName("user")
                .WithMachineName("machine")));
    }
}
