namespace RsGe.NET.WayBill.Soap.Models;

/// <summary>
/// Request for saving a waybill
/// </summary>
public class SaveWayBillRequest
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string SellerUnId { get; set; } = string.Empty;
    public string BuyerTin { get; set; } = string.Empty;
    public int? CheckBuyerTin { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public string StartAddress { get; set; } = string.Empty;
    public string EndAddress { get; set; } = string.Empty;
    public string DriverTin { get; set; } = string.Empty;
    public int? CheckDriverTin { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public int TransportTypeId { get; set; }
    public string CarNumber { get; set; } = string.Empty;
    public string TrailerNumber { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int? ReceptionInfo { get; set; }
    public int? IsDistributed { get; set; }
    public int? IsTurned { get; set; }
    public int? SubWaybillId { get; set; }
    public int? ParentId { get; set; }
    public string? FullAmount { get; set; }
    public List<SaveWayBillGoodRequest> Goods { get; set; } = new();
}

public class SaveWayBillGoodRequest
{
    public int Id { get; set; }
    public string WCode { get; set; } = string.Empty;
    public string BarCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int UnitId { get; set; }
    public string? UnitTxt { get; set; }
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public string VatType { get; set; } = "VAT";
    public int? ExciseId { get; set; }
    public List<string>? AStamps { get; set; }
}

public class GetWayBillsRequest
{
    public int? WaybillType { get; set; }
    public DateTime? CreateDateFrom { get; set; }
    public DateTime? CreateDateTo { get; set; }
    public string? BuyerTin { get; set; }
    public string? SellerTin { get; set; }
    public int? Status { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

public class GetBuyerWayBillsRequest
{
    public DateTime? CreateDateFrom { get; set; }
    public DateTime? CreateDateTo { get; set; }
    public string? SellerTin { get; set; }
    public int? Status { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}

public class SaveInvoiceRequest
{
    public int WaybillId { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal InvoiceAmount { get; set; }
}

public class ActivateSubWayBillRequest
{
    public int Id { get; set; }
    public DateTime BeginDate { get; set; }
    public string? DeliveryAddress { get; set; }
}

public class GetWayBillsExRequest
{
    public string? BuyerTin { get; set; }
    public string? Statuses { get; set; }
    public string? CarNumber { get; set; }
    public DateTime? BeginDateFrom { get; set; }
    public DateTime? BeginDateTo { get; set; }
    public DateTime? CreateDateFrom { get; set; }
    public DateTime? CreateDateTo { get; set; }
    public string? DriverTin { get; set; }
    public DateTime? DeliveryDateFrom { get; set; }
    public DateTime? DeliveryDateTo { get; set; }
    public string? FullAmount { get; set; }
    public string? WaybillNumber { get; set; }
    public DateTime? CloseDateFrom { get; set; }
    public DateTime? CloseDateTo { get; set; }
    public string? SUserIds { get; set; }
    public string? Comment { get; set; }
}

public class GetTransporterWayBillsRequest
{
    public string? BuyerTin { get; set; }
    public string? Statuses { get; set; }
    public string? CarNumber { get; set; }
    public DateTime? BeginDateFrom { get; set; }
    public DateTime? BeginDateTo { get; set; }
    public DateTime? CreateDateFrom { get; set; }
    public DateTime? CreateDateTo { get; set; }
    public DateTime? DeliveryDateFrom { get; set; }
    public DateTime? DeliveryDateTo { get; set; }
    public DateTime? CloseDateFrom { get; set; }
    public DateTime? CloseDateTo { get; set; }
    public string? FullAmount { get; set; }
    public string? WaybillNumber { get; set; }
    public string? SUserIds { get; set; }
    public string? Comment { get; set; }
    public int? IsConfirmed { get; set; }
}

public class SaveWayBillTransporterRequest
{
    public int WaybillId { get; set; }
    public string CarNumber { get; set; } = string.Empty;
    public string DriverTin { get; set; } = string.Empty;
    public int CheckDriverTin { get; set; } = 1;
    public string DriverName { get; set; } = string.Empty;
    public int TransportTypeId { get; set; }
    public string? TransportTypeTxt { get; set; }
    public string? ReceptionInfo { get; set; }
    public string? ReceiverInfo { get; set; }
}

public class CloseWayBillTransporterRequest
{
    public int WaybillId { get; set; }
    public string? ReceptionInfo { get; set; }
    public string? ReceiverInfo { get; set; }
    public DateTime DeliveryDate { get; set; }
}
