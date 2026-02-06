using RsGe.NET.WayBill.Soap.Models;

namespace RsGe.NET.WayBill.Soap;

/// <summary>
/// SOAP client interface for RS.GE WayBill Service.
/// Methods map directly to the SOAP service operations.
/// </summary>
public interface IWayBillSoapClient
{
    // Service utility methods
    Task<string> WhatIsMyIpAsync(CancellationToken cancellationToken = default);
    Task<bool> CheckServiceUserAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);
    Task<ServiceUserInfo?> GetServiceUserInfoAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);
    Task<List<ServiceUser>> GetServiceUsersAsync(string userName, string userPassword, CancellationToken cancellationToken = default);
    Task<string> CheckServiceUserRawAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);
    Task<bool> CreateServiceUserAsync(string serviceUser, string servicePassword, string ip, string name, string tin, CancellationToken cancellationToken = default);
    Task<bool> UpdateServiceUserAsync(string serviceUser, string servicePassword, string ip, string name, CancellationToken cancellationToken = default);

    // Reference data methods
    Task<List<WayBillType>> GetWayBillTypesAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);
    Task<List<WayBillUnit>> GetWayBillUnitsAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);
    Task<List<TransportType>> GetTransportTypesAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);
    Task<List<ErrorCode>> GetErrorCodesAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);

    // Bar code methods
    Task<BarCode?> GetBarCodeAsync(string serviceUser, string servicePassword, string barCode, CancellationToken cancellationToken = default);
    Task<bool> SaveBarCodeAsync(string serviceUser, string servicePassword, string barCode, string productName, int unitId, CancellationToken cancellationToken = default);

    // TIN lookup
    Task<TinInfoResponse?> GetNameFromTinAsync(string serviceUser, string servicePassword, string tin, CancellationToken cancellationToken = default);

    // Waybill CRUD operations
    Task<SaveWayBillResponse> SaveWayBillAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default);
    Task<WayBillDocument?> GetWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetWayBillsAsync(string serviceUser, string servicePassword, GetWayBillsRequest request, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetBuyerWayBillsAsync(string serviceUser, string servicePassword, GetBuyerWayBillsRequest request, CancellationToken cancellationToken = default);

    // Waybill workflow operations
    Task<WayBillOperationResponse> SendWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> ConfirmWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> RejectWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> CloseWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> DeleteWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> RefWayBillAsync(string serviceUser, string servicePassword, int waybillId, int? refWaybillId = null, CancellationToken cancellationToken = default);

    // Sub-waybill operations
    Task<SaveWayBillResponse> SaveSubWayBillAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> ActivateSubWayBillAsync(string serviceUser, string servicePassword, ActivateSubWayBillRequest request, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> CloseSubWayBillAsync(string serviceUser, string servicePassword, int subWaybillId, CancellationToken cancellationToken = default);

    // Invoice operations
    Task<bool> SaveInvoiceAsync(string serviceUser, string servicePassword, SaveInvoiceRequest request, CancellationToken cancellationToken = default);

    // Additional operations
    Task<int> GetCarNumbersAsync(string serviceUser, string servicePassword, string carNumber, CancellationToken cancellationToken = default);
    Task<bool> SyncWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);

    // Debug operation
    Task<(string Request, string Response, SaveWayBillResponse ParsedResponse)> SaveWayBillDebugAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default);
}
