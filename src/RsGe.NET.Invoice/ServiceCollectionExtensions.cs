using RsGe.NET.Invoice.Services;
using RsGe.NET.Invoice.Soap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RsGe.NET.Invoice;

/// <summary>
/// Extension methods for configuring RS.GE Invoice services in dependency injection container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds RS.GE Invoice services to the service collection using configuration from appsettings.json
    /// </summary>
    public static IServiceCollection AddRsGeInvoiceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("RsGe:Invoice").Get<InvoiceConfiguration>()
                     ?? new InvoiceConfiguration();
        return services.AddRsGeInvoiceServices(config);
    }

    /// <summary>
    /// Adds RS.GE Invoice services to the service collection using the provided configuration object
    /// </summary>
    public static IServiceCollection AddRsGeInvoiceServices(this IServiceCollection services, InvoiceConfiguration config)
    {
        services.AddSingleton(config);
        services.AddHttpClient<IInvoiceSoapClient, InvoiceSoapClient>();

        services.AddScoped<IRsGeInvoiceService>(sp =>
        {
            var soapClient = sp.GetRequiredService<IInvoiceSoapClient>();
            return new RsGeInvoiceService(soapClient, config.ServiceUser, config.ServicePassword, config.UserId);
        });

        return services;
    }

    /// <summary>
    /// Adds RS.GE Invoice services to the service collection using a configuration action
    /// </summary>
    public static IServiceCollection AddRsGeInvoiceServices(this IServiceCollection services, Action<InvoiceConfiguration> configure)
    {
        var config = new InvoiceConfiguration();
        configure(config);
        return services.AddRsGeInvoiceServices(config);
    }
}
