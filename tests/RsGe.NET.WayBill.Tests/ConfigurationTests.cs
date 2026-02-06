using RsGe.NET.Core;
using FluentAssertions;

namespace RsGe.NET.WayBill.Tests;

public class ConfigurationTests
{
    [Fact]
    public void RsGeConfiguration_IsValid_WhenCredentialsSet()
    {
        var config = new RsGeConfiguration
        {
            ServiceUser = "user",
            ServicePassword = "pass"
        };

        config.IsValid.Should().BeTrue();
    }

    [Fact]
    public void RsGeConfiguration_IsNotValid_WhenMissingUser()
    {
        var config = new RsGeConfiguration
        {
            ServicePassword = "pass"
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void RsGeConfiguration_IsNotValid_WhenMissingPassword()
    {
        var config = new RsGeConfiguration
        {
            ServiceUser = "user"
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void RsGeConfiguration_DefaultServiceUrl_IsNull()
    {
        var config = new RsGeConfiguration();
        config.ServiceUrl.Should().BeNull();
    }

    [Fact]
    public void WayBillConfiguration_ExtendsBaseConfig()
    {
        var config = new WayBillConfiguration
        {
            ServiceUser = "user",
            ServicePassword = "pass",
            CompanyTin = "123456789",
            DefaultStartAddress = "Tbilisi"
        };

        config.IsValid.Should().BeTrue();
        config.CompanyTin.Should().Be("123456789");
        config.DefaultStartAddress.Should().Be("Tbilisi");
    }

    [Fact]
    public void RsGeConfiguration_SectionName_IsRsGe()
    {
        RsGeConfiguration.SectionName.Should().Be("RsGe");
    }
}
