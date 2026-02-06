namespace RsGe.NET.WayBill.Soap.Models;

public class SoapResponse<T>
{
    public bool IsSuccess { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public T? Data { get; set; }
}

public class SaveWayBillResponse
{
    public int Id { get; set; }
    public string WaybillNumber { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
}

public class GetWayBillsResponse
{
    public List<WayBillSummary> WayBills { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class TinInfoResponse
{
    public string Tin { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsValid { get; set; }
}

public class ServiceUserResponse
{
    public string UserName { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class WayBillOperationResponse
{
    public int WaybillId { get; set; }
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class ErrorCode
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public class ServiceUserInfo
{
    public string PayerId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string RawResponse { get; set; } = string.Empty;
}

public class ServiceUser
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UnId { get; set; } = string.Empty;
    public string Ip { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}
