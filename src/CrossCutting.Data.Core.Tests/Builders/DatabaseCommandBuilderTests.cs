namespace CrossCutting.Data.Core.Tests.Builders;

public class DatabaseCommandBuilderTests
{
    [Fact]
    public void Can_Build_DatabaseCommand_Using_Builder()
    {
        // Arrange
        var sut = new DatabaseCommandBuilder();

        // Act
        var actual = sut
            .Append("SELECT * FROM Fridge")
            .Append(" WHERE Alcohol > @percentage")
            .AppendParameter("percentage", 10)
            .Build();

        // Assert
        AssertCommand(actual);
    }

    [Fact]
    public void Can_Build_DatabaseCommand_Using_Builder_Typed()
    {
        // Arrange
        var sut = new DatabaseCommandBuilder();

        // Act
        var actual = sut
            .Append("SELECT * FROM Fridge")
            .Append(" WHERE Alcohol > @percentage")
            .AppendParameter("percentage", 10)
            .BuildTyped();

        // Assert
        AssertCommand(actual);
    }

    [Fact]
    public void Can_Create_DatabaseCommand_Using_Builder_Implicit_Operator()
    {
        // Arrange
        var sut = new DatabaseCommandBuilder();

        // Act
        SqlDatabaseCommand actual = sut
            .Append("SELECT * FROM Fridge")
            .Append(" WHERE Alcohol > @percentage")
            .AppendParameter("percentage", 10);

        // Assert
        AssertCommand(actual);
    }

    [Fact]
    public void Can_Clear_DatabaseCommandBuilder()
    {
        // Arrange
        var sut = new DatabaseCommandBuilder();
        sut.Append("SELECT * FROM Fridge")
           .Append(" WHERE Alcohol > @percentage")
           .AppendParameter("percentage", 10);

        // Act
        var actual = sut.Clear().Append("test").Build();

        // Assert
        actual.CommandText.ShouldBe("test");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(0);
        }
    }

    private static void AssertCommand(IDatabaseCommand actual)
    {
        actual.CommandText.ShouldBe("SELECT * FROM Fridge WHERE Alcohol > @percentage");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(1);
            parameters.First().Key.ShouldBe("percentage");
            parameters.First().Value.ShouldBe(10);
        }
    }
}
