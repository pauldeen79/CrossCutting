namespace CrossCutting.Data.Core.Tests.Commands;

public class SqlDatabaseCommandTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Ctor_Throws_On_CommandText(string commandText)
    {
        this.Invoking(_ => new SqlDatabaseCommand(commandText, DatabaseCommandType.Text))
            .Should().Throw<ArgumentOutOfRangeException>()
            .And.ParamName.Should().Be("commandText");
    }

    [Fact]
    public void Can_Construct_NonGeneric()
    {
        // Act
        var actual = new SqlDatabaseCommand("A", DatabaseCommandType.StoredProcedure, DatabaseOperation.Update, new { Parameter = "Value" });

        // Assert
        actual.CommandText.Should().Be("A");
        actual.CommandType.Should().Be(DatabaseCommandType.StoredProcedure);
        actual.Operation.Should().Be(DatabaseOperation.Update);
        actual.CommandParameters.Should().NotBeNull();
        var parameters = actual.CommandParameters.ToExpandoObject() as IDictionary<string, object>;
        parameters.Should().NotBeNull();
        if (parameters is not null)
        {
            parameters["Parameter"].Should().Be("Value");
        }
    }
}
