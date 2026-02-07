using RsGe.NET.Invoice.Soap;
using RsGe.NET.Invoice.Soap.Models;

namespace RsGe.NET.Invoice.Services;

public class RsGeInvoiceService : IRsGeInvoiceService
{
    private readonly IInvoiceSoapClient _soapClient;
    private readonly string _serviceUser;
    private readonly string _servicePassword;
    private readonly int _userId;

    public RsGeInvoiceService(IInvoiceSoapClient soapClient, string serviceUser, string servicePassword, int userId)
    {
        _soapClient = soapClient ?? throw new ArgumentNullException(nameof(soapClient));
        _serviceUser = serviceUser ?? throw new ArgumentNullException(nameof(serviceUser));
        _servicePassword = servicePassword ?? throw new ArgumentNullException(nameof(servicePassword));
        _userId = userId > 0 ? userId : throw new ArgumentException("UserId must be greater than 0", nameof(userId));
    }

    public Task<CheckResponse> CheckCredentialsAsync(CancellationToken cancellationToken = default)
        => _soapClient.CheckAsync(_serviceUser, _servicePassword, _userId, cancellationToken);

    public Task<SaveInvoiceResponse> SaveInvoiceAsync(long invoiceId, DateTime operationDate, int sellerUnId, int buyerUnId, string overheadNo, DateTime overheadDate, int bsUserId, CancellationToken cancellationToken = default)
        => _soapClient.SaveInvoiceAsync(_userId, invoiceId, operationDate, sellerUnId, buyerUnId, overheadNo, overheadDate, bsUserId, _serviceUser, _servicePassword, cancellationToken);

    public Task<InvoiceInfo?> GetInvoiceAsync(long invoiceId, CancellationToken cancellationToken = default)
        => _soapClient.GetInvoiceAsync(_userId, invoiceId, _serviceUser, _servicePassword, cancellationToken);

    public Task<List<InvoiceSearchResult>> GetSellerInvoicesAsync(int unId, DateTime? startDate = null, DateTime? endDate = null, DateTime? opStartDate = null, DateTime? opEndDate = null, string invoiceNo = "", string saIdentNo = "", string desc = "", string docMosNom = "", CancellationToken cancellationToken = default)
        => _soapClient.GetSellerInvoicesAsync(_userId, unId, startDate, endDate, opStartDate, opEndDate, invoiceNo, saIdentNo, desc, docMosNom, _serviceUser, _servicePassword, cancellationToken);

    public Task<List<InvoiceSearchResult>> GetBuyerInvoicesAsync(int unId, DateTime? startDate = null, DateTime? endDate = null, DateTime? opStartDate = null, DateTime? opEndDate = null, string invoiceNo = "", string saIdentNo = "", string desc = "", string docMosNom = "", CancellationToken cancellationToken = default)
        => _soapClient.GetBuyerInvoicesAsync(_userId, unId, startDate, endDate, opStartDate, opEndDate, invoiceNo, saIdentNo, desc, docMosNom, _serviceUser, _servicePassword, cancellationToken);

    public Task<bool> ChangeInvoiceStatusAsync(long invoiceId, int status, CancellationToken cancellationToken = default)
        => _soapClient.ChangeInvoiceStatusAsync(_userId, invoiceId, status, _serviceUser, _servicePassword, cancellationToken);

    public Task<bool> AcceptInvoiceAsync(long invoiceId, int status, CancellationToken cancellationToken = default)
        => _soapClient.AcceptInvoiceStatusAsync(_userId, invoiceId, status, _serviceUser, _servicePassword, cancellationToken);

    public Task<bool> RejectInvoiceAsync(long invoiceId, string refText, CancellationToken cancellationToken = default)
        => _soapClient.RefInvoiceStatusAsync(_userId, invoiceId, refText, _serviceUser, _servicePassword, cancellationToken);

    public Task<CorrectiveInvoiceResponse> CreateCorrectiveInvoiceAsync(long invoiceId, int kType, CancellationToken cancellationToken = default)
        => _soapClient.KInvoiceAsync(_userId, invoiceId, kType, _serviceUser, _servicePassword, cancellationToken);

    public Task<SaveInvoiceResponse> SaveInvoiceDescriptionAsync(long id, long invoiceId, string goods, string gUnit, decimal gNumber, decimal fullAmount, decimal drgAmount, decimal aqciziAmount, int akcizId, CancellationToken cancellationToken = default)
        => _soapClient.SaveInvoiceDescAsync(_userId, id, _serviceUser, _servicePassword, invoiceId, goods, gUnit, gNumber, fullAmount, drgAmount, aqciziAmount, akcizId, cancellationToken);

    public Task<List<InvoiceDescriptionInfo>> GetInvoiceDescriptionsAsync(long invoiceId, CancellationToken cancellationToken = default)
        => _soapClient.GetInvoiceDescAsync(_userId, invoiceId, _serviceUser, _servicePassword, cancellationToken);

    public Task<bool> DeleteInvoiceDescriptionAsync(long id, long invoiceId, CancellationToken cancellationToken = default)
        => _soapClient.DeleteInvoiceDescAsync(_userId, id, invoiceId, _serviceUser, _servicePassword, cancellationToken);

    public Task<bool> SaveInvoiceRequestAsync(long invoiceId, int buyerUnId, int sellerUnId, string overheadNo, DateTime date, string notes, CancellationToken cancellationToken = default)
        => _soapClient.SaveInvoiceRequestAsync(invoiceId, _userId, buyerUnId, sellerUnId, overheadNo, date, notes, _serviceUser, _servicePassword, cancellationToken);

    public Task<List<InvoiceRequestItem>> GetInvoiceRequestsAsync(int buyerUnId, CancellationToken cancellationToken = default)
        => _soapClient.GetInvoiceRequestsAsync(buyerUnId, _userId, _serviceUser, _servicePassword, cancellationToken);

    public Task<TinLookupResponse> LookupTinFromUnIdAsync(int unId, CancellationToken cancellationToken = default)
        => _soapClient.GetTinFromUnIdAsync(_userId, unId, _serviceUser, _servicePassword, cancellationToken);

    public Task<UnIdLookupResponse> LookupUnIdFromTinAsync(string tin, CancellationToken cancellationToken = default)
        => _soapClient.GetUnIdFromTinAsync(_userId, tin, _serviceUser, _servicePassword, cancellationToken);
}
