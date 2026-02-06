using System.Xml.Linq;
using RsGe.NET.Core.Soap;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace RsGe.NET.WayBill.Tests;

public class SoapClientBaseTests
{
    private class TestSoapClient : SoapClientBase
    {
        public TestSoapClient(HttpClient? httpClient = null, ILogger? logger = null)
            : base(httpClient, logger) { }

        protected override string ServiceUrl => "http://test.example.com/service.asmx";

        public string TestBuildSoapEnvelope(string action, Dictionary<string, string> parameters)
            => BuildSoapEnvelope(action, parameters);

        public string TestBuildSoapEnvelopeWithXml(string action, string su, string sp, string innerXml)
            => BuildSoapEnvelopeWithXml(action, su, sp, innerXml);

        public string TestParseStringResponse(XDocument doc, string action)
            => ParseStringResponse(doc, action);

        public bool TestParseBoolResponse(XDocument doc, string action)
            => ParseBoolResponse(doc, action);

        public int TestParseIntResponse(XDocument doc, string action)
            => ParseIntResponse(doc, action);
    }

    private readonly TestSoapClient _client = new();

    [Fact]
    public void BuildSoapEnvelope_CreatesValidSoapXml()
    {
        var parameters = new Dictionary<string, string>
        {
            ["su"] = "testuser",
            ["sp"] = "testpass",
            ["waybill_id"] = "123"
        };

        var envelopeXml = _client.TestBuildSoapEnvelope("get_waybill", parameters);

        envelopeXml.Should().NotBeNullOrEmpty();
        var envelope = XDocument.Parse(envelopeXml);
        var ns = XNamespace.Get("http://tempuri.org/");
        var body = envelope.Descendants(ns + "get_waybill").FirstOrDefault();
        body.Should().NotBeNull();
        body!.Element(ns + "su")!.Value.Should().Be("testuser");
        body.Element(ns + "sp")!.Value.Should().Be("testpass");
        body.Element(ns + "waybill_id")!.Value.Should().Be("123");
    }

    [Fact]
    public void BuildSoapEnvelopeWithXml_IncludesInnerXml()
    {
        var innerXml = "<WAYBILL><ID>1</ID></WAYBILL>";

        var envelopeXml = _client.TestBuildSoapEnvelopeWithXml("save_waybill", "user", "pass", innerXml);

        envelopeXml.Should().NotBeNullOrEmpty();
        envelopeXml.Should().Contain("user");
        envelopeXml.Should().Contain("pass");
        envelopeXml.Should().Contain("<WAYBILL><ID>1</ID></WAYBILL>");
    }

    [Fact]
    public void ParseStringResponse_ExtractsValue()
    {
        var xml = XDocument.Parse(@"
            <soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                <soap:Body>
                    <get_nameResponse xmlns='http://tempuri.org/'>
                        <get_nameResult>Test Company LLC</get_nameResult>
                    </get_nameResponse>
                </soap:Body>
            </soap:Envelope>");

        var result = _client.TestParseStringResponse(xml, "get_nameResult");
        result.Should().Be("Test Company LLC");
    }

    [Fact]
    public void ParseBoolResponse_ReturnsTrueForTrue()
    {
        var xml = XDocument.Parse(@"
            <soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                <soap:Body>
                    <send_waybillResponse xmlns='http://tempuri.org/'>
                        <send_waybillResult>true</send_waybillResult>
                    </send_waybillResponse>
                </soap:Body>
            </soap:Envelope>");

        _client.TestParseBoolResponse(xml, "send_waybillResult").Should().BeTrue();
    }

    [Fact]
    public void ParseBoolResponse_ReturnsFalseForFalse()
    {
        var xml = XDocument.Parse(@"
            <soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                <soap:Body>
                    <send_waybillResponse xmlns='http://tempuri.org/'>
                        <send_waybillResult>false</send_waybillResult>
                    </send_waybillResponse>
                </soap:Body>
            </soap:Envelope>");

        _client.TestParseBoolResponse(xml, "send_waybillResult").Should().BeFalse();
    }

    [Fact]
    public void ParseIntResponse_ExtractsInteger()
    {
        var xml = XDocument.Parse(@"
            <soap:Envelope xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>
                <soap:Body>
                    <get_countResponse xmlns='http://tempuri.org/'>
                        <get_countResult>42</get_countResult>
                    </get_countResponse>
                </soap:Body>
            </soap:Envelope>");

        _client.TestParseIntResponse(xml, "get_countResult").Should().Be(42);
    }
}
