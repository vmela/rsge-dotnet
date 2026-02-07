using System.Data;
using RsGe.NET.SpecInvoice.Soap.Models;

namespace RsGe.NET.SpecInvoice.Services;

/// <summary>
/// High-level .NET-friendly interface for RS.GE SpecInvoice operations (სპეციალური ინვოისები)
/// </summary>
public interface IRsGeSpecInvoiceService
{
    #region Auth

    /// <summary>
    /// Check in to the SpecInvoices service
    /// </summary>
    Task<CheckInResult> CheckInAsync(string logText, CancellationToken cancellationToken = default);

    #endregion

    #region Invoice Operations

    /// <summary>
    /// Save invoice (version B with all parameters)
    /// </summary>
    Task<SaveInvoiceResult> SaveInvoiceAsync(
        int invoiceId, string fSeries, string operationDt, int sellerUnId, int buyerUnId,
        string ssdN, string ssafN, string calcDate, string kSsafN, string trStDate,
        string oilStAddress, string oilStN, string oilFnAddress, string oilFnN,
        string transportType, string transportMark, string driverInfo, string carrierInfo,
        string carrieSNo, int pUserId, int sUserId, int bSUserId, string ssdDate, string ssafDate,
        string payType, string sellerPhone, string buyerPhone, string driverNo,
        string ssafAltNumber, string ssafAltType, string ssdAltNumber, string ssdAltType,
        string ssafAltStatus, string ssdAltStatus, int driverIsGeo,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get invoice by ID
    /// </summary>
    Task<DataSet?> GetInvoiceAsync(int invoiceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get seller invoices
    /// </summary>
    Task<DataSet?> GetSellerInvoicesAsync(
        int unId, string startDate, string endDate, string opStartDate, string opEndDate,
        string invoiceNo, string saIdentNo, string description, string docMosNom,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get buyer invoices
    /// </summary>
    Task<DataSet?> GetBuyerInvoicesAsync(
        int unId, string startDate, string endDate, string opStartDate, string opEndDate,
        string invoiceNo, string saIdentNo, string description, string docMosNom,
        CancellationToken cancellationToken = default);

    #endregion

    #region Status Operations

    /// <summary>
    /// Change invoice status
    /// </summary>
    Task<ChangeStatusResult> ChangeStatusAsync(int invoiceId, int status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Accept invoice
    /// </summary>
    Task<ChangeStatusResult> AcceptInvoiceAsync(int invoiceId, int status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reject invoice with reason
    /// </summary>
    Task<ChangeStatusResult> RejectInvoiceAsync(int invoiceId, string reason, CancellationToken cancellationToken = default);

    #endregion

    #region Special Products

    /// <summary>
    /// Get list of special products
    /// </summary>
    Task<DataSet?> GetSpecProductsAsync(CancellationToken cancellationToken = default);

    #endregion

    #region SSAF/SSD Operations

    /// <summary>
    /// Add SSAF to invoice
    /// </summary>
    Task<AddSpecInvoicesSsafResult> AddSsafAsync(int invoiceId, string ssafNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove SSAF from invoice
    /// </summary>
    Task<DelSpecInvoicesSsafResult> RemoveSsafAsync(int invoiceId, string ssafNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add SSD to invoice
    /// </summary>
    Task<AddSpecInvoicesSsdResult> AddSsdAsync(int invoiceId, string ssdNumber, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove SSD from invoice
    /// </summary>
    Task<DelSpecInvoicesSsdResult> RemoveSsdAsync(int invoiceId, string ssdNumber, CancellationToken cancellationToken = default);

    #endregion

    #region Transport Operations

    /// <summary>
    /// Start transport for invoice
    /// </summary>
    Task<StartTransportResult> StartTransportAsync(int invoiceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Correct driver information
    /// </summary>
    Task<CorrectDriverInfoResult> CorrectDriverInfoAsync(
        int id, int sellerUnId, string driverInfo, string driverNo, int driverIsGeo,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Correct transport mark (vehicle number)
    /// </summary>
    Task<CorrectTransportMarkResult> CorrectTransportMarkAsync(
        int id, int sellerUnId, string transportMark,
        CancellationToken cancellationToken = default);

    #endregion

    #region Organization

    /// <summary>
    /// Get organization objects by UN ID
    /// </summary>
    Task<DataSet?> GetOrganizationObjectsAsync(int unId, CancellationToken cancellationToken = default);

    #endregion
}
