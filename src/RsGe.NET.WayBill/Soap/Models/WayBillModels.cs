namespace RsGe.NET.WayBill.Soap.Models;

/// <summary>
/// Waybill item/goods information
/// </summary>
public class WayBillGood
{
    public int Id { get; set; }
    public string WCode { get; set; } = string.Empty;
    public string BarCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Amount { get; set; }
    public string VatType { get; set; } = string.Empty;
    public int? ExciseId { get; set; }
    public List<ExciseStamp>? ExciseStamps { get; set; }
}

/// <summary>
/// Waybill header information
/// </summary>
public class WayBillHeader
{
    public int Id { get; set; }
    public int Type { get; set; }
    public DateTime CreateDate { get; set; }
    public string BuyerTin { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public string SellerTin { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public string StartAddress { get; set; } = string.Empty;
    public string EndAddress { get; set; } = string.Empty;
    public int TransportTypeId { get; set; }
    public string TransportTypeName { get; set; } = string.Empty;
    public string DriverTin { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string CarNumber { get; set; } = string.Empty;
    public string TrailerNumber { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int Status { get; set; }
    public DateTime? ActivateDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int? ParentId { get; set; }
    public string FullAmount { get; set; } = string.Empty;
    public bool IsConfirmed { get; set; }
    public bool IsReturned { get; set; }
    public bool IsCorrected { get; set; }
    public string WaybillNumber { get; set; } = string.Empty;
}

/// <summary>
/// Complete waybill with header and goods
/// </summary>
public class WayBillDocument
{
    public WayBillHeader Header { get; set; } = new();
    public List<WayBillGood> Goods { get; set; } = new();
}

/// <summary>
/// Waybill summary for list operations
/// </summary>
public class WayBillSummary
{
    public int Id { get; set; }
    public string WaybillNumber { get; set; } = string.Empty;
    public int Type { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public DateTime CreateDate { get; set; }
    public string BuyerTin { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public string SellerTin { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime? ActivateDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
}
