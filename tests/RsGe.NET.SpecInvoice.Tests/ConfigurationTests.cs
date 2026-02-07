using FluentAssertions;

namespace RsGe.NET.SpecInvoice.Tests;

public class ConfigurationTests
{
    [Fact]
    public void SpecInvoiceConfiguration_IsValid_WhenAllFieldsSet()
    {
        var config = new SpecInvoiceConfiguration
        {
            ServiceUser = "user",
            ServicePassword = "pass",
            UserId = 1
        };

        config.IsValid.Should().BeTrue();
    }

    [Fact]
    public void SpecInvoiceConfiguration_IsNotValid_WhenMissingServiceUser()
    {
        var config = new SpecInvoiceConfiguration
        {
            ServicePassword = "pass",
            UserId = 1
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SpecInvoiceConfiguration_IsNotValid_WhenMissingServicePassword()
    {
        var config = new SpecInvoiceConfiguration
        {
            ServiceUser = "user",
            UserId = 1
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SpecInvoiceConfiguration_IsNotValid_WhenUserIdIsZero()
    {
        var config = new SpecInvoiceConfiguration
        {
            ServiceUser = "user",
            ServicePassword = "pass",
            UserId = 0
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SpecInvoiceConfiguration_IsNotValid_WhenUserIdIsNegative()
    {
        var config = new SpecInvoiceConfiguration
        {
            ServiceUser = "user",
            ServicePassword = "pass",
            UserId = -1
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void SpecInvoiceConfiguration_DefaultUserId_IsZero()
    {
        var config = new SpecInvoiceConfiguration();

        config.UserId.Should().Be(0);
    }
}
