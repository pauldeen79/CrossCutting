﻿namespace CrossCutting.Data.Core.Tests.Builders;

public class InsertCommandBuilderTests
{
    [Fact]
    public void Build_Throws_When_TableName_Is_Missing()
    {
        // Arrange
        var input = new InsertCommandBuilder();

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("table name is missing");
    }

    [Fact]
    public void Build_Throws_When_FieldNames_Are_Empty()
    {
        // Arrange
        var input = new InsertCommandBuilder().Into("MyTable");

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("field names are missing");
    }

    [Fact]
    public void Build_Throws_When_FieldValues_Are_Empty()
    {
        // Arrange
        var input = new InsertCommandBuilder().Into("MyTable").AddFieldNames("Field1", "Field2", "Field3");

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("field values are missing");
    }

    [Fact]
    public void Build_Throws_When_FieldNames_And_FieldValues_Count_Are_Not_Equal()
    {
        // Arrange
        var input = new InsertCommandBuilder().Into("MyTable")
            .AddFieldNames(new[] { "Field1", "Field2", "Field3" }.AsEnumerable())
            .AddFieldValues(new[] { "Value1", "Value2" }.AsEnumerable());

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("field name count should be equal to field value count");
    }

    [Fact]
    public void Build_Generates_Command_With_Parameters()
    {
        // Arrange
        var input = new InsertCommandBuilder().Into("MyTable")
            .AddFieldNames("Field1", "Field2", "Field3")
            .AddFieldValues("@Field1", "@Field2", "@Field3")
            .AppendParameters(new { Field1 = "Value1", Field2 = "Value2", Field3 = "Value3" });

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Insert);
        actual.CommandText.ShouldBe("INSERT INTO MyTable(Field1, Field2, Field3) VALUES(@Field1, @Field2, @Field3)");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(3);
            parameters.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Field1", "Field2", "Field3" });
            parameters.Values.ToArray().ShouldBeEquivalentTo(new object[] { "Value1", "Value2", "Value3" });
        }
    }

    [Fact]
    public void Build_Generates_Command_With_Output_And_Into()
    {
        // Arrange
        var input = new InsertCommandBuilder().Into("MyTable")
            .AddFieldNames("Field1", "Field2", "Field3")
            .AddFieldValues("\"Value1\"", "\"Value2\"", "\"Value3\"")
            .AddOutputFields("INSERTED.Field1", "INSERTED.Field2", "INSERTED.Field3")
            .Into("MyTable")
            .WithTemporaryTable("@NewValues");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Insert);
        actual.CommandText.ShouldBe("INSERT INTO MyTable(Field1, Field2, Field3) OUTPUT INSERTED.Field1, INSERTED.Field2, INSERTED.Field3 INTO @NewValues VALUES(\"Value1\", \"Value2\", \"Value3\")");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        parameters.ShouldBeEmpty();
    }

    [Fact]
    public void Build_Generates_Command_With_Output()
    {
        // Arrange
        var input = new InsertCommandBuilder().Into("MyTable")
            .AddFieldNames(new[] { "Field1", "Field2", "Field3" }.AsEnumerable())
            .AddFieldValues(new[] { "\"Value1\"", "\"Value2\"", "\"Value3\"" }.AsEnumerable())
            .AddOutputFields(new[] { "INSERTED.Field1", "INSERTED.Field2", "INSERTED.Field3" }.AsEnumerable());

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Insert);
        actual.CommandText.ShouldBe("INSERT INTO MyTable(Field1, Field2, Field3) OUTPUT INSERTED.Field1, INSERTED.Field2, INSERTED.Field3 VALUES(\"Value1\", \"Value2\", \"Value3\")");
    }

    [Fact]
    public void Can_Clear_Builder()
    {
        // Arrange
        var input = new InsertCommandBuilder().Into("MyTable")
            .AddFieldNames("Field1", "Field2", "Field3")
            .AddFieldValues("@Field1", "@Field2", "@Field3")
            .AppendParameters(new { Field1 = "Value1", Field2 = "Value2", Field3 = "Value3" });

        // Act
        input.Clear();

        // Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("table name is missing");
    }
}
