using System.Text.Json.Serialization;
using RsGe.NET.WayBill;
using RsGe.NET.WayBill.Models;
using RsGe.NET.WayBill.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure JSON with enum string converter
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add RS.GE WayBill services from configuration
builder.Services.AddRsGeWayBillServices(builder.Configuration);

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
