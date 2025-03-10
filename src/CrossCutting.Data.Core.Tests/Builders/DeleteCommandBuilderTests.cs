﻿namespace CrossCutting.Data.Core.Tests.Builders;

public class DeleteCommandBuilderTests
{
    [Fact]
    public void Build_Throws_When_TableName_Is_Missing()
    {
        // Arrange
        var input = new DeleteCommandBuilder();

        // Act & Assert
        Action a = () => input.Build();
        a.ShouldThrow<InvalidOperationException>()
         .Message.ShouldBe("table name is missing");
    }

    [Fact]
    public void Build_Generates_Command_With_Parameters()
    {
        // Arrange
        var input = new DeleteCommandBuilder()
            .From("MyTable")
            .Where("Field1 = @Field1")
            .And("Field2 = @Field2")
            .And("Field3 = @Field3")
            .AppendParameters(new { Field1 = "Value1", Field2 = "Value2", Field3 = "Value3" });

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Delete);
        actual.CommandText.ShouldBe("DELETE FROM MyTable WHERE Field1 = @Field1 AND Field2 = @Field2 AND Field3 = @Field3");
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
    public void Build_Generates_Command_Without_Where_Statement()
    {
        // Arrange
        var input = new DeleteCommandBuilder().From("MyTable");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Delete);
        actual.CommandText.ShouldBe("DELETE FROM MyTable");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        parameters.ShouldBeEmpty();
    }

    [Fact]
    public void Build_Generates_Command_With_OutputValues()
    {
        // Arrange
        var input = new DeleteCommandBuilder()
            .From("MyTable")
            .Where("Field1 = @Field1")
            .And("Field2 = @Field2")
            .And("Field3 = @Field3")
            .AddOutputFields("Field1", "Field2");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Delete);
        actual.CommandText.ShouldBe("DELETE FROM MyTable OUTPUT Field1, Field2 WHERE Field1 = @Field1 AND Field2 = @Field2 AND Field3 = @Field3");
    }

    [Fact]
    public void Build_Generates_Command_With_OutputValues_And_TemporaryTable()
    {
        // Arrange
        var input = new DeleteCommandBuilder()
            .From("MyTable")
            .WithTemporaryTable("@NewValues")
            .Where("Field1 = @Field1")
            .And("Field2 = @Field2")
            .And("Field3 = @Field3")
            .AddOutputFields("Field1", "Field2");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Delete);
        actual.CommandText.ShouldBe("DELETE FROM MyTable OUTPUT Field1, Field2 INTO @NewValues WHERE Field1 = @Field1 AND Field2 = @Field2 AND Field3 = @Field3");
    }

    [Fact]
    public void Can_Clear_And_Rebuild()
    {
        // Arrange
        var input = new DeleteCommandBuilder()
            .From("MyTable")
            .Where("Field = @Field")
            .AppendParameter("Field", "Value");

        // Act
        var actual = input.Clear().From("MyTable2").Build();

        // Assert
        actual.Operation.ShouldBe(DatabaseOperation.Delete);
        actual.CommandText.ShouldBe("DELETE FROM MyTable2");
        actual.CommandParameters.ShouldBeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        parameters.ShouldBeEmpty();
    }
}
