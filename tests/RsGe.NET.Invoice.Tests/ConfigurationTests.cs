using FluentAssertions;

namespace RsGe.NET.Invoice.Tests;

public class ConfigurationTests
{
    [Fact]
    public void InvoiceConfiguration_IsValid_WhenAllFieldsSet()
    {
        var config = new InvoiceConfiguration
        {
            ServiceUser = "user",
            ServicePassword = "pass",
            UserId = 1
        };

        config.IsValid.Should().BeTrue();
    }

    [Fact]
    public void InvoiceConfiguration_IsNotValid_WhenMissingServiceUser()
    {
        var config = new InvoiceConfiguration
        {
            ServicePassword = "pass",
            UserId = 1
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void InvoiceConfiguration_IsNotValid_WhenMissingServicePassword()
    {
        var config = new InvoiceConfiguration
        {
            ServiceUser = "user",
            UserId = 1
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void InvoiceConfiguration_IsNotValid_WhenUserIdIsZero()
    {
        var config = new InvoiceConfiguration
        {
            ServiceUser = "user",
            ServicePassword = "pass",
            UserId = 0
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void InvoiceConfiguration_IsNotValid_WhenUserIdIsNegative()
    {
        var config = new InvoiceConfiguration
        {
            ServiceUser = "user",
            ServicePassword = "pass",
            UserId = -1
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void InvoiceConfiguration_DefaultUserId_IsZero()
    {
        var config = new InvoiceConfiguration();

        config.UserId.Should().Be(0);
    }
}
