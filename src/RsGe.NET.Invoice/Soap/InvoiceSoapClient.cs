using System.Globalization;
using System.Xml.Linq;
using RsGe.NET.Core.Soap;
using RsGe.NET.Invoice.Soap.Models;
using Microsoft.Extensions.Logging;

namespace RsGe.NET.Invoice.Soap;

/// <summary>
/// SOAP client for RS.GE Invoice Service (NTOS).
/// Endpoint: https://www.revenue.mof.ge/ntosservice/ntosservice.asmx
/// </summary>
public class InvoiceSoapClient : SoapClientBase, IInvoiceSoapClient
{
    private const string DefaultServiceUrl = "https://www.revenue.mof.ge/ntosservice/ntosservice.asmx";

    private readonly string _serviceUrl;

    protected override string ServiceUrl => _serviceUrl;

    public InvoiceSoapClient(HttpClient? httpClient = null, ILogger<InvoiceSoapClient>? logger = null, string? serviceUrl = null)
        : base(httpClient, logger)
    {
        _serviceUrl = serviceUrl ?? DefaultServiceUrl;
    }

    #region Auth Operations

    public async Task<string> WhatIsMyIpAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("what_is_my_ip", new Dictionary<string, string>(), cancellationToken);
        return ParseStringResponse(response, "what_is_my_ipResult");
    }

    public async Task<ServiceUserResponse> CreateServiceUserAsync(string userName, string userPassword, string ip, string notes, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("create_ser_user", new Dictionary<string, string>
        {
            ["user_name"] = userName,
            ["user_password"] = userPassword,
            ["ip"] = ip,
            ["notes"] = notes,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new ServiceUserResponse
        {
            IsSuccess = ParseBoolResponse(response, "create_ser_userResult"),
            UserId = ParseIntResponse(response, "user_id")
        };
    }

    public async Task<bool> UpdateServiceUserAsync(int userId, string userName, string userPassword, string ip, string notes, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("update_ser_user", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["user_name"] = userName,
            ["user_password"] = userPassword,
            ["ip"] = ip,
            ["notes"] = notes,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "update_ser_userResult");
    }

    public async Task<List<ServiceUserInfo>> GetServiceUsersAsync(string userName, string userPassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_ser_users", new Dictionary<string, string>
        {
            ["user_name"] = userName,
            ["user_password"] = userPassword
        }, cancellationToken);

        return ParseServiceUserInfoList(response);
    }

    public async Task<List<TinNotesInfo>> GetServiceUsersNotesAsync(string tin, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_ser_users_notes", new Dictionary<string, string>
        {
            ["tin"] = tin
        }, cancellationToken);

        return ParseTinNotesInfoList(response);
    }

    public async Task<CheckResponse> CheckAsync(string su, string sp, int userId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("chek", new Dictionary<string, string>
        {
            ["su"] = su,
            ["sp"] = sp,
            ["user_id"] = userId.ToString()
        }, cancellationToken);

        return new CheckResponse
        {
            IsSuccess = ParseBoolResponse(response, "chekResult"),
            UserId = ParseIntResponse(response, "user_id"),
            Sua = ParseStringResponse(response, "sua")
        };
    }

    #endregion

    #region Invoice CRUD

    public async Task<SaveInvoiceResponse> SaveInvoiceAsync(int userId, long invoiceId, DateTime operationDate, int sellerUnId, int buyerUnId, string overheadNo, DateTime overheadDate, int bsUserId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["operation_date"] = operationDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["seller_un_id"] = sellerUnId.ToString(),
            ["buyer_un_id"] = buyerUnId.ToString(),
            ["overhead_no"] = overheadNo,
            ["overhead_dt"] = overheadDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["b_s_user_id"] = bsUserId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new SaveInvoiceResponse
        {
            IsSuccess = ParseBoolResponse(response, "save_invoiceResult"),
            InvoiceId = ParseLongResponse(response, "invois_id")
        };
    }

    public async Task<SaveInvoiceResponse> SaveInvoiceNAsync(int userId, long invoiceId, DateTime operationDate, int sellerUnId, int buyerUnId, string overheadNo, DateTime overheadDate, int bsUserId, string note, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice_n", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["operation_date"] = operationDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["seller_un_id"] = sellerUnId.ToString(),
            ["buyer_un_id"] = buyerUnId.ToString(),
            ["overhead_no"] = overheadNo,
            ["overhead_dt"] = overheadDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["b_s_user_id"] = bsUserId.ToString(),
            ["note"] = note,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new SaveInvoiceResponse
        {
            IsSuccess = ParseBoolResponse(response, "save_invoice_nResult"),
            InvoiceId = ParseLongResponse(response, "invois_id")
        };
    }

    public async Task<SaveInvoiceResponse> SaveInvoiceAAsync(int userId, long invoiceId, DateTime operationDate, int sellerUnId, int buyerUnId, string overheadNo, DateTime overheadDate, int bsUserId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice_a", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["operation_date"] = operationDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["seller_un_id"] = sellerUnId.ToString(),
            ["buyer_un_id"] = buyerUnId.ToString(),
            ["overhead_no"] = overheadNo,
            ["overhead_dt"] = overheadDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["b_s_user_id"] = bsUserId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new SaveInvoiceResponse
        {
            IsSuccess = ParseBoolResponse(response, "save_invoice_aResult"),
            InvoiceId = ParseLongResponse(response, "invois_id")
        };
    }

    public async Task<InvoiceInfo?> GetInvoiceAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceInfo(response);
    }

    public async Task<long> GInvoiceAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("g_invoice", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseLongResponse(response, "k_id");
    }

    public async Task<long> GetInvoiceIdAsync(string fSeries, string fNumber, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_id", new Dictionary<string, string>
        {
            ["f_series"] = fSeries,
            ["f_number"] = fNumber,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseLongResponse(response, "get_invoice_idResult");
    }

    public async Task<PrintInvoiceResponse?> PrintInvoicesAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("print_invoices", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var content = ParseStringResponse(response, "print_invoicesResult");
        if (string.IsNullOrEmpty(content)) return null;

        return new PrintInvoiceResponse
        {
            Content = content,
            Format = "html"
        };
    }

    #endregion

    #region Invoice Descriptions

    public async Task<SaveInvoiceResponse> SaveInvoiceDescAsync(int userId, long id, string su, string sp, long invoiceId, string goods, string gUnit, decimal gNumber, decimal fullAmount, decimal drgAmount, decimal aqciziAmount, int akcizId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice_desc", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["id"] = id.ToString(),
            ["su"] = su,
            ["sp"] = sp,
            ["invois_id"] = invoiceId.ToString(),
            ["goods"] = goods,
            ["g_unit"] = gUnit,
            ["g_number"] = gNumber.ToString(CultureInfo.InvariantCulture),
            ["full_amount"] = fullAmount.ToString(CultureInfo.InvariantCulture),
            ["drg_amount"] = drgAmount.ToString(CultureInfo.InvariantCulture),
            ["aqcizi_amount"] = aqciziAmount.ToString(CultureInfo.InvariantCulture),
            ["akciz_id"] = akcizId.ToString()
        }, cancellationToken);

        return new SaveInvoiceResponse
        {
            IsSuccess = ParseBoolResponse(response, "save_invoice_descResult"),
            InvoiceId = ParseLongResponse(response, "id")
        };
    }

    public async Task<List<InvoiceDescriptionInfo>> GetInvoiceDescAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_desc", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceDescriptionList(response);
    }

    public async Task<bool> DeleteInvoiceDescAsync(int userId, long id, long invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("delete_invoice_desc", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["id"] = id.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "delete_invoice_descResult");
    }

    #endregion

    #region Invoice Status

    public async Task<bool> ChangeInvoiceStatusAsync(int userId, long invoiceId, int status, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("change_invoice_status", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["status"] = status.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "change_invoice_statusResult");
    }

    public async Task<bool> AcceptInvoiceStatusAsync(int userId, long invoiceId, int status, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("acsept_invoice_status", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["status"] = status.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "acsept_invoice_statusResult");
    }

    public async Task<bool> RefInvoiceStatusAsync(int userId, long invoiceId, string refText, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("ref_invoice_status", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["ref_text"] = refText,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "ref_invoice_statusResult");
    }

    public async Task<CorrectiveInvoiceResponse> KInvoiceAsync(int userId, long invoiceId, int kType, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("k_invoice", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["k_type"] = kType.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new CorrectiveInvoiceResponse
        {
            IsSuccess = ParseBoolResponse(response, "k_invoiceResult"),
            KId = ParseLongResponse(response, "k_id")
        };
    }

    public async Task<CorrectiveInvoiceResponse> GetMakoreqtirebeliAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_makoreqtirebeli", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new CorrectiveInvoiceResponse
        {
            IsSuccess = ParseBoolResponse(response, "get_makoreqtirebeliResult"),
            KId = ParseLongResponse(response, "k_id")
        };
    }

    #endregion

    #region Invoice Search

    public async Task<List<InvoiceSearchResult>> GetSellerInvoicesAsync(int userId, int unId, DateTime? startDate, DateTime? endDate, DateTime? opStartDate, DateTime? opEndDate, string invoiceNo, string saIdentNo, string desc, string docMosNom, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_seller_invoices", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["s_dt"] = startDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "",
            ["e_dt"] = endDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "",
            ["op_s_dt"] = opStartDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "",
            ["op_e_dt"] = opEndDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "",
            ["invoice_no"] = invoiceNo,
            ["sa_ident_no"] = saIdentNo,
            ["desc"] = desc,
            ["doc_mos_nom"] = docMosNom,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceSearchResults(response);
    }

    public async Task<List<InvoiceSearchResult>> GetBuyerInvoicesAsync(int userId, int unId, DateTime? startDate, DateTime? endDate, DateTime? opStartDate, DateTime? opEndDate, string invoiceNo, string saIdentNo, string desc, string docMosNom, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_buyer_invoices", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["s_dt"] = startDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "",
            ["e_dt"] = endDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "",
            ["op_s_dt"] = opStartDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "",
            ["op_e_dt"] = opEndDate?.ToString("yyyy-MM-ddTHH:mm:ss") ?? "",
            ["invoice_no"] = invoiceNo,
            ["sa_ident_no"] = saIdentNo,
            ["desc"] = desc,
            ["doc_mos_nom"] = docMosNom,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceSearchResults(response);
    }

    public async Task<List<InvoiceSearchResult>> GetSellerInvoicesRAsync(int userId, int unId, int status, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_seller_invoices_r", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["status"] = status.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceSearchResults(response);
    }

    public async Task<List<InvoiceSearchResult>> GetBuyerInvoicesRAsync(int userId, int unId, int status, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_buyer_invoices_r", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["status"] = status.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceSearchResults(response);
    }

    public async Task<List<InvoiceSearchResult>> GetUserInvoicesAsync(DateTime lastUpdateDateStart, DateTime lastUpdateDateEnd, string su, string sp, int userId, int unId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_user_invoices", new Dictionary<string, string>
        {
            ["last_update_date_s"] = lastUpdateDateStart.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["last_update_date_e"] = lastUpdateDateEnd.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["su"] = su,
            ["sp"] = sp,
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString()
        }, cancellationToken);

        return ParseInvoiceSearchResults(response);
    }

    public async Task<List<InvoiceSearchResult>> GetInvoiceNumbersAsync(int userId, int unId, string vInvoiceN, int vCount, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_numbers", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["v_invoice_n"] = vInvoiceN,
            ["v_count"] = vCount.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceSearchResults(response);
    }

    public async Task<List<InvoiceSearchResult>> GetInvoiceTinsAsync(int userId, int unId, string vInvoiceT, int vCount, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_tins", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["v_invoice_t"] = vInvoiceT,
            ["v_count"] = vCount.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceSearchResults(response);
    }

    public async Task<List<InvoiceSearchResult>> GetInvoiceDAsync(int userId, int unId, string vInvoiceD, int vCount, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_d", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["v_invoice_d"] = vInvoiceD,
            ["v_count"] = vCount.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceSearchResults(response);
    }

    public async Task<InvoiceFilterCount> GetSellerFilterCountAsync(int unId, int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_seller_filter_count", new Dictionary<string, string>
        {
            ["un_id"] = unId.ToString(),
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new InvoiceFilterCount
        {
            IsSuccess = ParseBoolResponse(response, "get_seller_filter_countResult"),
            Status0 = ParseIntResponse(response, "st_0"),
            Status1 = ParseIntResponse(response, "st_1"),
            Status2 = ParseIntResponse(response, "st_2"),
            Status3 = ParseIntResponse(response, "st_3"),
            Status4 = ParseIntResponse(response, "st_4"),
            Status5 = ParseIntResponse(response, "st_5"),
            Status6 = ParseIntResponse(response, "st_6"),
            Status7 = ParseIntResponse(response, "st_7"),
            Status8 = ParseIntResponse(response, "st_8")
        };
    }

    public async Task<InvoiceFilterCount> GetBuyerFilterCountAsync(int unId, int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_buyer_filter_count", new Dictionary<string, string>
        {
            ["un_id"] = unId.ToString(),
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new InvoiceFilterCount
        {
            IsSuccess = ParseBoolResponse(response, "get_buyer_filter_countResult"),
            Status1 = ParseIntResponse(response, "st_1"),
            Status2 = ParseIntResponse(response, "st_2"),
            Status3 = ParseIntResponse(response, "st_3"),
            Status4 = ParseIntResponse(response, "st_4"),
            Status5 = ParseIntResponse(response, "st_5"),
            Status6 = ParseIntResponse(response, "st_6"),
            Status7 = ParseIntResponse(response, "st_7"),
            Status8 = ParseIntResponse(response, "st_8")
        };
    }

    public async Task<List<InvoiceChangeInfo>> GetInvoiceChangesAsync(int userId, DateTime startDate, DateTime endDate, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_changes", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["st_date"] = startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["end_date"] = endDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceChangeList(response);
    }

    #endregion

    #region Invoice Requests

    public async Task<bool> SaveInvoiceRequestAsync(long invoiceId, int userId, int buyerUnId, int sellerUnId, string overheadNo, DateTime date, string notes, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice_request", new Dictionary<string, string>
        {
            ["inv_id"] = invoiceId.ToString(),
            ["user_id"] = userId.ToString(),
            ["bayer_un_id"] = buyerUnId.ToString(),
            ["seller_un_id"] = sellerUnId.ToString(),
            ["overhead_no"] = overheadNo,
            ["dt"] = date.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["notes"] = notes,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "save_invoice_requestResult");
    }

    public async Task<InvoiceRequestInfo?> GetInvoiceRequestAsync(long invoiceId, int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_request", new Dictionary<string, string>
        {
            ["inv_id"] = invoiceId.ToString(),
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceRequestInfo(response);
    }

    public async Task<List<InvoiceRequestItem>> GetInvoiceRequestsAsync(int buyerUnId, int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_requests", new Dictionary<string, string>
        {
            ["bayer_un_id"] = buyerUnId.ToString(),
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceRequestItems(response);
    }

    public async Task<List<InvoiceRequestItem>> GetRequestedInvoicesAsync(int userId, int sellerUnId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_requested_invoices", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["seller_un_id"] = sellerUnId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseInvoiceRequestItems(response);
    }

    public async Task<bool> DeleteInvoiceRequestAsync(long invoiceId, int userId, int buyerUnId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("del_invoice_request", new Dictionary<string, string>
        {
            ["inv_id"] = invoiceId.ToString(),
            ["user_id"] = userId.ToString(),
            ["bayer_un_id"] = buyerUnId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "del_invoice_requestResult");
    }

    public async Task<bool> AcceptInvoiceRequestStatusAsync(long id, int userId, int sellerUnId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("acsept_invoice_request_status", new Dictionary<string, string>
        {
            ["id"] = id.ToString(),
            ["user_id"] = userId.ToString(),
            ["seller_un_id"] = sellerUnId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "acsept_invoice_request_statusResult");
    }

    #endregion

    #region NTOS Numbers

    public async Task<List<NtosInvoiceNumber>> GetNtosInvoicesInvNosAsync(int userId, long invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_ntos_invoices_inv_nos", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseNtosInvoiceNumbers(response);
    }

    public async Task<bool> SaveNtosInvoicesInvNosAsync(long invoiceId, int userId, string overheadNo, DateTime overheadDate, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_ntos_invoices_inv_nos", new Dictionary<string, string>
        {
            ["invois_id"] = invoiceId.ToString(),
            ["user_id"] = userId.ToString(),
            ["overhead_no"] = overheadNo,
            ["overhead_dt"] = overheadDate.ToString("yyyy-MM-ddTHH:mm:ss"),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "save_ntos_invoices_inv_nosResult");
    }

    public async Task<bool> DeleteNtosInvoicesInvNosAsync(int userId, long id, long invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("delete_ntos_invoices_inv_nos", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["id"] = id.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "delete_ntos_invoices_inv_nosResult");
    }

    #endregion

    #region Lookups

    public async Task<UnIdLookupResponse> GetUnIdFromTinAsync(int userId, string tin, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_un_id_from_tin", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["tin"] = tin,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new UnIdLookupResponse
        {
            UnId = ParseIntResponse(response, "get_un_id_from_tinResult"),
            Name = ParseStringResponse(response, "name")
        };
    }

    public async Task<TinLookupResponse> GetTinFromUnIdAsync(int userId, int unId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_tin_from_un_id", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return new TinLookupResponse
        {
            Tin = ParseStringResponse(response, "get_tin_from_un_idResult"),
            Name = ParseStringResponse(response, "name")
        };
    }

    public async Task<string> GetOrgNameFromUnIdAsync(int userId, int unId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_org_name_from_un_id", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseStringResponse(response, "get_org_name_from_un_idResult");
    }

    public async Task<int> GetUnIdFromUserIdAsync(int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_un_id_from_user_id", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseIntResponse(response, "get_un_id_from_user_idResult");
    }

    #endregion

    #region Declaration

    public async Task<bool> AddInvoiceToDeclarationAsync(int userId, int seqNum, long invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("add_inv_to_decl", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["seq_num"] = seqNum.ToString(),
            ["inv_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseBoolResponse(response, "add_inv_to_declResult");
    }

    public async Task<DeclarationDateResponse?> GetDeclarationDateAsync(string su, string sp, string declNum, int unId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_decl_date", new Dictionary<string, string>
        {
            ["su"] = su,
            ["sp"] = sp,
            ["decl_num"] = declNum,
            ["un_id"] = unId.ToString()
        }, cancellationToken);

        var result = ParseStringResponse(response, "get_decl_dateResult");
        if (string.IsNullOrEmpty(result)) return null;

        return new DeclarationDateResponse { Result = result };
    }

    public async Task<List<AkcizInfo>> GetAkcizAsync(string sText, int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_akciz", new Dictionary<string, string>
        {
            ["s_text"] = sText,
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseAkcizList(response);
    }

    public async Task<List<SeqNumInfo>> GetSeqNumsAsync(string sagPeriodi, int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_seq_nums", new Dictionary<string, string>
        {
            ["sag_periodi"] = sagPeriodi,
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseSeqNumList(response);
    }

    #endregion

    #region Private Parse Methods

    private static long ParseLongResponse(XDocument doc, string elementName)
    {
        var value = ParseStringResponse(doc, elementName);
        return long.TryParse(value, out var result) ? result : 0;
    }

    private static decimal ParseDecimalResponse(XDocument doc, string elementName)
    {
        var value = ParseStringResponse(doc, elementName);
        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result) ? result : 0;
    }

    private static DateTime ParseDateTimeResponse(XDocument doc, string elementName)
    {
        var value = ParseStringResponse(doc, elementName);
        return DateTime.TryParse(value, out var result) ? result : DateTime.MinValue;
    }

    private static List<ServiceUserInfo> ParseServiceUserInfoList(XDocument doc)
    {
        var result = new List<ServiceUserInfo>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "ServiceUser" or "USER"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new ServiceUserInfo
            {
                UserId = int.TryParse(GetVal("user_id"), out var id) ? id : 0,
                UserName = GetVal("user_name"),
                Ip = GetVal("ip"),
                Notes = GetVal("notes"),
                CreatedDate = DateTime.TryParse(GetVal("created_date"), out var date) ? date : DateTime.MinValue
            });
        }
        return result;
    }

    private static List<TinNotesInfo> ParseTinNotesInfoList(XDocument doc)
    {
        var result = new List<TinNotesInfo>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "TinNotes" or "NOTES"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new TinNotesInfo
            {
                Tin = GetVal("tin"),
                Notes = GetVal("notes")
            });
        }
        return result;
    }

    private static InvoiceInfo? ParseInvoiceInfo(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName is "InvoiceInfo" or "INVOICE");
        if (el == null) return null;

        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new InvoiceInfo
        {
            FSeries = GetVal("f_series"),
            FNumber = GetVal("f_number"),
            OperationDate = DateTime.TryParse(GetVal("operation_dt"), out var opDt) ? opDt : DateTime.MinValue,
            RegDate = DateTime.TryParse(GetVal("reg_dt"), out var regDt) ? regDt : DateTime.MinValue,
            SellerUnId = int.TryParse(GetVal("seller_un_id"), out var sId) ? sId : 0,
            BuyerUnId = int.TryParse(GetVal("buyer_un_id"), out var bId) ? bId : 0,
            OverheadNo = GetVal("overhead_no"),
            OverheadDate = DateTime.TryParse(GetVal("overhead_dt"), out var ohDt) ? ohDt : DateTime.MinValue,
            Status = int.TryParse(GetVal("status"), out var st) ? st : 0,
            SeqNumS = GetVal("seq_num_s"),
            SeqNumB = GetVal("seq_num_b"),
            KId = long.TryParse(GetVal("k_id"), out var kId) ? kId : 0,
            RUnId = int.TryParse(GetVal("r_un_id"), out var rId) ? rId : 0,
            KType = int.TryParse(GetVal("k_type"), out var kType) ? kType : 0,
            BsUserId = int.TryParse(GetVal("b_s_user_id"), out var bsUid) ? bsUid : 0,
            DecStatus = int.TryParse(GetVal("dec_status"), out var decSt) ? decSt : 0
        };
    }

    private static List<InvoiceDescriptionInfo> ParseInvoiceDescriptionList(XDocument doc)
    {
        var result = new List<InvoiceDescriptionInfo>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "InvoiceDesc" or "DESC"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new InvoiceDescriptionInfo
            {
                Id = long.TryParse(GetVal("id"), out var id) ? id : 0,
                InvoiceId = long.TryParse(GetVal("invois_id"), out var invId) ? invId : 0,
                Goods = GetVal("goods"),
                GUnit = GetVal("g_unit"),
                GNumber = decimal.TryParse(GetVal("g_number"), NumberStyles.Any, CultureInfo.InvariantCulture, out var gNum) ? gNum : 0,
                FullAmount = decimal.TryParse(GetVal("full_amount"), NumberStyles.Any, CultureInfo.InvariantCulture, out var fAmt) ? fAmt : 0,
                DrgAmount = decimal.TryParse(GetVal("drg_amount"), NumberStyles.Any, CultureInfo.InvariantCulture, out var dAmt) ? dAmt : 0,
                AqciziAmount = decimal.TryParse(GetVal("aqcizi_amount"), NumberStyles.Any, CultureInfo.InvariantCulture, out var aAmt) ? aAmt : 0,
                AkcizId = int.TryParse(GetVal("akciz_id"), out var akcId) ? akcId : 0
            });
        }
        return result;
    }

    private static List<InvoiceSearchResult> ParseInvoiceSearchResults(XDocument doc)
    {
        var result = new List<InvoiceSearchResult>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "Invoice" or "INVOICE"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new InvoiceSearchResult
            {
                InvoiceId = long.TryParse(GetVal("invois_id"), out var invId) ? invId : 0,
                FSeries = GetVal("f_series"),
                FNumber = GetVal("f_number"),
                OperationDate = DateTime.TryParse(GetVal("operation_date"), out var opDt) ? opDt : DateTime.MinValue,
                RegDate = DateTime.TryParse(GetVal("reg_date"), out var regDt) ? regDt : DateTime.MinValue,
                SellerUnId = int.TryParse(GetVal("seller_un_id"), out var sId) ? sId : 0,
                SellerTin = GetVal("seller_tin"),
                SellerName = GetVal("seller_name"),
                BuyerUnId = int.TryParse(GetVal("buyer_un_id"), out var bId) ? bId : 0,
                BuyerTin = GetVal("buyer_tin"),
                BuyerName = GetVal("buyer_name"),
                TotalAmount = decimal.TryParse(GetVal("total_amount"), NumberStyles.Any, CultureInfo.InvariantCulture, out var amt) ? amt : 0,
                Status = int.TryParse(GetVal("status"), out var st) ? st : 0,
                OverheadNo = GetVal("overhead_no"),
                OverheadDate = DateTime.TryParse(GetVal("overhead_date"), out var ohDt) ? ohDt : DateTime.MinValue
            });
        }
        return result;
    }

    private static List<InvoiceChangeInfo> ParseInvoiceChangeList(XDocument doc)
    {
        var result = new List<InvoiceChangeInfo>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "Change" or "CHANGE"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new InvoiceChangeInfo
            {
                InvoiceId = long.TryParse(GetVal("invoice_id"), out var id) ? id : 0,
                ChangeType = GetVal("change_type"),
                ChangeDate = DateTime.TryParse(GetVal("change_date"), out var dt) ? dt : DateTime.MinValue,
                ChangedBy = GetVal("changed_by")
            });
        }
        return result;
    }

    private static InvoiceRequestInfo? ParseInvoiceRequestInfo(XDocument doc)
    {
        var el = doc.Descendants().FirstOrDefault(e => e.Name.LocalName is "InvoiceRequest" or "REQUEST");
        if (el == null) return null;

        string GetVal(string name) => el.Descendants().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
        return new InvoiceRequestInfo
        {
            IsSuccess = ParseBoolResponse(doc, "get_invoice_requestResult"),
            BuyerUnId = int.TryParse(GetVal("bayer_un_id"), out var bId) ? bId : 0,
            SellerUnId = int.TryParse(GetVal("seller_un_id"), out var sId) ? sId : 0,
            OverheadNo = GetVal("overhead_no"),
            Date = DateTime.TryParse(GetVal("dt"), out var dt) ? dt : DateTime.MinValue,
            Notes = GetVal("notes")
        };
    }

    private static List<InvoiceRequestItem> ParseInvoiceRequestItems(XDocument doc)
    {
        var result = new List<InvoiceRequestItem>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "Request" or "REQUEST"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new InvoiceRequestItem
            {
                Id = long.TryParse(GetVal("id"), out var id) ? id : 0,
                InvoiceId = long.TryParse(GetVal("inv_id"), out var invId) ? invId : 0,
                BuyerUnId = int.TryParse(GetVal("bayer_un_id"), out var bId) ? bId : 0,
                SellerUnId = int.TryParse(GetVal("seller_un_id"), out var sId) ? sId : 0,
                OverheadNo = GetVal("overhead_no"),
                Date = DateTime.TryParse(GetVal("dt"), out var dt) ? dt : DateTime.MinValue,
                Notes = GetVal("notes")
            });
        }
        return result;
    }

    private static List<NtosInvoiceNumber> ParseNtosInvoiceNumbers(XDocument doc)
    {
        var result = new List<NtosInvoiceNumber>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "NtosNumber" or "NUMBER"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new NtosInvoiceNumber
            {
                Id = long.TryParse(GetVal("id"), out var id) ? id : 0,
                InvoiceId = long.TryParse(GetVal("invois_id"), out var invId) ? invId : 0,
                OverheadNo = GetVal("overhead_no"),
                OverheadDate = DateTime.TryParse(GetVal("overhead_dt"), out var dt) ? dt : DateTime.MinValue
            });
        }
        return result;
    }

    private static List<AkcizInfo> ParseAkcizList(XDocument doc)
    {
        var result = new List<AkcizInfo>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "Akciz" or "AKCIZ"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new AkcizInfo
            {
                Id = int.TryParse(GetVal("id"), out var id) ? id : 0,
                Code = GetVal("code"),
                Name = GetVal("name")
            });
        }
        return result;
    }

    private static List<SeqNumInfo> ParseSeqNumList(XDocument doc)
    {
        var result = new List<SeqNumInfo>();
        foreach (var el in doc.Descendants().Where(e => e.Name.LocalName is "SeqNum" or "SEQ"))
        {
            string GetVal(string name) => el.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;
            result.Add(new SeqNumInfo
            {
                SeqNum = int.TryParse(GetVal("seq_num"), out var num) ? num : 0,
                Description = GetVal("description")
            });
        }
        return result;
    }

    #endregion
}
