using RsGe.NET.TaxPayer.Soap.Models;

namespace RsGe.NET.TaxPayer.Services;

/// <summary>
/// High-level .NET-friendly interface for RS.GE TaxPayer operations (საგადასახადო სერვისები)
/// </summary>
public interface IRsGeTaxPayerService
{
    Task<TaxPayerPublicInfo?> GetTaxPayerInfoAsync(string tpCode, CancellationToken cancellationToken = default);
    Task<TaxPayerContacts?> GetTaxPayerContactsAsync(string tpCode, CancellationToken cancellationToken = default);
    Task<LegalPersonInfo?> GetLegalPersonInfoAsync(string saidCode, CancellationToken cancellationToken = default);
    Task<PayerInfo?> GetPayerInfoAsync(string saidCode, CancellationToken cancellationToken = default);
    Task<List<NaceInfo>> GetNaceCodesAsync(string saidCode, CancellationToken cancellationToken = default);
    Task<List<ZReportDetail>> GetZReportDetailsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<ZReportSummary?> GetZReportSummaryAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<List<ComparisonActItem>> GetComparisonActAsync(string saidCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<List<WaybillMonthAmount>> GetWaybillMonthlyTotalsAsync(string saidCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<bool> IsVatPayerAsync(string tin, CancellationToken cancellationToken = default);
}
