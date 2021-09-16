using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DbCommandExtensionsTests
    {
        [Fact]
        public void AddParameter_Creates_Parameter_Correctly()
        {
            // Arrange
            var command = new DbCommand();

            // Act
            command.AddParameter("Name", "Value");

            // Assert
            var parameters = command.Parameters.Cast<IDbDataParameter>();
            parameters.Should().ContainSingle();
            parameters.First().ParameterName.Should().Be("Name");
            parameters.First().Value.Should().Be("Value");
        }

        [Fact]
        public void AddParameters_Throws_On_Null_Argument()
        {
            // Arrange
            var command = new DbCommand();

            // Act & Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            command.Invoking(x => x.AddParameters(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                   .Should()
                   .Throw<ArgumentNullException>().And.ParamName.Should().Be("keyValuePairs");
        }

        [Fact]
        public void AddParameters_Add_All_Parameters_When_Argument_Is_Not_Null()
        {
            // Arrange
            var command = new DbCommand();
            var parameters = new List<KeyValuePair<string, object?>>
            {
                new KeyValuePair<string, object?>("param1", "value1"),
                new KeyValuePair<string, object?>("param2", null),
            };

            // Act
            command.AddParameters(parameters);

            // Assert
            var result = command.Parameters.Cast<IDbDataParameter>();
            result.Should().HaveCount(2);
            result.First().ParameterName.Should().Be("param1");
            result.First().Value.Should().Be("value1");
            result.Last().ParameterName.Should().Be("param2");
            result.Last().Value.Should().Be(DBNull.Value);
        }

        [Fact]
        public void FillCommand_Fills_CommandText_Correctly()
        {
            // Arrange
            var command = new DbCommand();

            // Act
            command.FillCommand("Test", DatabaseCommandType.Text);

            // Assert
            command.CommandText.Should().Be("Test");
        }

        [Theory]
        [InlineData(DatabaseCommandType.StoredProcedure, CommandType.StoredProcedure)]
        [InlineData(DatabaseCommandType.Text, CommandType.Text)]
        public void FillCommand_Fills_CommandType_Correctly(DatabaseCommandType dbCommandType, CommandType expectedCommandType)
        {
            // Arrange
            var command = new DbCommand();

            // Act
            command.FillCommand("Test", dbCommandType);

            // Assert
            command.CommandType.Should().Be(expectedCommandType);
        }

        [Fact]
        public void FillCommand_Fills_Parameters_Correctly()
        {
            // Arrange
            var command = new DbCommand();
            var parameters = new List<KeyValuePair<string, object?>>
            {
                new KeyValuePair<string, object?>("param1", "value1"),
                new KeyValuePair<string, object?>("param2", null),
            };

            // Act
            command.FillCommand("Test", DatabaseCommandType.Text, parameters);

            // Assert
            var result = command.Parameters.Cast<IDbDataParameter>();
            result.Should().HaveCount(2);
            result.First().ParameterName.Should().Be("param1");
            result.First().Value.Should().Be("value1");
            result.Last().ParameterName.Should().Be("param2");
            result.Last().Value.Should().Be(DBNull.Value);
        }

        [Fact]
        public void FindOne_Returns_Correct_Result()
        {
            // Arrange
            var connection = new DbConnection();
            connection.AddResultForDataReader(new[]
            {
                new MyDataObject { Property = "test" }
            });
            var command = connection.CreateCommand();

            // Act
            var result = command.FindOne("SELECT TOP 1 * FROM FRIDGE WHERE Alcohol > 0", DatabaseCommandType.Text, reader => new MyDataObject { Property = reader.GetString("Property") });

            // Assert
            result.Should().NotBeNull();
            result.Property.Should().Be("test");
        }

        [Fact]
        public void FindMany_Returns_Correct_Result()
        {
            // Arrange
            var connection = new DbConnection();
            connection.AddResultForDataReader(new[]
            {
                new MyDataObject { Property = "test1" },
                new MyDataObject { Property = "test2" }
            });
            var command = connection.CreateCommand();

            // Act
            var result = command.FindMany("SELECT * FROM FRIDGE WHERE Alcohol > 0", DatabaseCommandType.Text, reader => new MyDataObject { Property = reader.GetString("Property") });

            // Assert
            result.Should().NotBeNull().And.HaveCount(2);
            result.First().Property.Should().Be("test1");
            result.Last().Property.Should().Be("test2");
        }

        private class MyDataObject
        {
            public string? Property { get; set; }
        }
    }
}
