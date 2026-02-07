# RsGe.NET — Georgian Revenue Service .NET SDK

A comprehensive .NET SDK for integrating with [RS.GE](https://www.rs.ge) (Georgian Revenue Service / საქართველოს შემოსავლების სამსახური) SOAP web services.

## Packages

| Package | Description | Service |
|---------|-------------|---------|
| **RsGe.NET.Core** | Shared SOAP infrastructure, configuration, exceptions | — |
| **RsGe.NET.WayBill** | Electronic waybills (ელექტრონული ზედნადები) | [WayBillService](http://services.rs.ge/WayBillService/WayBillService.asmx) |
| **RsGe.NET.TaxPayer** | Taxpayer info, Z-reports, compliance (საგადასახადო სერვისები) | [TaxPayerService](https://services.rs.ge/taxservice/taxpayerservice.asmx) |
| **RsGe.NET.Invoice** | Tax invoices (ელექტრონული ანგარიშ-ფაქტურა) | [NtosService](https://www.revenue.mof.ge/ntosservice/ntosservice.asmx) |
| **RsGe.NET.SpecInvoice** | Special invoices for regulated products (სპეციალური ანგარიშ-ფაქტურა) | [SpecInvoicesService](https://www.revenue.mof.ge/SpecInvoicesService/SpecInvoicesService.asmx) |

## Supported .NET Versions

- .NET 8.0
- .NET 9.0
- .NET 10.0

## Quick Start

### Installation

```bash
# Install the packages you need
dotnet add package RsGe.NET.WayBill
dotnet add package RsGe.NET.TaxPayer
dotnet add package RsGe.NET.Invoice
dotnet add package RsGe.NET.SpecInvoice
```

### WayBill — Create and Send a Waybill

```csharp
using RsGe.NET.WayBill;
using RsGe.NET.WayBill.Models;

// Option 1: Factory (no DI)
var service = RsGeServiceFactory.CreateWayBillService("su", "sp");

// Option 2: DI registration
services.AddRsGeWayBillServices(config =>
{
    config.ServiceUser = "MYAPP:206322102";
    config.ServicePassword = "password";
    config.CompanyTin = "206322102";
});

// Create a waybill
var result = await service.CreateWayBillAsync(new CreateWayBillRequest
{
    Type = WayBillTypeEnum.Sale,
    SellerUnId = "731937",
    BuyerTin = "123456789",
    StartAddress = "თბილისი, რუსთაველის 1",
    EndAddress = "ბათუმი, გორგილაძის 5",
    TransportType = TransportTypeEnum.Vehicle,
    CarNumber = "AA001BB",
    DriverTin = "01234567890",
    Items = new()
    {
        new() { ProductName = "კომპიუტერი", UnitId = 1, Quantity = 5, UnitPrice = 1500m }
    }
});

if (result.IsSuccess)
{
    await service.SendWayBillAsync(result.WaybillId);
}
```

### TaxPayer — Lookup Company Info

```csharp
using RsGe.NET.TaxPayer;

services.AddRsGeTaxPayerServices(config =>
{
    config.Username = "portal_user";
    config.Password = "portal_pass";
});

var info = await taxPayerService.GetTaxPayerInfoAsync("206322102");
Console.WriteLine($"{info.Name} — {info.Status}");
```

### Invoice — Create a Tax Invoice

```csharp
using RsGe.NET.Invoice;

services.AddRsGeInvoiceServices(config =>
{
    config.ServiceUser = "su";
    config.ServicePassword = "sp";
});

var (success, invoiceId) = await invoiceService.SaveInvoiceAsync(request);
```

## Architecture

Each package follows a 3-layer architecture:

```
┌─────────────────────────────┐
│   High-Level Service        │  IRsGe{X}Service — .NET-friendly API
│   (hides credentials)       │  with DTOs and simplified methods
├─────────────────────────────┤
│   SOAP Client               │  I{X}SoapClient — 1:1 mapping to
│   (low-level)               │  SOAP operations with full params
├─────────────────────────────┤
│   SoapClientBase            │  Shared HTTP/SOAP/XML infrastructure
│   (RsGe.NET.Core)           │  with envelope building & parsing
└─────────────────────────────┘
```

- **High-level service** — Handles credentials, maps between DTOs and SOAP models. Use this for most applications.
- **SOAP client** — Direct access to all SOAP operations. Use when you need full control over parameters.
- **SoapClientBase** — Abstract base with `SendSoapRequestAsync()`, response parsing helpers.

## Configuration

### appsettings.json

```json
{
  "RsGe": {
    "ServiceUser": "MYAPP:206322102",
    "ServicePassword": "MySecurePassword",
    "ServiceUrl": null,

    "TaxPayer": {
      "Username": "portal_username",
      "Password": "portal_password"
    }
  }
}
```

### Dependency Injection

```csharp
// Register services
builder.Services.AddRsGeWayBillServices(builder.Configuration);
builder.Services.AddRsGeTaxPayerServices(builder.Configuration);
builder.Services.AddRsGeInvoiceServices(builder.Configuration);
builder.Services.AddRsGeSpecInvoiceServices(builder.Configuration);

// Inject and use
public class MyController(IRsGeWayBillService waybillService)
{
    public async Task<IActionResult> GetWaybill(int id)
    {
        var waybill = await waybillService.GetWayBillAsync(id);
        return Ok(waybill);
    }
}
```

## REST API Gateway

The `RsGe.NET.Server` project provides a ready-to-use REST API that wraps all services:

```bash
cd src/RsGe.NET.Server
dotnet run
```

Endpoints: `/api/waybills`, `/api/reference`, `/api/company/{tin}`, `/api/reports`, `/health`

## Documentation

Detailed documentation for each service:

- [Authentication & Service Users](docs/authentication.md)
- [Electronic Waybills (ზედნადები)](docs/waybills.md)
- [Tax Invoices (ანგარიშ-ფაქტურა)](docs/tax-invoices.md)
- [Special Invoices (სპეციალური ანგარიშ-ფაქტურა)](docs/spec-invoices.md)
- [Taxpayer Services (საგადასახადო სერვისები)](docs/taxpayer.md)
- [Error Codes & Troubleshooting](docs/error-codes.md)

## License

MIT License. See [LICENSE](LICENSE) for details.
