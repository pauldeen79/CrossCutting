namespace CrossCutting.Data.Core.Tests.Commands;

[ExcludeFromCodeCoverage]
public class DatabaseCommandTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Ctor_Throws_On_CommandText(string commandText)
    {
        this.Invoking(_ => new DatabaseCommand<DatabaseCommandTests>(commandText, DatabaseCommandType.Text, this, _ => null))
            .Should().Throw<ArgumentOutOfRangeException>()
            .And.ParamName.Should().Be("commandText");
    }

    [Fact]
    public void CommandParameters_Returns_Null_When_Delegate_Is_Null()
    {
        // Act
        var actual = new DatabaseCommand<DatabaseCommandTests>("commandtext", DatabaseCommandType.Text, this, null);

        // Assert
        actual.CommandParameters.Should().BeNull();
    }

    [Fact]
    public void CommandParameters_Returns_Delegate_Result_When_Delegate_Is_Not_Null()
    {
        // Arrange
        var expected = new { Property = "Value" };

        // Act
        var actual = new DatabaseCommand<DatabaseCommandTests>("commandtext", DatabaseCommandType.Text, this, _ => expected);

        // Assert
        actual.CommandParameters.Should().BeSameAs(expected);
    }
}
