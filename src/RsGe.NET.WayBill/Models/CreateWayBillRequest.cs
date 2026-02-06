namespace RsGe.NET.WayBill.Models;

public class CreateWayBillRequest
{
    public WayBillTypeEnum Type { get; set; }
    public string SellerUnId { get; set; } = string.Empty;
    public string BuyerTin { get; set; } = string.Empty;
    public string? BuyerName { get; set; }
    public bool ValidateBuyerTin { get; set; } = true;
    public string StartAddress { get; set; } = string.Empty;
    public string EndAddress { get; set; } = string.Empty;
    public string DriverTin { get; set; } = string.Empty;
    public string? DriverName { get; set; }
    public bool ValidateDriverTin { get; set; } = true;
    public TransportTypeEnum TransportType { get; set; } = TransportTypeEnum.Vehicle;
    public string CarNumber { get; set; } = string.Empty;
    public string? TrailerNumber { get; set; }
    public string? Comment { get; set; }
    public List<CreateWayBillItemRequest> Items { get; set; } = new();
}

public class CreateWayBillItemRequest
{
    public string? ProductCode { get; set; }
    public string? BarCode { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int UnitId { get; set; }
    public string? UnitTxt { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public VatTypeEnum VatType { get; set; } = VatTypeEnum.Vat;
}

public class WayBillFilterOptions
{
    public WayBillTypeEnum? Type { get; set; }
    public WayBillStatusEnum? Status { get; set; }
    public string? BuyerTin { get; set; }
    public string? SellerTin { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}

public class CreateWayBillResult
{
    public bool IsSuccess { get; set; }
    public int WaybillId { get; set; }
    public string WaybillNumber { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}

public class WayBillOperationResult
{
    public bool IsSuccess { get; set; }
    public int WaybillId { get; set; }
    public string? ErrorMessage { get; set; }
}
