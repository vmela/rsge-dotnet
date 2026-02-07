# Taxpayer Services (საგადასახადო სერვისები)

**Package:** `RsGe.NET.TaxPayer`
**SOAP Endpoint:** `https://services.rs.ge/taxservice/taxpayerservice.asmx`
**Operations:** 20

## Overview

The TaxPayer service provides access to taxpayer information, fiscal reports, compliance data, and customs operations. Unlike other RS.GE services, it uses **portal credentials** (the same username/password used to log into rs.ge).

## Authentication

```csharp
// Uses portal credentials, not service user (su/sp)
services.AddRsGeTaxPayerServices(config =>
{
    config.Username = "portal_username";
    config.Password = "portal_password";
});
```

## Operations

### Taxpayer Information (გადამხდელის ინფორმაცია)

#### GetTPInfoPublic — Public Taxpayer Info

Retrieves publicly available information about any registered taxpayer by their TIN (საიდენტიფიკაციო კოდი).

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `TP_Code` | string | საიდენტიფიკაციო კოდი | Tax Identification Number (TIN) |

**Returns:** Name, English name, status, registration date, legal form, address.

```csharp
var info = await service.GetTaxPayerInfoAsync("206322102");
// info.Name = "შპს საქართველოს კომპანია"
// info.Status = "აქტიური"
```

#### GetTPInfoPublicContacts — Taxpayer Contacts

| Parameter | Type | Description |
|-----------|------|-------------|
| `TP_Code` | string | TIN to look up |

**Returns:** Email, phone, registered address.

```csharp
var contacts = await service.GetTaxPayerContactsAsync("206322102");
```

#### Get_LegalPerson_Info — Legal Entity Details

Detailed information about a legal entity (შპს, სს, etc.).

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `inSaidCode` | string | საიდენტიფიკაციო კოდი | Company TIN |

**Returns:** TIN, name, legal form, status, registration date, address, director name.

```csharp
var company = await service.GetLegalPersonInfoAsync("206322102");
// company.Director = "გიორგი გიორგაძე"
// company.LegalForm = "შპს" (LLC)
```

#### Get_Payer_Info — Payer Registration Info

| Parameter | Type | Description |
|-----------|------|-------------|
| `saidCode` | string | TIN |

**Returns:** Payer code, name, status, type.

### NACE Codes (ეკონომიკური საქმიანობის კლასიფიკატორი)

#### Get_Payer_Nace_Info

Returns the NACE (Nomenclature of Economic Activities) codes registered for a taxpayer.

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `inSaidCode` | string | საიდენტიფიკაციო კოდი | Company TIN |

**Returns:** List of NACE codes with names and primary indicator.

```csharp
var nace = await service.GetNaceCodesAsync("206322102");
foreach (var code in nace)
{
    var primary = code.IsPrimary ? "(ძირითადი)" : "";
    Console.WriteLine($"{code.Code}: {code.Name} {primary}");
}
// 62.01: პროგრამული უზრუნველყოფის შემუშავება (ძირითადი)
// 47.41: კომპიუტერებით ვაჭრობა
```

### Z-Reports / Fiscal Data (ზ-ანგარიშგება)

Z-reports are end-of-day summaries from fiscal cash registers (საკონტროლო-სალარო აპარატი).

#### Get_Z_Report_Details — Detailed Z-Reports

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `StartDate` | DateTime | დაწყების თარიღი | Report period start |
| `EndDate` | DateTime | დასრულების თარიღი | Report period end |

**Returns:** List of Z-reports with: cash register number, date, total/cash/card amounts, receipt count.

```csharp
var reports = await service.GetZReportDetailsAsync(
    startDate: new DateTime(2026, 1, 1),
    endDate: new DateTime(2026, 1, 31)
);
foreach (var r in reports)
{
    Console.WriteLine($"Register: {r.CashRegisterNumber}, Total: {r.TotalAmount:N2} GEL");
}
```

#### Get_Z_Report_Sum — Z-Report Summary

Same date parameters, returns aggregated totals across all cash registers.

```csharp
var summary = await service.GetZReportSummaryAsync(startDate, endDate);
// summary.TotalAmount, summary.CashAmount, summary.CardAmount, summary.ReportCount
```

### Comparison Acts (შედარების აქტი)

A comparison act (შედარების აქტი) is a tax reconciliation document showing all debits, credits, and balances with the Revenue Service.

#### Get_comp_act_new — Current Format

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `saidCode` | string | საიდენტიფიკაციო კოდი | Your company TIN |
| `start_date` | DateTime | დაწყების თარიღი | Period start |
| `end_date` | DateTime | დასრულების თარიღი | Period end |

**Returns:** List of items with document type, number, date, debit, credit, balance.

```csharp
var act = await service.GetComparisonActAsync(
    "206322102",
    new DateTime(2026, 1, 1),
    new DateTime(2026, 12, 31)
);
```

#### Get_comp_act_old — Legacy Format

Same parameters plus optional `session_id` for pagination.

### Waybill Monthly Totals (ზედნადებების თვიური მოცულობა)

#### Get_Waybill_Month_Amount

| Parameter | Type | Description |
|-----------|------|-------------|
| `saidCode` | string | Company TIN |
| `startDate` | DateTime | Period start |
| `endDate` | DateTime | Period end |

**Returns:** Monthly breakdown of sales/purchases amounts and counts.

```csharp
var monthly = await service.GetWaybillMonthlyTotalsAsync("206322102", startDate, endDate);
foreach (var m in monthly)
{
    Console.WriteLine($"{m.Year}/{m.Month}: Sales={m.SalesAmount:N2}, Purchases={m.PurchasesAmount:N2}");
}
```

### Income Data

#### GetPersonIncomeData

Retrieves employment income data for an individual.

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `inPersonalNumber` | string | პირადი ნომერი | Personal identification number |

#### Get_Income_Amount

| Parameter | Type | Description |
|-----------|------|-------------|
| `Year` | int | Tax year |

### Customs Operations (საბაჟო ოპერაციები)

#### Customs_WareHouse_Exit — Customs Warehouse Exit

Registers the exit of goods from a customs warehouse.

| Parameter | Type | Georgian | Description |
|-----------|------|----------|-------------|
| `DeclarationNumber` | string | დეკლარაციის ნომერი | Customs declaration number |
| `CustomsCode` | string | საბაჟოს კოდი | Customs office code |
| `CarNumber` | string | ავტომობილის ნომერი | Vehicle plate number |

#### Get_Cargo200_Info — Cargo Information

Retrieves cargo clearance information for a date range.

| Parameter | Type | Description |
|-----------|------|-------------|
| `inStartDate` | DateTime | Period start |
| `inEndDate` | DateTime | Period end |

### Special Services

#### Get_QuickCash_Info

Retrieves QuickCash payment service information for the organization.

### GITA Integration (გიტა ინტეგრაცია)

GITA (Government Information Technology Agency) integration operations:

#### get_payer_info_gita — GITA Payer Info

| Parameter | Type | Description |
|-----------|------|-------------|
| `payerCode` | string | Payer code |
| `startDate` | string | Period start |
| `endDate` | string | Period end |

#### Gita_Payer_Activation — Activate in GITA

| Parameter | Type | Description |
|-----------|------|-------------|
| `payerCode` | string | Payer code |
| `startDate` | DateTime | Activation date |
| `status` | int | Activation status |

#### Gita_Sms_Verification — GITA SMS Verify

| Parameter | Type | Description |
|-----------|------|-------------|
| `payerCode` | string | Payer code |
| `smsCode` | string | SMS verification code |

#### Payer_Info_Activation + Tp_sms_verification

Two-step payer activation with SMS verification.
