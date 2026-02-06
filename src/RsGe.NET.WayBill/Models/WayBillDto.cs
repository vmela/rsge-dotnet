namespace RsGe.NET.WayBill.Models;

public enum WayBillTypeEnum
{
    Inner = 1,
    Sale = 2,
    SaleNoTransport = 3,
    Distribution = 4,
    Return = 5
}

public enum TransportTypeEnum
{
    Vehicle = 1,
    Railway = 2,
    Air = 3,
    Other = 4
}

public enum WayBillStatusEnum
{
    Draft = 0,
    Active = 1,
    Completed = 2,
    Cancelled = -1,
    Rejected = -2
}

public enum VatTypeEnum
{
    Vat = 0,
    ZeroVat = 1,
    NoVat = 2
}

public class WayBillDto
{
    public int Id { get; set; }
    public string WaybillNumber { get; set; } = string.Empty;
    public WayBillTypeEnum Type { get; set; }
    public WayBillStatusEnum Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ActivatedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    public string SellerTin { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public string StartAddress { get; set; } = string.Empty;
    public string BuyerTin { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public string EndAddress { get; set; } = string.Empty;
    public TransportTypeEnum TransportType { get; set; }
    public string DriverTin { get; set; } = string.Empty;
    public string DriverName { get; set; } = string.Empty;
    public string CarNumber { get; set; } = string.Empty;
    public string TrailerNumber { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int? ParentWaybillId { get; set; }
    public List<WayBillItemDto> Items { get; set; } = new();
}

public class WayBillItemDto
{
    public int Id { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string BarCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int UnitId { get; set; }
    public string UnitName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public VatTypeEnum VatType { get; set; }
}

public class WayBillListItemDto
{
    public int Id { get; set; }
    public string WaybillNumber { get; set; } = string.Empty;
    public WayBillTypeEnum Type { get; set; }
    public WayBillStatusEnum Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string BuyerTin { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public string SellerTin { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
}

public class CompanyInfoDto
{
    public string Tin { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsValid { get; set; }
}

public class ReferenceItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
