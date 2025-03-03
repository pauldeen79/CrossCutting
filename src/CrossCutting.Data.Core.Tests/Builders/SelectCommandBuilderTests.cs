﻿namespace CrossCutting.Data.Core.Tests.Builders;

public class SelectCommandBuilderTests
{
    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Where_Clause_Using_Single_Parameter()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .Select("Field1", "Field2")
            .Where("Field1 = @field1")
            .AppendParameter("field1", "some value")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Field1, Field2 FROM Table WHERE Field1 = @field1");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(1);
            parameters.First().Key.ShouldBe("field1");
            parameters.First().Value.ShouldBe("some value");
        }
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Where_Clause_Using_Parameters_Object()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .Select("Field1", "Field2")
            .Where("Field1 = @field1")
            .AppendParameters(new { field1 = "some value" })
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Field1, Field2 FROM Table WHERE Field1 = @field1");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(1);
            parameters.First().Key.ShouldBe("field1");
            parameters.First().Value.ShouldBe("some value");
        }
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_And_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .Select("Field1", "Field2")
            .Where("Field1 = @field1")
            .And("Field2 IS NOT NULL")
            .AppendParameter("field1", "some value")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Field1, Field2 FROM Table WHERE Field1 = @field1 AND Field2 IS NOT NULL");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(1);
            parameters.First().Key.ShouldBe("field1");
            parameters.First().Value.ShouldBe("some value");
        }
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Or_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .Select("Field1", "Field2")
            .Where("Field1 = @field1")
            .Or("Field2 IS NOT NULL")
            .AppendParameter("field1", "some value")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Field1, Field2 FROM Table WHERE Field1 = @field1 OR Field2 IS NOT NULL");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(1);
            parameters.First().Key.ShouldBe("field1");
            parameters.First().Value.ShouldBe("some value");
        }
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_OrderBy_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .Select("Field1", "Field2")
            .OrderBy("Field1")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Field1, Field2 FROM Table ORDER BY Field1");
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_InnerJoin_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .InnerJoin("Table2 ON Table.Id = Table2.FkId")
            .Select("Table.Field1, Table.Field2, Table2.Field3")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Table.Field1, Table.Field2, Table2.Field3 FROM Table INNER JOIN Table2 ON Table.Id = Table2.FkId");
    }

    [Fact]
    public void InnerJoin_Throws_Exception_When_FromClause_Is_Empty()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        Action a = () => command.InnerJoin("Table2 ON Table.Id = Table2.FkId");
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("No FROM clause found to add INNER JOIN clause to");
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_LeftOuterJoin_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .LeftOuterJoin("Table2 ON Table.Id = Table2.FkId")
            .Select("Table.Field1", "Table.Field2", "Table2.Field3")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Table.Field1, Table.Field2, Table2.Field3 FROM Table LEFT OUTER JOIN Table2 ON Table.Id = Table2.FkId");
    }

    [Fact]
    public void LeftOuterJoin_Throws_Exception_When_FromClause_Is_Empty()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        Action a = () => command.LeftOuterJoin("Table2 ON Table.Id = Table2.FkId");
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("No FROM clause found to add LEFT OUTER JOIN clause to");
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_RightOuterJoin_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .RightOuterJoin("Table2 ON Table.Id = Table2.FkId")
            .Select("Table.Field1", "Table.Field2", "Table2.Field3")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Table.Field1, Table.Field2, Table2.Field3 FROM Table RIGHT OUTER JOIN Table2 ON Table.Id = Table2.FkId");
    }

    [Fact]
    public void RightOuterJoin_Throws_Exception_When_FromClause_Is_Empty()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        Action a = () => command.RightOuterJoin("Table2 ON Table.Id = Table2.FkId");
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("No FROM clause found to add RIGHT OUTER JOIN clause to");
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_CrossJoin_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .CrossJoin("Table2")
            .Select("Table.Field1", "Table.Field2", "Table2.Field3")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Table.Field1, Table.Field2, Table2.Field3 FROM Table CROSS JOIN Table2");
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Top_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .WithTop(1)
            .From("Table")
            .Select("Field1", "Field2")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT TOP 1 Field1, Field2 FROM Table");
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Distinct_Clause()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .DistinctValues()
            .From("Table")
            .Select("Field1", "Field2")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT DISTINCT Field1, Field2 FROM Table");
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_Without_Fields()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT * FROM Table");
    }

    [Fact]
    public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_GroupBy_And_Having_Clauses()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        var actual = command
            .From("Table")
            .Select("Field1, Field2, COUNT(Field3)") // use single string, works as well
            .GroupBy("Field3")
            .Having("Field3 IS NOT NULL")
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Field1, Field2, COUNT(Field3) FROM Table GROUP BY Field3 HAVING Field3 IS NOT NULL");
    }

    [Fact]
    public void Can_Clear_SelectCommandBuilder()
    {
        // Arrange
        var command = new SelectCommandBuilder()
            .DistinctValues()
            .WithTop(1)
            .From("Table2")
            .Select("Field4", "Field5", "Field6");

        // Act
        var actual = command.Clear()
            .From("Table")
            .Select(new List<string>(["Field1", "Field2", "Field3"])) // use enumerable, works as well
            .Build();

        // Assert
        actual.CommandText.ShouldBe("SELECT Field1, Field2, Field3 FROM Table");
    }

    [Fact]
    public void SelectCommandBuilder_Throws_When_No_From_Clause_Is_Found()
    {
        // Arrange
        var command = new SelectCommandBuilder();

        // Act
        Action a = () => command.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("FROM clause is missing");
    }
}
