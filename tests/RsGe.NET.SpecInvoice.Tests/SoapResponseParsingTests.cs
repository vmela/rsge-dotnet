using System.Net;
using System.Text;
using RsGe.NET.SpecInvoice.Soap;
using RsGe.NET.SpecInvoice.Soap.Models;
using FluentAssertions;

namespace RsGe.NET.SpecInvoice.Tests;

public class SoapResponseParsingTests
{
    private static HttpClient CreateMockHttpClient(string soapResponseXml)
    {
        var handler = new MockHttpMessageHandler(soapResponseXml);
        return new HttpClient(handler);
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;
        public MockHttpMessageHandler(string response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_response, Encoding.UTF8, "text/xml")
            });
        }
    }

    private static string WrapInSoapEnvelope(string actionResponse, string actionName)
    {
        return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <soap:Body>
    <{actionName}Response xmlns=""http://tempuri.org/"">
      {actionResponse}
    </{actionName}Response>
  </soap:Body>
</soap:Envelope>";
    }

    #region Auth

    [Fact]
    public async Task CheckIn_ParsesResult()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<chek_inResult>true</chek_inResult><sui>42</sui>",
            "chek_in");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.CheckInAsync("testuser", "testpass", 1, "login");

        // Assert
        result.Success.Should().BeTrue();
        result.Sui.Should().Be(42);
        result.Message.Should().Be("Check-in successful");
    }

    [Fact]
    public async Task CheckIn_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<chek_inResult>false</chek_inResult><sui>0</sui>",
            "chek_in");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.CheckInAsync("testuser", "testpass", 1, "login");

        // Assert
        result.Success.Should().BeFalse();
        result.Sui.Should().Be(0);
        result.Message.Should().Be("Check-in failed");
    }

    [Fact]
    public async Task CheckSpecUsers_ParsesResult()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<check_spec_usersResult>true</check_spec_usersResult>",
            "check_spec_users");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.CheckSpecUsersAsync("testuser", "testpass", 100, "check");

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("User validation successful");
    }

    [Fact]
    public async Task CheckSpecUsers_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<check_spec_usersResult>false</check_spec_usersResult>",
            "check_spec_users");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.CheckSpecUsersAsync("testuser", "testpass", 100, "check");

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("User validation failed");
    }

    #endregion

    #region Invoice CRUD

    [Fact]
    public async Task SaveInvoice_ParsesResult()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<save_invoiceResult>true</save_invoiceResult><invois_id>9876</invois_id>",
            "save_invoice");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.SaveInvoiceAsync(
            invoiceId: 0, fSeries: "AA", operationDt: "2025-01-01",
            sellerUnId: 100, buyerUnId: 200,
            ssdN: "SSD1", ssafN: "SSAF1", calcDate: "2025-01-01",
            kSsafN: "", trStDate: "2025-01-01",
            oilStAddress: "addr1", oilStN: "n1",
            oilFnAddress: "addr2", oilFnN: "n2",
            transportType: "auto", transportMark: "BMW",
            driverInfo: "Driver Name", carrierInfo: "Carrier",
            carrieSNo: "C123", pUserId: 1, sUserId: 2, bSUserId: 3,
            ssdDate: "2025-01-01", ssafDate: "2025-01-01",
            payType: "cash", sellerPhone: "555111222",
            buyerPhone: "555333444", driverNo: "01001001001",
            ssafAltNumber: "", ssafAltType: "", ssdAltNumber: "", ssdAltType: "",
            ssafAltStatus: "", ssdAltStatus: "",
            userId: 1, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeTrue();
        result.InvoiceId.Should().Be(9876);
        result.Message.Should().Be("Invoice saved successfully");
    }

    [Fact]
    public async Task SaveInvoice_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<save_invoiceResult>false</save_invoiceResult><invois_id>0</invois_id>",
            "save_invoice");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.SaveInvoiceAsync(
            invoiceId: 0, fSeries: "AA", operationDt: "2025-01-01",
            sellerUnId: 100, buyerUnId: 200,
            ssdN: "", ssafN: "", calcDate: "", kSsafN: "", trStDate: "",
            oilStAddress: "", oilStN: "", oilFnAddress: "", oilFnN: "",
            transportType: "", transportMark: "", driverInfo: "", carrierInfo: "",
            carrieSNo: "", pUserId: 0, sUserId: 0, bSUserId: 0,
            ssdDate: "", ssafDate: "", payType: "", sellerPhone: "",
            buyerPhone: "", driverNo: "", ssafAltNumber: "", ssafAltType: "",
            ssdAltNumber: "", ssdAltType: "", ssafAltStatus: "", ssdAltStatus: "",
            userId: 1, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeFalse();
        result.InvoiceId.Should().Be(0);
        result.Message.Should().Be("Failed to save invoice");
    }

    #endregion

    #region Status

    [Fact]
    public async Task ChangeInvoiceStatus_ParsesResult()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<change_invoice_statusResult>true</change_invoice_statusResult>",
            "change_invoice_status");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.ChangeInvoiceStatusAsync(
            userId: 1, invId: 100, status: 2, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Invoice status changed successfully");
    }

    [Fact]
    public async Task ChangeInvoiceStatus_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<change_invoice_statusResult>false</change_invoice_statusResult>",
            "change_invoice_status");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.ChangeInvoiceStatusAsync(
            userId: 1, invId: 100, status: 2, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to change invoice status");
    }

    [Fact]
    public async Task AcceptInvoiceStatus_ParsesResult()
    {
        // Arrange — note the actual SOAP action is "acsept_invoice_status" (service typo)
        var xml = WrapInSoapEnvelope(
            "<acsept_invoice_statusResult>true</acsept_invoice_statusResult>",
            "acsept_invoice_status");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.AcceptInvoiceStatusAsync(
            userId: 1, invId: 200, status: 3, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Invoice accepted successfully");
    }

    [Fact]
    public async Task AcceptInvoiceStatus_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<acsept_invoice_statusResult>false</acsept_invoice_statusResult>",
            "acsept_invoice_status");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.AcceptInvoiceStatusAsync(
            userId: 1, invId: 200, status: 3, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to accept invoice");
    }

    [Fact]
    public async Task RefuseInvoiceStatus_ParsesResult()
    {
        // Arrange — actual SOAP action is "ref_invoice_status"
        var xml = WrapInSoapEnvelope(
            "<ref_invoice_statusResult>true</ref_invoice_statusResult>",
            "ref_invoice_status");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.RefuseInvoiceStatusAsync(
            userId: 1, invId: 300, refText: "Incorrect data", su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Invoice refused successfully");
    }

    [Fact]
    public async Task RefuseInvoiceStatus_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<ref_invoice_statusResult>false</ref_invoice_statusResult>",
            "ref_invoice_status");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.RefuseInvoiceStatusAsync(
            userId: 1, invId: 300, refText: "Incorrect data", su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to refuse invoice");
    }

    [Fact]
    public async Task GauqmebisMizezi_ParsesCancellationReason()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<gauqmebis_mizeziResult>true</gauqmebis_mizeziResult>",
            "gauqmebis_mizezi");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.GauqmebisMizeziAsync(
            userId: 1, invId: 400, reason: "Product defective", su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Cancellation reason saved successfully");
    }

    [Fact]
    public async Task GauqmebisMizezi_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<gauqmebis_mizeziResult>false</gauqmebis_mizeziResult>",
            "gauqmebis_mizezi");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.GauqmebisMizeziAsync(
            userId: 1, invId: 400, reason: "Product defective", su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to save cancellation reason");
    }

    #endregion

    #region Descriptions

    [Fact]
    public async Task SaveInvoiceDesc_ParsesResult()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<save_invoice_descResult>true</save_invoice_descResult><id>555</id>",
            "save_invoice_desc");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.SaveInvoiceDescAsync(
            userId: 1, id: 0, su: "testuser", sp: "testpass",
            invId: 100, goods: "Fuel", gUnit: "liter",
            gNumber: 50.5m, unPrice: 3.20m, drgAmount: 0m, aqciziAmount: 10m,
            pUserId: 1, aqciziId: 5, aqciziRate: "18", dggRate: "0",
            gNumberAlt: "", goodId: "G001", drgType: "");

        // Assert
        result.Success.Should().BeTrue();
        result.DescriptionId.Should().Be(555);
        result.Message.Should().Be("Invoice description saved successfully");
    }

    [Fact]
    public async Task SaveInvoiceDesc_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<save_invoice_descResult>false</save_invoice_descResult><id>0</id>",
            "save_invoice_desc");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.SaveInvoiceDescAsync(
            userId: 1, id: 0, su: "testuser", sp: "testpass",
            invId: 100, goods: "Fuel", gUnit: "liter",
            gNumber: 50.5m, unPrice: 3.20m, drgAmount: 0m, aqciziAmount: 10m,
            pUserId: 1, aqciziId: 5, aqciziRate: "18", dggRate: "0",
            gNumberAlt: "", goodId: "G001", drgType: "");

        // Assert
        result.Success.Should().BeFalse();
        result.DescriptionId.Should().Be(0);
        result.Message.Should().Be("Failed to save invoice description");
    }

    [Fact]
    public async Task DeleteInvoiceDesc_ParsesResult()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<delete_invoice_descResult>true</delete_invoice_descResult>",
            "delete_invoice_desc");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.DeleteInvoiceDescAsync(
            userId: 1, id: 555, invId: 100, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Invoice description deleted successfully");
    }

    [Fact]
    public async Task DeleteInvoiceDesc_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<delete_invoice_descResult>false</delete_invoice_descResult>",
            "delete_invoice_desc");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.DeleteInvoiceDescAsync(
            userId: 1, id: 555, invId: 100, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to delete invoice description");
    }

    #endregion

    #region SSAF/SSD

    [Fact]
    public async Task AddSpecInvoicesSsaf_ParsesResult()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<add_spec_invoices_ssafResult>true</add_spec_invoices_ssafResult>",
            "add_spec_invoices_ssaf");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.AddSpecInvoicesSsafAsync(
            userId: 1, invId: 100, ssafN: "SSAF-001", su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("SSAF added successfully");
    }

    [Fact]
    public async Task AddSpecInvoicesSsaf_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<add_spec_invoices_ssafResult>false</add_spec_invoices_ssafResult>",
            "add_spec_invoices_ssaf");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.AddSpecInvoicesSsafAsync(
            userId: 1, invId: 100, ssafN: "SSAF-001", su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to add SSAF");
    }

    #endregion

    #region Search

    [Fact]
    public async Task GetSellerFilterCount_ParsesStatusCounts()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            @"<get_seller_filter_countResult>true</get_seller_filter_countResult>
              <st_0>10</st_0>
              <st_1>20</st_1>
              <st_2>5</st_2>
              <st_3>3</st_3>
              <st_4>15</st_4>
              <st_5>0</st_5>
              <st_6>7</st_6>
              <st_7>1</st_7>
              <st_8>42</st_8>",
            "get_seller_filter_count");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.GetSellerFilterCountAsync(
            unId: 100, userId: 1, su: "testuser", sp: "testpass");

        // Assert
        result.Status0.Should().Be(10);
        result.Status1.Should().Be(20);
        result.Status2.Should().Be(5);
        result.Status3.Should().Be(3);
        result.Status4.Should().Be(15);
        result.Status5.Should().Be(0);
        result.Status6.Should().Be(7);
        result.Status7.Should().Be(1);
        result.Status8.Should().Be(42);
    }

    [Fact]
    public async Task GetSellerFilterCount_AllZeros_ParsesCorrectly()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            @"<get_seller_filter_countResult>true</get_seller_filter_countResult>
              <st_0>0</st_0>
              <st_1>0</st_1>
              <st_2>0</st_2>
              <st_3>0</st_3>
              <st_4>0</st_4>
              <st_5>0</st_5>
              <st_6>0</st_6>
              <st_7>0</st_7>
              <st_8>0</st_8>",
            "get_seller_filter_count");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.GetSellerFilterCountAsync(
            unId: 100, userId: 1, su: "testuser", sp: "testpass");

        // Assert
        result.Status0.Should().Be(0);
        result.Status1.Should().Be(0);
        result.Status2.Should().Be(0);
        result.Status3.Should().Be(0);
        result.Status4.Should().Be(0);
        result.Status5.Should().Be(0);
        result.Status6.Should().Be(0);
        result.Status7.Should().Be(0);
        result.Status8.Should().Be(0);
    }

    #endregion

    #region Transport

    [Fact]
    public async Task StartTransport_ParsesResult()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<start_transportResult>true</start_transportResult>",
            "start_transport");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.StartTransportAsync(
            userId: 1, invId: 100, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Transport started successfully");
    }

    [Fact]
    public async Task StartTransport_FailedResponse_ParsesAsFalse()
    {
        // Arrange
        var xml = WrapInSoapEnvelope(
            "<start_transportResult>false</start_transportResult>",
            "start_transport");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.StartTransportAsync(
            userId: 1, invId: 100, su: "testuser", sp: "testpass");

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Failed to start transport");
    }

    #endregion

    #region Bool parsing with "1" value

    [Fact]
    public async Task CheckIn_ParsesBoolFromNumericOne()
    {
        // Arrange — ParseBoolResponse also accepts "1" as true
        var xml = WrapInSoapEnvelope(
            "<chek_inResult>1</chek_inResult><sui>99</sui>",
            "chek_in");
        var httpClient = CreateMockHttpClient(xml);
        var client = new SpecInvoiceSoapClient(httpClient: httpClient);

        // Act
        var result = await client.CheckInAsync("testuser", "testpass", 1, "login");

        // Assert
        result.Success.Should().BeTrue();
        result.Sui.Should().Be(99);
    }

    #endregion
}
