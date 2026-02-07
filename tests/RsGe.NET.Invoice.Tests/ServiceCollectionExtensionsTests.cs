using RsGe.NET.Invoice.Services;
using RsGe.NET.Invoice.Soap;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RsGe.NET.Invoice.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRsGeInvoiceServices_WithConfig_RegistersAllServices()
    {
        var services = new ServiceCollection();
        var config = new InvoiceConfiguration
        {
            ServiceUser = "test",
            ServicePassword = "pass",
            UserId = 1
        };

        services.AddRsGeInvoiceServices(config);

        var provider = services.BuildServiceProvider();

        provider.GetService<InvoiceConfiguration>().Should().NotBeNull();
        provider.GetService<IInvoiceSoapClient>().Should().NotBeNull();
        provider.GetService<IRsGeInvoiceService>().Should().NotBeNull();
    }

    [Fact]
    public void AddRsGeInvoiceServices_WithConfiguration_BindsFromSection()
    {
        var services = new ServiceCollection();
        var configData = new Dictionary<string, string?>
        {
            ["RsGe:Invoice:ServiceUser"] = "myuser",
            ["RsGe:Invoice:ServicePassword"] = "mypass",
            ["RsGe:Invoice:UserId"] = "42"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        services.AddRsGeInvoiceServices(configuration);

        var provider = services.BuildServiceProvider();
        var invoiceConfig = provider.GetRequiredService<InvoiceConfiguration>();
        invoiceConfig.ServiceUser.Should().Be("myuser");
        invoiceConfig.ServicePassword.Should().Be("mypass");
        invoiceConfig.UserId.Should().Be(42);
    }

    [Fact]
    public void AddRsGeInvoiceServices_WithAction_ConfiguresCorrectly()
    {
        var services = new ServiceCollection();

        services.AddRsGeInvoiceServices(config =>
        {
            config.ServiceUser = "actionuser";
            config.ServicePassword = "actionpass";
            config.UserId = 99;
        });

        var provider = services.BuildServiceProvider();
        var invoiceConfig = provider.GetRequiredService<InvoiceConfiguration>();
        invoiceConfig.ServiceUser.Should().Be("actionuser");
        invoiceConfig.ServicePassword.Should().Be("actionpass");
        invoiceConfig.UserId.Should().Be(99);
    }
}
