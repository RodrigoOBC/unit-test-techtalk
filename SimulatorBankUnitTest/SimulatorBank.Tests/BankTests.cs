namespace SimulatorBank.Tests;
using System.Data;
using ModelsBank;
using Moq;
using SimulatorBankUnitTest.ModelsBank.DBConnect;
using SimulatorBankUnitTest.ModelsBank;

public class BankTests
{
    [Fact]
    public void Bank_CreateAccount_ShouldThrowExceptionWhenSaveClientFails()
    {
        // Arrange
        var mockClient = new Mock<IClient>();
        mockClient.Setup(client => client.Save()).Throws(new Exception("UNIQUE constraint failed: user.numberID"));

        var bank = new Bank();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>  bank.CreateAccount(mockClient.Object, 0));
        Assert.Equal("Error to save account", exception.Message);
    }

    [Fact]
    public void Bank_CreateAccount_Success()
    {
        // Arrange
        var mockClient = new Mock<Client>("John Doe", "12345678900");
        
 
        var mockConnection = new Mock<IDbConnection>();
        var mockCommand = new Mock<IDbCommand>();
        var mockParameters = new Mock<IDataParameterCollection>();
        mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameters.Object);
        mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
        mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns('1');
        var conector = new Conector(mockConnection.Object);
        mockClient.Object.setConector(conector);

        

        var bank = new Bank();
        decimal initialBalance = 1000;

        // Act
        bank.conector = conector;
        var account = bank.CreateAccount(mockClient.Object, initialBalance);

        // Assert
        Assert.NotNull(account);
        Assert.Equal(initialBalance, account.Balance);
        Assert.Contains(account, bank.GetAccounts());
    }

    [Fact]
    public void Bank_CreateAccount_ShouldThrowExceptionWhenSaveAccountFails()
    {
    
        // Arrange
        var mockClient = new Mock<Client>("John Doe", "12345678900");
        var mockConnection = new Mock<IDbConnection>();
        var mockCommand = new Mock<IDbCommand>();
        var mockParameters = new Mock<IDataParameterCollection>();
        mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameters.Object);
        mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
        mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns('1');
        var conector = new Conector(mockConnection.Object);
        mockClient.Object.setConector(conector);


        var mockConnectionBank = new Mock<IDbConnection>();
        var mockCommandBank = new Mock<IDbCommand>();
        var mockParametersBank = new Mock<IDataParameterCollection>();
        mockCommandBank.Setup(cmd => cmd.Parameters).Returns(mockParametersBank.Object);
        mockConnectionBank.Setup(conn => conn.CreateCommand()).Returns(mockCommandBank.Object);
        mockCommandBank.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception("Database error"));
        var conectorBank = new Conector(mockConnectionBank.Object);
        var bank = new Bank();
        bank.conector = conectorBank;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>  bank.CreateAccount(mockClient.Object, 0));
        Assert.Equal("Error to save account", exception.Message);
    }

    [Fact]
    public void Bank_GetAccounts_ShouldReturnAllAccounts_WithMock()
    {
        // Arrange
        var mockClient1 = new Mock<Client>("John Doe", "12345678900");
        var mockClient2 = new Mock<Client>("Jane Doe", "98765432100");

        var mockConnection = new Mock<IDbConnection>();
        var mockCommand = new Mock<IDbCommand>();
        var mockParameters = new Mock<IDataParameterCollection>();
        mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameters.Object);
        mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
        mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Returns(1);

        var conector = new Conector(mockConnection.Object);
        mockClient1.Object.setConector(conector);
        mockClient2.Object.setConector(conector);

        var account1 = new Account(mockClient1.Object, 1000, conector);
        var account2 = new Account(mockClient2.Object, 2000, conector);

        var bank = new Bank();
        bank.SetAccounts(account1);
        bank.SetAccounts(account2);

        // Act
        var accounts = bank.GetAccounts();

        // Assert
        Assert.Equal(2, accounts.Count);
        Assert.Contains(account1, accounts);
        Assert.Contains(account2, accounts);
    }
}