using System.Data;
using RsGe.NET.SpecInvoice.Soap.Models;

namespace RsGe.NET.SpecInvoice.Soap;

/// <summary>
/// SOAP client interface for RS.GE SpecInvoices Service.
/// Uses su/sp + user_id authentication pattern.
/// Endpoint: https://www.revenue.mof.ge/SpecInvoicesService/SpecInvoicesService.asmx
/// </summary>
public interface ISpecInvoiceSoapClient
{
    #region Auth (2)

    /// <summary>
    /// Check in to the SpecInvoices service
    /// </summary>
    Task<CheckInResult> CheckInAsync(string su, string sp, int userId, string logText, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check special users validation
    /// </summary>
    Task<CheckSpecUsersResult> CheckSpecUsersAsync(string su, string sp, int userId, string logText, CancellationToken cancellationToken = default);

    #endregion

    #region Invoice CRUD (6)

    /// <summary>
    /// Save invoice (original version)
    /// </summary>
    Task<SaveInvoiceResult> SaveInvoiceAsync(
        int invoiceId, string fSeries, string operationDt, int sellerUnId, int buyerUnId,
        string ssdN, string ssafN, string calcDate, string kSsafN, string trStDate,
        string oilStAddress, string oilStN, string oilFnAddress, string oilFnN,
        string transportType, string transportMark, string driverInfo, string carrierInfo,
        string carrieSNo, int pUserId, int sUserId, int bSUserId, string ssdDate, string ssafDate,
        string payType, string sellerPhone, string buyerPhone, string driverNo,
        string ssafAltNumber, string ssafAltType, string ssdAltNumber, string ssdAltType,
        string ssafAltStatus, string ssdAltStatus, int userId, string su, string sp,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Save invoice (version B with driver_is_geo flag)
    /// </summary>
    Task<SaveInvoiceResult> SaveInvoiceBAsync(
        int invoiceId, string fSeries, string operationDt, int sellerUnId, int buyerUnId,
        string ssdN, string ssafN, string calcDate, string kSsafN, string trStDate,
        string oilStAddress, string oilStN, string oilFnAddress, string oilFnN,
        string transportType, string transportMark, string driverInfo, string carrierInfo,
        string carrieSNo, int pUserId, int sUserId, int bSUserId, string ssdDate, string ssafDate,
        string payType, string sellerPhone, string buyerPhone, string driverNo,
        string ssafAltNumber, string ssafAltType, string ssdAltNumber, string ssdAltType,
        string ssafAltStatus, string ssdAltStatus, int driverIsGeo, int userId, string su, string sp,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get invoice by ID
    /// </summary>
    Task<DataSet?> GetInvoiceAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get invoice details (version D)
    /// </summary>
    Task<DataSet?> GetInvoiceDAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// K invoice operation
    /// </summary>
    Task<DataSet?> KInvoiceAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Print invoices
    /// </summary>
    Task<DataSet?> PrintInvoicesAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default);

    #endregion

    #region Descriptions (3)

    /// <summary>
    /// Save invoice description/line item
    /// </summary>
    Task<SaveInvoiceDescResult> SaveInvoiceDescAsync(
        int userId, int id, string su, string sp, int invId, string goods, string gUnit,
        decimal gNumber, decimal unPrice, decimal drgAmount, decimal aqciziAmount, int pUserId,
        int aqciziId, string aqciziRate, string dggRate, string gNumberAlt, string goodId, string drgType,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get invoice descriptions/line items
    /// </summary>
    Task<DataSet?> GetInvoiceDescAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete invoice description/line item
    /// </summary>
    Task<DeleteInvoiceDescResult> DeleteInvoiceDescAsync(int userId, int id, int invId, string su, string sp, CancellationToken cancellationToken = default);

    #endregion

    #region Status (5)

    /// <summary>
    /// Change invoice status
    /// </summary>
    Task<ChangeStatusResult> ChangeInvoiceStatusAsync(int userId, int invId, int status, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Accept invoice status
    /// </summary>
    Task<ChangeStatusResult> AcceptInvoiceStatusAsync(int userId, int invId, int status, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Refuse invoice with reason
    /// </summary>
    Task<ChangeStatusResult> RefuseInvoiceStatusAsync(int userId, int invId, string refText, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Save invoice request
    /// </summary>
    Task<ChangeStatusResult> SaveInvoiceRequestAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancellation reason (gauqmebis_mizezi)
    /// </summary>
    Task<CancellationReasonResult> GauqmebisMizeziAsync(int userId, int invId, string reason, string su, string sp, CancellationToken cancellationToken = default);

    #endregion

    #region Search (8)

    /// <summary>
    /// Get seller invoices
    /// </summary>
    Task<DataSet?> GetSellerInvoicesAsync(
        int userId, int unId, string sDt, string eDt, string opSDt, string opEDt,
        string invoiceNo, string saIdentNo, string desc, string docMosNom,
        string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get buyer invoices
    /// </summary>
    Task<DataSet?> GetBuyerInvoicesAsync(
        int userId, int unId, string sDt, string eDt, string opSDt, string opEDt,
        string invoiceNo, string saIdentNo, string desc, string docMosNom,
        string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get seller invoices by status
    /// </summary>
    Task<DataSet?> GetSellerInvoicesRAsync(int userId, int unId, int status, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get buyer invoices by status
    /// </summary>
    Task<DataSet?> GetBuyerInvoicesRAsync(int userId, int unId, int status, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get seller filter count by status
    /// </summary>
    Task<InvoiceFilterCount> GetSellerFilterCountAsync(int unId, int userId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get buyer filter count by status
    /// </summary>
    Task<InvoiceFilterCount> GetBuyerFilterCountAsync(int unId, int userId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get invoice numbers
    /// </summary>
    Task<DataSet?> GetInvoiceNumbersAsync(int userId, int unId, string vInvoiceN, int vCount, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get invoice TINs
    /// </summary>
    Task<DataSet?> GetInvoiceTinsAsync(int userId, int unId, string vInvoiceT, int vCount, string su, string sp, CancellationToken cancellationToken = default);

    #endregion

    #region Special Products (4)

    /// <summary>
    /// Get special products list
    /// </summary>
    Task<DataSet?> GetSpecProductsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get special product by ID
    /// </summary>
    Task<DataSet?> GetSpecProductByIdAsync(int productId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get special SSAF list
    /// </summary>
    Task<DataSet?> GetSpecSsafsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get special SSD list
    /// </summary>
    Task<DataSet?> GetSpecSsdsAsync(CancellationToken cancellationToken = default);

    #endregion

    #region SSAF/SSD (4)

    /// <summary>
    /// Add SSAF to special invoices
    /// </summary>
    Task<AddSpecInvoicesSsafResult> AddSpecInvoicesSsafAsync(int userId, int invId, string ssafN, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove SSAF from special invoices
    /// </summary>
    Task<DelSpecInvoicesSsafResult> DelSpecInvoicesSsafAsync(int userId, int invId, string ssafN, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add SSD to special invoices
    /// </summary>
    Task<AddSpecInvoicesSsdResult> AddSpecInvoicesSsdAsync(int userId, int invId, string ssdN, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove SSD from special invoices
    /// </summary>
    Task<DelSpecInvoicesSsdResult> DelSpecInvoicesSsdAsync(int userId, int invId, string ssdN, string su, string sp, CancellationToken cancellationToken = default);

    #endregion

    #region Transport (4)

    /// <summary>
    /// Start transport (original version)
    /// </summary>
    Task<StartTransportResult> StartTransportAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Start transport (new version)
    /// </summary>
    Task<StartTransportResult> StartTransportNewAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Correct driver information
    /// </summary>
    Task<CorrectDriverInfoResult> CorrectDriverInfoAsync(
        int id, int sellerUnId, string driverInfo, string driverNo, int driverIsGeo,
        int userId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Correct transport mark
    /// </summary>
    Task<CorrectTransportMarkResult> CorrectTransportMarkAsync(
        int id, int sellerUnId, string transportMark, int userId, string su, string sp,
        CancellationToken cancellationToken = default);

    #endregion

    #region Organization (3)

    /// <summary>
    /// Get RS organization objects
    /// </summary>
    Task<DataSet?> GetRsOrgObjectsAsync(int userId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get organization objects by UN ID
    /// </summary>
    Task<DataSet?> GetVOrgObjectsByUnIdAsync(int userId, int unId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get organization object address
    /// </summary>
    Task<DataSet?> GetVOrgObjectAddressAsync(int userId, int objectId, string su, string sp, CancellationToken cancellationToken = default);

    #endregion

    #region Other (3)

    /// <summary>
    /// Get makoreqtirebeli (corrector)
    /// </summary>
    Task<DataSet?> GetMakoreqtirebeliAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add invoice to declaration
    /// </summary>
    Task<ChangeStatusResult> AddInvToDeclAsync(int userId, int invId, string declId, string su, string sp, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get piradi nomeri (personal number)
    /// </summary>
    Task<DataSet?> GetPiradiNomeriAsync(int userId, string personalNo, string su, string sp, CancellationToken cancellationToken = default);

    #endregion
}
