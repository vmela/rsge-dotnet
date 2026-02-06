using RsGe.NET.WayBill.Models;

namespace RsGe.NET.WayBill.Services;

/// <summary>
/// High-level .NET-friendly interface for RS.GE WayBill operations
/// </summary>
public interface IRsGeWayBillService
{
    Task<List<ReferenceItemDto>> GetWayBillTypesAsync(CancellationToken cancellationToken = default);
    Task<List<ReferenceItemDto>> GetUnitsAsync(CancellationToken cancellationToken = default);
    Task<List<ReferenceItemDto>> GetTransportTypesAsync(CancellationToken cancellationToken = default);
    Task<CompanyInfoDto?> GetCompanyByTinAsync(string tin, CancellationToken cancellationToken = default);
    Task<CreateWayBillResult> CreateWayBillAsync(CreateWayBillRequest request, CancellationToken cancellationToken = default);
    Task<WayBillDto?> GetWayBillAsync(int waybillId, CancellationToken cancellationToken = default);
    Task<List<WayBillListItemDto>> GetWayBillsAsync(WayBillFilterOptions? filter = null, CancellationToken cancellationToken = default);
    Task<List<WayBillListItemDto>> GetBuyerWayBillsAsync(WayBillFilterOptions? filter = null, CancellationToken cancellationToken = default);
    Task<WayBillOperationResult> SendWayBillAsync(int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResult> ConfirmWayBillAsync(int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResult> RejectWayBillAsync(int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResult> CloseWayBillAsync(int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResult> DeleteWayBillAsync(int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResult> ReferenceWayBillAsync(int waybillId, int referenceWaybillId, CancellationToken cancellationToken = default);
    Task<List<WayBillDto>> GetSalesAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<List<WayBillDto>> GetPurchasesAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<List<WayBillDto>> GetCompletedWayBillsAsync(DateTime since, CancellationToken cancellationToken = default);
    Task<List<WayBillListItemDto>> GetIncomingWaybillsAsync(DateTime from, DateTime to, int[]? statuses = null, CancellationToken cancellationToken = default);
}
