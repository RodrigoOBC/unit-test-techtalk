using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using ModelsBank;
using SimulatorBankUnitTest.ModelsBank.DBConnect;
using System.Net;
using System.Text.Json;
namespace SimulatorBank.Tests;

public class AccountTests
{

    [Fact]
    public void Account_Creation_ShouldInitializeWithValidData()
    {
        // Arrange
        var clientMock = new Mock<Client>("teste", "123");
        var balance = 0;
        

        // Act
        var account = new Account(clientMock.Object, balance);

        // Assert
        Assert.Equal(clientMock.Object.Name, account.Holder.Name);
        Assert.Equal(balance, account.Balance);
    }


    [Fact]
    public async Task Account_setInitalBalance_ShouldSetBalanceCorrectly()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(new { Balance = 90 }))
        };

        handlerMock
           .Protected()
           .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
           )
           .ReturnsAsync(responseMessage);

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://fd757376-23b1-4fa3-a13f-93fa8cf11485.mock.pstmn.io")
        };

        var clientMock = new Mock<Client>("teste", "123");
        var account = new Account(clientMock.Object, 0, httpClient);

        // Act
        await account.setInitalBalance();

        // Assert
        Assert.Equal(90, account.Balance);
    }
}