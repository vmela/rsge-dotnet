using System.Data;
using RsGe.NET.SpecInvoice.Soap;
using RsGe.NET.SpecInvoice.Soap.Models;

namespace RsGe.NET.SpecInvoice.Services;

public class RsGeSpecInvoiceService : IRsGeSpecInvoiceService
{
    private readonly ISpecInvoiceSoapClient _soapClient;
    private readonly string _serviceUser;
    private readonly string _servicePassword;
    private readonly int _userId;

    public RsGeSpecInvoiceService(ISpecInvoiceSoapClient soapClient, string serviceUser, string servicePassword, int userId)
    {
        _soapClient = soapClient ?? throw new ArgumentNullException(nameof(soapClient));
        _serviceUser = serviceUser ?? throw new ArgumentNullException(nameof(serviceUser));
        _servicePassword = servicePassword ?? throw new ArgumentNullException(nameof(servicePassword));
        _userId = userId;
    }

    #region Auth

    public Task<CheckInResult> CheckInAsync(string logText, CancellationToken cancellationToken = default)
        => _soapClient.CheckInAsync(_serviceUser, _servicePassword, _userId, logText, cancellationToken);

    #endregion

    #region Invoice Operations

    public Task<SaveInvoiceResult> SaveInvoiceAsync(
        int invoiceId, string fSeries, string operationDt, int sellerUnId, int buyerUnId,
        string ssdN, string ssafN, string calcDate, string kSsafN, string trStDate,
        string oilStAddress, string oilStN, string oilFnAddress, string oilFnN,
        string transportType, string transportMark, string driverInfo, string carrierInfo,
        string carrieSNo, int pUserId, int sUserId, int bSUserId, string ssdDate, string ssafDate,
        string payType, string sellerPhone, string buyerPhone, string driverNo,
        string ssafAltNumber, string ssafAltType, string ssdAltNumber, string ssdAltType,
        string ssafAltStatus, string ssdAltStatus, int driverIsGeo,
        CancellationToken cancellationToken = default)
        => _soapClient.SaveInvoiceBAsync(
            invoiceId, fSeries, operationDt, sellerUnId, buyerUnId,
            ssdN, ssafN, calcDate, kSsafN, trStDate,
            oilStAddress, oilStN, oilFnAddress, oilFnN,
            transportType, transportMark, driverInfo, carrierInfo,
            carrieSNo, pUserId, sUserId, bSUserId, ssdDate, ssafDate,
            payType, sellerPhone, buyerPhone, driverNo,
            ssafAltNumber, ssafAltType, ssdAltNumber, ssdAltType,
            ssafAltStatus, ssdAltStatus, driverIsGeo, _userId, _serviceUser, _servicePassword,
            cancellationToken);

    public Task<DataSet?> GetInvoiceAsync(int invoiceId, CancellationToken cancellationToken = default)
        => _soapClient.GetInvoiceAsync(_userId, invoiceId, _serviceUser, _servicePassword, cancellationToken);

    public Task<DataSet?> GetSellerInvoicesAsync(
        int unId, string startDate, string endDate, string opStartDate, string opEndDate,
        string invoiceNo, string saIdentNo, string description, string docMosNom,
        CancellationToken cancellationToken = default)
        => _soapClient.GetSellerInvoicesAsync(
            _userId, unId, startDate, endDate, opStartDate, opEndDate,
            invoiceNo, saIdentNo, description, docMosNom,
            _serviceUser, _servicePassword, cancellationToken);

    public Task<DataSet?> GetBuyerInvoicesAsync(
        int unId, string startDate, string endDate, string opStartDate, string opEndDate,
        string invoiceNo, string saIdentNo, string description, string docMosNom,
        CancellationToken cancellationToken = default)
        => _soapClient.GetBuyerInvoicesAsync(
            _userId, unId, startDate, endDate, opStartDate, opEndDate,
            invoiceNo, saIdentNo, description, docMosNom,
            _serviceUser, _servicePassword, cancellationToken);

    #endregion

    #region Status Operations

    public Task<ChangeStatusResult> ChangeStatusAsync(int invoiceId, int status, CancellationToken cancellationToken = default)
        => _soapClient.ChangeInvoiceStatusAsync(_userId, invoiceId, status, _serviceUser, _servicePassword, cancellationToken);

    public Task<ChangeStatusResult> AcceptInvoiceAsync(int invoiceId, int status, CancellationToken cancellationToken = default)
        => _soapClient.AcceptInvoiceStatusAsync(_userId, invoiceId, status, _serviceUser, _servicePassword, cancellationToken);

    public Task<ChangeStatusResult> RejectInvoiceAsync(int invoiceId, string reason, CancellationToken cancellationToken = default)
        => _soapClient.RefuseInvoiceStatusAsync(_userId, invoiceId, reason, _serviceUser, _servicePassword, cancellationToken);

    #endregion

    #region Special Products

    public Task<DataSet?> GetSpecProductsAsync(CancellationToken cancellationToken = default)
        => _soapClient.GetSpecProductsAsync(cancellationToken);

    #endregion

    #region SSAF/SSD Operations

    public Task<AddSpecInvoicesSsafResult> AddSsafAsync(int invoiceId, string ssafNumber, CancellationToken cancellationToken = default)
        => _soapClient.AddSpecInvoicesSsafAsync(_userId, invoiceId, ssafNumber, _serviceUser, _servicePassword, cancellationToken);

    public Task<DelSpecInvoicesSsafResult> RemoveSsafAsync(int invoiceId, string ssafNumber, CancellationToken cancellationToken = default)
        => _soapClient.DelSpecInvoicesSsafAsync(_userId, invoiceId, ssafNumber, _serviceUser, _servicePassword, cancellationToken);

    public Task<AddSpecInvoicesSsdResult> AddSsdAsync(int invoiceId, string ssdNumber, CancellationToken cancellationToken = default)
        => _soapClient.AddSpecInvoicesSsdAsync(_userId, invoiceId, ssdNumber, _serviceUser, _servicePassword, cancellationToken);

    public Task<DelSpecInvoicesSsdResult> RemoveSsdAsync(int invoiceId, string ssdNumber, CancellationToken cancellationToken = default)
        => _soapClient.DelSpecInvoicesSsdAsync(_userId, invoiceId, ssdNumber, _serviceUser, _servicePassword, cancellationToken);

    #endregion

    #region Transport Operations

    public Task<StartTransportResult> StartTransportAsync(int invoiceId, CancellationToken cancellationToken = default)
        => _soapClient.StartTransportNewAsync(_userId, invoiceId, _serviceUser, _servicePassword, cancellationToken);

    public Task<CorrectDriverInfoResult> CorrectDriverInfoAsync(
        int id, int sellerUnId, string driverInfo, string driverNo, int driverIsGeo,
        CancellationToken cancellationToken = default)
        => _soapClient.CorrectDriverInfoAsync(id, sellerUnId, driverInfo, driverNo, driverIsGeo, _userId, _serviceUser, _servicePassword, cancellationToken);

    public Task<CorrectTransportMarkResult> CorrectTransportMarkAsync(
        int id, int sellerUnId, string transportMark,
        CancellationToken cancellationToken = default)
        => _soapClient.CorrectTransportMarkAsync(id, sellerUnId, transportMark, _userId, _serviceUser, _servicePassword, cancellationToken);

    #endregion

    #region Organization

    public Task<DataSet?> GetOrganizationObjectsAsync(int unId, CancellationToken cancellationToken = default)
        => _soapClient.GetVOrgObjectsByUnIdAsync(_userId, unId, _serviceUser, _servicePassword, cancellationToken);

    #endregion
}
