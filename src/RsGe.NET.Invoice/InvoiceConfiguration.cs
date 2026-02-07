using RsGe.NET.Core;

namespace RsGe.NET.Invoice;

/// <summary>
/// Configuration for RS.GE Invoice (NTOS) service.
/// NtosService uses service user credentials (su/sp) and requires a user_id obtained from the chek method.
/// </summary>
public class InvoiceConfiguration : RsGeConfiguration
{
    /// <summary>
    /// User ID obtained from the chek method (required for most invoice operations)
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Validate the configuration
    /// </summary>
    public new bool IsValid => base.IsValid && UserId > 0;
}
