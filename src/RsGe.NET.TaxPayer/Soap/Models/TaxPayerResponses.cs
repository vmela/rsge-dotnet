namespace RsGe.NET.TaxPayer.Soap.Models;

public class TaxPayerPublicInfo
{
    public string Tin { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string RegistrationDate { get; set; } = string.Empty;
    public string LegalForm { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class TaxPayerContacts
{
    public string Tin { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class LegalPersonInfo
{
    public string Tin { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LegalForm { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string RegistrationDate { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
}

public class PayerInfo
{
    public string SaidCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class NaceInfo
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

public class ZReportDetail
{
    public string CashRegisterNumber { get; set; } = string.Empty;
    public DateTime ReportDate { get; set; }
    public int ReportNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal CashAmount { get; set; }
    public decimal CardAmount { get; set; }
    public int ReceiptCount { get; set; }
}

public class ZReportSummary
{
    public decimal TotalAmount { get; set; }
    public decimal CashAmount { get; set; }
    public decimal CardAmount { get; set; }
    public int TotalReceiptCount { get; set; }
    public int ReportCount { get; set; }
}

public class ComparisonActItem
{
    public string DocumentType { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime DocumentDate { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
}

public class WaybillMonthAmount
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal SalesAmount { get; set; }
    public decimal PurchasesAmount { get; set; }
    public int SalesCount { get; set; }
    public int PurchasesCount { get; set; }
}

public class IncomeAmount
{
    public int Year { get; set; }
    public decimal Amount { get; set; }
}

public class PersonIncomeData
{
    public string PersonalNumber { get; set; } = string.Empty;
    public string EmployerTin { get; set; } = string.Empty;
    public string EmployerName { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public decimal IncomeTax { get; set; }
}

public class CustomsWarehouseExitResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class Cargo200Info
{
    public string DeclarationNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class QuickCashInfo
{
    public string OrganizationName { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
}

public class GitaPayerInfo
{
    public string PayerCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ActivationResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class SmsVerificationResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}
