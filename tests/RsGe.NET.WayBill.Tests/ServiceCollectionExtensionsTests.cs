using RsGe.NET.Core;
using RsGe.NET.WayBill.Services;
using RsGe.NET.WayBill.Soap;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RsGe.NET.WayBill.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRsGeWayBillServices_WithConfig_RegistersAllServices()
    {
        var services = new ServiceCollection();
        var config = new WayBillConfiguration
        {
            ServiceUser = "test",
            ServicePassword = "pass",
            CompanyTin = "123456789"
        };

        services.AddRsGeWayBillServices(config);

        var provider = services.BuildServiceProvider();

        provider.GetService<RsGeConfiguration>().Should().NotBeNull();
        provider.GetService<WayBillConfiguration>().Should().NotBeNull();
        provider.GetService<IWayBillSoapClient>().Should().NotBeNull();
        provider.GetService<IRsGeDynamicServiceFactory>().Should().NotBeNull();
        provider.GetService<IRsGeWayBillService>().Should().NotBeNull();
    }

    [Fact]
    public void AddRsGeWayBillServices_WithConfiguration_BindsFromSection()
    {
        var services = new ServiceCollection();
        var configData = new Dictionary<string, string?>
        {
            ["RsGe:ServiceUser"] = "myuser",
            ["RsGe:ServicePassword"] = "mypass",
            ["RsGe:CompanyTin"] = "999888777"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        services.AddRsGeWayBillServices(configuration);

        var provider = services.BuildServiceProvider();
        var rsgeConfig = provider.GetRequiredService<WayBillConfiguration>();
        rsgeConfig.ServiceUser.Should().Be("myuser");
        rsgeConfig.ServicePassword.Should().Be("mypass");
        rsgeConfig.CompanyTin.Should().Be("999888777");
    }

    [Fact]
    public void AddRsGeWayBillServices_WithAction_ConfiguresCorrectly()
    {
        var services = new ServiceCollection();

        services.AddRsGeWayBillServices(config =>
        {
            config.ServiceUser = "actionuser";
            config.ServicePassword = "actionpass";
        });

        var provider = services.BuildServiceProvider();
        var rsgeConfig = provider.GetRequiredService<WayBillConfiguration>();
        rsgeConfig.ServiceUser.Should().Be("actionuser");
    }
}
