using System.Data;
using Moq;
using Xunit;
using SimulatorBankUnitTest.ModelsBank.DBConnect;

namespace SimulatorBankUnitTest.Tests
{
    public class ConectorTests
    {
        [Fact]
        public void AddUser_ShouldAddUserToDatabase()
        {
            // Arrange
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var mockParameters = new Mock<IDataParameterCollection>();

            mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameters.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            var conector = new Conector(mockConnection.Object);

            // Act
            conector.AddUser("060a3de1-f7c0-4d71-a69e-4030fa77157c", "John Doe", "123456");

            // Assert
            // Verifica se a propriedade CommandText foi configurada corretamente
            mockCommand.VerifySet(cmd => cmd.CommandText = "INSERT INTO user (id, name, numberID) VALUES (@id, @name, @numberID)");
            // Verifica se três parâmetros foram adicionados ao comando
            mockCommand.Verify(cmd => cmd.Parameters.Add(It.IsAny<IDataParameter>()), Times.Exactly(3));
            // Verifica se o método ExecuteNonQuery foi chamado uma vez
            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
            // verificar se a tabela user foi preenchida corretamente
            mockParameters.Verify(p => p.Add(It.Is<IDataParameter>(p => p.ParameterName == "@id" && p.Value.Equals("060a3de1-f7c0-4d71-a69e-4030fa77157c"))), Times.Once);

        }
    }
}
