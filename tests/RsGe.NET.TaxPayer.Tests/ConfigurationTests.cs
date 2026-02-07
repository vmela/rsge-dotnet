using FluentAssertions;

namespace RsGe.NET.TaxPayer.Tests;

public class ConfigurationTests
{
    [Fact]
    public void TaxPayerConfiguration_IsValid_WhenUsernameAndPasswordSet()
    {
        var config = new TaxPayerConfiguration
        {
            Username = "user",
            Password = "pass"
        };

        config.IsValid.Should().BeTrue();
    }

    [Fact]
    public void TaxPayerConfiguration_IsNotValid_WhenMissingUsername()
    {
        var config = new TaxPayerConfiguration
        {
            Password = "pass"
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void TaxPayerConfiguration_IsNotValid_WhenMissingPassword()
    {
        var config = new TaxPayerConfiguration
        {
            Username = "user"
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void TaxPayerConfiguration_IsNotValid_WhenBothEmpty()
    {
        var config = new TaxPayerConfiguration();

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void TaxPayerConfiguration_IsNotValid_WhenWhitespaceOnly()
    {
        var config = new TaxPayerConfiguration
        {
            Username = "  ",
            Password = "  "
        };

        config.IsValid.Should().BeFalse();
    }

    [Fact]
    public void TaxPayerConfiguration_DefaultUsernameAndPassword_AreEmpty()
    {
        var config = new TaxPayerConfiguration();

        config.Username.Should().BeEmpty();
        config.Password.Should().BeEmpty();
    }
}
