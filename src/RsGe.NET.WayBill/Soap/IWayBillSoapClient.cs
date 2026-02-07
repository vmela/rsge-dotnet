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
    Task<List<AkcizCode>> GetAkcizCodesAsync(string serviceUser, string servicePassword, string searchText = "", CancellationToken cancellationToken = default);
    Task<List<WoodType>> GetWoodTypesAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);
    Task<DateTime> GetServerTimeAsync(CancellationToken cancellationToken = default);

    // Bar code methods
    Task<BarCode?> GetBarCodeAsync(string serviceUser, string servicePassword, string barCode, CancellationToken cancellationToken = default);
    Task<bool> SaveBarCodeAsync(string serviceUser, string servicePassword, string barCode, string productName, int unitId, CancellationToken cancellationToken = default);
    Task<bool> DeleteBarCodeAsync(string serviceUser, string servicePassword, string barCode, CancellationToken cancellationToken = default);

    // TIN / Payer lookup
    Task<TinInfoResponse?> GetNameFromTinAsync(string serviceUser, string servicePassword, string tin, CancellationToken cancellationToken = default);
    Task<string> GetTinFromUnIdAsync(string serviceUser, string servicePassword, int unId, CancellationToken cancellationToken = default);
    Task<string> GetPayerTypeFromUnIdAsync(string serviceUser, string servicePassword, int unId, CancellationToken cancellationToken = default);
    Task<bool> IsVatPayerAsync(string serviceUser, string servicePassword, int unId, CancellationToken cancellationToken = default);
    Task<bool> IsVatPayerByTinAsync(string serviceUser, string servicePassword, string tin, CancellationToken cancellationToken = default);

    // Waybill CRUD operations
    Task<SaveWayBillResponse> SaveWayBillAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default);
    Task<WayBillDocument?> GetWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillDocument?> GetWayBillByNumberAsync(string serviceUser, string servicePassword, string waybillNumber, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetWayBillsAsync(string serviceUser, string servicePassword, GetWayBillsRequest request, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetWayBillsExAsync(string serviceUser, string servicePassword, GetWayBillsExRequest request, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetWayBillsV1Async(string serviceUser, string servicePassword, string? buyerTin, DateTime? lastUpdateFrom, DateTime? lastUpdateTo, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetBuyerWayBillsAsync(string serviceUser, string servicePassword, GetBuyerWayBillsRequest request, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetBuyerWayBillsExAsync(string serviceUser, string servicePassword, GetWayBillsExRequest request, CancellationToken cancellationToken = default);
    Task<List<WayBillGood>> GetWayBillGoodsListAsync(string serviceUser, string servicePassword, GetWayBillsExRequest request, CancellationToken cancellationToken = default);
    Task<List<WayBillGood>> GetBuyerWayBillGoodsListAsync(string serviceUser, string servicePassword, GetWayBillsExRequest request, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetCompletedWayBillsByDateAsync(string serviceUser, string servicePassword, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetAdjustedWayBillsAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillDocument?> GetAdjustedWayBillAsync(string serviceUser, string servicePassword, int id, CancellationToken cancellationToken = default);
    Task<byte[]> GetPrintPdfAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);

    // Waybill workflow operations
    Task<WayBillOperationResponse> SendWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> SendWayBillVdAsync(string serviceUser, string servicePassword, int waybillId, DateTime beginDate, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> ConfirmWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> RejectWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> CloseWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> CloseWayBillVdAsync(string serviceUser, string servicePassword, int waybillId, DateTime deliveryDate, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> DeleteWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> RefWayBillAsync(string serviceUser, string servicePassword, int waybillId, int? refWaybillId = null, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> RefWayBillVdAsync(string serviceUser, string servicePassword, int waybillId, string? comment = null, CancellationToken cancellationToken = default);

    // Transporter operations
    Task<bool> SaveWayBillTransporterAsync(string serviceUser, string servicePassword, SaveWayBillTransporterRequest request, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> SendWayBillTransporterAsync(string serviceUser, string servicePassword, int waybillId, DateTime beginDate, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> CloseWayBillTransporterAsync(string serviceUser, string servicePassword, CloseWayBillTransporterRequest request, CancellationToken cancellationToken = default);
    Task<GetWayBillsResponse> GetTransporterWayBillsAsync(string serviceUser, string servicePassword, GetTransporterWayBillsRequest request, CancellationToken cancellationToken = default);

    // Sub-waybill operations
    Task<SaveWayBillResponse> SaveSubWayBillAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> ActivateSubWayBillAsync(string serviceUser, string servicePassword, ActivateSubWayBillRequest request, CancellationToken cancellationToken = default);
    Task<WayBillOperationResponse> CloseSubWayBillAsync(string serviceUser, string servicePassword, int subWaybillId, CancellationToken cancellationToken = default);

    // Template operations
    Task<bool> SaveWayBillTemplateAsync(string serviceUser, string servicePassword, string templateName, SaveWayBillRequest waybill, CancellationToken cancellationToken = default);
    Task<List<WayBillTemplate>> GetWayBillTemplatesAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default);
    Task<WayBillTemplate?> GetWayBillTemplateAsync(string serviceUser, string servicePassword, int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteWayBillTemplateAsync(string serviceUser, string servicePassword, int id, CancellationToken cancellationToken = default);

    // Invoice operations
    Task<bool> SaveInvoiceAsync(string serviceUser, string servicePassword, SaveInvoiceRequest request, CancellationToken cancellationToken = default);

    // Vehicle management
    Task<int> GetCarNumbersAsync(string serviceUser, string servicePassword, string carNumber, CancellationToken cancellationToken = default);
    Task<bool> SaveCarNumberAsync(string serviceUser, string servicePassword, string carNumber, CancellationToken cancellationToken = default);
    Task<bool> DeleteCarNumberAsync(string serviceUser, string servicePassword, string carNumber, CancellationToken cancellationToken = default);

    // Additional operations
    Task<bool> SyncWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default);

    // Debug operation
    Task<(string Request, string Response, SaveWayBillResponse ParsedResponse)> SaveWayBillDebugAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default);
}
