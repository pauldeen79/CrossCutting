using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Core.Builders;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Core.Tests.Builders
{
    [ExcludeFromCodeCoverage]
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
            actual.CommandText.Should().Be("SELECT * FROM Fridge WHERE Alcohol > @percentage");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            if (parameters != null)
            {
                parameters.Should().HaveCount(1);
                parameters.First().Key.Should().Be("percentage");
                parameters.First().Value.Should().Be(10);
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
            actual.CommandText.Should().Be("test");
            actual.CommandParameters.Should().BeAssignableTo<IDictionary<string, object>>();
            var parameters = actual.CommandParameters as IDictionary<string, object>;
            if (parameters != null)
            {
                parameters.Should().HaveCount(0);
            }
        }
    }
}
