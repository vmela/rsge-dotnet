using System.Data;
using System.Xml.Linq;
using RsGe.NET.Core.Soap;
using RsGe.NET.SpecInvoice.Soap.Models;
using Microsoft.Extensions.Logging;

namespace RsGe.NET.SpecInvoice.Soap;

/// <summary>
/// SOAP client for RS.GE SpecInvoices Service (სპეციალური ინვოისები).
/// Endpoint: https://www.revenue.mof.ge/SpecInvoicesService/SpecInvoicesService.asmx
/// </summary>
public class SpecInvoiceSoapClient : SoapClientBase, ISpecInvoiceSoapClient
{
    private const string DefaultServiceUrl = "https://www.revenue.mof.ge/SpecInvoicesService/SpecInvoicesService.asmx";

    private readonly string _serviceUrl;

    protected override string ServiceUrl => _serviceUrl;

    public SpecInvoiceSoapClient(HttpClient? httpClient = null, ILogger<SpecInvoiceSoapClient>? logger = null, string? serviceUrl = null)
        : base(httpClient, logger)
    {
        _serviceUrl = serviceUrl ?? DefaultServiceUrl;
    }

    #region Auth (2)

    public async Task<CheckInResult> CheckInAsync(string su, string sp, int userId, string logText, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("chek_in", new Dictionary<string, string>
        {
            ["su"] = su,
            ["sp"] = sp,
            ["user_id"] = userId.ToString(),
            ["log_text"] = logText
        }, cancellationToken);

        var success = ParseBoolResponse(response, "chek_inResult");
        var sui = ParseIntResponse(response, "sui");

        return new CheckInResult
        {
            Success = success,
            Sui = sui,
            Message = success ? "Check-in successful" : "Check-in failed"
        };
    }

    public async Task<CheckSpecUsersResult> CheckSpecUsersAsync(string su, string sp, int userId, string logText, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("check_spec_users", new Dictionary<string, string>
        {
            ["su"] = su,
            ["sp"] = sp,
            ["user_id"] = userId.ToString(),
            ["log_text"] = logText
        }, cancellationToken);

        var success = ParseBoolResponse(response, "check_spec_usersResult");

        return new CheckSpecUsersResult
        {
            Success = success,
            Message = success ? "User validation successful" : "User validation failed"
        };
    }

    #endregion

    #region Invoice CRUD (6)

    public async Task<SaveInvoiceResult> SaveInvoiceAsync(
        int invoiceId, string fSeries, string operationDt, int sellerUnId, int buyerUnId,
        string ssdN, string ssafN, string calcDate, string kSsafN, string trStDate,
        string oilStAddress, string oilStN, string oilFnAddress, string oilFnN,
        string transportType, string transportMark, string driverInfo, string carrierInfo,
        string carrieSNo, int pUserId, int sUserId, int bSUserId, string ssdDate, string ssafDate,
        string payType, string sellerPhone, string buyerPhone, string driverNo,
        string ssafAltNumber, string ssafAltType, string ssdAltNumber, string ssdAltType,
        string ssafAltStatus, string ssdAltStatus, int userId, string su, string sp,
        CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice", new Dictionary<string, string>
        {
            ["invois_id"] = invoiceId.ToString(),
            ["p_F_SERIES"] = fSeries,
            ["p_OPERATION_DT"] = operationDt,
            ["p_SELLER_UN_ID"] = sellerUnId.ToString(),
            ["p_BUYER_UN_ID"] = buyerUnId.ToString(),
            ["p_SSD_N"] = ssdN,
            ["p_SSAF_N"] = ssafN,
            ["p_CALC_DATE"] = calcDate,
            ["p_K_SSAF_N"] = kSsafN,
            ["p_TR_ST_DATE"] = trStDate,
            ["p_OIL_ST_ADDRESS"] = oilStAddress,
            ["p_OIL_ST_N"] = oilStN,
            ["p_OIL_FN_ADDRESS"] = oilFnAddress,
            ["p_OIL_FN_N"] = oilFnN,
            ["p_TRANSPORT_TYPE"] = transportType,
            ["p_TRANSPORT_MARK"] = transportMark,
            ["p_DRIVER_INFO"] = driverInfo,
            ["p_CARRIER_INFO"] = carrierInfo,
            ["p_CARRIE_S_NO"] = carrieSNo,
            ["p_USER_ID"] = pUserId.ToString(),
            ["p_S_USER_ID"] = sUserId.ToString(),
            ["p_B_S_USER_ID"] = bSUserId.ToString(),
            ["p_SSD_DATE"] = ssdDate,
            ["p_SSAF_DATE"] = ssafDate,
            ["p_PAY_TYPE"] = payType,
            ["p_SELLER_PHONE"] = sellerPhone,
            ["p_BUYER_PHONE"] = buyerPhone,
            ["p_driver_no"] = driverNo,
            ["p_SSAF_ALT_NUMBER"] = ssafAltNumber,
            ["p_SSAF_ALT_TYPE"] = ssafAltType,
            ["p_SSD_ALT_NUMBER"] = ssdAltNumber,
            ["p_SSD_ALT_TYPE"] = ssdAltType,
            ["p_SSAF_ALT_STATUS"] = ssafAltStatus,
            ["p_SSD_ALT_STATUS"] = ssdAltStatus,
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "save_invoiceResult");
        var newInvoiceId = ParseIntResponse(response, "invois_id");

        return new SaveInvoiceResult
        {
            Success = success,
            InvoiceId = newInvoiceId,
            Message = success ? "Invoice saved successfully" : "Failed to save invoice"
        };
    }

    public async Task<SaveInvoiceResult> SaveInvoiceBAsync(
        int invoiceId, string fSeries, string operationDt, int sellerUnId, int buyerUnId,
        string ssdN, string ssafN, string calcDate, string kSsafN, string trStDate,
        string oilStAddress, string oilStN, string oilFnAddress, string oilFnN,
        string transportType, string transportMark, string driverInfo, string carrierInfo,
        string carrieSNo, int pUserId, int sUserId, int bSUserId, string ssdDate, string ssafDate,
        string payType, string sellerPhone, string buyerPhone, string driverNo,
        string ssafAltNumber, string ssafAltType, string ssdAltNumber, string ssdAltType,
        string ssafAltStatus, string ssdAltStatus, int driverIsGeo, int userId, string su, string sp,
        CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice_b", new Dictionary<string, string>
        {
            ["invois_id"] = invoiceId.ToString(),
            ["p_F_SERIES"] = fSeries,
            ["p_OPERATION_DT"] = operationDt,
            ["p_SELLER_UN_ID"] = sellerUnId.ToString(),
            ["p_BUYER_UN_ID"] = buyerUnId.ToString(),
            ["p_SSD_N"] = ssdN,
            ["p_SSAF_N"] = ssafN,
            ["p_CALC_DATE"] = calcDate,
            ["p_K_SSAF_N"] = kSsafN,
            ["p_TR_ST_DATE"] = trStDate,
            ["p_OIL_ST_ADDRESS"] = oilStAddress,
            ["p_OIL_ST_N"] = oilStN,
            ["p_OIL_FN_ADDRESS"] = oilFnAddress,
            ["p_OIL_FN_N"] = oilFnN,
            ["p_TRANSPORT_TYPE"] = transportType,
            ["p_TRANSPORT_MARK"] = transportMark,
            ["p_DRIVER_INFO"] = driverInfo,
            ["p_CARRIER_INFO"] = carrierInfo,
            ["p_CARRIE_S_NO"] = carrieSNo,
            ["p_USER_ID"] = pUserId.ToString(),
            ["p_S_USER_ID"] = sUserId.ToString(),
            ["p_B_S_USER_ID"] = bSUserId.ToString(),
            ["p_SSD_DATE"] = ssdDate,
            ["p_SSAF_DATE"] = ssafDate,
            ["p_PAY_TYPE"] = payType,
            ["p_SELLER_PHONE"] = sellerPhone,
            ["p_BUYER_PHONE"] = buyerPhone,
            ["p_driver_no"] = driverNo,
            ["p_SSAF_ALT_NUMBER"] = ssafAltNumber,
            ["p_SSAF_ALT_TYPE"] = ssafAltType,
            ["p_SSD_ALT_NUMBER"] = ssdAltNumber,
            ["p_SSD_ALT_TYPE"] = ssdAltType,
            ["p_SSAF_ALT_STATUS"] = ssafAltStatus,
            ["p_SSD_ALT_STATUS"] = ssdAltStatus,
            ["p_driver_is_geo"] = driverIsGeo.ToString(),
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "save_invoice_bResult");
        var newInvoiceId = ParseIntResponse(response, "invois_id");

        return new SaveInvoiceResult
        {
            Success = success,
            InvoiceId = newInvoiceId,
            Message = success ? "Invoice saved successfully" : "Failed to save invoice"
        };
    }

    public async Task<DataSet?> GetInvoiceAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_invoiceResult");
    }

    public async Task<DataSet?> GetInvoiceDAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_d", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_invoice_dResult");
    }

    public async Task<DataSet?> KInvoiceAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("k_invoice", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "k_invoiceResult");
    }

    public async Task<DataSet?> PrintInvoicesAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("print_invoices", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "print_invoicesResult");
    }

    #endregion

    #region Descriptions (3)

    public async Task<SaveInvoiceDescResult> SaveInvoiceDescAsync(
        int userId, int id, string su, string sp, int invId, string goods, string gUnit,
        decimal gNumber, decimal unPrice, decimal drgAmount, decimal aqciziAmount, int pUserId,
        int aqciziId, string aqciziRate, string dggRate, string gNumberAlt, string goodId, string drgType,
        CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice_desc", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["id"] = id.ToString(),
            ["su"] = su,
            ["sp"] = sp,
            ["p_inv_id"] = invId.ToString(),
            ["p_goods"] = goods,
            ["p_g_unit"] = gUnit,
            ["p_g_number"] = gNumber.ToString(),
            ["p_un_price"] = unPrice.ToString(),
            ["p_drg_amount"] = drgAmount.ToString(),
            ["p_aqcizi_amount"] = aqciziAmount.ToString(),
            ["p_user_id"] = pUserId.ToString(),
            ["p_aqcizi_id"] = aqciziId.ToString(),
            ["p_aqcizi_rate"] = aqciziRate,
            ["p_dgg_rate"] = dggRate,
            ["p_g_number_alt"] = gNumberAlt,
            ["p_good_id"] = goodId,
            ["p_drg_type"] = drgType
        }, cancellationToken);

        var success = ParseBoolResponse(response, "save_invoice_descResult");
        var descId = ParseIntResponse(response, "id");

        return new SaveInvoiceDescResult
        {
            Success = success,
            DescriptionId = descId,
            Message = success ? "Invoice description saved successfully" : "Failed to save invoice description"
        };
    }

    public async Task<DataSet?> GetInvoiceDescAsync(int userId, int invoiceId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_invoice_desc", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["invois_id"] = invoiceId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_invoice_descResult");
    }

    public async Task<DeleteInvoiceDescResult> DeleteInvoiceDescAsync(int userId, int id, int invId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("delete_invoice_desc", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["id"] = id.ToString(),
            ["inv_id"] = invId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "delete_invoice_descResult");

        return new DeleteInvoiceDescResult
        {
            Success = success,
            Message = success ? "Invoice description deleted successfully" : "Failed to delete invoice description"
        };
    }

    #endregion

    #region Status (5)

    public async Task<ChangeStatusResult> ChangeInvoiceStatusAsync(int userId, int invId, int status, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("change_invoice_status", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["status"] = status.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "change_invoice_statusResult");

        return new ChangeStatusResult
        {
            Success = success,
            Message = success ? "Invoice status changed successfully" : "Failed to change invoice status"
        };
    }

    public async Task<ChangeStatusResult> AcceptInvoiceStatusAsync(int userId, int invId, int status, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("acsept_invoice_status", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["status"] = status.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "acsept_invoice_statusResult");

        return new ChangeStatusResult
        {
            Success = success,
            Message = success ? "Invoice accepted successfully" : "Failed to accept invoice"
        };
    }

    public async Task<ChangeStatusResult> RefuseInvoiceStatusAsync(int userId, int invId, string refText, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("ref_invoice_status", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["ref_text"] = refText,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "ref_invoice_statusResult");

        return new ChangeStatusResult
        {
            Success = success,
            Message = success ? "Invoice refused successfully" : "Failed to refuse invoice"
        };
    }

    public async Task<ChangeStatusResult> SaveInvoiceRequestAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice_request", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "save_invoice_requestResult");

        return new ChangeStatusResult
        {
            Success = success,
            Message = success ? "Invoice request saved successfully" : "Failed to save invoice request"
        };
    }

    public async Task<CancellationReasonResult> GauqmebisMizeziAsync(int userId, int invId, string reason, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("gauqmebis_mizezi", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["reason"] = reason,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "gauqmebis_mizeziResult");

        return new CancellationReasonResult
        {
            Success = success,
            Message = success ? "Cancellation reason saved successfully" : "Failed to save cancellation reason"
        };
    }

    #endregion

    #region Search (8)

    public async Task<DataSet?> GetSellerInvoicesAsync(
        int userId, int unId, string sDt, string eDt, string opSDt, string opEDt,
        string invoiceNo, string saIdentNo, string desc, string docMosNom,
        string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_seller_invoices", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["s_dt"] = sDt,
            ["e_dt"] = eDt,
            ["op_s_dt"] = opSDt,
            ["op_e_dt"] = opEDt,
            ["invoice_no"] = invoiceNo,
            ["sa_ident_no"] = saIdentNo,
            ["desc"] = desc,
            ["doc_mos_nom"] = docMosNom,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_seller_invoicesResult");
    }

    public async Task<DataSet?> GetBuyerInvoicesAsync(
        int userId, int unId, string sDt, string eDt, string opSDt, string opEDt,
        string invoiceNo, string saIdentNo, string desc, string docMosNom,
        string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_buyer_invoices", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["s_dt"] = sDt,
            ["e_dt"] = eDt,
            ["op_s_dt"] = opSDt,
            ["op_e_dt"] = opEDt,
            ["invoice_no"] = invoiceNo,
            ["sa_ident_no"] = saIdentNo,
            ["desc"] = desc,
            ["doc_mos_nom"] = docMosNom,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_buyer_invoicesResult");
    }

    public async Task<DataSet?> GetSellerInvoicesRAsync(int userId, int unId, int status, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_seller_invoices_r", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["status"] = status.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_seller_invoices_rResult");
    }

    public async Task<DataSet?> GetBuyerInvoicesRAsync(int userId, int unId, int status, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_buyer_invoices_r", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["status"] = status.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_buyer_invoices_rResult");
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

    public async Task<DataSet?> GetInvoiceNumbersAsync(int userId, int unId, string vInvoiceN, int vCount, string su, string sp, CancellationToken cancellationToken = default)
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

        return ParseDataSetResponse(response, "get_invoice_numbersResult");
    }

    public async Task<DataSet?> GetInvoiceTinsAsync(int userId, int unId, string vInvoiceT, int vCount, string su, string sp, CancellationToken cancellationToken = default)
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

        return ParseDataSetResponse(response, "get_invoice_tinsResult");
    }

    #endregion

    #region Special Products (4)

    public async Task<DataSet?> GetSpecProductsAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_spec_products", new Dictionary<string, string>(), cancellationToken);
        return ParseDataSetResponse(response, "get_spec_productsResult");
    }

    public async Task<DataSet?> GetSpecProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_spec_product_by_id", new Dictionary<string, string>
        {
            ["product_id"] = productId.ToString()
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_spec_product_by_idResult");
    }

    public async Task<DataSet?> GetSpecSsafsAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_spec_ssafs", new Dictionary<string, string>(), cancellationToken);
        return ParseDataSetResponse(response, "get_spec_ssafsResult");
    }

    public async Task<DataSet?> GetSpecSsdsAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_spec_ssds", new Dictionary<string, string>(), cancellationToken);
        return ParseDataSetResponse(response, "get_spec_ssdsResult");
    }

    #endregion

    #region SSAF/SSD (4)

    public async Task<AddSpecInvoicesSsafResult> AddSpecInvoicesSsafAsync(int userId, int invId, string ssafN, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("add_spec_invoices_ssaf", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["ssaf_n"] = ssafN,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "add_spec_invoices_ssafResult");

        return new AddSpecInvoicesSsafResult
        {
            Success = success,
            Message = success ? "SSAF added successfully" : "Failed to add SSAF"
        };
    }

    public async Task<DelSpecInvoicesSsafResult> DelSpecInvoicesSsafAsync(int userId, int invId, string ssafN, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("del_spec_invoices_ssaf", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["ssaf_n"] = ssafN,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "del_spec_invoices_ssafResult");

        return new DelSpecInvoicesSsafResult
        {
            Success = success,
            Message = success ? "SSAF removed successfully" : "Failed to remove SSAF"
        };
    }

    public async Task<AddSpecInvoicesSsdResult> AddSpecInvoicesSsdAsync(int userId, int invId, string ssdN, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("add_spec_invoices_ssd", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["ssd_n"] = ssdN,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "add_spec_invoices_ssdResult");

        return new AddSpecInvoicesSsdResult
        {
            Success = success,
            Message = success ? "SSD added successfully" : "Failed to add SSD"
        };
    }

    public async Task<DelSpecInvoicesSsdResult> DelSpecInvoicesSsdAsync(int userId, int invId, string ssdN, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("del_spec_invoices_ssd", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["ssd_n"] = ssdN,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "del_spec_invoices_ssdResult");

        return new DelSpecInvoicesSsdResult
        {
            Success = success,
            Message = success ? "SSD removed successfully" : "Failed to remove SSD"
        };
    }

    #endregion

    #region Transport (4)

    public async Task<StartTransportResult> StartTransportAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("start_transport", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "start_transportResult");

        return new StartTransportResult
        {
            Success = success,
            Message = success ? "Transport started successfully" : "Failed to start transport"
        };
    }

    public async Task<StartTransportResult> StartTransportNewAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("start_transport_new", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "start_transport_newResult");

        return new StartTransportResult
        {
            Success = success,
            Message = success ? "Transport started successfully" : "Failed to start transport"
        };
    }

    public async Task<CorrectDriverInfoResult> CorrectDriverInfoAsync(
        int id, int sellerUnId, string driverInfo, string driverNo, int driverIsGeo,
        int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("correct_driver_info", new Dictionary<string, string>
        {
            ["p_id"] = id.ToString(),
            ["p_seller_un_id"] = sellerUnId.ToString(),
            ["p_driver_info"] = driverInfo,
            ["p_driver_no"] = driverNo,
            ["p_driver_is_geo"] = driverIsGeo.ToString(),
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var result = ParseIntResponse(response, "correct_driver_infoResult");

        return new CorrectDriverInfoResult
        {
            Result = result,
            Message = result > 0 ? "Driver info corrected successfully" : "Failed to correct driver info"
        };
    }

    public async Task<CorrectTransportMarkResult> CorrectTransportMarkAsync(
        int id, int sellerUnId, string transportMark, int userId, string su, string sp,
        CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("correct_transport_mark", new Dictionary<string, string>
        {
            ["p_id"] = id.ToString(),
            ["p_seller_un_id"] = sellerUnId.ToString(),
            ["p_transport_mark"] = transportMark,
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var result = ParseIntResponse(response, "correct_transport_markResult");

        return new CorrectTransportMarkResult
        {
            Result = result,
            Message = result > 0 ? "Transport mark corrected successfully" : "Failed to correct transport mark"
        };
    }

    #endregion

    #region Organization (3)

    public async Task<DataSet?> GetRsOrgObjectsAsync(int userId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_rs_org_objects", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_rs_org_objectsResult");
    }

    public async Task<DataSet?> GetVOrgObjectsByUnIdAsync(int userId, int unId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_v_org_objects_by_un_id", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["un_id"] = unId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_v_org_objects_by_un_idResult");
    }

    public async Task<DataSet?> GetVOrgObjectAddressAsync(int userId, int objectId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_v_org_object_address", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["object_id"] = objectId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_v_org_object_addressResult");
    }

    #endregion

    #region Other (3)

    public async Task<DataSet?> GetMakoreqtirebeliAsync(int userId, int invId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_makoreqtirebeli", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_makoreqtirebeliResult");
    }

    public async Task<ChangeStatusResult> AddInvToDeclAsync(int userId, int invId, string declId, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("add_inv_to_decl", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["inv_id"] = invId.ToString(),
            ["decl_id"] = declId,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        var success = ParseBoolResponse(response, "add_inv_to_declResult");

        return new ChangeStatusResult
        {
            Success = success,
            Message = success ? "Invoice added to declaration successfully" : "Failed to add invoice to declaration"
        };
    }

    public async Task<DataSet?> GetPiradiNomeriAsync(int userId, string personalNo, string su, string sp, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_piradi_nomeri", new Dictionary<string, string>
        {
            ["user_id"] = userId.ToString(),
            ["personal_no"] = personalNo,
            ["su"] = su,
            ["sp"] = sp
        }, cancellationToken);

        return ParseDataSetResponse(response, "get_piradi_nomeriResult");
    }

    #endregion

    #region Helper Methods

    private static DataSet? ParseDataSetResponse(XDocument doc, string elementName)
    {
        try
        {
            var element = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == elementName);
            if (element == null) return null;

            var dataSet = new DataSet();
            using var reader = element.CreateReader();
            reader.MoveToContent();
            dataSet.ReadXml(reader);
            return dataSet;
        }
        catch
        {
            return null;
        }
    }

    #endregion
}
