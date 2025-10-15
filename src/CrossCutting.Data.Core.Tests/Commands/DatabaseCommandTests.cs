namespace CrossCutting.Data.Core.Tests.Commands;

public class DatabaseCommandTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Ctor_Throws_On_CommandText(string? commandText)
    {
        Action a = () => _ = new DatabaseCommand<DatabaseCommandTests>(commandText!, DatabaseCommandType.Text, this, _ => null);
        a.ShouldThrow<ArgumentException>()
         .ParamName.ShouldBe("commandText");
    }

    [Fact]
    public void CommandParameters_Returns_Null_When_Delegate_Is_Null()
    {
        // Act
        var actual = new DatabaseCommand<DatabaseCommandTests>("commandtext", DatabaseCommandType.Text, this, null);

        // Assert
        actual.CommandParameters.ShouldBeNull();
    }

    [Fact]
    public void CommandParameters_Returns_Delegate_Result_When_Delegate_Is_Not_Null()
    {
        // Arrange
        var expected = new { Property = "Value" };

        // Act
        var actual = new DatabaseCommand<DatabaseCommandTests>("commandtext", DatabaseCommandType.Text, this, _ => expected);

        // Assert
        actual.CommandParameters.ShouldBeSameAs(expected);
    }
}
