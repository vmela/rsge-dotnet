using System.Net;
using System.Text;
using System.Xml.Linq;
using RsGe.NET.Core.Soap;
using RsGe.NET.WayBill.Soap.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace RsGe.NET.WayBill.Soap;

/// <summary>
/// SOAP client implementation for RS.GE WayBill Service
/// </summary>
public class WayBillSoapClient : SoapClientBase, IWayBillSoapClient
{
    private const string DefaultServiceUrl = "http://services.rs.ge/WayBillService/WayBillService.asmx";

    private readonly string _serviceUrl;

    protected override string ServiceUrl => _serviceUrl;

    public WayBillSoapClient(HttpClient? httpClient = null, ILogger<WayBillSoapClient>? logger = null, string? serviceUrl = null)
        : base(httpClient, logger)
    {
        _serviceUrl = serviceUrl ?? DefaultServiceUrl;
    }

    #region Service Utility Methods

    public async Task<string> WhatIsMyIpAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("what_is_my_ip", new Dictionary<string, string>(), cancellationToken);
        return ParseStringResponse(response, "what_is_my_ipResult");
    }

    public async Task<bool> CheckServiceUserAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("chek_service_user", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword
        }, cancellationToken);
        return ParseBoolResponse(response, "chek_service_userResult");
    }

    public async Task<ServiceUserInfo?> GetServiceUserInfoAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("chek_service_user", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword
        }, cancellationToken);
        return ParseServiceUserInfo(response);
    }

    public async Task<List<ServiceUser>> GetServiceUsersAsync(string userName, string userPassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_service_users", new Dictionary<string, string>
        {
            ["user_name"] = userName,
            ["user_password"] = userPassword
        }, cancellationToken);
        return ParseServiceUsers(response);
    }

    public async Task<string> CheckServiceUserRawAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("chek_service_user", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword
        }, cancellationToken);
        return response.ToString();
    }

    public async Task<bool> CreateServiceUserAsync(string serviceUser, string servicePassword, string ip, string name, string tin, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("create_service_user", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["ip"] = ip,
            ["name"] = name,
            ["tin"] = tin
        }, cancellationToken);
        return ParseBoolResponse(response, "create_service_userResult");
    }

    public async Task<bool> UpdateServiceUserAsync(string serviceUser, string servicePassword, string ip, string name, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("update_service_user", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["ip"] = ip,
            ["name"] = name
        }, cancellationToken);
        return ParseBoolResponse(response, "update_service_userResult");
    }

    #endregion

    #region Reference Data Methods

    public async Task<List<WayBillType>> GetWayBillTypesAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_waybill_types", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword
        }, cancellationToken);
        return ParseWayBillTypes(response);
    }

    public async Task<List<WayBillUnit>> GetWayBillUnitsAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_waybill_units", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword
        }, cancellationToken);
        return ParseWayBillUnits(response);
    }

    public async Task<List<TransportType>> GetTransportTypesAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_trans_types", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword
        }, cancellationToken);
        return ParseTransportTypes(response);
    }

    public async Task<List<ErrorCode>> GetErrorCodesAsync(string serviceUser, string servicePassword, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_error_codes", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword
        }, cancellationToken);
        return ParseErrorCodes(response);
    }

    #endregion

    #region Bar Code Methods

    public async Task<BarCode?> GetBarCodeAsync(string serviceUser, string servicePassword, string barCode, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_bar_codes", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["bar_code"] = barCode
        }, cancellationToken);
        return ParseBarCode(response);
    }

    public async Task<bool> SaveBarCodeAsync(string serviceUser, string servicePassword, string barCode, string productName, int unitId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_bar_code", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["bar_code"] = barCode,
            ["good_name"] = productName,
            ["unit_id"] = unitId.ToString()
        }, cancellationToken);
        return ParseBoolResponse(response, "save_bar_codeResult");
    }

    #endregion

    #region TIN Lookup

    public async Task<TinInfoResponse?> GetNameFromTinAsync(string serviceUser, string servicePassword, string tin, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_name_from_tin", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["tin"] = tin
        }, cancellationToken);
        return ParseTinInfo(response, tin);
    }

    #endregion

    #region Waybill CRUD Operations

    public async Task<SaveWayBillResponse> SaveWayBillAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default)
    {
        var waybillXml = BuildWaybillXml(request);
        var response = await SendSoapRequestWithXmlAsync("save_waybill", serviceUser, servicePassword, waybillXml, cancellationToken);
        return ParseSaveWayBillResponse(response);
    }

    public async Task<WayBillDocument?> GetWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = waybillId.ToString()
        }, cancellationToken);
        return ParseWayBill(response);
    }

    public async Task<GetWayBillsResponse> GetWayBillsAsync(string serviceUser, string servicePassword, GetWayBillsRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_waybills", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_type"] = (request.WaybillType ?? 0).ToString(),
            ["create_date_s"] = request.CreateDateFrom?.ToString("yyyy-MM-dd") ?? "",
            ["create_date_e"] = request.CreateDateTo?.ToString("yyyy-MM-dd") ?? "",
            ["buyer_tin"] = request.BuyerTin ?? "",
            ["status"] = (request.Status ?? 0).ToString(),
            ["seller_tin"] = request.SellerTin ?? ""
        }, cancellationToken);
        return ParseWayBillsResponse(response);
    }

    public async Task<GetWayBillsResponse> GetBuyerWayBillsAsync(string serviceUser, string servicePassword, GetBuyerWayBillsRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_buyer_waybills", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["create_date_s"] = request.CreateDateFrom?.ToString("yyyy-MM-dd") ?? "",
            ["create_date_e"] = request.CreateDateTo?.ToString("yyyy-MM-dd") ?? "",
            ["seller_tin"] = request.SellerTin ?? "",
            ["status"] = (request.Status ?? 0).ToString()
        }, cancellationToken);
        return ParseWayBillsResponse(response);
    }

    #endregion

    #region Waybill Workflow Operations

    public async Task<WayBillOperationResponse> SendWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("send_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = waybillId.ToString()
        }, cancellationToken);
        return ParseOperationResponse(response, waybillId, "send_waybillResult");
    }

    public async Task<WayBillOperationResponse> ConfirmWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("confirm_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = waybillId.ToString()
        }, cancellationToken);
        return ParseOperationResponse(response, waybillId, "confirm_waybillResult");
    }

    public async Task<WayBillOperationResponse> RejectWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("reject_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = waybillId.ToString()
        }, cancellationToken);
        return ParseOperationResponse(response, waybillId, "reject_waybillResult");
    }

    public async Task<WayBillOperationResponse> CloseWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("close_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = waybillId.ToString()
        }, cancellationToken);
        return ParseOperationResponse(response, waybillId, "close_waybillResult");
    }

    public async Task<WayBillOperationResponse> DeleteWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("del_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = waybillId.ToString()
        }, cancellationToken);
        return ParseOperationResponse(response, waybillId, "del_waybillResult");
    }

    public async Task<WayBillOperationResponse> RefWayBillAsync(string serviceUser, string servicePassword, int waybillId, int? refWaybillId = null, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("ref_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = waybillId.ToString(),
            ["ref_waybill_id"] = (refWaybillId ?? 0).ToString()
        }, cancellationToken);
        return ParseOperationResponse(response, waybillId, "ref_waybillResult");
    }

    #endregion

    #region Sub-Waybill Operations

    public async Task<SaveWayBillResponse> SaveSubWayBillAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default)
    {
        var goodsXml = BuildGoodsXml(request.Goods);

        var response = await SendSoapRequestAsync("save_sub_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = request.Id.ToString(),
            ["waybill_type"] = request.Type.ToString(),
            ["buyer_tin"] = request.BuyerTin,
            ["buyer_name"] = request.BuyerName,
            ["end_address"] = request.EndAddress,
            ["par_id"] = (request.ParentId ?? 0).ToString(),
            ["waybill_goods_xml"] = goodsXml
        }, cancellationToken);

        return ParseSaveWayBillResponse(response);
    }

    public async Task<WayBillOperationResponse> ActivateSubWayBillAsync(string serviceUser, string servicePassword, ActivateSubWayBillRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("activate_sub_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["sub_waybill_id"] = request.Id.ToString(),
            ["begin_date"] = request.BeginDate.ToString("yyyy-MM-dd HH:mm:ss"),
            ["delivery_address"] = request.DeliveryAddress ?? ""
        }, cancellationToken);
        return ParseOperationResponse(response, request.Id, "activate_sub_waybillResult");
    }

    public async Task<WayBillOperationResponse> CloseSubWayBillAsync(string serviceUser, string servicePassword, int subWaybillId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("close_sub_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["sub_waybill_id"] = subWaybillId.ToString()
        }, cancellationToken);
        return ParseOperationResponse(response, subWaybillId, "close_sub_waybillResult");
    }

    #endregion

    #region Invoice Operations

    public async Task<bool> SaveInvoiceAsync(string serviceUser, string servicePassword, SaveInvoiceRequest request, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("save_invoice", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = request.WaybillId.ToString(),
            ["invoice_number"] = request.InvoiceNumber,
            ["invoice_date"] = request.InvoiceDate.ToString("yyyy-MM-dd"),
            ["invoice_amount"] = request.InvoiceAmount.ToString("F2")
        }, cancellationToken);
        return ParseBoolResponse(response, "save_invoiceResult");
    }

    #endregion

    #region Additional Operations

    public async Task<int> GetCarNumbersAsync(string serviceUser, string servicePassword, string carNumber, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("get_car_numbers", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["car_number"] = carNumber
        }, cancellationToken);
        return ParseIntResponse(response, "get_car_numbersResult");
    }

    public async Task<bool> SyncWayBillAsync(string serviceUser, string servicePassword, int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await SendSoapRequestAsync("sync_waybill", new Dictionary<string, string>
        {
            ["su"] = serviceUser,
            ["sp"] = servicePassword,
            ["waybill_id"] = waybillId.ToString()
        }, cancellationToken);
        return ParseBoolResponse(response, "sync_waybillResult");
    }

    public async Task<(string Request, string Response, SaveWayBillResponse ParsedResponse)> SaveWayBillDebugAsync(string serviceUser, string servicePassword, SaveWayBillRequest request, CancellationToken cancellationToken = default)
    {
        var waybillXml = BuildWaybillXml(request);
        var soapEnvelope = BuildSoapEnvelopeWithXml("save_waybill", serviceUser, servicePassword, waybillXml);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, ServiceUrl);
        httpRequest.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
        httpRequest.Headers.Add("SOAPAction", $"{SoapNamespace}save_waybill");

        var httpResponse = await HttpClient.SendAsync(httpRequest, cancellationToken);
        var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

        var doc = XDocument.Parse(responseContent);
        var parsedResponse = ParseSaveWayBillResponse(doc);

        return (soapEnvelope, responseContent, parsedResponse);
    }

    #endregion

    #region Private Parse/Build Methods

    private static string BuildGoodsXml(List<SaveWayBillGoodRequest> goods)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<GOODS>");

        foreach (var good in goods)
        {
            sb.AppendLine("  <GOOD>");
            sb.AppendLine($"    <ID>{good.Id}</ID>");
            sb.AppendLine($"    <W_NAME>{WebUtility.HtmlEncode(good.Name)}</W_NAME>");
            sb.AppendLine($"    <UNIT_ID>{good.UnitId}</UNIT_ID>");

            if (good.UnitId == 99 && !string.IsNullOrEmpty(good.UnitTxt))
            {
                sb.AppendLine($"    <UNIT_TXT>{WebUtility.HtmlEncode(good.UnitTxt)}</UNIT_TXT>");
            }

            sb.AppendLine($"    <QUANTITY>{good.Quantity}</QUANTITY>");
            sb.AppendLine($"    <PRICE>{good.Price:F2}</PRICE>");
            sb.AppendLine($"    <BAR_CODE>{WebUtility.HtmlEncode(good.BarCode)}</BAR_CODE>");
            sb.AppendLine($"    <W_CODE>{WebUtility.HtmlEncode(good.WCode)}</W_CODE>");
            sb.AppendLine($"    <VAT_TYPE>{good.VatType}</VAT_TYPE>");

            if (good.ExciseId.HasValue)
            {
                sb.AppendLine($"    <A_ID>{good.ExciseId}</A_ID>");
            }

            if (good.AStamps != null && good.AStamps.Count > 0)
            {
                sb.AppendLine("    <A_STAMPS>");
                foreach (var stamp in good.AStamps)
                {
                    sb.AppendLine($"      <A_STAMP>{WebUtility.HtmlEncode(stamp)}</A_STAMP>");
                }
                sb.AppendLine("    </A_STAMPS>");
            }

            sb.AppendLine("  </GOOD>");
        }

        sb.AppendLine("</GOODS>");
        return sb.ToString();
    }

    private static string BuildWaybillXml(SaveWayBillRequest request)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<WAYBILL>");
        sb.AppendLine("  <SUB_WAYBILLS></SUB_WAYBILLS>");

        sb.AppendLine("  <GOODS_LIST>");
        foreach (var good in request.Goods)
        {
            sb.AppendLine("    <GOODS>");
            sb.AppendLine($"      <ID>{good.Id}</ID>");
            sb.AppendLine($"      <W_NAME>{WebUtility.HtmlEncode(good.Name)}</W_NAME>");
            sb.AppendLine($"      <UNIT_ID>{good.UnitId}</UNIT_ID>");
            if (good.UnitId == 99 && !string.IsNullOrEmpty(good.UnitTxt))
            {
                sb.AppendLine($"      <UNIT_TXT>{WebUtility.HtmlEncode(good.UnitTxt)}</UNIT_TXT>");
            }
            sb.AppendLine($"      <QUANTITY>{good.Quantity}</QUANTITY>");
            sb.AppendLine($"      <PRICE>{good.Price:F2}</PRICE>");
            sb.AppendLine($"      <STATUS>1</STATUS>");
            sb.AppendLine($"      <AMOUNT>{(good.Quantity * good.Price):F2}</AMOUNT>");
            sb.AppendLine($"      <BAR_CODE>{WebUtility.HtmlEncode(good.BarCode)}</BAR_CODE>");
            sb.AppendLine($"      <A_ID>{good.ExciseId ?? 0}</A_ID>");
            sb.AppendLine($"      <VAT_TYPE>{good.VatType}</VAT_TYPE>");
            sb.AppendLine("    </GOODS>");
        }
        sb.AppendLine("  </GOODS_LIST>");

        sb.AppendLine($"  <ID>{request.Id}</ID>");
        sb.AppendLine($"  <TYPE>{request.Type}</TYPE>");
        sb.AppendLine($"  <BUYER_TIN>{WebUtility.HtmlEncode(request.BuyerTin)}</BUYER_TIN>");
        sb.AppendLine($"  <CHEK_BUYER_TIN>{request.CheckBuyerTin ?? 1}</CHEK_BUYER_TIN>");
        sb.AppendLine($"  <BUYER_NAME>{WebUtility.HtmlEncode(request.BuyerName)}</BUYER_NAME>");
        sb.AppendLine($"  <START_ADDRESS>{WebUtility.HtmlEncode(request.StartAddress)}</START_ADDRESS>");
        sb.AppendLine($"  <END_ADDRESS>{WebUtility.HtmlEncode(request.EndAddress)}</END_ADDRESS>");
        sb.AppendLine($"  <DRIVER_TIN>{WebUtility.HtmlEncode(request.DriverTin)}</DRIVER_TIN>");
        sb.AppendLine($"  <CHEK_DRIVER_TIN>{request.CheckDriverTin ?? 1}</CHEK_DRIVER_TIN>");
        sb.AppendLine($"  <DRIVER_NAME>{WebUtility.HtmlEncode(request.DriverName)}</DRIVER_NAME>");
        sb.AppendLine($"  <TRANSPORT_COAST>0</TRANSPORT_COAST>");
        sb.AppendLine($"  <RECEPTION_INFO></RECEPTION_INFO>");
        sb.AppendLine($"  <RECEIVER_INFO></RECEIVER_INFO>");
        sb.AppendLine($"  <DELIVERY_DATE></DELIVERY_DATE>");
        sb.AppendLine($"  <STATUS>0</STATUS>");
        sb.AppendLine($"  <SELER_UN_ID>{WebUtility.HtmlEncode(request.SellerUnId)}</SELER_UN_ID>");
        sb.AppendLine($"  <PAR_ID>{request.ParentId ?? 0}</PAR_ID>");
        sb.AppendLine($"  <FULL_AMOUNT>{WebUtility.HtmlEncode(request.FullAmount ?? "")}</FULL_AMOUNT>");
        sb.AppendLine($"  <CAR_NUMBER>{WebUtility.HtmlEncode(request.CarNumber)}</CAR_NUMBER>");
        sb.AppendLine($"  <WAYBILL_NUMBER></WAYBILL_NUMBER>");
        sb.AppendLine($"  <S_USER_ID></S_USER_ID>");
        sb.AppendLine($"  <BEGIN_DATE></BEGIN_DATE>");
        sb.AppendLine($"  <TRAN_COST_PAYER>1</TRAN_COST_PAYER>");
        sb.AppendLine($"  <TRANS_ID>{request.TransportTypeId}</TRANS_ID>");
        sb.AppendLine($"  <TRANS_TXT></TRANS_TXT>");
        sb.AppendLine($"  <COMMENT>{WebUtility.HtmlEncode(request.Comment ?? "")}</COMMENT>");
        sb.AppendLine($"  <CATEGORY></CATEGORY>");
        sb.AppendLine($"  <IS_MED></IS_MED>");
        sb.AppendLine("</WAYBILL>");

        return sb.ToString();
    }

    private static List<WayBillType> ParseWayBillTypes(XDocument doc)
    {
        var result = new List<WayBillType>();

        foreach (var typeElement in doc.Descendants().Where(e => e.Name.LocalName == "WAYBILL_TYPE"))
        {
            result.Add(new WayBillType
            {
                Id = int.TryParse(typeElement.Elements().FirstOrDefault(e => e.Name.LocalName == "ID")?.Value, out var id) ? id : 0,
                Name = typeElement.Elements().FirstOrDefault(e => e.Name.LocalName == "NAME")?.Value ?? string.Empty
            });
        }

        return result;
    }

    private static List<WayBillUnit> ParseWayBillUnits(XDocument doc)
    {
        var result = new List<WayBillUnit>();

        foreach (var unitElement in doc.Descendants().Where(e => e.Name.LocalName == "WAYBILL_UNIT"))
        {
            result.Add(new WayBillUnit
            {
                Id = int.TryParse(unitElement.Elements().FirstOrDefault(e => e.Name.LocalName == "ID")?.Value, out var id) ? id : 0,
                Name = unitElement.Elements().FirstOrDefault(e => e.Name.LocalName == "NAME")?.Value ?? string.Empty
            });
        }

        return result;
    }

    private static List<TransportType> ParseTransportTypes(XDocument doc)
    {
        var result = new List<TransportType>();

        foreach (var typeElement in doc.Descendants().Where(e => e.Name.LocalName == "TRANSPORT_TYPE"))
        {
            result.Add(new TransportType
            {
                Id = int.TryParse(typeElement.Elements().FirstOrDefault(e => e.Name.LocalName == "ID")?.Value, out var id) ? id : 0,
                Name = typeElement.Elements().FirstOrDefault(e => e.Name.LocalName == "NAME")?.Value ?? string.Empty
            });
        }

        return result;
    }

    private static List<ErrorCode> ParseErrorCodes(XDocument doc)
    {
        var result = new List<ErrorCode>();
        var ns = GetResponseNamespace();

        var errorElements = doc.Descendants().Where(e =>
            e.Name.LocalName is "ERROR_CODE" or "ERROR" or "ERRORCODE");

        foreach (var element in errorElements)
        {
            string GetVal(string name) => element.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;

            result.Add(new ErrorCode
            {
                Id = int.TryParse(GetVal("ID"), out var id) ? id : 0,
                Text = GetVal("TEXT") is { Length: > 0 } t ? t
                    : GetVal("NAME") is { Length: > 0 } n ? n
                    : GetVal("DESCRIPTION")
            });
        }

        if (result.Count == 0)
        {
            var rawResult = doc.Descendants(ns + "get_error_codesResult").FirstOrDefault()?.Value;
            if (!string.IsNullOrEmpty(rawResult))
            {
                result.Add(new ErrorCode { Id = 0, Text = rawResult });
            }
        }

        return result;
    }

    private static ServiceUserInfo? ParseServiceUserInfo(XDocument doc)
    {
        var ns = GetResponseNamespace();
        var rawResult = doc.Descendants(ns + "chek_service_userResult").FirstOrDefault()?.Value;

        if (string.IsNullOrEmpty(rawResult))
            return null;

        var result = new ServiceUserInfo { RawResponse = rawResult };

        var unId = doc.Descendants(ns + "un_id").FirstOrDefault()?.Value;
        var sUserId = doc.Descendants(ns + "s_user_id").FirstOrDefault()?.Value;

        if (!string.IsNullOrEmpty(unId))
        {
            result.PayerId = unId;
        }
        if (!string.IsNullOrEmpty(sUserId))
        {
            result.UserId = sUserId;
        }

        return result;
    }

    private static List<ServiceUser> ParseServiceUsers(XDocument doc)
    {
        var result = new List<ServiceUser>();

        foreach (var userElement in doc.Descendants().Where(e => e.Name.LocalName == "ServiceUser"))
        {
            string GetVal(string name) => userElement.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;

            result.Add(new ServiceUser
            {
                Id = int.TryParse(GetVal("ID"), out var id) ? id : 0,
                UserName = GetVal("USER_NAME"),
                UnId = GetVal("UN_ID"),
                Ip = GetVal("IP"),
                Name = GetVal("NAME")
            });
        }

        return result;
    }

    private static BarCode? ParseBarCode(XDocument doc)
    {
        var element = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "BAR_CODE_INFO");

        if (element == null) return null;

        string GetVal(string name) => element.Elements().FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;

        return new BarCode
        {
            Code = GetVal("BAR_CODE"),
            ProductName = GetVal("GOOD_NAME"),
            UnitId = int.TryParse(GetVal("UNIT_ID"), out var unitId) ? unitId : 0,
            UnitName = GetVal("UNIT_NAME")
        };
    }

    private static TinInfoResponse? ParseTinInfo(XDocument doc, string tin)
    {
        var ns = GetResponseNamespace();
        var name = doc.Descendants(ns + "get_name_from_tinResult").FirstOrDefault()?.Value;

        var isValid = !string.IsNullOrWhiteSpace(name)
            && !name.Equals("null", StringComparison.OrdinalIgnoreCase)
            && !name.StartsWith("-")
            && !name.Contains("არ არის")
            && !name.Contains("არასწორი")
            && name.Length > 2;

        return new TinInfoResponse
        {
            Tin = tin,
            Name = isValid ? name! : string.Empty,
            IsValid = isValid
        };
    }

    private static SaveWayBillResponse ParseSaveWayBillResponse(XDocument doc)
    {
        var ns = GetResponseNamespace();

        var resultElement = doc.Descendants("RESULT").FirstOrDefault();
        if (resultElement != null)
        {
            var status = int.TryParse(resultElement.Element("STATUS")?.Value, out var s) ? s : 0;
            var id = int.TryParse(resultElement.Element("ID")?.Value, out var i) ? i : 0;
            var waybillNumber = resultElement.Element("WAYBILL_NUMBER")?.Value ?? string.Empty;

            if (status < 0)
            {
                return new SaveWayBillResponse
                {
                    Id = status,
                    WaybillNumber = string.Empty,
                    ErrorMessage = $"RS.GE returned error code: {status}"
                };
            }

            return new SaveWayBillResponse
            {
                Id = id > 0 ? id : status,
                WaybillNumber = waybillNumber
            };
        }

        var result = doc.Descendants(ns + "save_waybillResult").FirstOrDefault()?.Value ?? "0";
        var parts = result.Split('|');
        var parsedId = int.TryParse(parts.FirstOrDefault(), out var pid) ? pid : 0;

        if (parsedId <= 0)
        {
            return new SaveWayBillResponse
            {
                Id = parsedId,
                WaybillNumber = string.Empty,
                ErrorMessage = parts.Length > 1 ? parts[1] : $"RS.GE returned error code: {parsedId}"
            };
        }

        return new SaveWayBillResponse
        {
            Id = parsedId,
            WaybillNumber = parts.Length > 1 ? parts[1] : string.Empty
        };
    }

    private static WayBillDocument? ParseWayBill(XDocument doc)
    {
        var headerElement = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "WAYBILL");

        if (headerElement == null) return null;

        string GetValue(XElement parent, string name) => parent.Elements()
            .FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;

        var waybill = new WayBillDocument
        {
            Header = new WayBillHeader
            {
                Id = int.TryParse(GetValue(headerElement, "ID"), out var id) ? id : 0,
                Type = int.TryParse(GetValue(headerElement, "TYPE"), out var type) ? type : 0,
                CreateDate = DateTime.TryParse(GetValue(headerElement, "CREATE_DATE"), out var createDate) ? createDate : DateTime.MinValue,
                BuyerTin = GetValue(headerElement, "BUYER_TIN"),
                BuyerName = GetValue(headerElement, "BUYER_NAME"),
                SellerTin = GetValue(headerElement, "SELLER_TIN"),
                SellerName = GetValue(headerElement, "SELLER_NAME"),
                StartAddress = GetValue(headerElement, "START_ADDRESS"),
                EndAddress = GetValue(headerElement, "END_ADDRESS"),
                TransportTypeId = int.TryParse(GetValue(headerElement, "TRANSPORT_TYPE_ID"), out var transType) ? transType : 0,
                DriverTin = GetValue(headerElement, "DRIVER_TIN"),
                DriverName = GetValue(headerElement, "DRIVER_NAME"),
                CarNumber = GetValue(headerElement, "CAR_NUMBER"),
                Status = int.TryParse(GetValue(headerElement, "STATUS"), out var status) ? status : 0,
                WaybillNumber = GetValue(headerElement, "WAYBILL_NUMBER")
            }
        };

        var goodsListElement = headerElement.Descendants().FirstOrDefault(e => e.Name.LocalName == "GOODS_LIST");
        var goodElements = goodsListElement != null
            ? goodsListElement.Elements().Where(e => e.Name.LocalName == "GOODS")
            : headerElement.Descendants().Where(e => e.Name.LocalName == "GOODS");

        foreach (var goodElement in goodElements)
        {
            waybill.Goods.Add(new WayBillGood
            {
                Id = int.TryParse(GetValue(goodElement, "ID"), out var goodId) ? goodId : 0,
                WCode = GetValue(goodElement, "W_CODE"),
                BarCode = GetValue(goodElement, "BAR_CODE"),
                Name = GetValue(goodElement, "W_NAME"),
                UnitId = int.TryParse(GetValue(goodElement, "UNIT_ID"), out var unitId) ? unitId : 0,
                UnitName = GetValue(goodElement, "UNIT_NAME"),
                Quantity = decimal.TryParse(GetValue(goodElement, "QUANTITY"), out var qty) ? qty : 0,
                Price = decimal.TryParse(GetValue(goodElement, "PRICE"), out var price) ? price : 0,
                Amount = decimal.TryParse(GetValue(goodElement, "AMOUNT"), out var amount) ? amount : 0,
                VatType = GetValue(goodElement, "VAT_TYPE")
            });
        }

        return waybill;
    }

    private static GetWayBillsResponse ParseWayBillsResponse(XDocument doc)
    {
        var result = new GetWayBillsResponse();

        var waybillElements = doc.Descendants().Where(e => e.Name.LocalName == "WAYBILL");

        foreach (var waybillElement in waybillElements)
        {
            string GetValue(string name) => waybillElement.Elements()
                .FirstOrDefault(e => e.Name.LocalName == name)?.Value ?? string.Empty;

            result.WayBills.Add(new WayBillSummary
            {
                Id = int.TryParse(GetValue("ID"), out var id) ? id : 0,
                WaybillNumber = GetValue("WAYBILL_NUMBER"),
                Type = int.TryParse(GetValue("TYPE"), out var type) ? type : 0,
                CreateDate = DateTime.TryParse(GetValue("CREATE_DATE"), out var createDate) ? createDate : DateTime.MinValue,
                BuyerTin = GetValue("BUYER_TIN"),
                BuyerName = GetValue("BUYER_NAME"),
                SellerTin = GetValue("SELLER_TIN"),
                SellerName = GetValue("SELLER_NAME"),
                TotalAmount = decimal.TryParse(GetValue("FULL_AMOUNT"), out var amount) ? amount : 0,
                Status = int.TryParse(GetValue("STATUS"), out var status) ? status : 0
            });
        }

        result.TotalCount = result.WayBills.Count;
        return result;
    }

    private static WayBillOperationResponse ParseOperationResponse(XDocument doc, int waybillId, string elementName)
    {
        var value = ParseStringResponse(doc, elementName);
        var isSuccess = value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase) || int.TryParse(value, out var code) && code > 0;

        return new WayBillOperationResponse
        {
            WaybillId = waybillId,
            IsSuccess = isSuccess,
            Message = isSuccess ? "Success" : value
        };
    }

    #endregion
}
