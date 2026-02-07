# Authentication & Service Users

## Overview

RS.GE uses two distinct authentication mechanisms depending on the service:

| Service | Auth Method | Credentials |
|---|---|---|
| WayBillService | Service User | `su` (service user) + `sp` (service password) |
| NtosService (Invoices) | Service User + User ID | `su` + `sp` + `user_id` |
| SpecInvoicesService | Service User + User ID | `su` + `sp` + `user_id` |
| TaxPayerService | Portal Credentials | `Username` + `Password` |

## Service User Authentication (su/sp)

Most RS.GE services authenticate using **service users** — special API credentials created for machine-to-machine communication.

### What is a Service User? (სერვის მომხმარებელი)

A service user is an API account tied to your organization's taxpayer account. Each service user has:
- **Username (su)** — format: `USERNAME:TIN` (e.g., `MYSERVICE:206322102`)
- **Password (sp)** — the service password you set when creating the user
- **IP whitelist** — requests are only accepted from registered IP addresses
- **Unified ID (un_id)** — your organization's internal RS.GE identifier

### Creating a Service User

```csharp
var client = new WayBillSoapClient();

// Step 1: Find your external IP as seen by RS.GE
var myIp = await client.WhatIsMyIpAsync();
// Returns: "185.xxx.xxx.xxx"

// Step 2: Create the service user
var created = await client.CreateServiceUserAsync(
    serviceUser: "MYAPP:206322102",
    servicePassword: "MySecurePassword123",
    ip: myIp,
    name: "My Application",
    tin: "206322102"  // Your company TIN (საიდენტიფიკაციო კოდი)
);

// Step 3: Validate credentials
var isValid = await client.CheckServiceUserAsync("MYAPP:206322102", "MySecurePassword123");
```

### Updating IP Address

If your server IP changes, update the service user:

```csharp
await client.UpdateServiceUserAsync(
    serviceUser: "MYAPP:206322102",
    servicePassword: "MySecurePassword123",
    ip: "new.ip.address",
    name: "My Application"
);
```

### Listing Service Users

```csharp
// Uses portal credentials (not service user credentials)
var users = await client.GetServiceUsersAsync("portalUsername", "portalPassword");
foreach (var user in users)
{
    Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, IP: {user.Ip}");
}
```

## Portal Credentials (Username/Password)

The **TaxPayerService** uses RS.GE portal login credentials directly — the same username and password you use to log into [rs.ge](https://www.rs.ge).

```csharp
var taxService = new TaxPayerSoapClient();
var info = await taxService.GetTaxPayerInfoPublicAsync(
    username: "your_portal_username",
    password: "your_portal_password",
    tpCode: "206322102"  // TIN to look up
);
```

## User ID Authentication (NtosService / SpecInvoicesService)

The invoice services require an additional `user_id` parameter obtained by calling the `chek` method first:

```csharp
var invoiceClient = new InvoiceSoapClient();

// Step 1: Authenticate and get user_id
var (isValid, userId) = await invoiceClient.CheckAsync("su", "sp");

// Step 2: Use user_id in subsequent calls
var invoice = await invoiceClient.GetInvoiceAsync(userId, invoiceId, "su", "sp");
```

## Configuration in appsettings.json

```json
{
  "RsGe": {
    "ServiceUser": "MYAPP:206322102",
    "ServicePassword": "MySecurePassword123",
    "TaxPayer": {
      "Username": "portal_username",
      "Password": "portal_password"
    }
  }
}
```

## IP Whitelisting

RS.GE requires that API requests come from the IP address registered with the service user. If you receive authentication errors:

1. Call `WhatIsMyIpAsync()` to check your current external IP
2. Update your service user with `UpdateServiceUserAsync()` if the IP has changed
3. For cloud deployments, ensure your outbound IP is static

## Security Notes

- Never hardcode credentials in source code — use environment variables or secret managers
- Service passwords should be strong (RS.GE may enforce minimum requirements)
- Rotate service passwords periodically
- Each application/environment should have its own service user
- Monitor `CheckServiceUserAsync()` failures for unauthorized access attempts
