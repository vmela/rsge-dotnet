using RsGe.NET.WayBill.Models;
using RsGe.NET.WayBill.Soap;
using RsGe.NET.WayBill.Soap.Models;

namespace RsGe.NET.WayBill.Services;

public class RsGeWayBillService : IRsGeWayBillService
{
    private readonly IWayBillSoapClient _soapClient;
    private readonly string _serviceUser;
    private readonly string _servicePassword;

    public RsGeWayBillService(IWayBillSoapClient soapClient, string serviceUser, string servicePassword)
    {
        _soapClient = soapClient ?? throw new ArgumentNullException(nameof(soapClient));
        _serviceUser = serviceUser ?? throw new ArgumentNullException(nameof(serviceUser));
        _servicePassword = servicePassword ?? throw new ArgumentNullException(nameof(servicePassword));
    }

    public async Task<List<ReferenceItemDto>> GetWayBillTypesAsync(CancellationToken cancellationToken = default)
    {
        var types = await _soapClient.GetWayBillTypesAsync(_serviceUser, _servicePassword, cancellationToken);
        return types.Select(t => new ReferenceItemDto { Id = t.Id, Name = t.Name }).ToList();
    }

    public async Task<List<ReferenceItemDto>> GetUnitsAsync(CancellationToken cancellationToken = default)
    {
        var units = await _soapClient.GetWayBillUnitsAsync(_serviceUser, _servicePassword, cancellationToken);
        return units.Select(u => new ReferenceItemDto { Id = u.Id, Name = u.Name }).ToList();
    }

    public async Task<List<ReferenceItemDto>> GetTransportTypesAsync(CancellationToken cancellationToken = default)
    {
        var types = await _soapClient.GetTransportTypesAsync(_serviceUser, _servicePassword, cancellationToken);
        return types.Select(t => new ReferenceItemDto { Id = t.Id, Name = t.Name }).ToList();
    }

    public async Task<CompanyInfoDto?> GetCompanyByTinAsync(string tin, CancellationToken cancellationToken = default)
    {
        var result = await _soapClient.GetNameFromTinAsync(_serviceUser, _servicePassword, tin, cancellationToken);
        if (result == null) return null;

        return new CompanyInfoDto
        {
            Tin = result.Tin,
            Name = result.Name,
            IsValid = result.IsValid
        };
    }

    public async Task<CreateWayBillResult> CreateWayBillAsync(CreateWayBillRequest request, CancellationToken cancellationToken = default)
    {
        var soapRequest = new SaveWayBillRequest
        {
            Id = 0,
            Type = (int)request.Type,
            SellerUnId = request.SellerUnId,
            BuyerTin = request.BuyerTin,
            CheckBuyerTin = request.ValidateBuyerTin ? 1 : 0,
            BuyerName = request.BuyerName ?? string.Empty,
            StartAddress = request.StartAddress,
            EndAddress = request.EndAddress,
            DriverTin = request.DriverTin,
            CheckDriverTin = request.ValidateDriverTin ? 1 : 0,
            DriverName = request.DriverName ?? string.Empty,
            TransportTypeId = (int)request.TransportType,
            CarNumber = request.CarNumber,
            TrailerNumber = request.TrailerNumber ?? string.Empty,
            Comment = request.Comment ?? string.Empty,
            Goods = request.Items.Select((i, index) => new SaveWayBillGoodRequest
            {
                Id = 0,
                WCode = i.ProductCode ?? string.Empty,
                BarCode = i.BarCode ?? string.Empty,
                Name = i.ProductName,
                UnitId = i.UnitId,
                UnitTxt = i.UnitId == 99 ? (i.UnitTxt ?? "ცალი") : null,
                Quantity = i.Quantity,
                Price = i.UnitPrice,
                VatType = ((int)i.VatType).ToString()
            }).ToList()
        };

        try
        {
            var response = await _soapClient.SaveWayBillAsync(_serviceUser, _servicePassword, soapRequest, cancellationToken);
            return new CreateWayBillResult
            {
                IsSuccess = response.Id > 0,
                WaybillId = response.Id,
                WaybillNumber = response.WaybillNumber,
                ErrorMessage = response.ErrorMessage
            };
        }
        catch (Exception ex)
        {
            return new CreateWayBillResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<WayBillDto?> GetWayBillAsync(int waybillId, CancellationToken cancellationToken = default)
    {
        var waybill = await _soapClient.GetWayBillAsync(_serviceUser, _servicePassword, waybillId, cancellationToken);
        return waybill == null ? null : MapToDto(waybill);
    }

    public async Task<List<WayBillListItemDto>> GetWayBillsAsync(WayBillFilterOptions? filter = null, CancellationToken cancellationToken = default)
    {
        var request = new GetWayBillsRequest
        {
            WaybillType = filter?.Type != null ? (int)filter.Type : null,
            Status = filter?.Status != null ? (int)filter.Status : null,
            BuyerTin = filter?.BuyerTin,
            SellerTin = filter?.SellerTin,
            CreateDateFrom = filter?.CreatedFrom,
            CreateDateTo = filter?.CreatedTo
        };

        var response = await _soapClient.GetWayBillsAsync(_serviceUser, _servicePassword, request, cancellationToken);
        return response.WayBills.Select(MapToListItemDto).ToList();
    }

    public async Task<List<WayBillListItemDto>> GetBuyerWayBillsAsync(WayBillFilterOptions? filter = null, CancellationToken cancellationToken = default)
    {
        var request = new GetBuyerWayBillsRequest
        {
            Status = filter?.Status != null ? (int)filter.Status : null,
            SellerTin = filter?.SellerTin,
            CreateDateFrom = filter?.CreatedFrom,
            CreateDateTo = filter?.CreatedTo
        };

        var response = await _soapClient.GetBuyerWayBillsAsync(_serviceUser, _servicePassword, request, cancellationToken);
        return response.WayBills.Select(MapToListItemDto).ToList();
    }

    public async Task<WayBillOperationResult> SendWayBillAsync(int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await _soapClient.SendWayBillAsync(_serviceUser, _servicePassword, waybillId, cancellationToken);
        return MapOperationResult(response);
    }

    public async Task<WayBillOperationResult> ConfirmWayBillAsync(int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await _soapClient.ConfirmWayBillAsync(_serviceUser, _servicePassword, waybillId, cancellationToken);
        return MapOperationResult(response);
    }

    public async Task<WayBillOperationResult> RejectWayBillAsync(int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await _soapClient.RejectWayBillAsync(_serviceUser, _servicePassword, waybillId, cancellationToken);
        return MapOperationResult(response);
    }

    public async Task<WayBillOperationResult> CloseWayBillAsync(int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await _soapClient.CloseWayBillAsync(_serviceUser, _servicePassword, waybillId, cancellationToken);
        return MapOperationResult(response);
    }

    public async Task<WayBillOperationResult> DeleteWayBillAsync(int waybillId, CancellationToken cancellationToken = default)
    {
        var response = await _soapClient.DeleteWayBillAsync(_serviceUser, _servicePassword, waybillId, cancellationToken);
        return MapOperationResult(response);
    }

    public async Task<WayBillOperationResult> ReferenceWayBillAsync(int waybillId, int referenceWaybillId, CancellationToken cancellationToken = default)
    {
        var response = await _soapClient.RefWayBillAsync(_serviceUser, _servicePassword, waybillId, referenceWaybillId, cancellationToken);
        return MapOperationResult(response);
    }

    public async Task<List<WayBillDto>> GetSalesAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var filter = new WayBillFilterOptions
        {
            Type = WayBillTypeEnum.Sale,
            CreatedFrom = from,
            CreatedTo = to
        };

        var waybills = await GetWayBillsAsync(filter, cancellationToken);
        var results = new List<WayBillDto>();

        foreach (var item in waybills)
        {
            var fullWaybill = await GetWayBillAsync(item.Id, cancellationToken);
            if (fullWaybill != null)
            {
                results.Add(fullWaybill);
            }
        }

        return results;
    }

    public async Task<List<WayBillDto>> GetPurchasesAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
    {
        var filter = new WayBillFilterOptions
        {
            CreatedFrom = from,
            CreatedTo = to
        };

        var waybills = await GetBuyerWayBillsAsync(filter, cancellationToken);
        var results = new List<WayBillDto>();

        foreach (var item in waybills)
        {
            var fullWaybill = await GetWayBillAsync(item.Id, cancellationToken);
            if (fullWaybill != null)
            {
                results.Add(fullWaybill);
            }
        }

        return results;
    }

    public async Task<List<WayBillDto>> GetCompletedWayBillsAsync(DateTime since, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var sales = await GetSalesAsync(since, now, cancellationToken);
        var purchases = await GetPurchasesAsync(since, now, cancellationToken);

        return sales.Concat(purchases)
            .OrderBy(w => w.CreatedAt)
            .ToList();
    }

    public async Task<List<WayBillListItemDto>> GetIncomingWaybillsAsync(DateTime from, DateTime to, int[]? statuses = null, CancellationToken cancellationToken = default)
    {
        var results = new List<WayBillListItemDto>();
        var statusesToFetch = statuses ?? new[] { 1, 2 };

        foreach (var status in statusesToFetch)
        {
            var filter = new WayBillFilterOptions
            {
                CreatedFrom = from,
                CreatedTo = to,
                Status = (WayBillStatusEnum)status
            };

            var waybills = await GetBuyerWayBillsAsync(filter, cancellationToken);
            results.AddRange(waybills);
        }

        return results
            .GroupBy(w => w.Id)
            .Select(g => g.First())
            .OrderByDescending(w => w.CreatedAt)
            .ToList();
    }

    #region Mapping Helpers

    private static WayBillDto MapToDto(Soap.Models.WayBillDocument waybill)
    {
        return new WayBillDto
        {
            Id = waybill.Header.Id,
            WaybillNumber = waybill.Header.WaybillNumber,
            Type = (WayBillTypeEnum)waybill.Header.Type,
            Status = (WayBillStatusEnum)waybill.Header.Status,
            CreatedAt = waybill.Header.CreateDate,
            ActivatedAt = waybill.Header.ActivateDate,
            DeliveredAt = waybill.Header.DeliveryDate,
            SellerTin = waybill.Header.SellerTin,
            SellerName = waybill.Header.SellerName,
            StartAddress = waybill.Header.StartAddress,
            BuyerTin = waybill.Header.BuyerTin,
            BuyerName = waybill.Header.BuyerName,
            EndAddress = waybill.Header.EndAddress,
            TransportType = (TransportTypeEnum)waybill.Header.TransportTypeId,
            DriverTin = waybill.Header.DriverTin,
            DriverName = waybill.Header.DriverName,
            CarNumber = waybill.Header.CarNumber,
            TrailerNumber = waybill.Header.TrailerNumber,
            Comment = waybill.Header.Comment,
            ParentWaybillId = waybill.Header.ParentId,
            TotalAmount = waybill.Goods.Sum(g => g.Amount),
            Items = waybill.Goods.Select(g => new WayBillItemDto
            {
                Id = g.Id,
                ProductCode = g.WCode,
                BarCode = g.BarCode,
                ProductName = g.Name,
                UnitId = g.UnitId,
                UnitName = g.UnitName,
                Quantity = g.Quantity,
                UnitPrice = g.Price,
                TotalAmount = g.Amount,
                VatType = ParseVatType(g.VatType)
            }).ToList()
        };
    }

    private static WayBillListItemDto MapToListItemDto(WayBillSummary summary)
    {
        return new WayBillListItemDto
        {
            Id = summary.Id,
            WaybillNumber = summary.WaybillNumber,
            Type = (WayBillTypeEnum)summary.Type,
            Status = (WayBillStatusEnum)summary.Status,
            CreatedAt = summary.CreateDate,
            BuyerTin = summary.BuyerTin,
            BuyerName = summary.BuyerName,
            SellerTin = summary.SellerTin,
            SellerName = summary.SellerName,
            TotalAmount = summary.TotalAmount
        };
    }

    private static WayBillOperationResult MapOperationResult(WayBillOperationResponse response)
    {
        return new WayBillOperationResult
        {
            IsSuccess = response.IsSuccess,
            WaybillId = response.WaybillId,
            ErrorMessage = response.IsSuccess ? null : response.Message
        };
    }

    private static VatTypeEnum ParseVatType(string? vatType)
    {
        if (string.IsNullOrEmpty(vatType))
            return VatTypeEnum.Vat;

        return vatType switch
        {
            "0" => VatTypeEnum.Vat,
            "1" => VatTypeEnum.ZeroVat,
            "2" => VatTypeEnum.NoVat,
            _ when vatType.ToUpperInvariant() == "VAT" => VatTypeEnum.Vat,
            _ => VatTypeEnum.NoVat
        };
    }

    #endregion
}
