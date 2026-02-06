using RsGe.NET.Core;
using RsGe.NET.WayBill.Services;
using RsGe.NET.WayBill.Soap;

namespace RsGe.NET.WayBill;

/// <summary>
/// Factory for creating RS.GE service instances without dependency injection
/// </summary>
public static class RsGeServiceFactory
{
    public static IRsGeWayBillService CreateWayBillService(string serviceUser, string servicePassword)
    {
        var soapClient = new WayBillSoapClient();
        return new RsGeWayBillService(soapClient, serviceUser, servicePassword);
    }

    public static IRsGeWayBillService CreateWayBillService(string serviceUser, string servicePassword, HttpClient httpClient)
    {
        var soapClient = new WayBillSoapClient(httpClient);
        return new RsGeWayBillService(soapClient, serviceUser, servicePassword);
    }

    public static IRsGeWayBillService CreateWayBillService(RsGeConfiguration config)
    {
        if (!config.IsValid)
            throw new ArgumentException("Invalid RS.GE configuration. ServiceUser and ServicePassword are required.");

        return CreateWayBillService(config.ServiceUser, config.ServicePassword);
    }

    public static IWayBillSoapClient CreateSoapClient()
    {
        return new WayBillSoapClient();
    }

    public static IWayBillSoapClient CreateSoapClient(HttpClient httpClient)
    {
        return new WayBillSoapClient(httpClient);
    }
}
