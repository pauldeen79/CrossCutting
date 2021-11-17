using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Core.Builders;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Core.Tests.Builders
{
    [ExcludeFromCodeCoverage]
    public class SelectCommandBuilderTests
    {
        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Where_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .Select("Field1, Field2")
                .Where("Field1 = @field1")
                .AppendParameter("field1", "some value")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Field1, Field2 FROM Table WHERE Field1 = @field1");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            if (parameters != null)
            {
                parameters.Should().HaveCount(1);
                parameters.First().Key.Should().Be("field1");
                parameters.First().Value.Should().Be("some value");
            }
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_And_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .Select("Field1, Field2")
                .Where("Field1 = @field1")
                .And("Field2 IS NOT NULL")
                .AppendParameter("field1", "some value")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Field1, Field2 FROM Table WHERE Field1 = @field1 AND Field2 IS NOT NULL");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            if (parameters != null)
            {
                parameters.Should().HaveCount(1);
                parameters.First().Key.Should().Be("field1");
                parameters.First().Value.Should().Be("some value");
            }
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Or_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .Select("Field1, Field2")
                .Where("Field1 = @field1")
                .Or("Field2 IS NOT NULL")
                .AppendParameter("field1", "some value")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Field1, Field2 FROM Table WHERE Field1 = @field1 OR Field2 IS NOT NULL");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            if (parameters != null)
            {
                parameters.Should().HaveCount(1);
                parameters.First().Key.Should().Be("field1");
                parameters.First().Value.Should().Be("some value");
            }
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_OrderBy_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .Select("Field1, Field2")
                .OrderBy("Field1")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Field1, Field2 FROM Table ORDER BY Field1");
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_InnerJoin_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .InnerJoin("Table2 ON Table.Id = Table2.FkId")
                .Select("Table.Field1, Table.Field2, Table2.Field3")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Table.Field1, Table.Field2, Table2.Field3 FROM Table INNER JOIN Table2 ON Table.Id = Table2.FkId");
        }

        [Fact]
        public void InnerJoin_Throws_Exception_When_FromClause_Is_Empty()
        {
            // Arrange
            var command = new SelectCommandBuilder().AsText();

            // Act
            command.Invoking(x => x.InnerJoin("Table2 ON Table.Id = Table2.FkId"))
                   .Should().Throw<InvalidOperationException>()
                   .WithMessage("No FROM clause found to add INNER JOIN clause to");
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_LeftOuterJoin_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .LeftOuterJoin("Table2 ON Table.Id = Table2.FkId")
                .Select("Table.Field1, Table.Field2, Table2.Field3")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Table.Field1, Table.Field2, Table2.Field3 FROM Table LEFT OUTER JOIN Table2 ON Table.Id = Table2.FkId");
        }

        [Fact]
        public void LeftOuterJoin_Throws_Exception_When_FromClause_Is_Empty()
        {
            // Arrange
            var command = new SelectCommandBuilder().AsText();

            // Act
            command.Invoking(x => x.LeftOuterJoin("Table2 ON Table.Id = Table2.FkId"))
                   .Should().Throw<InvalidOperationException>()
                   .WithMessage("No FROM clause found to add LEFT OUTER JOIN clause to");
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_RightOuterJoin_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .RightOuterJoin("Table2 ON Table.Id = Table2.FkId")
                .Select("Table.Field1, Table.Field2, Table2.Field3")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Table.Field1, Table.Field2, Table2.Field3 FROM Table RIGHT OUTER JOIN Table2 ON Table.Id = Table2.FkId");
        }

        [Fact]
        public void RightOuterJoin_Throws_Exception_When_FromClause_Is_Empty()
        {
            // Arrange
            var command = new SelectCommandBuilder().AsText();

            // Act
            command.Invoking(x => x.RightOuterJoin("Table2 ON Table.Id = Table2.FkId"))
                   .Should().Throw<InvalidOperationException>()
                   .WithMessage("No FROM clause found to add RIGHT OUTER JOIN clause to");
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_CrossJoin_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .CrossJoin("Table2")
                .Select("Table.Field1, Table.Field2, Table2.Field3")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Table.Field1, Table.Field2, Table2.Field3 FROM Table CROSS JOIN Table2");
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Top_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .Top(1)
                .From("Table")
                .Select("Field1, Field2")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT TOP 1 Field1, Field2 FROM Table");
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_Distinct_Clause()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .Distinct()
                .From("Table")
                .Select("Field1, Field2")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT DISTINCT Field1, Field2 FROM Table");
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_Without_Fields()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT * FROM Table");
        }

        [Fact]
        public void Can_Build_SelectCommand_From_SelectCommandBuilder_With_GroupBy_And_Having_Clauses()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            var actual = command
                .AsText()
                .From("Table")
                .Select("Field1, Field2, COUNT(Field3)")
                .GroupBy("Field3")
                .Having("Field3 IS NOT NULL")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Field1, Field2, COUNT(Field3) FROM Table GROUP BY Field3 HAVING Field3 IS NOT NULL");
        }

        [Fact]
        public void Can_Clear_SelectCommandBuilder()
        {
            // Arrange
            var command = new SelectCommandBuilder().AsStoredProcedure()
                .From("Table2")
                .Select("Field4, Field5, Field6");

            // Act
            var actual = command.Clear()
                .AsText()
                .From("Table")
                .Select("Field1, Field2, Field3")
                .Build();

            // Assert
            actual.CommandText.Should().Be("SELECT Field1, Field2, Field3 FROM Table");
        }

        [Fact]
        public void SelectCommandBuilder_Throws_When_No_From_Clause_Is_Found()
        {
            // Arrange
            var command = new SelectCommandBuilder();

            // Act
            command.Invoking(x => x.Build())
                   .Should().Throw<InvalidOperationException>()
                   .And.Message.Should().Be("FROM clause is missing");
        }
    }
}
