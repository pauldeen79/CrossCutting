using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Core.Builders;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Core.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class UpdateCommandBuilderTests
    {
        [Fact]
        public void Build_Throws_When_TableName_Is_Missing()
        {
            // Arrange
            var input = new UpdateCommandBuilder();

            // Act & Assert
            input.Invoking(x => x.Build())
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("table name is missing");
        }

        [Fact]
        public void Build_Throws_When_FieldNames_Are_Empty()
        {
            // Arrange
            var input = new UpdateCommandBuilder().Table("MyTable");

            // Act & Assert
            input.Invoking(x => x.Build())
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("field names are missing");
        }

        [Fact]
        public void Build_Throws_When_FieldValues_Are_Empty()
        {
            // Arrange
            var input = new UpdateCommandBuilder().Table("MyTable").WithFieldNames("Field1", "Field2", "Field3");

            // Act & Assert
            input.Invoking(x => x.Build())
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("field values are missing");
        }

        [Fact]
        public void Build_Throws_When_FieldNames_And_FieldValues_Count_Are_Not_Equal()
        {
            // Arrange
            var input = new UpdateCommandBuilder().Table("MyTable")
                .WithFieldNames("Field1", "Field2", "Field3")
                .WithFieldValues("Value1", "Value2");

            // Act & Assert
            input.Invoking(x => x.Build())
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("field name count should be equal to field value count");
        }

        [Fact]
        public void Build_Generates_Command_With_Parameters()
        {
            // Arrange
            var input = new UpdateCommandBuilder().Table("MyTable")
                .WithFieldNames("Field1", "Field2", "Field3")
                .WithFieldValues("@Field1", "@Field2", "@Field3")
                .AppendParameters(new { Field1 = "Value1", Field2 = "Value2", Field3 = "Value3" });

            // Act
            var actual = input.Build();

            // Assert
            actual.Operation.Should().Be(Abstractions.DatabaseOperation.Update);
            actual.CommandText.Should().Be("UPDATE MyTable SET Field1 = @Field1, Field2 = @Field2, Field3 = @Field3");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            if (parameters != null)
            {
                parameters.Should().HaveCount(3);
                parameters.Keys.Should().BeEquivalentTo(new[] { "Field1", "Field2", "Field3" });
                parameters.Values.Should().BeEquivalentTo(new[] { "Value1", "Value2", "Value3" });
            }
        }

        [Fact]
        public void Build_Generates_Command_With_Where_Statement()
        {
            // Arrange
            var input = new UpdateCommandBuilder().Table("MyTable")
                .WithFieldNames("Field1", "Field2", "Field3")
                .WithFieldValues("\"Field1\"", "\"Field2\"", "\"Field3\"")
                .Where("Field1 = \"OldValue1\"")
                .And("Field2 = \"OldValue2\"")
                .And("Field3 = \"OldValue3\"");

            // Act
            var actual = input.Build();

            // Assert
            actual.Operation.Should().Be(Abstractions.DatabaseOperation.Update);
            actual.CommandText.Should().Be("UPDATE MyTable SET Field1 = \"Field1\", Field2 = \"Field2\", Field3 = \"Field3\" WHERE Field1 = \"OldValue1\" AND Field2 = \"OldValue2\" AND Field3 = \"OldValue3\"");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            parameters.Should().BeEmpty();
        }
    }
}
