using RsGe.NET.TaxPayer.Services;
using RsGe.NET.TaxPayer.Soap;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RsGe.NET.TaxPayer.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRsGeTaxPayerServices_WithConfig_RegistersAllServices()
    {
        var services = new ServiceCollection();
        var config = new TaxPayerConfiguration
        {
            Username = "test",
            Password = "pass"
        };

        services.AddRsGeTaxPayerServices(config);

        var provider = services.BuildServiceProvider();

        provider.GetService<TaxPayerConfiguration>().Should().NotBeNull();
        provider.GetService<ITaxPayerSoapClient>().Should().NotBeNull();
        provider.GetService<IRsGeTaxPayerService>().Should().NotBeNull();
    }

    [Fact]
    public void AddRsGeTaxPayerServices_WithConfiguration_BindsFromSection()
    {
        var services = new ServiceCollection();
        var configData = new Dictionary<string, string?>
        {
            ["RsGe:TaxPayer:Username"] = "myuser",
            ["RsGe:TaxPayer:Password"] = "mypass"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configData)
            .Build();

        services.AddRsGeTaxPayerServices(configuration);

        var provider = services.BuildServiceProvider();
        var taxPayerConfig = provider.GetRequiredService<TaxPayerConfiguration>();
        taxPayerConfig.Username.Should().Be("myuser");
        taxPayerConfig.Password.Should().Be("mypass");
    }

    [Fact]
    public void AddRsGeTaxPayerServices_WithAction_ConfiguresCorrectly()
    {
        var services = new ServiceCollection();

        services.AddRsGeTaxPayerServices(config =>
        {
            config.Username = "actionuser";
            config.Password = "actionpass";
        });

        var provider = services.BuildServiceProvider();
        var taxPayerConfig = provider.GetRequiredService<TaxPayerConfiguration>();
        taxPayerConfig.Username.Should().Be("actionuser");
        taxPayerConfig.Password.Should().Be("actionpass");
    }
}
