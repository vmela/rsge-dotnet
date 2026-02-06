using RsGe.NET.Core;

namespace RsGe.NET.WayBill;

/// <summary>
/// Configuration for RS.GE WayBill service (extends base config with WayBill-specific fields)
/// </summary>
public class WayBillConfiguration : RsGeConfiguration
{
    /// <summary>
    /// Your company's TIN (Tax Identification Number)
    /// </summary>
    public string CompanyTin { get; set; } = string.Empty;

    /// <summary>
    /// Default warehouse/start address for outgoing waybills
    /// </summary>
    public string DefaultStartAddress { get; set; } = string.Empty;
}
