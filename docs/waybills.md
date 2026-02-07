# Electronic Waybills (ელექტრონული ზედნადები)

**Package:** `RsGe.NET.WayBill`
**SOAP Endpoint:** `http://services.rs.ge/WayBillService/WayBillService.asmx`
**Operations:** 56

## What is a Waybill?

An electronic waybill (ელექტრონული ზედნადები / eზედნადები) is a mandatory document for transporting goods within Georgia. It tracks the movement of goods from seller to buyer and is required by the Georgian Revenue Service for tax compliance.

Every business-to-business goods transfer must be accompanied by a waybill — whether it's a sale, internal transfer, distribution, or return.

## Waybill Types (ზედნადების ტიპები)

| ID | Type | Georgian | Description |
|----|------|----------|-------------|
| 1 | Inner | შიდა | Internal transfer between company locations |
| 2 | Sale | გაყიდვა | Standard sale with transport |
| 3 | SaleNoTransport | გაყიდვა ტრანსპორტირების გარეშე | Sale without transport (e.g., buyer picks up) |
| 4 | Distribution | დისტრიბუცია | Distribution to multiple buyers |
| 5 | Return | უკან დაბრუნება | Return of previously sold goods |

## Waybill Statuses (სტატუსები)

| Code | Status | Georgian | Description |
|------|--------|----------|-------------|
| 0 | Draft | მონახაზი | Created but not yet sent. Can be edited or deleted. |
| 1 | Active | აქტიური | Sent/activated. Goods are in transit. |
| 2 | Completed | დასრულებული | Delivered and closed. |
| -1 | Cancelled | გაუქმებული | Cancelled by the seller. |
| -2 | Rejected | უარყოფილი | Rejected by the buyer. |

## Waybill Lifecycle

```
Draft (0) ──send──> Active (1) ──close──> Completed (2)
    │                   │
    │                   ├──reject──> Rejected (-2)
    │                   │
    └──delete           └──ref──> Cancelled (-1)
```

## Transport Types (ტრანსპორტის ტიპები)

| ID | Type | Georgian |
|----|------|----------|
| 1 | Vehicle | ავტოსატრანსპორტო |
| 2 | Railway | სარკინიგზო |
| 3 | Air | საჰაერო |
| 4 | Other | სხვა |

## Key Parameters

### Waybill Header Fields

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `buyer_tin` | string | მყიდველის საიდენტიფიკაციო | Buyer's Tax Identification Number |
| `buyer_name` | string | მყიდველის სახელი | Buyer's name (auto-filled from TIN) |
| `seller_un_id` | string | გამყიდველის უნიფიცირებული ID | Seller's unified ID in RS.GE |
| `start_address` | string | გამოსვლის მისამართი | Origin/warehouse address |
| `end_address` | string | დანიშნულების მისამართი | Destination address |
| `driver_tin` | string | მძღოლის პირადი ნომერი | Driver's personal number |
| `driver_name` | string | მძღოლის სახელი | Driver's name |
| `car_number` | string | ავტომობილის ნომერი | Vehicle plate number |
| `transport_type_id` | int | ტრანსპორტის ტიპი | Transport type (1-4) |
| `comment` | string | კომენტარი | Optional comment |
| `chek_buyer_tin` | int | | 1 = validate buyer TIN, 0 = skip validation |
| `chek_driver_tin` | int | | 1 = validate driver TIN, 0 = skip |

### Goods (საქონელი) Fields

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `w_name` | string | საქონლის დასახელება | Product name |
| `unit_id` | int | ზომის ერთეული | Measurement unit ID |
| `unit_txt` | string | ზომის ერთეული (ტექსტი) | Custom unit text (when unit_id=99) |
| `quantity` | decimal | რაოდენობა | Quantity |
| `price` | decimal | ფასი | Unit price |
| `bar_code` | string | შტრიხკოდი | Product barcode |
| `vat_type` | string | დღგ ტიპი | VAT type: 0=VAT, 1=ZeroVAT, 2=NoVAT |
| `a_id` | int | აქციზის კოდი | Excise code (for regulated products) |

## Common Operations

### Create and Send a Waybill

```csharp
// Using high-level service
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
    Items = new List<CreateWayBillItemRequest>
    {
        new()
        {
            ProductName = "კომპიუტერი",
            UnitId = 1,           // pieces (ცალი)
            Quantity = 5,
            UnitPrice = 1500.00m,
            VatType = VatTypeEnum.Vat
        }
    }
});

if (result.IsSuccess)
{
    // Send the waybill (activate it)
    await service.SendWayBillAsync(result.WaybillId);
}
```

### Query Waybills

```csharp
// Get waybills with filters
var waybills = await service.GetWayBillsAsync(new WayBillFilterOptions
{
    Type = WayBillTypeEnum.Sale,
    Status = WayBillStatusEnum.Active,
    CreatedFrom = DateTime.Today.AddDays(-30),
    CreatedTo = DateTime.Today
});

// Get waybill by number
var waybill = await soapClient.GetWayBillByNumberAsync(su, sp, "12345678");

// Get buyer's incoming waybills
var incoming = await service.GetBuyerWayBillsAsync(new WayBillFilterOptions
{
    Status = WayBillStatusEnum.Active
});
```

### Waybill Workflow

```csharp
// Buyer confirms receipt
await service.ConfirmWayBillAsync(waybillId);

// Buyer rejects
await service.RejectWayBillAsync(waybillId);

// Seller closes (marks delivery complete)
await service.CloseWayBillAsync(waybillId);

// Delete draft waybill
await service.DeleteWayBillAsync(waybillId);
```

### VD (Vehicle Declaration) Variants

VD operations allow specifying exact dates for activation/delivery:

```csharp
// Send with specific begin date
await soapClient.SendWayBillVdAsync(su, sp, waybillId, beginDate);

// Close with specific delivery date
await soapClient.CloseWayBillVdAsync(su, sp, waybillId, deliveryDate);

// Reject with comment
await soapClient.RefWayBillVdAsync(su, sp, waybillId, "Goods damaged");
```

### Transporter Operations

When a third-party transporter is involved:

```csharp
// Assign transporter to waybill
await soapClient.SaveWayBillTransporterAsync(su, sp, new SaveWayBillTransporterRequest
{
    WaybillId = waybillId,
    CarNumber = "CC001DD",
    DriverTin = "01234567890",
    DriverName = "გიორგი გიორგაძე",
    TransportTypeId = 1
});

// Transporter activates
await soapClient.SendWayBillTransporterAsync(su, sp, waybillId, DateTime.Now);

// Transporter closes delivery
await soapClient.CloseWayBillTransporterAsync(su, sp, new CloseWayBillTransporterRequest
{
    WaybillId = waybillId,
    DeliveryDate = DateTime.Now
});
```

### Templates (შაბლონები)

Save frequently used waybill configurations as templates:

```csharp
// Save template
await soapClient.SaveWayBillTemplateAsync(su, sp, "Weekly Delivery", waybillRequest);

// List templates
var templates = await soapClient.GetWayBillTemplatesAsync(su, sp);

// Load template
var template = await soapClient.GetWayBillTemplateAsync(su, sp, templateId);

// Delete template
await soapClient.DeleteWayBillTemplateAsync(su, sp, templateId);
```

### PDF Export

```csharp
// Get waybill as PDF
byte[] pdf = await soapClient.GetPrintPdfAsync(su, sp, waybillId);
File.WriteAllBytes("waybill.pdf", pdf);
```

### Reference Data

```csharp
// Lookup company by TIN
var company = await service.GetCompanyByTinAsync("206322102");

// Check if company is VAT payer
bool isVat = await soapClient.IsVatPayerByTinAsync(su, sp, "206322102");

// Get TIN from unified ID
string tin = await soapClient.GetTinFromUnIdAsync(su, sp, 731937);

// Get excise codes
var codes = await soapClient.GetAkcizCodesAsync(su, sp, "ალკოჰოლი");

// Get wood types
var woodTypes = await soapClient.GetWoodTypesAsync(su, sp);
```

### Vehicle & Barcode Management

```csharp
// Register vehicle
await soapClient.SaveCarNumberAsync(su, sp, "AA001BB");

// Remove vehicle
await soapClient.DeleteCarNumberAsync(su, sp, "AA001BB");

// Save product barcode
await soapClient.SaveBarCodeAsync(su, sp, "4860123456789", "კომპიუტერი", 1);

// Delete barcode
await soapClient.DeleteBarCodeAsync(su, sp, "4860123456789");
```

## Error Codes

Negative return values indicate errors. Common codes:

| Code | Description |
|------|-------------|
| -1 | General error |
| -2 | Authentication failed |
| -3 | Invalid parameters |
| -1013 | Invalid TIN |
| -4002 | Waybill not found |

Use `GetErrorCodesAsync()` to get the full list of error codes from RS.GE.

## Sub-Waybills (ქვე-ზედნადები)

Sub-waybills are used for distribution scenarios where goods from one waybill are split and delivered to multiple destinations:

```csharp
// Create sub-waybill
var subResult = await soapClient.SaveSubWayBillAsync(su, sp, subRequest);

// Activate sub-waybill
await soapClient.ActivateSubWayBillAsync(su, sp, new ActivateSubWayBillRequest
{
    Id = subResult.Id,
    BeginDate = DateTime.Now
});

// Close sub-waybill
await soapClient.CloseSubWayBillAsync(su, sp, subResult.Id);
```
