namespace CrossCutting.Data.Core.Tests.Builders;

public class UpdateCommandBuilderTests
{
    [Fact]
    public void Build_Throws_When_TableName_Is_Missing()
    {
        // Arrange
        var input = new UpdateCommandBuilder();

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("table name is missing");
    }

    [Fact]
    public void Build_Throws_When_FieldNames_Are_Empty()
    {
        // Arrange
        var input = new UpdateCommandBuilder().WithTable("MyTable");

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("field names are missing");
    }

    [Fact]
    public void Build_Throws_When_FieldValues_Are_Empty()
    {
        // Arrange
        var input = new UpdateCommandBuilder()
            .WithTable("MyTable")
            .AddFieldNames("Field1", "Field2", "Field3");

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("field values are missing");
    }

    [Fact]
    public void Build_Throws_When_FieldNames_And_FieldValues_Count_Are_Not_Equal()
    {
        // Arrange
        var input = new UpdateCommandBuilder()
            .WithTable("MyTable")
            .AddFieldNames("Field1", "Field2", "Field3")
            .AddFieldValues("Value1", "Value2");

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("field name count should be equal to field value count");
    }

    [Fact]
    public void Build_Generates_Command_With_Parameters()
    {
        // Arrange
        var input = new UpdateCommandBuilder()
            .WithTable("MyTable")
            .AddFieldNames("Field1", "Field2", "Field3")
            .AddFieldValues("@Field1", "@Field2", "@Field3")
            .AppendParameters(new { Field1 = "Value1", Field2 = "Value2", Field3 = "Value3" });

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Update);
        actual.CommandText.ShouldBe("UPDATE MyTable SET Field1 = @Field1, Field2 = @Field2, Field3 = @Field3");
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
    public void Build_Generates_Command_With_Where_Statement()
    {
        // Arrange
        var input = new UpdateCommandBuilder()
            .WithTable("MyTable")
            .AddFieldNames("Field1", "Field2", "Field3")
            .AddFieldValues("\"Field1\"", "\"Field2\"", "\"Field3\"")
            .Where("Field1 = \"OldValue1\"")
            .And("Field2 = \"OldValue2\"")
            .And("Field3 = \"OldValue3\"");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Update);
        actual.CommandText.ShouldBe("UPDATE MyTable SET Field1 = \"Field1\", Field2 = \"Field2\", Field3 = \"Field3\" WHERE Field1 = \"OldValue1\" AND Field2 = \"OldValue2\" AND Field3 = \"OldValue3\"");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        parameters.ShouldBeEmpty();
    }

    [Fact]
    public void Build_Generates_Command_With_Output_Statement()
    {
        // Arrange
        var input = new UpdateCommandBuilder()
            .WithTable("MyTable")
            .AddFieldNames("Field1", "Field2", "Field3")
            .AddFieldValues("\"Field1\"", "\"Field2\"", "\"Field3\"")
            .AddOutputFields("Field1", "Field2")
            .Where("Field1 = \"OldValue1\"")
            .And("Field2 = \"OldValue2\"")
            .And("Field3 = \"OldValue3\"");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Update);
        actual.CommandText.ShouldBe("UPDATE MyTable SET Field1 = \"Field1\", Field2 = \"Field2\", Field3 = \"Field3\" OUTPUT Field1, Field2 WHERE Field1 = \"OldValue1\" AND Field2 = \"OldValue2\" AND Field3 = \"OldValue3\"");
    }

    [Fact]
    public void Build_Generates_Command_With_Output_Statement_And_TemporaryTable()
    {
        // Arrange
        var input = new UpdateCommandBuilder()
            .WithTable("MyTable")
            .AddFieldNames(new[] { "Field1", "Field2", "Field3" }.AsEnumerable())
            .AddFieldValues(new[] { "\"Field1\"", "\"Field2\"", "\"Field3\"" }.AsEnumerable())
            .AddOutputFields(new[] { "Field1", "Field2" }.AsEnumerable())
            .WithTemporaryTable("@NewValues")
            .Where("Field1 = \"OldValue1\"")
            .And("Field2 = \"OldValue2\"")
            .And("Field3 = \"OldValue3\"");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Update);
        actual.CommandText.ShouldBe("UPDATE MyTable SET Field1 = \"Field1\", Field2 = \"Field2\", Field3 = \"Field3\" OUTPUT Field1, Field2 INTO @NewValues WHERE Field1 = \"OldValue1\" AND Field2 = \"OldValue2\" AND Field3 = \"OldValue3\"");
    }

    [Fact]
    public void Can_Add_Single_Parameter()
    {
        // Arrange
        var input = new UpdateCommandBuilder()
            .WithTable("MyTable")
            .AddFieldNames("Field1")
            .AddFieldValues("@Field1")
            .AppendParameter("Field1", "Value1");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Update);
        actual.CommandText.ShouldBe("UPDATE MyTable SET Field1 = @Field1");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(1);
            parameters.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Field1" });
            parameters.Values.ToArray().ShouldBeEquivalentTo(new object[] { "Value1" });
        }
    }

    [Fact]
    public void Can_Clear_And_Rebuild()
    {
        // Arrange
        var input = new UpdateCommandBuilder()
            .WithTable("MyTable")
            .AddFieldNames("Field1")
            .AddFieldValues("@Field1")
            .AppendParameter("Field1", "Value1");

        // Act
        var actual = input.Clear()
            .WithTable("MyTable2")
            .AddFieldNames("Field2")
            .AddFieldValues("@Field2")
            .AppendParameter("Field2", "Value2")
            .Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Update);
        actual.CommandText.ShouldBe("UPDATE MyTable2 SET Field2 = @Field2");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Count.ShouldBe(1);
            parameters.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Field2" });
            parameters.Values.ToArray().ShouldBeEquivalentTo(new object[] { "Value2" });
        }
    }
}
