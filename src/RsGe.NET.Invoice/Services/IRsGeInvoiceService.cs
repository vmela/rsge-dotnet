using RsGe.NET.Invoice.Soap.Models;

namespace RsGe.NET.Invoice.Services;

/// <summary>
/// High-level .NET-friendly interface for RS.GE Invoice (NTOS) operations
/// </summary>
public interface IRsGeInvoiceService
{
    // Auth
    Task<CheckResponse> CheckCredentialsAsync(CancellationToken cancellationToken = default);

    // Invoice CRUD
    Task<SaveInvoiceResponse> SaveInvoiceAsync(long invoiceId, DateTime operationDate, int sellerUnId, int buyerUnId, string overheadNo, DateTime overheadDate, int bsUserId, CancellationToken cancellationToken = default);
    Task<InvoiceInfo?> GetInvoiceAsync(long invoiceId, CancellationToken cancellationToken = default);

    // Invoice search
    Task<List<InvoiceSearchResult>> GetSellerInvoicesAsync(int unId, DateTime? startDate = null, DateTime? endDate = null, DateTime? opStartDate = null, DateTime? opEndDate = null, string invoiceNo = "", string saIdentNo = "", string desc = "", string docMosNom = "", CancellationToken cancellationToken = default);
    Task<List<InvoiceSearchResult>> GetBuyerInvoicesAsync(int unId, DateTime? startDate = null, DateTime? endDate = null, DateTime? opStartDate = null, DateTime? opEndDate = null, string invoiceNo = "", string saIdentNo = "", string desc = "", string docMosNom = "", CancellationToken cancellationToken = default);

    // Invoice status
    Task<bool> ChangeInvoiceStatusAsync(long invoiceId, int status, CancellationToken cancellationToken = default);
    Task<bool> AcceptInvoiceAsync(long invoiceId, int status, CancellationToken cancellationToken = default);
    Task<bool> RejectInvoiceAsync(long invoiceId, string refText, CancellationToken cancellationToken = default);
    Task<CorrectiveInvoiceResponse> CreateCorrectiveInvoiceAsync(long invoiceId, int kType, CancellationToken cancellationToken = default);

    // Invoice descriptions
    Task<SaveInvoiceResponse> SaveInvoiceDescriptionAsync(long id, long invoiceId, string goods, string gUnit, decimal gNumber, decimal fullAmount, decimal drgAmount, decimal aqciziAmount, int akcizId, CancellationToken cancellationToken = default);
    Task<List<InvoiceDescriptionInfo>> GetInvoiceDescriptionsAsync(long invoiceId, CancellationToken cancellationToken = default);
    Task<bool> DeleteInvoiceDescriptionAsync(long id, long invoiceId, CancellationToken cancellationToken = default);

    // Invoice requests
    Task<bool> SaveInvoiceRequestAsync(long invoiceId, int buyerUnId, int sellerUnId, string overheadNo, DateTime date, string notes, CancellationToken cancellationToken = default);
    Task<List<InvoiceRequestItem>> GetInvoiceRequestsAsync(int buyerUnId, CancellationToken cancellationToken = default);

    // Lookups
    Task<TinLookupResponse> LookupTinFromUnIdAsync(int unId, CancellationToken cancellationToken = default);
    Task<UnIdLookupResponse> LookupUnIdFromTinAsync(string tin, CancellationToken cancellationToken = default);
}
