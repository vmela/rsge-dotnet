namespace RsGe.NET.Invoice.Soap.Models;

// Auth responses
public class CheckResponse
{
    public bool IsSuccess { get; set; }
    public int UserId { get; set; }
    public string Sua { get; set; } = string.Empty;
}

public class ServiceUserResponse
{
    public bool IsSuccess { get; set; }
    public int UserId { get; set; }
}

public class ServiceUserInfo
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class TinNotesInfo
{
    public string Tin { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

// Invoice responses
public class SaveInvoiceResponse
{
    public bool IsSuccess { get; set; }
    public long InvoiceId { get; set; }
}

public class InvoiceInfo
{
    public string FSeries { get; set; } = string.Empty;
    public string FNumber { get; set; } = string.Empty;
    public DateTime OperationDate { get; set; }
    public DateTime RegDate { get; set; }
    public int SellerUnId { get; set; }
    public int BuyerUnId { get; set; }
    public string OverheadNo { get; set; } = string.Empty;
    public DateTime OverheadDate { get; set; }
    public int Status { get; set; }
    public string SeqNumS { get; set; } = string.Empty;
    public string SeqNumB { get; set; } = string.Empty;
    public long KId { get; set; }
    public int RUnId { get; set; }
    public int KType { get; set; }
    public int BsUserId { get; set; }
    public int DecStatus { get; set; }
}

public class InvoiceDescriptionInfo
{
    public long Id { get; set; }
    public long InvoiceId { get; set; }
    public string Goods { get; set; } = string.Empty;
    public string GUnit { get; set; } = string.Empty;
    public decimal GNumber { get; set; }
    public decimal FullAmount { get; set; }
    public decimal DrgAmount { get; set; }
    public decimal AqciziAmount { get; set; }
    public int AkcizId { get; set; }
}

public class CorrectiveInvoiceResponse
{
    public bool IsSuccess { get; set; }
    public long KId { get; set; }
}

// Invoice search results
public class InvoiceSearchResult
{
    public long InvoiceId { get; set; }
    public string FSeries { get; set; } = string.Empty;
    public string FNumber { get; set; } = string.Empty;
    public DateTime OperationDate { get; set; }
    public DateTime RegDate { get; set; }
    public int SellerUnId { get; set; }
    public string SellerTin { get; set; } = string.Empty;
    public string SellerName { get; set; } = string.Empty;
    public int BuyerUnId { get; set; }
    public string BuyerTin { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public string OverheadNo { get; set; } = string.Empty;
    public DateTime OverheadDate { get; set; }
}

public class InvoiceFilterCount
{
    public bool IsSuccess { get; set; }
    public int Status0 { get; set; }
    public int Status1 { get; set; }
    public int Status2 { get; set; }
    public int Status3 { get; set; }
    public int Status4 { get; set; }
    public int Status5 { get; set; }
    public int Status6 { get; set; }
    public int Status7 { get; set; }
    public int Status8 { get; set; }
}

public class InvoiceChangeInfo
{
    public long InvoiceId { get; set; }
    public string ChangeType { get; set; } = string.Empty;
    public DateTime ChangeDate { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
}

// Invoice request responses
public class InvoiceRequestInfo
{
    public bool IsSuccess { get; set; }
    public int BuyerUnId { get; set; }
    public int SellerUnId { get; set; }
    public string OverheadNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class InvoiceRequestItem
{
    public long Id { get; set; }
    public long InvoiceId { get; set; }
    public int BuyerUnId { get; set; }
    public int SellerUnId { get; set; }
    public string OverheadNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Notes { get; set; } = string.Empty;
}

// NTOS numbers
public class NtosInvoiceNumber
{
    public long Id { get; set; }
    public long InvoiceId { get; set; }
    public string OverheadNo { get; set; } = string.Empty;
    public DateTime OverheadDate { get; set; }
}

// Lookup responses
public class UnIdLookupResponse
{
    public int UnId { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TinLookupResponse
{
    public string Tin { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

// Declaration responses
public class DeclarationDateResponse
{
    public string Result { get; set; } = string.Empty;
}

public class AkcizInfo
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class SeqNumInfo
{
    public int SeqNum { get; set; }
    public string Description { get; set; } = string.Empty;
}

// Print invoice response
public class PrintInvoiceResponse
{
    public string Content { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
}
