using RsGe.NET.Invoice.Soap.Models;

namespace RsGe.NET.Invoice.Soap;

/// <summary>
/// SOAP client interface for RS.GE Invoice Service (NTOS).
/// Uses service user credentials (su/sp) and user_id for authentication.
/// </summary>
public interface IInvoiceSoapClient
{
    // Auth operations (6)
    Task<string> WhatIsMyIpAsync(CancellationToken cancellationToken = default);
    Task<ServiceUserResponse> CreateServiceUserAsync(string userName, string userPassword, string ip, string notes, string su, string sp, CancellationToken cancellationToken = default);
    Task<bool> UpdateServiceUserAsync(int userId, string userName, string userPassword, string ip, string notes, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<ServiceUserInfo>> GetServiceUsersAsync(string userName, string userPassword, CancellationToken cancellationToken = default);
    Task<List<TinNotesInfo>> GetServiceUsersNotesAsync(string tin, CancellationToken cancellationToken = default);
    Task<CheckResponse> CheckAsync(string su, string sp, int userId, CancellationToken cancellationToken = default);

    // Invoice CRUD (7)
    Task<SaveInvoiceResponse> SaveInvoiceAsync(int userId, long invoiceId, DateTime operationDate, int sellerUnId, int buyerUnId, string overheadNo, DateTime overheadDate, int bsUserId, string su, string sp, CancellationToken cancellationToken = default);
    Task<SaveInvoiceResponse> SaveInvoiceNAsync(int userId, long invoiceId, DateTime operationDate, int sellerUnId, int buyerUnId, string overheadNo, DateTime overheadDate, int bsUserId, string note, string su, string sp, CancellationToken cancellationToken = default);
    Task<SaveInvoiceResponse> SaveInvoiceAAsync(int userId, long invoiceId, DateTime operationDate, int sellerUnId, int buyerUnId, string overheadNo, DateTime overheadDate, int bsUserId, string su, string sp, CancellationToken cancellationToken = default);
    Task<InvoiceInfo?> GetInvoiceAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default);
    Task<long> GInvoiceAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default);
    Task<long> GetInvoiceIdAsync(string fSeries, string fNumber, string su, string sp, CancellationToken cancellationToken = default);
    Task<PrintInvoiceResponse?> PrintInvoicesAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default);

    // Invoice descriptions (3)
    Task<SaveInvoiceResponse> SaveInvoiceDescAsync(int userId, long id, string su, string sp, long invoiceId, string goods, string gUnit, decimal gNumber, decimal fullAmount, decimal drgAmount, decimal aqciziAmount, int akcizId, CancellationToken cancellationToken = default);
    Task<List<InvoiceDescriptionInfo>> GetInvoiceDescAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default);
    Task<bool> DeleteInvoiceDescAsync(int userId, long id, long invoiceId, string su, string sp, CancellationToken cancellationToken = default);

    // Invoice status (5)
    Task<bool> ChangeInvoiceStatusAsync(int userId, long invoiceId, int status, string su, string sp, CancellationToken cancellationToken = default);
    Task<bool> AcceptInvoiceStatusAsync(int userId, long invoiceId, int status, string su, string sp, CancellationToken cancellationToken = default);
    Task<bool> RefInvoiceStatusAsync(int userId, long invoiceId, string refText, string su, string sp, CancellationToken cancellationToken = default);
    Task<CorrectiveInvoiceResponse> KInvoiceAsync(int userId, long invoiceId, int kType, string su, string sp, CancellationToken cancellationToken = default);
    Task<CorrectiveInvoiceResponse> GetMakoreqtirebeliAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default);

    // Invoice search (11)
    Task<List<InvoiceSearchResult>> GetSellerInvoicesAsync(int userId, int unId, DateTime? startDate, DateTime? endDate, DateTime? opStartDate, DateTime? opEndDate, string invoiceNo, string saIdentNo, string desc, string docMosNom, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceSearchResult>> GetBuyerInvoicesAsync(int userId, int unId, DateTime? startDate, DateTime? endDate, DateTime? opStartDate, DateTime? opEndDate, string invoiceNo, string saIdentNo, string desc, string docMosNom, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceSearchResult>> GetSellerInvoicesRAsync(int userId, int unId, int status, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceSearchResult>> GetBuyerInvoicesRAsync(int userId, int unId, int status, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceSearchResult>> GetUserInvoicesAsync(DateTime lastUpdateDateStart, DateTime lastUpdateDateEnd, string su, string sp, int userId, int unId, CancellationToken cancellationToken = default);
    Task<List<InvoiceSearchResult>> GetInvoiceNumbersAsync(int userId, int unId, string vInvoiceN, int vCount, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceSearchResult>> GetInvoiceTinsAsync(int userId, int unId, string vInvoiceT, int vCount, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceSearchResult>> GetInvoiceDAsync(int userId, int unId, string vInvoiceD, int vCount, string su, string sp, CancellationToken cancellationToken = default);
    Task<InvoiceFilterCount> GetSellerFilterCountAsync(int unId, int userId, string su, string sp, CancellationToken cancellationToken = default);
    Task<InvoiceFilterCount> GetBuyerFilterCountAsync(int unId, int userId, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceChangeInfo>> GetInvoiceChangesAsync(int userId, DateTime startDate, DateTime endDate, string su, string sp, CancellationToken cancellationToken = default);

    // Invoice requests (6)
    Task<bool> SaveInvoiceRequestAsync(long invoiceId, int userId, int buyerUnId, int sellerUnId, string overheadNo, DateTime date, string notes, string su, string sp, CancellationToken cancellationToken = default);
    Task<InvoiceRequestInfo?> GetInvoiceRequestAsync(long invoiceId, int userId, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceRequestItem>> GetInvoiceRequestsAsync(int buyerUnId, int userId, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<InvoiceRequestItem>> GetRequestedInvoicesAsync(int userId, int sellerUnId, string su, string sp, CancellationToken cancellationToken = default);
    Task<bool> DeleteInvoiceRequestAsync(long invoiceId, int userId, int buyerUnId, string su, string sp, CancellationToken cancellationToken = default);
    Task<bool> AcceptInvoiceRequestStatusAsync(long id, int userId, int sellerUnId, string su, string sp, CancellationToken cancellationToken = default);

    // NTOS numbers (3)
    Task<List<NtosInvoiceNumber>> GetNtosInvoicesInvNosAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default);
    Task<bool> SaveNtosInvoicesInvNosAsync(long invoiceId, int userId, string overheadNo, DateTime overheadDate, string su, string sp, CancellationToken cancellationToken = default);
    Task<bool> DeleteNtosInvoicesInvNosAsync(int userId, long id, long invoiceId, string su, string sp, CancellationToken cancellationToken = default);

    // Lookups (4)
    Task<UnIdLookupResponse> GetUnIdFromTinAsync(int userId, string tin, string su, string sp, CancellationToken cancellationToken = default);
    Task<TinLookupResponse> GetTinFromUnIdAsync(int userId, int unId, string su, string sp, CancellationToken cancellationToken = default);
    Task<string> GetOrgNameFromUnIdAsync(int userId, int unId, string su, string sp, CancellationToken cancellationToken = default);
    Task<int> GetUnIdFromUserIdAsync(int userId, string su, string sp, CancellationToken cancellationToken = default);

    // Declaration (4)
    Task<bool> AddInvoiceToDeclarationAsync(int userId, int seqNum, long invoiceId, string su, string sp, CancellationToken cancellationToken = default);
    Task<DeclarationDateResponse?> GetDeclarationDateAsync(string su, string sp, string declNum, int unId, CancellationToken cancellationToken = default);
    Task<List<AkcizInfo>> GetAkcizAsync(string sText, int userId, string su, string sp, CancellationToken cancellationToken = default);
    Task<List<SeqNumInfo>> GetSeqNumsAsync(string sagPeriodi, int userId, string su, string sp, CancellationToken cancellationToken = default);
}
