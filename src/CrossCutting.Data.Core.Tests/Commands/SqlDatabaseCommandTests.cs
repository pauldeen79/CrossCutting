namespace CrossCutting.Data.Core.Tests.Commands;

public class SqlDatabaseCommandTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Ctor_Throws_On_CommandText(string? commandText)
    {
        Action a = () => _ = new SqlDatabaseCommand(commandText!, DatabaseCommandType.Text);
        a.ShouldThrow<ArgumentOutOfRangeException>()
         .ParamName.ShouldBe("commandText");
    }

    [Fact]
    public void Can_Construct_NonGeneric()
    {
        // Act
        var actual = new SqlDatabaseCommand("A", DatabaseCommandType.StoredProcedure, DatabaseOperation.Update, new { Parameter = "Value" });

        // Assert
        actual.CommandText.ShouldBe("A");
        actual.CommandType.ShouldBe(DatabaseCommandType.StoredProcedure);
        actual.Operation.ShouldBe(DatabaseOperation.Update);
        actual.CommandParameters.ShouldNotBeNull();
        var parameters = actual.CommandParameters.ToExpandoObject() as IDictionary<string, object>;
        parameters.ShouldNotBeNull();
        parameters["Parameter"].ShouldBe("Value");
    }
}
