using RsGe.NET.WayBill.Soap;

namespace RsGe.NET.WayBill.Services;

public class RsGeDynamicServiceFactory : IRsGeDynamicServiceFactory
{
    private readonly IWayBillSoapClient _soapClient;

    public RsGeDynamicServiceFactory(IWayBillSoapClient soapClient)
    {
        _soapClient = soapClient ?? throw new ArgumentNullException(nameof(soapClient));
    }

    public IRsGeWayBillService CreateService(string userName, string password)
    {
        return new RsGeWayBillService(_soapClient, userName, password);
    }
}
