using System.Net;
using System.Text;
using RsGe.NET.Invoice.Soap;
using RsGe.NET.Invoice.Soap.Models;
using FluentAssertions;

namespace RsGe.NET.Invoice.Tests;

public class SoapResponseParsingTests
{
    private static HttpClient CreateMockHttpClient(string soapResponseXml)
    {
        var handler = new MockHttpMessageHandler(soapResponseXml);
        return new HttpClient(handler);
    }

    private static InvoiceSoapClient CreateClient(string soapResponseXml)
    {
        return new InvoiceSoapClient(
            httpClient: CreateMockHttpClient(soapResponseXml),
            serviceUrl: "http://localhost/test");
    }

    private class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;

        public MockHttpMessageHandler(string response) => _response = response;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_response, Encoding.UTF8, "text/xml")
            });
        }
    }

    #region WhatIsMyIp

    [Fact]
    public async Task WhatIsMyIp_ParsesIp()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <what_is_my_ipResponse xmlns="http://tempuri.org/">
                  <what_is_my_ipResult>1.2.3.4</what_is_my_ipResult>
                </what_is_my_ipResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.WhatIsMyIpAsync();

        // Assert
        result.Should().Be("1.2.3.4");
    }

    #endregion

    #region Check

    [Fact]
    public async Task Check_ParsesUserIdAndSuccess()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <chekResponse xmlns="http://tempuri.org/">
                  <chekResult>true</chekResult>
                  <user_id>42</user_id>
                  <sua>test_sua</sua>
                </chekResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.CheckAsync("su", "sp", 0);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.UserId.Should().Be(42);
    }

    #endregion

    #region SaveInvoice

    [Fact]
    public async Task SaveInvoice_ParsesInvoiceId()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <save_invoiceResponse xmlns="http://tempuri.org/">
                  <save_invoiceResult>true</save_invoiceResult>
                  <invois_id>12345</invois_id>
                </save_invoiceResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.SaveInvoiceAsync(
            userId: 1,
            invoiceId: 0,
            operationDate: new DateTime(2026, 1, 15),
            sellerUnId: 731937,
            buyerUnId: 123456,
            overheadNo: "INV-001",
            overheadDate: new DateTime(2026, 1, 15),
            bsUserId: 0,
            su: "su",
            sp: "sp");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.InvoiceId.Should().Be(12345);
    }

    #endregion

    #region GetInvoice

    [Fact]
    public async Task GetInvoice_ParsesInvoiceInfo()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <get_invoiceResponse xmlns="http://tempuri.org/">
                  <get_invoiceResult>
                    <InvoiceInfo xmlns="">
                      <f_series>AF</f_series>
                      <f_number>00001234</f_number>
                      <operation_dt>2026-01-15</operation_dt>
                      <reg_dt>2026-01-15</reg_dt>
                      <seller_un_id>731937</seller_un_id>
                      <buyer_un_id>123456</buyer_un_id>
                      <overhead_no>INV-001</overhead_no>
                      <overhead_dt>2026-01-15</overhead_dt>
                      <status>1</status>
                      <seq_num_s></seq_num_s>
                      <seq_num_b></seq_num_b>
                      <k_id>0</k_id>
                      <r_un_id>0</r_un_id>
                      <k_type>0</k_type>
                      <b_s_user_id>0</b_s_user_id>
                      <dec_status>0</dec_status>
                    </InvoiceInfo>
                  </get_invoiceResult>
                </get_invoiceResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetInvoiceAsync(
            userId: 1,
            invoiceId: 100,
            su: "su",
            sp: "sp");

        // Assert
        result.Should().NotBeNull();
        result!.FSeries.Should().Be("AF");
        result.FNumber.Should().Be("00001234");
        result.OperationDate.Should().Be(new DateTime(2026, 1, 15));
        result.RegDate.Should().Be(new DateTime(2026, 1, 15));
        result.SellerUnId.Should().Be(731937);
        result.BuyerUnId.Should().Be(123456);
        result.OverheadNo.Should().Be("INV-001");
        result.OverheadDate.Should().Be(new DateTime(2026, 1, 15));
        result.Status.Should().Be(1);
        result.SeqNumS.Should().BeEmpty();
        result.SeqNumB.Should().BeEmpty();
        result.KId.Should().Be(0);
        result.RUnId.Should().Be(0);
        result.KType.Should().Be(0);
        result.BsUserId.Should().Be(0);
        result.DecStatus.Should().Be(0);
    }

    [Fact]
    public async Task GetInvoice_ReturnsNull_WhenNoInvoiceElement()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <get_invoiceResponse xmlns="http://tempuri.org/">
                  <get_invoiceResult />
                </get_invoiceResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetInvoiceAsync(
            userId: 1,
            invoiceId: 999,
            su: "su",
            sp: "sp");

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetInvoiceDesc

    [Fact]
    public async Task GetInvoiceDescriptions_ParsesList()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <get_invoice_descResponse xmlns="http://tempuri.org/">
                  <get_invoice_descResult>
                    <InvoiceDesc xmlns="">
                      <id>1001</id>
                      <invois_id>500</invois_id>
                      <goods>Widget A</goods>
                      <g_unit>pcs</g_unit>
                      <g_number>10.5</g_number>
                      <full_amount>1050.75</full_amount>
                      <drg_amount>157.61</drg_amount>
                      <aqcizi_amount>0.00</aqcizi_amount>
                      <akciz_id>0</akciz_id>
                    </InvoiceDesc>
                    <InvoiceDesc xmlns="">
                      <id>1002</id>
                      <invois_id>500</invois_id>
                      <goods>Widget B</goods>
                      <g_unit>kg</g_unit>
                      <g_number>5.25</g_number>
                      <full_amount>262.50</full_amount>
                      <drg_amount>39.38</drg_amount>
                      <aqcizi_amount>10.00</aqcizi_amount>
                      <akciz_id>3</akciz_id>
                    </InvoiceDesc>
                  </get_invoice_descResult>
                </get_invoice_descResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetInvoiceDescAsync(
            userId: 1,
            invoiceId: 500,
            su: "su",
            sp: "sp");

        // Assert
        result.Should().HaveCount(2);

        result[0].Id.Should().Be(1001);
        result[0].InvoiceId.Should().Be(500);
        result[0].Goods.Should().Be("Widget A");
        result[0].GUnit.Should().Be("pcs");
        result[0].GNumber.Should().Be(10.5m);
        result[0].FullAmount.Should().Be(1050.75m);
        result[0].DrgAmount.Should().Be(157.61m);
        result[0].AqciziAmount.Should().Be(0.00m);
        result[0].AkcizId.Should().Be(0);

        result[1].Id.Should().Be(1002);
        result[1].InvoiceId.Should().Be(500);
        result[1].Goods.Should().Be("Widget B");
        result[1].GUnit.Should().Be("kg");
        result[1].GNumber.Should().Be(5.25m);
        result[1].FullAmount.Should().Be(262.50m);
        result[1].DrgAmount.Should().Be(39.38m);
        result[1].AqciziAmount.Should().Be(10.00m);
        result[1].AkcizId.Should().Be(3);
    }

    #endregion

    #region GetSellerInvoices

    [Fact]
    public async Task GetSellerInvoices_ParsesSearchResults()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <get_seller_invoicesResponse xmlns="http://tempuri.org/">
                  <get_seller_invoicesResult>
                    <Invoice xmlns="">
                      <invois_id>2001</invois_id>
                      <f_series>AF</f_series>
                      <f_number>00005678</f_number>
                      <operation_date>2026-01-10</operation_date>
                      <reg_date>2026-01-10</reg_date>
                      <seller_un_id>731937</seller_un_id>
                      <seller_tin>206322102</seller_tin>
                      <seller_name>Test Seller LLC</seller_name>
                      <buyer_un_id>123456</buyer_un_id>
                      <buyer_tin>302112233</buyer_tin>
                      <buyer_name>Test Buyer LLC</buyer_name>
                      <total_amount>5000.50</total_amount>
                      <status>2</status>
                      <overhead_no>OH-001</overhead_no>
                      <overhead_date>2026-01-10</overhead_date>
                    </Invoice>
                    <Invoice xmlns="">
                      <invois_id>2002</invois_id>
                      <f_series>AF</f_series>
                      <f_number>00005679</f_number>
                      <operation_date>2026-01-12</operation_date>
                      <reg_date>2026-01-12</reg_date>
                      <seller_un_id>731937</seller_un_id>
                      <seller_tin>206322102</seller_tin>
                      <seller_name>Test Seller LLC</seller_name>
                      <buyer_un_id>789012</buyer_un_id>
                      <buyer_tin>405566778</buyer_tin>
                      <buyer_name>Another Buyer</buyer_name>
                      <total_amount>1250.00</total_amount>
                      <status>1</status>
                      <overhead_no>OH-002</overhead_no>
                      <overhead_date>2026-01-12</overhead_date>
                    </Invoice>
                  </get_seller_invoicesResult>
                </get_seller_invoicesResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetSellerInvoicesAsync(
            userId: 1,
            unId: 731937,
            startDate: new DateTime(2026, 1, 1),
            endDate: new DateTime(2026, 1, 31),
            opStartDate: null,
            opEndDate: null,
            invoiceNo: "",
            saIdentNo: "",
            desc: "",
            docMosNom: "",
            su: "su",
            sp: "sp");

        // Assert
        result.Should().HaveCount(2);

        result[0].InvoiceId.Should().Be(2001);
        result[0].FSeries.Should().Be("AF");
        result[0].FNumber.Should().Be("00005678");
        result[0].SellerUnId.Should().Be(731937);
        result[0].BuyerUnId.Should().Be(123456);
        result[0].TotalAmount.Should().Be(5000.50m);
        result[0].Status.Should().Be(2);
        result[0].SellerTin.Should().Be("206322102");
        result[0].BuyerTin.Should().Be("302112233");

        result[1].InvoiceId.Should().Be(2002);
        result[1].FSeries.Should().Be("AF");
        result[1].FNumber.Should().Be("00005679");
        result[1].TotalAmount.Should().Be(1250.00m);
        result[1].Status.Should().Be(1);
    }

    #endregion

    #region GetSellerFilterCount

    [Fact]
    public async Task GetSellerFilterCount_ParsesStatusCounts()
    {
        // Arrange
        // The ParseIntResponse method searches for elements in the http://tempuri.org/ namespace,
        // so st_0 through st_8 must be direct children of the response element (in tempuri namespace).
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <get_seller_filter_countResponse xmlns="http://tempuri.org/">
                  <get_seller_filter_countResult>true</get_seller_filter_countResult>
                  <st_0>5</st_0>
                  <st_1>3</st_1>
                  <st_2>10</st_2>
                  <st_3>1</st_3>
                  <st_4>2</st_4>
                  <st_5>0</st_5>
                  <st_6>0</st_6>
                  <st_7>0</st_7>
                  <st_8>0</st_8>
                </get_seller_filter_countResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetSellerFilterCountAsync(
            unId: 731937,
            userId: 1,
            su: "su",
            sp: "sp");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Status0.Should().Be(5);
        result.Status1.Should().Be(3);
        result.Status2.Should().Be(10);
        result.Status3.Should().Be(1);
        result.Status4.Should().Be(2);
        result.Status5.Should().Be(0);
        result.Status6.Should().Be(0);
        result.Status7.Should().Be(0);
        result.Status8.Should().Be(0);
    }

    #endregion
}
