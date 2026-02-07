using RsGe.NET.SpecInvoice.Services;
using RsGe.NET.SpecInvoice.Soap;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RsGe.NET.SpecInvoice.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRsGeSpecInvoiceServices_WithConfig_RegistersAllServices()
    {
        var services = new ServiceCollection();
        var config = new SpecInvoiceConfiguration
        {
            ServiceUser = "test",
            ServicePassword = "pass",
            UserId = 1
        };

        services.AddRsGeSpecInvoiceServices(config);

        var provider = services.BuildServiceProvider();

        provider.GetService<SpecInvoiceConfiguration>().Should().NotBeNull();
        provider.GetService<ISpecInvoiceSoapClient>().Should().NotBeNull();
        provider.GetService<IRsGeSpecInvoiceService>().Should().NotBeNull();
    }

    [Fact]
    public void AddRsGeSpecInvoiceServices_WithConfiguration_BindsFromSection()
    {
        var services = new ServiceCollection();
        var configData = new Dictionary<string, string?>
        {
            ["RsGe:SpecInvoice:ServiceUser"] = "myuser",
            ["RsGe:SpecInvoice:ServicePassword"] = "mypass",
            ["RsGe:SpecInvoice:UserId"] = "42"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        services.AddRsGeSpecInvoiceServices(configuration);

        var provider = services.BuildServiceProvider();
        var specConfig = provider.GetRequiredService<SpecInvoiceConfiguration>();
        specConfig.ServiceUser.Should().Be("myuser");
        specConfig.ServicePassword.Should().Be("mypass");
        specConfig.UserId.Should().Be(42);
    }

    [Fact]
    public void AddRsGeSpecInvoiceServices_WithAction_ConfiguresCorrectly()
    {
        var services = new ServiceCollection();

        services.AddRsGeSpecInvoiceServices(config =>
        {
            config.ServiceUser = "actionuser";
            config.ServicePassword = "actionpass";
            config.UserId = 99;
        });

        var provider = services.BuildServiceProvider();
        var specConfig = provider.GetRequiredService<SpecInvoiceConfiguration>();
        specConfig.ServiceUser.Should().Be("actionuser");
        specConfig.ServicePassword.Should().Be("actionpass");
        specConfig.UserId.Should().Be(99);
    }
}
