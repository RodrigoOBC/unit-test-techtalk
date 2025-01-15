using System.Data;
using Moq;
using Xunit;
using SimulatorBankUnitTest.ModelsBank.DBConnect;

namespace SimulatorBankUnitTest.Tests
{
    public class ConectorTests
    {
        [Fact]
        public void Conector_AddUser_ShouldAddUserToDatabase()
        {
            // Arrange
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var mockParameters = new Mock<IDataParameterCollection>();
            var Id_user = Guid.NewGuid();

            mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameters.Object);
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            var conector = new Conector(mockConnection.Object);

            // Act
            conector.AddUser(Id_user.ToString(), "John Doe", "123456");

            // Assert
            // Verifica se a propriedade CommandText foi configurada corretamente
            mockCommand.VerifySet(cmd => cmd.CommandText = "INSERT INTO user (id, name, numberID) VALUES (@id, @name, @numberID)");
            // Verifica se três parâmetros foram adicionados ao comando
            mockCommand.Verify(cmd => cmd.Parameters.Add(It.IsAny<IDataParameter>()), Times.Exactly(3));
            // Verifica se o método ExecuteNonQuery foi chamado uma vez
            mockCommand.Verify(cmd => cmd.ExecuteNonQuery(), Times.Once);
            // verificar se a tabela user foi preenchida corretamente
            mockParameters.Verify(p => p.Add(It.Is<IDataParameter>(p => p.ParameterName == "@id" && p.Value.Equals(Id_user.ToString()))), Times.Once);

        }

        [Fact]
        public void Conector_AddUser_ShouldThrowExceptionWhenDatabaseErrorOccurs()
        {
            // Arrange
            var mockConnection = new Mock<IDbConnection>();
            var mockCommand = new Mock<IDbCommand>();
            var mockParameters = new Mock<IDataParameterCollection>();
            var Id_user = Guid.NewGuid();

            mockCommand.Setup(cmd => cmd.Parameters).Returns(mockParameters.Object);
            mockCommand.Setup(cmd => cmd.ExecuteNonQuery()).Throws(new Exception("Database error"));
            mockConnection.Setup(conn => conn.CreateCommand()).Returns(mockCommand.Object);
            var conector = new Conector(mockConnection.Object);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => conector.AddUser(Id_user.ToString(), "John Doe", "123456"));
            Assert.Equal("Database error", exception.Message);
        }
    }
}
