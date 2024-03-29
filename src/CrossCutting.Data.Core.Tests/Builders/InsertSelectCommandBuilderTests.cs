﻿namespace CrossCutting.Data.Core.Tests.Builders;

public class InsertSelectCommandBuilderTests
{
    [Fact]
    public void Build_Throws_When_TableName_Is_Missing()
    {
        // Arrange
        var input = new InsertSelectCommandBuilder();

        // Act & Assert
        input.Invoking(x => x.Build())
             .Should().Throw<InvalidOperationException>()
             .WithMessage("table name is missing");
    }

    [Fact]
    public void Build_Throws_When_FieldNames_Are_Empty()
    {
        // Arrange
        var input = new InsertSelectCommandBuilder().Into("MyTable");

        // Act & Assert
        input.Invoking(x => x.Build())
             .Should().Throw<InvalidOperationException>()
             .WithMessage("field names are missing");
    }

    [Fact]
    public void Build_Throws_When_SelectCommand_Is_Empty()
    {
        // Arrange
        var input = new InsertSelectCommandBuilder().Into("MyTable").WithFieldNames("Field1", "Field2", "Field3");

        // Act & Assert
        input.Invoking(x => x.Build())
             .Should().Throw<InvalidOperationException>()
             .WithMessage("FROM clause is missing");
    }

    [Fact]
    public void Build_Generates_Command_With_Parameters()
    {
        // Arrange
        var input = new InsertSelectCommandBuilder().Into("MyTable")
            .WithFieldNames("Field1", "Field2", "Field3")
            .WithSelectCommand(new SelectCommandBuilder()
                .Select("Field1", "Field2", "Field3")
                .From("SomeOtherTable")
                .Where("Field1 = @Field1")
                .And("Field2 = @Field2")
                .And("Field3 = @Field3"))
            .AppendParameters(new { Field1 = "Value1", Field2 = "Value2", Field3 = "Value3" });

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.Should().Be(DatabaseOperation.Insert);
        actual.CommandText.Should().Be("INSERT INTO MyTable(Field1, Field2, Field3) SELECT Field1, Field2, Field3 FROM SomeOtherTable WHERE Field1 = @Field1 AND Field2 = @Field2 AND Field3 = @Field3");
        actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Should().HaveCount(3);
            parameters.Keys.Should().BeEquivalentTo("Field1", "Field2", "Field3");
            parameters.Values.Should().BeEquivalentTo(new[] { "Value1", "Value2", "Value3" });
        }
    }

    [Fact]
    public void Build_Generates_Command_With_Output_And_Into()
    {
        // Arrange
        var input = new InsertSelectCommandBuilder().Into("MyTable")
            .WithFieldNames("Field1", "Field2", "Field3")
            .WithSelectCommand(new SelectCommandBuilder()
                .Select("Field1", "Field2", "Field3")
                .From("SomeOtherTable")
                .Where("Field1 = \"Value1\"")
                .And("Field2 = \"Value2\"")
                .And("Field3 = \"Value3\""))
            .AddOutputFields("INSERTED.Field1", "INSERTED.Field2", "INSERTED.Field3")
            .Into("MyTable")
            .WithTemporaryTable("@NewValues");

        // Act
        var actual = input.Build();

        // Assert
        actual.Operation.Should().Be(DatabaseOperation.Insert);
        actual.CommandText.Should().Be("INSERT INTO MyTable(Field1, Field2, Field3) OUTPUT INSERTED.Field1, INSERTED.Field2, INSERTED.Field3 INTO @NewValues SELECT Field1, Field2, Field3 FROM SomeOtherTable WHERE Field1 = \"Value1\" AND Field2 = \"Value2\" AND Field3 = \"Value3\"");
        actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        parameters.Should().BeEmpty();
    }

    [Fact]
    public void Can_Clear_And_Rebuild()
    {
        // Arrange
        var input = new InsertSelectCommandBuilder().Into("MyTable")
            .WithFieldNames(new[] { "Field1", "Field2", "Field3" }.AsEnumerable())
            .WithSelectCommand(new SelectCommandBuilder()
                .Select("Field1", "Field2", "Field3")
                .From("SomeOtherTable")
                .Where("Field1 = \"Value1\"")
                .And("Field2 = \"Value2\"")
                .And("Field3 = \"Value3\""))
            .AddOutputFields(new[] { "INSERTED.Field1", "INSERTED.Field2", "INSERTED.Field3" }.AsEnumerable())
            .Into("MyTable")
            .WithTemporaryTable("@NewValues");

        // Act
        var actual = input
            .Clear()
            .Into("MyTable")
            .WithFieldNames("Field1", "Field2", "Field3")
            .WithSelectCommand(new SelectCommandBuilder()
                .Select("Field1", "Field2", "Field3")
                .From("SomeOtherTable")
                .Where("Field1 = @Field1")
                .And("Field2 = @Field2")
                .And("Field3 = @Field3"))
            .AppendParameters(new { Field1 = "Value1", Field2 = "Value2", Field3 = "Value3" })
            .Build();

        // Assert
        actual.Operation.Should().Be(DatabaseOperation.Insert);
        actual.CommandText.Should().Be("INSERT INTO MyTable(Field1, Field2, Field3) SELECT Field1, Field2, Field3 FROM SomeOtherTable WHERE Field1 = @Field1 AND Field2 = @Field2 AND Field3 = @Field3");
        actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
        var parameters = actual.CommandParameters as IDictionary<string, object>;
        if (parameters is not null)
        {
            parameters.Should().HaveCount(3);
            parameters.Keys.Should().BeEquivalentTo("Field1", "Field2", "Field3");
            parameters.Values.Should().BeEquivalentTo(new[] { "Value1", "Value2", "Value3" });
        }
    }
}
