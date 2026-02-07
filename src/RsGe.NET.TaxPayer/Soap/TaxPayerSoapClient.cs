using System.Xml.Linq;
using RsGe.NET.Core.Soap;
using RsGe.NET.TaxPayer.Soap.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace RsGe.NET.TaxPayer.Soap;

/// <summary>
/// SOAP client for RS.GE TaxPayer Service (საგადასახადო სერვისები).
/// Endpoint: https://services.rs.ge/taxservice/taxpayerservice.asmx
/// </summary>
public class TaxPayerSoapClient : SoapClientBase, ITaxPayerSoapClient
{
    private const string DefaultServiceUrl = "https://services.rs.ge/taxservice/taxpayerservice.asmx";

    private readonly string _serviceUrl;

    protected override string ServiceUrl => _serviceUrl;

    public TaxPayerSoapClient(HttpClient? httpClient = null, ILogger<TaxPayerSoapClient>? logger = null, string? serviceUrl = null)
        : base(httpClient, logger)
    {
        _serviceUrl = serviceUrl ?? DefaultServiceUrl;
    }

    #region Public Taxpayer Info

    public async Task<TaxPayerPublicInfo?> GetTaxPayerInfoPublicAsync(string username, string password, string tpCode, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("GetTPInfoPublic", new Dictionary<string, string>
        {
            ["Username"] = username,
            ["Password"] = password,
            ["TP_Code"] = tpCode
        }, cancellationToken);
        return ParseTaxPayerPublicInfo(response);
    }

    public async Task<TaxPayerContacts?> GetTaxPayerInfoPublicContactsAsync(string username, string password, string tpCode, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("GetTPInfoPublicContacts", new Dictionary<string, string>
        {
            ["Username"] = username,
            ["Password"] = password,
            ["TP_Code"] = tpCode
        }, cancellationToken);
        return ParseTaxPayerContacts(response);
    }

    public async Task<LegalPersonInfo?> GetLegalPersonInfoAsync(string username, string password, string saidCode, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_LegalPerson_Info", new Dictionary<string, string>
        {
            ["inUserName"] = username,
            ["inPassword"] = password,
            ["inSaidCode"] = saidCode
        }, cancellationToken);
        return ParseLegalPersonInfo(response);
    }

    public async Task<PayerInfo?> GetPayerInfoAsync(string username, string password, string saidCode, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_Payer_Info", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["saidCode"] = saidCode
        }, cancellationToken);
        return ParsePayerInfo(response);
    }

    #endregion

    #region NACE Codes

    public async Task<List<NaceInfo>> GetPayerNaceInfoAsync(string username, string password, string saidCode, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_Payer_Nace_Info", new Dictionary<string, string>
        {
            ["inUserName"] = username,
            ["inPassword"] = password,
            ["inSaidCode"] = saidCode
        }, cancellationToken);
        return ParseNaceInfoList(response);
    }

    #endregion

    #region Financial Data

    public async Task<List<PersonIncomeData>> GetPersonIncomeDataAsync(string username, string password, string personalNumber, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("GetPersonIncomeData", new Dictionary<string, string>
        {
            ["inUserName"] = username,
            ["inPassword"] = password,
            ["inPersonalNumber"] = personalNumber
        }, cancellationToken);
        return ParsePersonIncomeDataList(response);
    }

    public async Task<IncomeAmount?> GetIncomeAmountAsync(string username, string password, int year, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_Income_Amount", new Dictionary<string, string>
        {
            ["UserName"] = username,
            ["Password"] = password,
            ["Year"] = year.ToString()
        }, cancellationToken);
        return ParseIncomeAmount(response, year);
    }

    #endregion

    #region Z-Reports

    public async Task<List<ZReportDetail>> GetZReportDetailsAsync(string username, string password, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_Z_Report_Details", new Dictionary<string, string>
        {
            ["UserName"] = username,
            ["Password"] = password,
            ["StartDate"] = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["EndDate"] = endDate.ToString("yyyy-MM-ddTHH:mm:ss")
        }, cancellationToken);
        return ParseZReportDetails(response);
    }

    public async Task<ZReportSummary?> GetZReportSumAsync(string username, string password, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_Z_Report_Sum", new Dictionary<string, string>
        {
            ["UserName"] = username,
            ["Password"] = password,
            ["StartDate"] = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["EndDate"] = endDate.ToString("yyyy-MM-ddTHH:mm:ss")
        }, cancellationToken);
        return ParseZReportSummary(response);
    }

    #endregion

    #region Compliance

    public async Task<List<ComparisonActItem>> GetComparisonActNewAsync(string username, string password, string saidCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_comp_act_new", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["saidCode"] = saidCode,
            ["start_date"] = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["end_date"] = endDate.ToString("yyyy-MM-ddTHH:mm:ss")
        }, cancellationToken);
        return ParseComparisonActItems(response);
    }

    public async Task<List<ComparisonActItem>> GetComparisonActOldAsync(string username, string password, string saidCode, DateTime startDate, DateTime endDate, string? sessionId = null, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_comp_act_old", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["saidCode"] = saidCode,
            ["start_date"] = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["end_date"] = endDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["session_id"] = sessionId ?? ""
        }, cancellationToken);
        return ParseComparisonActItems(response);
    }

    public async Task<List<WaybillMonthAmount>> GetWaybillMonthAmountAsync(string username, string password, string saidCode, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_Waybill_Month_Amount", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["saidCode"] = saidCode,
            ["startDate"] = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["endDate"] = endDate.ToString("yyyy-MM-ddTHH:mm:ss")
        }, cancellationToken);
        return ParseWaybillMonthAmounts(response);
    }

    #endregion

    #region Customs

    public async Task<CustomsWarehouseExitResult> CustomsWarehouseExitAsync(string username, string password, string declarationNumber, string customsCode, string carNumber, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Customs_WareHouse_Exit", new Dictionary<string, string>
        {
            ["UserName"] = username,
            ["Password"] = password,
            ["DeclarationNumber"] = declarationNumber,
            ["CustomsCode"] = customsCode,
            ["CarNumber"] = carNumber
        }, cancellationToken);
        var result = ParseStringResponse(response, "Customs_WareHouse_ExitResult");
        return new CustomsWarehouseExitResult
        {
            IsSuccess = !string.IsNullOrEmpty(result) && !result.StartsWith("-"),
            Message = result
        };
    }

    public async Task<List<Cargo200Info>> GetCargo200InfoAsync(string username, string password, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_Cargo200_Info", new Dictionary<string, string>
        {
            ["inUserName"] = username,
            ["inPassword"] = password,
            ["inStartDate"] = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["inEndDate"] = endDate.ToString("yyyy-MM-ddTHH:mm:ss")
        }, cancellationToken);
        return ParseCargo200InfoList(response);
    }

    #endregion

    #region Special Services

    public async Task<QuickCashInfo?> GetQuickCashInfoAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Get_QuickCash_Info", new Dictionary<string, string>
        {
            ["user"] = username,
            ["password"] = password
        }, cancellationToken);
        return ParseQuickCashInfo(response);
    }

    #endregion

    #region GITA Integration

    public async Task<GitaPayerInfo?> GetPayerInfoGitaAsync(string username, string password, string payerCode, string startDate, string endDate, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_payer_info_gita", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["payerCode"] = payerCode,
            ["startDate"] = startDate,
            ["endDate"] = endDate
        }, cancellationToken);
        return ParseGitaPayerInfo(response);
    }

    public async Task<ActivationResult> GitaPayerActivationAsync(string username, string password, string payerCode, DateTime startDate, int status, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Gita_Payer_Activation", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["payerCode"] = payerCode,
            ["startDate"] = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["status"] = status.ToString()
        }, cancellationToken);
        var result = ParseStringResponse(response, "Gita_Payer_ActivationResult");
        return new ActivationResult { IsSuccess = result == "1" || result.Equals("true", StringComparison.OrdinalIgnoreCase), Message = result };
    }

    public async Task<SmsVerificationResult> GitaSmsVerificationAsync(string username, string password, string payerCode, string smsCode, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Gita_Sms_Verification", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["payerCode"] = payerCode,
            ["smsCode"] = smsCode
        }, cancellationToken);
        var result = ParseStringResponse(response, "Gita_Sms_VerificationResult");
        return new SmsVerificationResult { IsSuccess = result == "1" || result.Equals("true", StringComparison.OrdinalIgnoreCase), Message = result };
    }

    public async Task<ActivationResult> PayerInfoActivationAsync(string username, string password, string saidCode, int status, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Payer_Info_Activation", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["saidCode"] = saidCode,
            ["status"] = status.ToString()
        }, cancellationToken);
        var result = ParseStringResponse(response, "Payer_Info_ActivationResult");
        return new ActivationResult { IsSuccess = result == "1" || result.Equals("true", StringComparison.OrdinalIgnoreCase), Message = result };
    }

    public async Task<SmsVerificationResult> TpSmsVerificationAsync(string username, string password, string saidCode, string smsCode, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("Tp_sms_verification", new Dictionary<string, string>
        {
            ["userName"] = username,
            ["password"] = password,
            ["saidCode"] = saidCode,
            ["smsCode"] = smsCode
        }, cancellationToken);
        var result = ParseStringResponse(response, "Tp_sms_verificationResult");
        return new SmsVerificationResult { IsSuccess = result == "1" || result.Equals("true", StringComparison.OrdinalIgnoreCase), Message = result };
    }

    #endregion

    #region Private Parse Methods

    private static TaxPayerPublicInfo? ParseTaxPayerPublicInfo(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "ResponseTPInfoPublic");
        if (el == null) return null;
        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new TaxPayerPublicInfo
        {
            Tin = GetVal("TP_Code"),
            Name = GetVal("TP_Name"),
            NameEn = GetVal("TP_Name_En"),
            Status = GetVal("TP_Status"),
            RegistrationDate = GetVal("Registration_Date"),
            LegalForm = GetVal("Legal_Form"),
            Address = GetVal("Address")
        };
    }

    private static TaxPayerContacts? ParseTaxPayerContacts(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "ResponseTPInfoPublicContacts");
        if (el == null) return null;
        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new TaxPayerContacts
        {
            Tin = GetVal("TP_Code"),
            Email = GetVal("Email"),
            Phone = GetVal("Phone"),
            Address = GetVal("Address")
        };
    }

    private static LegalPersonInfo? ParseLegalPersonInfo(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "LegalPersonResponse");
        if (el == null) return null;
        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new LegalPersonInfo
        {
            Tin = GetVal("Tin"),
            Name = GetVal("Name"),
            LegalForm = GetVal("LegalForm"),
            Status = GetVal("Status"),
            RegistrationDate = GetVal("RegistrationDate"),
            Address = GetVal("Address"),
            Director = GetVal("Director")
        };
    }

    private static PayerInfo? ParsePayerInfo(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "TaxyPayerResponse");
        if (el == null) return null;
        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new PayerInfo
        {
            SaidCode = GetVal("SaidCode"),
            Name = GetVal("Name"),
            Status = GetVal("Status"),
            Type = GetVal("Type")
        };
    }

    private static List<NaceInfo> ParseNaceInfoList(XDocument doc)
    {
        var result = new List<NaceInfo>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "NaceInfo" or "NACE"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new NaceInfo
            {
                Code = GetVal("Code"),
                Name = GetVal("Name"),
                IsPrimary = GetVal("IsPrimary") == "1" || GetVal("IsPrimary").Equals("true", StringComparison.OrdinalIgnoreCase)
            });
        }
        return result;
    }

    private static List<PersonIncomeData> ParsePersonIncomeDataList(XDocument doc)
    {
        var result = new List<PersonIncomeData>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "PersonIncomeData" or "IncomeData"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new PersonIncomeData
            {
                PersonalNumber = GetVal("PersonalNumber"),
                EmployerTin = GetVal("EmployerTin"),
                EmployerName = GetVal("EmployerName"),
                Salary = decimal.TryParse(GetVal("Salary"), out var s) ? s : 0,
                IncomeTax = decimal.TryParse(GetVal("IncomeTax"), out var t) ? t : 0
            });
        }
        return result;
    }

    private static IncomeAmount? ParseIncomeAmount(XDocument doc, int year)
    {
        var value = ParseStringResponse(doc, "Get_Income_AmountResult");
        if (string.IsNullOrEmpty(value)) return null;
        return new IncomeAmount
        {
            Year = year,
            Amount = decimal.TryParse(value, out var a) ? a : 0
        };
    }

    private static List<ZReportDetail> ParseZReportDetails(XDocument doc)
    {
        var result = new List<ZReportDetail>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "ZReportDetail" or "Z_REPORT"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new ZReportDetail
            {
                CashRegisterNumber = GetVal("CashRegisterNumber") is { Length: > 0 } crn ? crn : GetVal("CASH_REG_NO"),
                ReportDate = DateTime.TryParse(GetVal("ReportDate") is { Length: > 0 } rd ? rd : GetVal("REPORT_DATE"), out var d) ? d : DateTime.MinValue,
                ReportNumber = int.TryParse(GetVal("ReportNumber") is { Length: > 0 } rn ? rn : GetVal("REPORT_NO"), out var n) ? n : 0,
                TotalAmount = decimal.TryParse(GetVal("TotalAmount") is { Length: > 0 } ta ? ta : GetVal("TOTAL_AMOUNT"), out var total) ? total : 0,
                CashAmount = decimal.TryParse(GetVal("CashAmount") is { Length: > 0 } ca ? ca : GetVal("CASH_AMOUNT"), out var cash) ? cash : 0,
                CardAmount = decimal.TryParse(GetVal("CardAmount") is { Length: > 0 } cda ? cda : GetVal("CARD_AMOUNT"), out var card) ? card : 0,
                ReceiptCount = int.TryParse(GetVal("ReceiptCount") is { Length: > 0 } rc ? rc : GetVal("RECEIPT_COUNT"), out var cnt) ? cnt : 0
            });
        }
        return result;
    }

    private static ZReportSummary? ParseZReportSummary(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName is "ResponseGetZReportSum" or "GetZReportSumResult");
        if (el == null) return null;
        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new ZReportSummary
        {
            TotalAmount = decimal.TryParse(GetVal("TotalAmount") is { Length: > 0 } ta ? ta : GetVal("TOTAL_AMOUNT"), out var total) ? total : 0,
            CashAmount = decimal.TryParse(GetVal("CashAmount") is { Length: > 0 } ca ? ca : GetVal("CASH_AMOUNT"), out var cash) ? cash : 0,
            CardAmount = decimal.TryParse(GetVal("CardAmount") is { Length: > 0 } cda ? cda : GetVal("CARD_AMOUNT"), out var card) ? card : 0,
            TotalReceiptCount = int.TryParse(GetVal("TotalReceiptCount") is { Length: > 0 } rc ? rc : GetVal("RECEIPT_COUNT"), out var cnt) ? cnt : 0,
            ReportCount = int.TryParse(GetVal("ReportCount") is { Length: > 0 } rpc ? rpc : GetVal("REPORT_COUNT"), out var rpt) ? rpt : 0
        };
    }

    private static List<ComparisonActItem> ParseComparisonActItems(XDocument doc)
    {
        var result = new List<ComparisonActItem>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "ComparisonActItem" or "ACT_ITEM"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new ComparisonActItem
            {
                DocumentType = GetVal("DocumentType") is { Length: > 0 } dt ? dt : GetVal("DOC_TYPE"),
                DocumentNumber = GetVal("DocumentNumber") is { Length: > 0 } dn ? dn : GetVal("DOC_NUMBER"),
                DocumentDate = DateTime.TryParse(GetVal("DocumentDate") is { Length: > 0 } dd ? dd : GetVal("DOC_DATE"), out var d) ? d : DateTime.MinValue,
                Debit = decimal.TryParse(GetVal("Debit") is { Length: > 0 } deb ? deb : GetVal("DEBIT"), out var debit) ? debit : 0,
                Credit = decimal.TryParse(GetVal("Credit") is { Length: > 0 } cr ? cr : GetVal("CREDIT"), out var credit) ? credit : 0,
                Balance = decimal.TryParse(GetVal("Balance") is { Length: > 0 } bal ? bal : GetVal("BALANCE"), out var balance) ? balance : 0
            });
        }
        return result;
    }

    private static List<WaybillMonthAmount> ParseWaybillMonthAmounts(XDocument doc)
    {
        var result = new List<WaybillMonthAmount>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "WaybillMonthAmount" or "MONTH_DATA"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new WaybillMonthAmount
            {
                Year = int.TryParse(GetVal("Year") is { Length: > 0 } y ? y : GetVal("YEAR"), out var year) ? year : 0,
                Month = int.TryParse(GetVal("Month") is { Length: > 0 } m ? m : GetVal("MONTH"), out var month) ? month : 0,
                SalesAmount = decimal.TryParse(GetVal("SalesAmount") is { Length: > 0 } sa ? sa : GetVal("SALES_AMOUNT"), out var sales) ? sales : 0,
                PurchasesAmount = decimal.TryParse(GetVal("PurchasesAmount") is { Length: > 0 } pa ? pa : GetVal("PURCHASES_AMOUNT"), out var purchases) ? purchases : 0,
                SalesCount = int.TryParse(GetVal("SalesCount") is { Length: > 0 } sc ? sc : GetVal("SALES_COUNT"), out var sCount) ? sCount : 0,
                PurchasesCount = int.TryParse(GetVal("PurchasesCount") is { Length: > 0 } pc ? pc : GetVal("PURCHASES_COUNT"), out var pCount) ? pCount : 0
            });
        }
        return result;
    }

    private static List<Cargo200Info> ParseCargo200InfoList(XDocument doc)
    {
        var result = new List<Cargo200Info>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "Cargo200Info" or "CARGO"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new Cargo200Info
            {
                DeclarationNumber = GetVal("DeclarationNumber") is { Length: > 0 } dn ? dn : GetVal("DECL_NUMBER"),
                Date = DateTime.TryParse(GetVal("Date") is { Length: > 0 } d ? d : GetVal("DATE"), out var date) ? date : DateTime.MinValue,
                Status = GetVal("Status") is { Length: > 0 } s ? s : GetVal("STATUS"),
                Amount = decimal.TryParse(GetVal("Amount") is { Length: > 0 } a ? a : GetVal("AMOUNT"), out var amount) ? amount : 0
            });
        }
        return result;
    }

    private static QuickCashInfo? ParseQuickCashInfo(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName is "OrgRevenue" or "QuickCashInfo");
        if (el == null) return null;
        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new QuickCashInfo
        {
            OrganizationName = GetVal("OrganizationName") is { Length: > 0 } on ? on : GetVal("ORG_NAME"),
            Revenue = decimal.TryParse(GetVal("Revenue") is { Length: > 0 } r ? r : GetVal("REVENUE"), out var rev) ? rev : 0
        };
    }

    private static GitaPayerInfo? ParseGitaPayerInfo(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName is "GitaResponse" or "GitaPayerInfo");
        if (el == null) return null;
        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new GitaPayerInfo
        {
            PayerCode = GetVal("PayerCode") is { Length: > 0 } pc ? pc : GetVal("PAYER_CODE"),
            Name = GetVal("Name") is { Length: > 0 } n ? n : GetVal("NAME"),
            Status = GetVal("Status") is { Length: > 0 } s ? s : GetVal("STATUS")
        };
    }

    #endregion
}
