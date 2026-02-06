namespace RsGe.NET.Core;

/// <summary>
/// Base configuration for RS.GE services
/// </summary>
public class RsGeConfiguration
{
    /// <summary>
    /// Configuration section name for appsettings.json
    /// </summary>
    public const string SectionName = "RsGe";

    /// <summary>
    /// Service username provided by RS.GE
    /// </summary>
    public string ServiceUser { get; set; } = string.Empty;

    /// <summary>
    /// Service password provided by RS.GE
    /// </summary>
    public string ServicePassword { get; set; } = string.Empty;

    /// <summary>
    /// Optional: Service URL override (defaults to production)
    /// </summary>
    public string? ServiceUrl { get; set; }

    /// <summary>
    /// Validate the configuration
    /// </summary>
    public bool IsValid => !string.IsNullOrWhiteSpace(ServiceUser)
                           && !string.IsNullOrWhiteSpace(ServicePassword);
}
