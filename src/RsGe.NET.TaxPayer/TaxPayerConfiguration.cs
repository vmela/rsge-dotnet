using RsGe.NET.Core;

namespace RsGe.NET.TaxPayer;

/// <summary>
/// Configuration for RS.GE TaxPayer service.
/// TaxPayerService uses portal credentials (Username/Password) instead of service user (su/sp).
/// </summary>
public class TaxPayerConfiguration : RsGeConfiguration
{
    /// <summary>
    /// RS.GE portal username (used instead of service user for TaxPayer operations)
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// RS.GE portal password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Validate the configuration
    /// </summary>
    public new bool IsValid => !string.IsNullOrWhiteSpace(Username)
                               && !string.IsNullOrWhiteSpace(Password);
}
