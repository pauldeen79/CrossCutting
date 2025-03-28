﻿namespace CrossCutting.Data.Core.Tests.Builders;

public class DatabaseCommandBuilderTests
{
    [Fact]
    public void Can_Build_DatabaseCommand_Using_Builder()
    {
        // Arrange
        var sut = new DatabaseCommandBuilder();

        // Act
        var actual = sut.Append("SELECT * FROM Fridge")
                        .Append(" WHERE Alcohol > @percentage")
                        .AppendParameter("percentage", 10)
                        .Build();

        // Assert
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
}
