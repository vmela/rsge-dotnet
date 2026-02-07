using RsGe.NET.TaxPayer.Soap.Models;

namespace RsGe.NET.TaxPayer.Soap;

/// <summary>
/// SOAP client interface for RS.GE TaxPayer Service.
/// Uses portal Username/Password credentials (not service user su/sp).
/// </summary>
public interface ITaxPayerSoapClient
{
    // Public taxpayer info
    Task<TaxPayerPublicInfo?> GetTaxPayerInfoPublicAsync(string username, string password, string tpCode, CancellationToken cancellationToken = default);
    Task<TaxPayerContacts?> GetTaxPayerInfoPublicContactsAsync(string username, string password, string tpCode, CancellationToken cancellationToken = default);
    Task<LegalPersonInfo?> GetLegalPersonInfoAsync(string username, string password, string saidCode, CancellationToken cancellationToken = default);
    Task<PayerInfo?> GetPayerInfoAsync(string username, string password, string saidCode, CancellationToken cancellationToken = default);

    // NACE codes
    Task<List<NaceInfo>> GetPayerNaceInfoAsync(string username, string password, string saidCode, CancellationToken cancellationToken = default);

    // Financial data
    Task<List<PersonIncomeData>> GetPersonIncomeDataAsync(string username, string password, string personalNumber, CancellationToken cancellationToken = default);
    Task<IncomeAmount?> GetIncomeAmountAsync(string username, string password, int year, CancellationToken cancellationToken = default);

    // Z-Reports
    Task<List<ZReportDetail>> GetZReportDetailsAsync(string username, string password, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<ZReportSummary?> GetZReportSumAsync(string username, string password, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    // Compliance
    Task<List<ComparisonActItem>> GetComparisonActNewAsync(string username, string password, string saidCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<List<ComparisonActItem>> GetComparisonActOldAsync(string username, string password, string saidCode, DateTime startDate, DateTime endDate, string? sessionId = null, CancellationToken cancellationToken = default);
    Task<List<WaybillMonthAmount>> GetWaybillMonthAmountAsync(string username, string password, string saidCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    // Customs
    Task<CustomsWarehouseExitResult> CustomsWarehouseExitAsync(string username, string password, string declarationNumber, string customsCode, string carNumber, CancellationToken cancellationToken = default);
    Task<List<Cargo200Info>> GetCargo200InfoAsync(string username, string password, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

    // Special services
    Task<QuickCashInfo?> GetQuickCashInfoAsync(string username, string password, CancellationToken cancellationToken = default);

    // GITA integration
    Task<GitaPayerInfo?> GetPayerInfoGitaAsync(string username, string password, string payerCode, string startDate, string endDate, CancellationToken cancellationToken = default);
    Task<ActivationResult> GitaPayerActivationAsync(string username, string password, string payerCode, DateTime startDate, int status, CancellationToken cancellationToken = default);
    Task<SmsVerificationResult> GitaSmsVerificationAsync(string username, string password, string payerCode, string smsCode, CancellationToken cancellationToken = default);
    Task<ActivationResult> PayerInfoActivationAsync(string username, string password, string saidCode, int status, CancellationToken cancellationToken = default);
    Task<SmsVerificationResult> TpSmsVerificationAsync(string username, string password, string saidCode, string smsCode, CancellationToken cancellationToken = default);
}
