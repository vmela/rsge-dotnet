# Error Codes & Troubleshooting

## Error Code Format

RS.GE SOAP services return negative integers as error codes. A positive or zero value typically means success.

## Common Error Codes

| Code | Description | Georgian | Common Cause |
|------|-------------|----------|--------------|
| -1 | General error | ზოგადი შეცდომა | Various |
| -2 | Authentication failed | ავთენტიფიკაცია ვერ მოხერხდა | Wrong su/sp credentials |
| -3 | Invalid parameters | არასწორი პარამეტრები | Missing or malformed input |
| -4 | Access denied | წვდომა აკრძალულია | IP not whitelisted or no permission |
| -5 | Service unavailable | სერვისი მიუწვდომელია | RS.GE maintenance or downtime |
| -1013 | Invalid TIN | არასწორი საიდენტიფიკაციო კოდი | TIN doesn't exist or is inactive |
| -4002 | Waybill not found | ზედნადები ვერ მოიძებნა | Wrong waybill ID |

Use `GetErrorCodesAsync()` on the WayBill service to retrieve the full list directly from RS.GE.

## RS.GE SOAP Quirks

### Empty Namespace in Inner Elements

RS.GE response XML uses `xmlns=""` (empty namespace) for inner data elements, not `http://tempuri.org/`. Always use `element.Name.LocalName` for matching:

```csharp
// Correct
var value = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "ID")?.Value;

// Wrong — won't find elements with empty namespace
var value = doc.Descendants(ns + "ID").FirstOrDefault()?.Value;
```

The only elements that use `http://tempuri.org/` namespace are the top-level response wrappers (e.g., `save_waybillResult`, `chek_service_userResult`).

### Transport Type Element Name

The transport type element is named `TRANSPORT_TYPE` (not `TRANS_TYPE`) in waybill responses.

### Literal "null" String for Invalid TIN

When `get_name_from_tin` is called with an invalid TIN, RS.GE returns the literal string `"null"` — not an empty response or XML null. The SDK handles this automatically.

### Error Codes as Return Values

Some operations return error codes directly as the result value (negative integer) instead of using a separate error element:

```xml
<!-- Error response for save_waybill -->
<save_waybillResult>-1013</save_waybillResult>

<!-- Success response -->
<save_waybillResult>12345|WB-00001</save_waybillResult>
```

### Date Format

RS.GE accepts dates in `yyyy-MM-dd` or `yyyy-MM-dd HH:mm:ss` format. DateTime parameters should use `ToString("yyyy-MM-dd")` or `ToString("yyyy-MM-ddTHH:mm:ss")`.

## Troubleshooting

### "Authentication failed" (-2)

1. Verify `su` and `sp` credentials are correct
2. Check your IP is whitelisted: call `WhatIsMyIpAsync()` and compare
3. Update IP if needed: `UpdateServiceUserAsync()`
4. Ensure the service user is still active

### "Invalid parameters" (-3)

1. Check all required fields are populated
2. Verify TIN format (11 digits for individuals, 9 digits for companies)
3. Ensure numeric fields don't contain text
4. Check date formats

### Connection/Timeout Issues

1. RS.GE services may have maintenance windows (usually weekends)
2. Configure appropriate HTTP timeout (default: 100 seconds)
3. Implement retry logic for transient failures

```csharp
services.AddHttpClient<IWayBillSoapClient, WayBillSoapClient>()
    .ConfigureHttpClient(c => c.Timeout = TimeSpan.FromSeconds(30));
```

### Empty Responses

If you get empty collections or null results:
1. Verify the entity exists (waybill ID, invoice ID, TIN)
2. Check date range filters aren't too narrow
3. Ensure you have permission to view the requested data
4. For buyer operations, verify you're the buyer on the document
