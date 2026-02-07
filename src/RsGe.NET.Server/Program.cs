using System.Text.Json.Serialization;
using RsGe.NET.WayBill;
using RsGe.NET.WayBill.Models;
using RsGe.NET.WayBill.Services;
using RsGe.NET.TaxPayer;
using RsGe.NET.TaxPayer.Services;
using RsGe.NET.Invoice;
using RsGe.NET.Invoice.Services;
using RsGe.NET.Invoice.Soap.Models;
using RsGe.NET.SpecInvoice;
using RsGe.NET.SpecInvoice.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure JSON with enum string converter
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add RS.GE services from configuration
builder.Services.AddRsGeWayBillServices(builder.Configuration);
builder.Services.AddRsGeTaxPayerServices(builder.Configuration);
builder.Services.AddRsGeInvoiceServices(builder.Configuration);
builder.Services.AddRsGeSpecInvoiceServices(builder.Configuration);

// Add CORS
builder.Services.AddCors(options =>
{
    var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>();
    options.AddDefaultPolicy(policy =>
    {
        if (origins is { Length: > 0 })
            policy.WithOrigins(origins);
        else
            policy.AllowAnyOrigin();

        policy.AllowAnyHeader().AllowAnyMethod();
    });
});

// Add OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ── Reference Data ──────────────────────────────────────────────────

var reference = app.MapGroup("/api/reference").WithTags("Reference Data");

reference.MapGet("/waybill-types", async (IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetWayBillTypesAsync(ct)));

reference.MapGet("/units", async (IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetUnitsAsync(ct)));

reference.MapGet("/transport-types", async (IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetTransportTypesAsync(ct)));

// ── Company Lookup ──────────────────────────────────────────────────

var company = app.MapGroup("/api/company").WithTags("Company");

company.MapGet("/{tin}", async (string tin, IRsGeWayBillService svc, CancellationToken ct) =>
{
    var result = await svc.GetCompanyByTinAsync(tin, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

// ── Waybill CRUD ────────────────────────────────────────────────────

var waybills = app.MapGroup("/api/waybills").WithTags("Waybills");

waybills.MapPost("/", async (CreateWayBillRequest request, IRsGeWayBillService svc, CancellationToken ct) =>
{
    var result = await svc.CreateWayBillAsync(request, ct);
    return result.IsSuccess
        ? Results.Created($"/api/waybills/{result.WaybillId}", result)
        : Results.BadRequest(result);
});

waybills.MapGet("/{id:int}", async (int id, IRsGeWayBillService svc, CancellationToken ct) =>
{
    var result = await svc.GetWayBillAsync(id, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

waybills.MapGet("/", async (
    [AsParameters] WayBillFilterQuery filter,
    IRsGeWayBillService svc,
    CancellationToken ct) =>
{
    var options = filter.ToFilterOptions();
    return Results.Ok(await svc.GetWayBillsAsync(options, ct));
});

waybills.MapGet("/buyer", async (
    [AsParameters] WayBillFilterQuery filter,
    IRsGeWayBillService svc,
    CancellationToken ct) =>
{
    var options = filter.ToFilterOptions();
    return Results.Ok(await svc.GetBuyerWayBillsAsync(options, ct));
});

// ── Waybill Workflow ────────────────────────────────────────────────

var workflow = app.MapGroup("/api/waybills").WithTags("Waybill Workflow");

workflow.MapPost("/{id:int}/send", async (int id, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.SendWayBillAsync(id, ct)));

workflow.MapPost("/{id:int}/confirm", async (int id, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.ConfirmWayBillAsync(id, ct)));

workflow.MapPost("/{id:int}/reject", async (int id, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.RejectWayBillAsync(id, ct)));

workflow.MapPost("/{id:int}/close", async (int id, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.CloseWayBillAsync(id, ct)));

workflow.MapDelete("/{id:int}", async (int id, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.DeleteWayBillAsync(id, ct)));

workflow.MapPost("/{id:int}/reference/{referenceId:int}", async (int id, int referenceId, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.ReferenceWayBillAsync(id, referenceId, ct)));

// ── Reporting ───────────────────────────────────────────────────────

var reports = app.MapGroup("/api/reports").WithTags("Reports");

reports.MapGet("/sales", async (DateTime from, DateTime to, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetSalesAsync(from, to, ct)));

reports.MapGet("/purchases", async (DateTime from, DateTime to, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetPurchasesAsync(from, to, ct)));

reports.MapGet("/completed", async (DateTime since, IRsGeWayBillService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetCompletedWayBillsAsync(since, ct)));

reports.MapGet("/incoming", async (
    DateTime from,
    DateTime to,
    int[]? statuses,
    IRsGeWayBillService svc,
    CancellationToken ct) =>
    Results.Ok(await svc.GetIncomingWaybillsAsync(from, to, statuses, ct)));

// ── TaxPayer ───────────────────────────────────────────────────────

var taxpayer = app.MapGroup("/api/taxpayer").WithTags("Taxpayer");

taxpayer.MapGet("/{tin}", async (string tin, IRsGeTaxPayerService svc, CancellationToken ct) =>
{
    var result = await svc.GetTaxPayerInfoAsync(tin, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

taxpayer.MapGet("/{tin}/contacts", async (string tin, IRsGeTaxPayerService svc, CancellationToken ct) =>
{
    var result = await svc.GetTaxPayerContactsAsync(tin, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

taxpayer.MapGet("/{tin}/legal", async (string tin, IRsGeTaxPayerService svc, CancellationToken ct) =>
{
    var result = await svc.GetLegalPersonInfoAsync(tin, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

taxpayer.MapGet("/{tin}/nace", async (string tin, IRsGeTaxPayerService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetNaceCodesAsync(tin, ct)));

taxpayer.MapGet("/{tin}/vat-status", async (string tin, IRsGeTaxPayerService svc, CancellationToken ct) =>
    Results.Ok(new { isVatPayer = await svc.IsVatPayerAsync(tin, ct) }));

taxpayer.MapGet("/z-reports", async (DateTime from, DateTime to, IRsGeTaxPayerService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetZReportDetailsAsync(from, to, ct)));

taxpayer.MapGet("/z-reports/summary", async (DateTime from, DateTime to, IRsGeTaxPayerService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetZReportSummaryAsync(from, to, ct)));

taxpayer.MapGet("/{tin}/comparison-act", async (string tin, DateTime from, DateTime to, IRsGeTaxPayerService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetComparisonActAsync(tin, from, to, ct)));

taxpayer.MapGet("/{tin}/waybill-totals", async (string tin, DateTime from, DateTime to, IRsGeTaxPayerService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetWaybillMonthlyTotalsAsync(tin, from, to, ct)));

// ── Invoices ───────────────────────────────────────────────────────

var invoices = app.MapGroup("/api/invoices").WithTags("Invoices");

invoices.MapGet("/{id:long}", async (long id, IRsGeInvoiceService svc, CancellationToken ct) =>
{
    var result = await svc.GetInvoiceAsync(id, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

invoices.MapGet("/seller", async (
    int unId, DateTime? from, DateTime? to,
    IRsGeInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetSellerInvoicesAsync(unId, from, to, cancellationToken: ct)));

invoices.MapGet("/buyer", async (
    int unId, DateTime? from, DateTime? to,
    IRsGeInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetBuyerInvoicesAsync(unId, from, to, cancellationToken: ct)));

invoices.MapPost("/{id:long}/status/{status:int}", async (long id, int status, IRsGeInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.ChangeInvoiceStatusAsync(id, status, ct)));

invoices.MapPost("/{id:long}/accept/{status:int}", async (long id, int status, IRsGeInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.AcceptInvoiceAsync(id, status, ct)));

invoices.MapGet("/{id:long}/descriptions", async (long id, IRsGeInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetInvoiceDescriptionsAsync(id, ct)));

invoices.MapGet("/lookup/tin/{unId:int}", async (int unId, IRsGeInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.LookupTinFromUnIdAsync(unId, ct)));

invoices.MapGet("/lookup/unid/{tin}", async (string tin, IRsGeInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.LookupUnIdFromTinAsync(tin, ct)));

// ── Special Invoices ───────────────────────────────────────────────

var specInvoices = app.MapGroup("/api/spec-invoices").WithTags("Special Invoices");

specInvoices.MapGet("/{id:int}", async (int id, IRsGeSpecInvoiceService svc, CancellationToken ct) =>
{
    var result = await svc.GetInvoiceAsync(id, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

specInvoices.MapPost("/{id:int}/status/{status:int}", async (int id, int status, IRsGeSpecInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.ChangeStatusAsync(id, status, ct)));

specInvoices.MapPost("/{id:int}/accept/{status:int}", async (int id, int status, IRsGeSpecInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.AcceptInvoiceAsync(id, status, ct)));

specInvoices.MapGet("/products", async (IRsGeSpecInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.GetSpecProductsAsync(ct)));

specInvoices.MapPost("/{id:int}/transport/start", async (int id, IRsGeSpecInvoiceService svc, CancellationToken ct) =>
    Results.Ok(await svc.StartTransportAsync(id, ct)));

specInvoices.MapGet("/org-objects/{unId:int}", async (int unId, IRsGeSpecInvoiceService svc, CancellationToken ct) =>
{
    var result = await svc.GetOrganizationObjectsAsync(unId, ct);
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

// ── Health Check ────────────────────────────────────────────────────

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "RsGe.NET.Server" }))
   .WithTags("Health");

app.Run();

/// <summary>
/// Query parameters for filtering waybills
/// </summary>
public record WayBillFilterQuery(
    WayBillTypeEnum? Type = null,
    WayBillStatusEnum? Status = null,
    string? BuyerTin = null,
    string? SellerTin = null,
    DateTime? CreatedFrom = null,
    DateTime? CreatedTo = null)
{
    public WayBillFilterOptions? ToFilterOptions()
    {
        if (Type is null && Status is null && BuyerTin is null && SellerTin is null && CreatedFrom is null && CreatedTo is null)
            return null;

        return new WayBillFilterOptions
        {
            Type = Type,
            Status = Status,
            BuyerTin = BuyerTin,
            SellerTin = SellerTin,
            CreatedFrom = CreatedFrom,
            CreatedTo = CreatedTo
        };
    }
}
