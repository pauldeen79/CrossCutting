﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Core.Builders;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Core.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class DeleteCommandBuilderTests
    {
        [Fact]
        public void Build_Throws_When_TableName_Is_Missing()
        {
            // Arrange
            var input = new DeleteCommandBuilder();

            // Act & Assert
            input.Invoking(x => x.Build())
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("table name is missing");
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
            actual.Operation.Should().Be(Abstractions.DatabaseOperation.Delete);
            actual.CommandText.Should().Be("DELETE FROM MyTable WHERE Field1 = @Field1 AND Field2 = @Field2 AND Field3 = @Field3");
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
        public void Build_Generates_Command_Without_Where_Statement()
        {
            // Arrange
            var input = new DeleteCommandBuilder().From("MyTable");

            // Act
            var actual = input.Build();

            // Assert
            actual.Operation.Should().Be(Abstractions.DatabaseOperation.Delete);
            actual.CommandText.Should().Be("DELETE FROM MyTable");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            parameters.Should().BeEmpty();
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
            actual.Operation.Should().Be(Abstractions.DatabaseOperation.Delete);
            actual.CommandText.Should().Be("DELETE FROM MyTable2");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            parameters.Should().BeEmpty();
        }
    }
}