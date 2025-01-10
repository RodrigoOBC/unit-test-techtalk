namespace SimulatorBank.Tests;
using ModelsBank;

public class ClientTests
{
    [Fact]
    public void Client_Creation_ShouldInitializeWithValidData()
    {

        // Arrange
        string name = "John Doe";
        string NumberIdentify = "123.456.789-00";


        // Act
        var client = new Client(name, NumberIdentify);

        // Assert
        Assert.Equal(name, client.Name);
        Assert.Equal(NumberIdentify, client.NumberIdentify);

    }


    [Theory]
    [InlineData(null, "123.456.789-00")]
    [InlineData("John Doe", null)]
    [InlineData(null, null)]
    public void Client_Creation_ShouldThrowExceptionWhenAttributIsNull(string name, string NumberIdentify)
    {

        // Act
        var exception = Assert.Throws<ArgumentNullException>(() => new Client(name, NumberIdentify));

        // Assert
        Assert.Contains("Value cannot be null", exception.Message);
    }



    [Fact]
    public void Client_ChangeName_ShouldChangeName()
    {
        // Arrange
        string name = "John Doe";
        string NumberIdentify = "123.456.789-00";
        var client = new Client(name, NumberIdentify);

        // Act
        client.ChangeName("Jane Doe");

        // Assert
        Assert.Equal("Jane Doe", client.Name);
    }

    [Fact]
    public void Client_ChangeName_ShouldThrowExceptionWhenNameIsNull()
    {
        // Arrange
        string name = "John Doe";
        string NumberIdentify = "123.456.789-00";
        var client = new Client(name, NumberIdentify);

        // Act
        var exception = Assert.Throws<Exception>(() => client.ChangeName(null));

        // Assert
        Assert.Contains("Name is required", exception.Message);
    }

    [Theory]
    [InlineData("Jo", "123.456.789-00")]
    [InlineData("J", "123.456.789-00")]
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "123.456.789-00")]
    public void Client_ChangeName_ShouldThrowExceptionWhenNameIsInvalid(string name, string NumberIdentify)
    {
        // Arrange
        var client = new Client(name, NumberIdentify);

        // Act
        var exception = Assert.Throws<Exception>(() => client.ChangeName(name));

        // Assert
        Assert.Contains("Name must be between 3 and 100 characters", exception.Message);
    }

    
}