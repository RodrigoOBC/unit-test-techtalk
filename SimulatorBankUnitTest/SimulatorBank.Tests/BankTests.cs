namespace SimulatorBank.Tests;
using System.Data;
using ModelsBank;
using Moq;
using SimulatorBankUnitTest.ModelsBank.DBConnect;

public class BankTests
{
    [Fact]
    public void Bank_CreateAccount_ShouldThrowExceptionWhenSaveClientFails()
    {
        // Arrange
        var mockClient = new Mock<IClient>();
        mockClient.Setup(client => client.Save()).Throws(new Exception("Save failed"));

        var bank = new Bank();

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => bank.CreateAccount(mockClient.Object, 0));
        Assert.Equal("Save failed", exception.Message);
    }
}