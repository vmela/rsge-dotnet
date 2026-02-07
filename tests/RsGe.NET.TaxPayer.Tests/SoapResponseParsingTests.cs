using System.Net;
using System.Text;
using FluentAssertions;
using RsGe.NET.TaxPayer.Soap;
using RsGe.NET.TaxPayer.Soap.Models;

namespace RsGe.NET.TaxPayer.Tests;

public class SoapResponseParsingTests
{
    private static HttpClient CreateMockHttpClient(string soapResponseXml)
    {
        var handler = new MockHttpMessageHandler(soapResponseXml);
        return new HttpClient(handler);
    }

    private static TaxPayerSoapClient CreateClient(string soapResponseXml)
    {
        return new TaxPayerSoapClient(
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

    #region ParseTaxPayerPublicInfo

    [Fact]
    public async Task GetTaxPayerInfoPublicAsync_ParsesAllFields()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <GetTPInfoPublicResponse xmlns="http://tempuri.org/">
                  <GetTPInfoPublicResult>
                    <ResponseTPInfoPublic xmlns="">
                      <TP_Code>206322102</TP_Code>
                      <TP_Name>შპს ტესტ კომპანია</TP_Name>
                      <TP_Name_En>Test Company LLC</TP_Name_En>
                      <TP_Status>აქტიური</TP_Status>
                      <Registration_Date>2020-01-15</Registration_Date>
                      <Legal_Form>შპს</Legal_Form>
                      <Address>თბილისი, რუსთაველის 1</Address>
                    </ResponseTPInfoPublic>
                  </GetTPInfoPublicResult>
                </GetTPInfoPublicResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetTaxPayerInfoPublicAsync("user", "pass", "206322102");

        // Assert
        result.Should().NotBeNull();
        result!.Tin.Should().Be("206322102");
        result.Name.Should().Be("შპს ტესტ კომპანია");
        result.NameEn.Should().Be("Test Company LLC");
        result.Status.Should().Be("აქტიური");
        result.RegistrationDate.Should().Be("2020-01-15");
        result.LegalForm.Should().Be("შპს");
        result.Address.Should().Be("თბილისი, რუსთაველის 1");
    }

    #endregion

    #region ParseTaxPayerContacts

    [Fact]
    public async Task GetTaxPayerInfoPublicContactsAsync_ParsesAllFields()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <GetTPInfoPublicContactsResponse xmlns="http://tempuri.org/">
                  <GetTPInfoPublicContactsResult>
                    <ResponseTPInfoPublicContacts xmlns="">
                      <TP_Code>206322102</TP_Code>
                      <Email>info@testcompany.ge</Email>
                      <Phone>+995555123456</Phone>
                      <Address>თბილისი, რუსთაველის 1</Address>
                    </ResponseTPInfoPublicContacts>
                  </GetTPInfoPublicContactsResult>
                </GetTPInfoPublicContactsResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetTaxPayerInfoPublicContactsAsync("user", "pass", "206322102");

        // Assert
        result.Should().NotBeNull();
        result!.Tin.Should().Be("206322102");
        result.Email.Should().Be("info@testcompany.ge");
        result.Phone.Should().Be("+995555123456");
        result.Address.Should().Be("თბილისი, რუსთაველის 1");
    }

    #endregion

    #region ParseLegalPersonInfo

    [Fact]
    public async Task GetLegalPersonInfoAsync_ParsesAllFields()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <Get_LegalPerson_InfoResponse xmlns="http://tempuri.org/">
                  <Get_LegalPerson_InfoResult>
                    <LegalPersonResponse xmlns="">
                      <Tin>206322102</Tin>
                      <Name>შპს ტესტ კომპანია</Name>
                      <LegalForm>შპს</LegalForm>
                      <Status>აქტიური</Status>
                      <RegistrationDate>2020-01-15</RegistrationDate>
                      <Address>თბილისი, რუსთაველის 1</Address>
                      <Director>გიორგი გიორგაძე</Director>
                    </LegalPersonResponse>
                  </Get_LegalPerson_InfoResult>
                </Get_LegalPerson_InfoResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetLegalPersonInfoAsync("user", "pass", "206322102");

        // Assert
        result.Should().NotBeNull();
        result!.Tin.Should().Be("206322102");
        result.Name.Should().Be("შპს ტესტ კომპანია");
        result.LegalForm.Should().Be("შპს");
        result.Status.Should().Be("აქტიური");
        result.RegistrationDate.Should().Be("2020-01-15");
        result.Address.Should().Be("თბილისი, რუსთაველის 1");
        result.Director.Should().Be("გიორგი გიორგაძე");
    }

    #endregion

    #region ParsePayerInfo

    [Fact]
    public async Task GetPayerInfoAsync_ParsesAllFields()
    {
        // Arrange — note the RS.GE typo "TaxyPayerResponse"
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <Get_Payer_InfoResponse xmlns="http://tempuri.org/">
                  <Get_Payer_InfoResult>
                    <TaxyPayerResponse xmlns="">
                      <SaidCode>206322102</SaidCode>
                      <Name>შპს ტესტ კომპანია</Name>
                      <Status>აქტიური</Status>
                      <Type>იურიდიული პირი</Type>
                    </TaxyPayerResponse>
                  </Get_Payer_InfoResult>
                </Get_Payer_InfoResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetPayerInfoAsync("user", "pass", "206322102");

        // Assert
        result.Should().NotBeNull();
        result!.SaidCode.Should().Be("206322102");
        result.Name.Should().Be("შპს ტესტ კომპანია");
        result.Status.Should().Be("აქტიური");
        result.Type.Should().Be("იურიდიული პირი");
    }

    #endregion

    #region ParseNaceInfoList

    [Fact]
    public async Task GetPayerNaceInfoAsync_ParsesMultipleNaceEntries()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <Get_Payer_Nace_InfoResponse xmlns="http://tempuri.org/">
                  <Get_Payer_Nace_InfoResult>
                    <NaceInfo xmlns="">
                      <Code>62.01</Code>
                      <Name>კომპიუტერული პროგრამირება</Name>
                      <IsPrimary>true</IsPrimary>
                    </NaceInfo>
                    <NaceInfo xmlns="">
                      <Code>63.11</Code>
                      <Name>მონაცემთა დამუშავება</Name>
                      <IsPrimary>false</IsPrimary>
                    </NaceInfo>
                    <NaceInfo xmlns="">
                      <Code>47.91</Code>
                      <Name>საცალო ვაჭრობა</Name>
                      <IsPrimary>false</IsPrimary>
                    </NaceInfo>
                  </Get_Payer_Nace_InfoResult>
                </Get_Payer_Nace_InfoResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetPayerNaceInfoAsync("user", "pass", "206322102");

        // Assert
        result.Should().HaveCount(3);

        result[0].Code.Should().Be("62.01");
        result[0].Name.Should().Be("კომპიუტერული პროგრამირება");
        result[0].IsPrimary.Should().BeTrue();

        result[1].Code.Should().Be("63.11");
        result[1].Name.Should().Be("მონაცემთა დამუშავება");
        result[1].IsPrimary.Should().BeFalse();

        result[2].Code.Should().Be("47.91");
        result[2].Name.Should().Be("საცალო ვაჭრობა");
        result[2].IsPrimary.Should().BeFalse();
    }

    #endregion

    #region ParseZReportDetails

    [Fact]
    public async Task GetZReportDetailsAsync_ParsesMultipleReportsWithDecimals()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <Get_Z_Report_DetailsResponse xmlns="http://tempuri.org/">
                  <Get_Z_Report_DetailsResult>
                    <ZReportDetail xmlns="">
                      <CashRegisterNumber>CR-001</CashRegisterNumber>
                      <ReportDate>2024-06-15T00:00:00</ReportDate>
                      <ReportNumber>42</ReportNumber>
                      <TotalAmount>1523.75</TotalAmount>
                      <CashAmount>823.50</CashAmount>
                      <CardAmount>700.25</CardAmount>
                      <ReceiptCount>87</ReceiptCount>
                    </ZReportDetail>
                    <ZReportDetail xmlns="">
                      <CashRegisterNumber>CR-002</CashRegisterNumber>
                      <ReportDate>2024-06-15T00:00:00</ReportDate>
                      <ReportNumber>18</ReportNumber>
                      <TotalAmount>2450.00</TotalAmount>
                      <CashAmount>1200.00</CashAmount>
                      <CardAmount>1250.00</CardAmount>
                      <ReceiptCount>134</ReceiptCount>
                    </ZReportDetail>
                  </Get_Z_Report_DetailsResult>
                </Get_Z_Report_DetailsResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetZReportDetailsAsync(
            "user", "pass",
            new DateTime(2024, 6, 15),
            new DateTime(2024, 6, 15));

        // Assert
        result.Should().HaveCount(2);

        result[0].CashRegisterNumber.Should().Be("CR-001");
        result[0].ReportDate.Should().Be(new DateTime(2024, 6, 15));
        result[0].ReportNumber.Should().Be(42);
        result[0].TotalAmount.Should().Be(1523.75m);
        result[0].CashAmount.Should().Be(823.50m);
        result[0].CardAmount.Should().Be(700.25m);
        result[0].ReceiptCount.Should().Be(87);

        result[1].CashRegisterNumber.Should().Be("CR-002");
        result[1].TotalAmount.Should().Be(2450.00m);
        result[1].CashAmount.Should().Be(1200.00m);
        result[1].CardAmount.Should().Be(1250.00m);
        result[1].ReceiptCount.Should().Be(134);
    }

    #endregion

    #region Empty and missing response edge cases

    [Fact]
    public async Task GetPayerNaceInfoAsync_ReturnsEmptyList_WhenNoNaceElements()
    {
        // Arrange
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <Get_Payer_Nace_InfoResponse xmlns="http://tempuri.org/">
                  <Get_Payer_Nace_InfoResult />
                </Get_Payer_Nace_InfoResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetPayerNaceInfoAsync("user", "pass", "206322102");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetTaxPayerInfoPublicAsync_ReturnsNull_WhenResponseElementMissing()
    {
        // Arrange — valid SOAP envelope but no ResponseTPInfoPublic element inside
        const string xml = """
            <soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
              <soap:Body>
                <GetTPInfoPublicResponse xmlns="http://tempuri.org/">
                  <GetTPInfoPublicResult />
                </GetTPInfoPublicResponse>
              </soap:Body>
            </soap:Envelope>
            """;

        var client = CreateClient(xml);

        // Act
        var result = await client.GetTaxPayerInfoPublicAsync("user", "pass", "000000000");

        // Assert
        result.Should().BeNull();
    }

    #endregion
}
