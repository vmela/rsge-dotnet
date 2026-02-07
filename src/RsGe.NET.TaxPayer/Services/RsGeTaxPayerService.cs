using RsGe.NET.TaxPayer.Soap;
using RsGe.NET.TaxPayer.Soap.Models;

namespace RsGe.NET.TaxPayer.Services;

public class RsGeTaxPayerService : IRsGeTaxPayerService
{
    private readonly ITaxPayerSoapClient _soapClient;
    private readonly string _username;
    private readonly string _password;

    public RsGeTaxPayerService(ITaxPayerSoapClient soapClient, string username, string password)
    {
        _soapClient = soapClient ?? throw new ArgumentNullException(nameof(soapClient));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _password = password ?? throw new ArgumentNullException(nameof(password));
    }

    public Task<TaxPayerPublicInfo?> GetTaxPayerInfoAsync(string tpCode, CancellationToken cancellationToken = default)
        => _soapClient.GetTaxPayerInfoPublicAsync(_username, _password, tpCode, cancellationToken);

    public Task<TaxPayerContacts?> GetTaxPayerContactsAsync(string tpCode, CancellationToken cancellationToken = default)
        => _soapClient.GetTaxPayerInfoPublicContactsAsync(_username, _password, tpCode, cancellationToken);

    public Task<LegalPersonInfo?> GetLegalPersonInfoAsync(string saidCode, CancellationToken cancellationToken = default)
        => _soapClient.GetLegalPersonInfoAsync(_username, _password, saidCode, cancellationToken);

    public Task<PayerInfo?> GetPayerInfoAsync(string saidCode, CancellationToken cancellationToken = default)
        => _soapClient.GetPayerInfoAsync(_username, _password, saidCode, cancellationToken);

    public Task<List<NaceInfo>> GetNaceCodesAsync(string saidCode, CancellationToken cancellationToken = default)
        => _soapClient.GetPayerNaceInfoAsync(_username, _password, saidCode, cancellationToken);

    public Task<List<ZReportDetail>> GetZReportDetailsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        => _soapClient.GetZReportDetailsAsync(_username, _password, startDate, endDate, cancellationToken);

    public Task<ZReportSummary?> GetZReportSummaryAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        => _soapClient.GetZReportSumAsync(_username, _password, startDate, endDate, cancellationToken);

    public Task<List<ComparisonActItem>> GetComparisonActAsync(string saidCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        => _soapClient.GetComparisonActNewAsync(_username, _password, saidCode, startDate, endDate, cancellationToken);

    public Task<List<WaybillMonthAmount>> GetWaybillMonthlyTotalsAsync(string saidCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        => _soapClient.GetWaybillMonthAmountAsync(_username, _password, saidCode, startDate, endDate, cancellationToken);

    public async Task<bool> IsVatPayerAsync(string tin, CancellationToken cancellationToken = default)
    {
        // TaxPayer service doesn't have direct VAT check; delegate to info lookup
        var info = await _soapClient.GetTaxPayerInfoPublicAsync(_username, _password, tin, cancellationToken);
        return info?.Status?.Contains("VAT", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
