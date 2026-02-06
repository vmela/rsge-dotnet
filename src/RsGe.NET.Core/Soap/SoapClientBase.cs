using System.Net;
using System.Text;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace RsGe.NET.Core.Soap;

/// <summary>
/// Base class for RS.GE SOAP clients providing shared HTTP/SOAP infrastructure
/// </summary>
public abstract class SoapClientBase
{
    private const string SoapNamespaceUri = "http://tempuri.org/";

    /// <summary>
    /// The SOAP namespace used in RS.GE service requests
    /// </summary>
    protected static XNamespace SoapNamespace => SoapNamespaceUri;

    /// <summary>
    /// HTTP client for making SOAP requests
    /// </summary>
    protected HttpClient HttpClient { get; }

    /// <summary>
    /// Logger instance
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// The RS.GE service URL
    /// </summary>
    protected abstract string ServiceUrl { get; }

    protected SoapClientBase(HttpClient? httpClient = null, ILogger? logger = null)
    {
        HttpClient = httpClient ?? new HttpClient();
        Logger = logger ?? NullLogger.Instance;
    }

    /// <summary>
    /// Send a SOAP request with key-value parameters
    /// </summary>
    protected async Task<XDocument> SendSoapRequestAsync(string action, Dictionary<string, string> parameters, CancellationToken cancellationToken = default)
    {
        var soapEnvelope = BuildSoapEnvelope(action, parameters);

        Logger.LogDebug("RS.GE SOAP Request - Action: {Action}", action);

        var request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl);
        request.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
        request.Headers.Add("SOAPAction", $"{SoapNamespaceUri}{action}");

        var response = await HttpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        Logger.LogDebug("RS.GE SOAP Response - Status: {StatusCode}", response.StatusCode);

        return XDocument.Parse(responseContent);
    }

    /// <summary>
    /// Send a SOAP request with XML body content (e.g., waybill XML)
    /// </summary>
    protected async Task<XDocument> SendSoapRequestWithXmlAsync(string action, string serviceUser, string servicePassword, string xmlContent, CancellationToken cancellationToken = default)
    {
        var soapEnvelope = BuildSoapEnvelopeWithXml(action, serviceUser, servicePassword, xmlContent);

        Logger.LogDebug("RS.GE SOAP Request - Action: {Action}", action);

        var request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl);
        request.Content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");
        request.Headers.Add("SOAPAction", $"{SoapNamespaceUri}{action}");

        var response = await HttpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        Logger.LogDebug("RS.GE SOAP Response - Status: {StatusCode}", response.StatusCode);

        return XDocument.Parse(responseContent);
    }

    /// <summary>
    /// Build a SOAP envelope with key-value parameters
    /// </summary>
    protected static string BuildSoapEnvelope(string action, Dictionary<string, string> parameters)
    {
        var sb = new StringBuilder();
        sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
        sb.AppendLine(@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">");
        sb.AppendLine(@"  <soap:Body>");
        sb.AppendLine($@"    <{action} xmlns=""{SoapNamespaceUri}"">");

        foreach (var param in parameters)
        {
            sb.AppendLine($@"      <{param.Key}>{WebUtility.HtmlEncode(param.Value)}</{param.Key}>");
        }

        sb.AppendLine($@"    </{action}>");
        sb.AppendLine(@"  </soap:Body>");
        sb.AppendLine(@"</soap:Envelope>");

        return sb.ToString();
    }

    /// <summary>
    /// Build a SOAP envelope with XML content (for waybill operations)
    /// </summary>
    protected static string BuildSoapEnvelopeWithXml(string action, string serviceUser, string servicePassword, string xmlContent)
    {
        var sb = new StringBuilder();
        sb.AppendLine(@"<?xml version=""1.0"" encoding=""utf-8""?>");
        sb.AppendLine(@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">");
        sb.AppendLine(@"  <soap:Body>");
        sb.AppendLine($@"    <{action} xmlns=""{SoapNamespaceUri}"">");
        sb.AppendLine($@"      <su>{WebUtility.HtmlEncode(serviceUser)}</su>");
        sb.AppendLine($@"      <sp>{WebUtility.HtmlEncode(servicePassword)}</sp>");
        sb.AppendLine($@"      <waybill>");
        sb.Append(xmlContent);
        sb.AppendLine($@"      </waybill>");
        sb.AppendLine($@"    </{action}>");
        sb.AppendLine(@"  </soap:Body>");
        sb.AppendLine(@"</soap:Envelope>");

        return sb.ToString();
    }

    /// <summary>
    /// Parse a string value from SOAP response
    /// </summary>
    protected static string ParseStringResponse(XDocument doc, string elementName)
    {
        XNamespace ns = SoapNamespaceUri;
        return doc.Descendants(ns + elementName).FirstOrDefault()?.Value ?? string.Empty;
    }

    /// <summary>
    /// Parse a boolean value from SOAP response
    /// </summary>
    protected static bool ParseBoolResponse(XDocument doc, string elementName)
    {
        var value = ParseStringResponse(doc, elementName);
        return value == "1" || value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Parse an integer value from SOAP response
    /// </summary>
    protected static int ParseIntResponse(XDocument doc, string elementName)
    {
        var value = ParseStringResponse(doc, elementName);
        return int.TryParse(value, out var result) ? result : 0;
    }

    /// <summary>
    /// Get the SOAP response namespace
    /// </summary>
    protected static XNamespace GetResponseNamespace()
    {
        return SoapNamespaceUri;
    }
}
