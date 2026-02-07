# Tax Invoices / ანგარიშ-ფაქტურა (NtosService)

**Package:** `RsGe.NET.Invoice`
**SOAP Endpoint:** `https://www.revenue.mof.ge/ntosservice/ntosservice.asmx`
**Operations:** 49

## What is a Tax Invoice?

A tax invoice (ანგარიშ-ფაქტურა / ე-ანგარიშფაქტურა) is an electronic document used for VAT reporting between businesses in Georgia. It records the details of a transaction — seller, buyer, goods/services, amounts, and VAT. Tax invoices are mandatory for VAT-registered entities.

Unlike waybills (which track physical goods movement), invoices are financial documents used for tax deduction purposes.

## Authentication

NtosService uses a two-step authentication:
1. Service user credentials (`su` / `sp`) for API access
2. A `user_id` obtained from the `chek` method for subsequent operations

```csharp
// Step 1: Get user_id from credentials
var (isValid, userId) = await invoiceClient.CheckAsync(su, sp);

// Step 2: Use userId in all subsequent calls
var invoice = await invoiceClient.GetInvoiceAsync(userId, invoiceId, su, sp);
```

## Invoice Statuses (სტატუსები)

| Code | Status | Georgian | Description |
|------|--------|----------|-------------|
| 0 | Draft | მონახაზი | Created, not yet sent |
| 1 | Sent | გაგზავნილი | Sent to buyer |
| 2 | Accepted | მიღებული | Accepted by buyer |
| 3 | Rejected | უარყოფილი | Rejected by buyer |
| 4 | Corrected | შესწორებული | Corrective invoice issued |
| 5 | Cancelled | გაუქმებული | Cancelled |

## Invoice Lifecycle

```
Draft (0) ──send──> Sent (1) ──accept──> Accepted (2)
    │                  │
    └──delete          ├──reject──> Rejected (3)
                       │
                       └──correct──> Corrected (4)
```

## Key Concepts

### Invoice Series & Number (სერია და ნომერი)

Each invoice has a series (`f_series`) and number (`f_number`) that uniquely identify it. These are assigned automatically by RS.GE.

### Unified ID (un_id / უნიფიცირებული ID)

Every registered entity has a `un_id` in RS.GE. You can convert between TIN and un_id:

```csharp
// TIN -> un_id
var unId = await client.GetUnIdFromTinAsync(userId, "206322102", su, sp);

// un_id -> TIN
var tin = await client.GetTinFromUnIdAsync(userId, 731937, su, sp);

// un_id -> Organization name
var name = await client.GetOrgNameFromUnIdAsync(userId, 731937, su, sp);
```

### Corrective Invoices (მაკორექტირებელი ანგარიშ-ფაქტურა)

When an invoice needs to be modified after acceptance, a corrective invoice (k_invoice) is issued:

| k_type | Description | Georgian |
|--------|-------------|----------|
| 1 | Correction | კორექტირება |
| 2 | Cancellation | გაუქმება |

### Invoice Descriptions / Line Items (საქონლის აღწერა)

Each invoice contains line items (`invoice_desc`) describing the goods or services:

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `goods` | string | საქონელი/მომსახურება | Product/service name |
| `g_unit` | string | ზომის ერთეული | Measurement unit |
| `g_number` | decimal | რაოდენობა | Quantity |
| `full_amount` | decimal | სრული თანხა | Total amount |
| `drg_amount` | decimal | დღგ თანხა | VAT amount |
| `aqcizi_amount` | decimal | აქციზის თანხა | Excise amount |
| `akciz_id` | int | აქციზის კოდი | Excise code |

## Common Operations

### Create an Invoice

```csharp
var (success, invoiceId) = await client.SaveInvoiceAsync(
    userId: userId,
    invoiceId: 0,                    // 0 = new invoice
    operationDate: DateTime.Today,
    sellerUnId: 731937,              // Seller's un_id
    buyerUnId: 123456,               // Buyer's un_id
    overheadNo: "INV-2026-001",      // Your internal reference number
    overheadDate: DateTime.Today,
    bsUserId: 0,
    su: su,
    sp: sp
);
```

### Add Line Items

```csharp
var (success, lineId) = await client.SaveInvoiceDescAsync(
    userId: userId,
    id: 0,                     // 0 = new line
    su: su,
    sp: sp,
    invoiceId: invoiceId,
    goods: "IT კონსულტაცია",
    unit: "საათი",
    quantity: 40m,
    fullAmount: 4000m,
    vatAmount: 720m,            // 18% VAT
    exciseAmount: 0m,
    exciseId: 0
);
```

### Search Invoices

```csharp
// Get seller's invoices
var sellerInvoices = await client.GetSellerInvoicesAsync(
    userId, unId,
    startDate: new DateTime(2026, 1, 1),
    endDate: new DateTime(2026, 12, 31),
    su: su, sp: sp
);

// Get buyer's invoices
var buyerInvoices = await client.GetBuyerInvoicesAsync(
    userId, unId, startDate, endDate, su: su, sp: sp
);

// Get status counts per filter
var counts = await client.GetSellerFilterCountAsync(unId, userId, su, sp);
// counts: st_0 (draft), st_1 (sent), st_2 (accepted), etc.
```

### Invoice Workflow

```csharp
// Send invoice (change status from draft to sent)
await client.ChangeInvoiceStatusAsync(userId, invoiceId, status: 1, su, sp);

// Buyer accepts
await client.AcceptInvoiceStatusAsync(userId, invoiceId, status: 2, su, sp);

// Buyer rejects with reason
await client.RejectInvoiceStatusAsync(userId, invoiceId, "თანხა არასწორია", su, sp);

// Create corrective invoice
var (success, correctiveId) = await client.CreateCorrectiveInvoiceAsync(
    userId, invoiceId, kType: 1, su, sp
);
```

### Invoice Requests (მოთხოვნა)

A buyer can request an invoice from a seller:

```csharp
// Buyer requests invoice from seller
await client.SaveInvoiceRequestAsync(
    invoiceId: 0,
    userId: userId,
    buyerUnId: buyerUnId,
    sellerUnId: sellerUnId,
    overheadNo: "PO-001",
    date: DateTime.Today,
    notes: "გთხოვთ ანგარიშ-ფაქტურის გამოწერა",
    su: su, sp: sp
);

// Seller accepts the request
await client.AcceptInvoiceRequestStatusAsync(requestId, userId, sellerUnId, su, sp);
```

### Declaration Integration (დეკლარაციის ინტეგრაცია)

Link invoices to tax declarations:

```csharp
// Add invoice to declaration
await client.AddInvoiceToDeclarationAsync(userId, seqNum, invoiceId, su, sp);

// Get declaration date
var date = await client.GetDeclarationDateAsync(su, sp, declarationNumber, unId);
```

### Excise Codes (აქციზის კოდები)

```csharp
// Search excise codes
var codes = await client.GetAkcizAsync("ალკოჰოლი", userId, su, sp);
```

## NTOS Invoice Numbers

Manage waybill-linked invoice numbers:

```csharp
// Save invoice number for NTOS
await client.SaveNtosInvoiceNumberAsync(invoiceId, userId, overheadNo, overheadDate, su, sp);

// Get invoice numbers
var numbers = await client.GetNtosInvoiceNumbersAsync(userId, invoiceId, su, sp);

// Delete invoice number
await client.DeleteNtosInvoiceNumberAsync(userId, id, invoiceId, su, sp);
```
