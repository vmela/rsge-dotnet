# Special Invoices (სპეციალური ანგარიშ-ფაქტურა)

**Package:** `RsGe.NET.SpecInvoice`
**SOAP Endpoint:** `https://www.revenue.mof.ge/SpecInvoicesService/SpecInvoicesService.asmx`
**Operations:** 42

## What are Special Invoices?

Special invoices (სპეციალური ანგარიშ-ფაქტურა) are used for regulated products that require additional tracking:

- **Alcohol** (ალკოჰოლური პროდუქცია) — wines, spirits, beer
- **Tobacco** (თამბაქო) — cigarettes, tobacco products
- **Fuel/Oil** (საწვავი/ნავთობპროდუქტები) — gasoline, diesel, LPG
- **Pharmaceuticals** (ფარმაცევტული პროდუქცია) — medicines

These products require excise stamps (აქციზის მარკა) and special sale documents for regulatory compliance.

## Key Concepts

### SSAF — State Stamp for Alcohol/Fuel (აქციზის მარკა)

SSAF (State Stamp of Alcohol and Fuel) is a unique identifier attached to each unit of regulated product. It tracks the product from production/import to final sale.

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `p_SSAF_N` | string | SSAF ნომერი | SSAF stamp number |
| `p_SSAF_DATE` | DateTime | SSAF თარიღი | Stamp issuance date |
| `p_SSAF_ALT_NUMBER` | string | ალტერნატიული ნომერი | Alternative stamp number |
| `p_SSAF_ALT_TYPE` | string | ალტერნატიული ტიპი | Alternative stamp type |
| `p_SSAF_ALT_STATUS` | string | ალტერნატიული სტატუსი | Alternative stamp status |

### SSD — Special Sale Document (სპეციალური გაყიდვის დოკუმენტი)

SSD accompanies the sale of regulated products:

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `p_SSD_N` | string | SSD ნომერი | SSD document number |
| `p_SSD_DATE` | DateTime | SSD თარიღი | Document date |
| `p_SSD_ALT_NUMBER` | string | ალტერნატიული ნომერი | Alternative number |
| `p_SSD_ALT_TYPE` | string | ალტერნატიული ტიპი | Alternative type |

### Transport for Regulated Products

Special invoices include detailed transport information:

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `p_TRANSPORT_TYPE` | string | ტრანსპორტის ტიპი | Transport type |
| `p_TRANSPORT_MARK` | string | სატრანსპორტო საშუალება | Vehicle registration mark |
| `p_DRIVER_INFO` | string | მძღოლის ინფორმაცია | Driver information |
| `p_driver_no` | string | მძღოლის ნომერი | Driver personal number |
| `p_driver_is_geo` | int | | 1 = Georgian citizen, 0 = foreign |
| `p_CARRIER_INFO` | string | გადამზიდის ინფორმაცია | Carrier company info |
| `p_CARRIE_S_NO` | string | გადამზიდის ნომერი | Carrier registration number |

### Oil/Fuel Station Information

For fuel transactions:

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `p_OIL_ST_ADDRESS` | string | საწვავის სადგურის მისამართი | Station origin address |
| `p_OIL_ST_N` | string | სადგურის ნომერი | Station origin number |
| `p_OIL_FN_ADDRESS` | string | საწვავის დანიშნულება | Station destination address |
| `p_OIL_FN_N` | string | დანიშნულების ნომერი | Destination station number |

## Invoice Parameters

The `save_invoice` operation has many parameters:

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `p_F_SERIES` | string | სერია | Invoice series |
| `p_OPERATION_DT` | DateTime | ოპერაციის თარიღი | Operation date |
| `p_SELLER_UN_ID` | int | გამყიდველის ID | Seller unified ID |
| `p_BUYER_UN_ID` | int | მყიდველის ID | Buyer unified ID |
| `p_CALC_DATE` | DateTime | ანგარიშგების თარიღი | Calculation date |
| `p_TR_ST_DATE` | DateTime | ტრანსპორტირების თარიღი | Transport start date |
| `p_PAY_TYPE` | string | გადახდის ტიპი | Payment type |
| `p_SELLER_PHONE` | string | გამყიდველის ტელეფონი | Seller phone |
| `p_BUYER_PHONE` | string | მყიდველის ტელეფონი | Buyer phone |

### Line Item Parameters

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `p_goods` | string | საქონელი | Product name |
| `p_g_unit` | string | ზომის ერთეული | Unit |
| `p_g_number` | decimal | რაოდენობა | Quantity |
| `p_un_price` | decimal | ერთეულის ფასი | Unit price |
| `p_drg_amount` | decimal | დღგ თანხა | VAT amount |
| `p_aqcizi_amount` | decimal | აქციზის თანხა | Excise amount |
| `p_aqcizi_id` | int | აქციზის კოდი | Excise code |
| `p_aqcizi_rate` | string | აქციზის განაკვეთი | Excise rate |
| `p_dgg_rate` | string | დღგ განაკვეთი | VAT rate |
| `p_good_id` | string | საქონლის ID | Product ID |
| `p_drg_type` | string | დღგ ტიპი | VAT type |

## Common Operations

### Create a Special Invoice

```csharp
var (success, invoiceId) = await client.SaveInvoiceAsync(new SaveSpecInvoiceRequest
{
    Series = "SP",
    OperationDate = DateTime.Today,
    SellerUnId = 731937,
    BuyerUnId = 123456,
    SsdNumber = "SSD-001",
    SsafNumber = "SSAF-001",
    TransportType = "1",
    TransportMark = "AA001BB",
    DriverInfo = "გიორგი გიორგაძე",
    DriverNo = "01234567890"
}, userId, su, sp);
```

### SSAF/SSD Management

```csharp
// Add SSAF stamp to invoice
await client.AddSpecInvoiceSsafAsync(invoiceId, ssafData, userId, su, sp);

// Remove SSAF stamp
await client.DeleteSpecInvoiceSsafAsync(invoiceId, ssafId, userId, su, sp);

// Add SSD document
await client.AddSpecInvoiceSsdAsync(invoiceId, ssdData, userId, su, sp);

// Remove SSD document
await client.DeleteSpecInvoiceSsdAsync(invoiceId, ssdId, userId, su, sp);
```

### Special Products (სპეციალური პროდუქტები)

```csharp
// List all special products
var products = await client.GetSpecProductsAsync(userId, su, sp);

// Get product by ID
var product = await client.GetSpecProductByIdAsync(productId, userId, su, sp);

// Get SSAF stamps
var ssafs = await client.GetSpecSsafsAsync(userId, su, sp);

// Get SSD documents
var ssds = await client.GetSpecSsdsAsync(userId, su, sp);
```

### Transport Operations

```csharp
// Start transport tracking
await client.StartTransportAsync(invoiceId, transportData, userId, su, sp);

// Correct driver information
await client.CorrectDriverInfoAsync(
    invoiceId, sellerUnId,
    driverInfo: "ახალი მძღოლი",
    driverNo: "98765432100",
    driverIsGeo: 1,
    userId, su, sp
);

// Correct vehicle registration
await client.CorrectTransportMarkAsync(
    invoiceId, sellerUnId,
    transportMark: "BB002CC",
    userId, su, sp
);
```

### Organization Objects (ორგანიზაციის ობიექტები)

```csharp
// Get RS organization objects
var objects = await client.GetRsOrgObjectsAsync(userId, su, sp);

// Get objects by organization unified ID
var orgObjects = await client.GetOrgObjectsByUnIdAsync(unId, userId, su, sp);

// Get object address
var address = await client.GetOrgObjectAddressAsync(objectId, userId, su, sp);
```

### Invoice Status Operations

```csharp
// Change status
await client.ChangeInvoiceStatusAsync(userId, invoiceId, status: 1, su, sp);

// Accept invoice
await client.AcceptInvoiceStatusAsync(userId, invoiceId, status: 2, su, sp);

// Reject with reason
await client.RejectInvoiceStatusAsync(userId, invoiceId, "მიზეზი", su, sp);

// Get cancellation reason
var reason = await client.GetCancellationReasonAsync(invoiceId, userId, su, sp);
```

## Differences from Regular Tax Invoices

| Feature | Tax Invoice (ანგარიშ-ფაქტურა) | Special Invoice |
|---------|------|----------------|
| Service | NtosService | SpecInvoicesService |
| Products | All goods/services | Regulated products only |
| SSAF/SSD | Not required | Required |
| Transport | Basic | Detailed (driver, vehicle, stations) |
| Excise | Optional | Required for excisable goods |
| Oil stations | N/A | Required for fuel transactions |
