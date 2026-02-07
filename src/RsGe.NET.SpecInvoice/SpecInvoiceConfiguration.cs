using RsGe.NET.Core;

namespace RsGe.NET.SpecInvoice;

/// <summary>
/// Configuration for RS.GE SpecInvoices service.
/// SpecInvoicesService uses su/sp + user_id authentication pattern.
/// </summary>
public class SpecInvoiceConfiguration : RsGeConfiguration
{
    /// <summary>
    /// User ID for SpecInvoices service operations
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Validate the configuration
    /// </summary>
    public new bool IsValid => base.IsValid && UserId > 0;
}
