namespace RsGe.NET.WayBill.Services;

/// <summary>
/// Factory for creating RS.GE service instances with dynamic credentials
/// </summary>
public interface IRsGeDynamicServiceFactory
{
    IRsGeWayBillService CreateService(string userName, string password);
}
