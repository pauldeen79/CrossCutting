using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DbConnectionExtensionsTests
    {
        [Fact]
        public void ExecuteScalar_Throws_When_Command_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.ExecuteScalar(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("command");
        }

        [Fact]
        public void ExecuteScalar_Returns_Correct_Value_When_Command_Is_Not_Null()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteScalarResult = 12345;
            };
            var command = new SqlDbCommand("Select 12345", DatabaseCommandType.Text);

            // Act
            var actual = connection.ExecuteScalar(command);

            // Assert
            actual.Should().Be(12345);
        }

        [Fact]
        public void ExecuteNonQuery_Throws_When_Command_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.ExecuteNonQuery(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("command");
        }

        [Fact]
        public void ExecuteNonQuery_Returns_Correct_Value_When_Command_Is_Not_Null()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResult = 12345;
            };
            var command = new SqlDbCommand("Select 12345", DatabaseCommandType.Text);

            // Act
            var actual = connection.ExecuteNonQuery(command);

            // Assert
            actual.Should().Be(12345);
        }

        [Fact]
        public void Invoke_Throws_When_CommandDelegate_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = "filled" }, null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("commandDelegate");
        }

        [Fact]
        public void Invoke_Throws_When_CommandDelegate_Returns_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8603 // Possible null reference return.
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = "filled" }, _ => null))
#pragma warning restore CS8603 // Possible null reference return.
                      .Should().Throw<ArgumentException>()
                      .And.ParamName.Should().Be("commandDelegate");
        }

        [Fact]
        public void Invoke_Throws_When_ResultEntityDelegate_Returns_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = "filled" }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), null, _ => null))
                      .Should().Throw<ArgumentException>()
                      .WithMessage("Instance should be supplied, or result entity delegate should deliver an instance");
        }

        [Fact]
        public void Invoke_Does_Not_Throw_When_OperationValidationDelegate_Returns_True()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = "filled" }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), null, x => x))
                      .Should().NotThrow<ArgumentException>();
        }

        [Fact]
        public void Invoke_Throws_When_Instance_Validation_Fails()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = null }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text)))
                      .Should().Throw<ValidationException>()
                      .WithMessage("The Property field is required.");
        }

        [Fact]
        public void Invoke_No_AfterReadDelegate_Throws_When_ExecuteNonQuery_Returns_0()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResult = 0; //0 rows affected
            };

            // Act
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), exceptionMessage: "MyEntity entity was not added"))
                      .Should().Throw<DataException>()
                      .WithMessage("MyEntity entity was not added");
        }

        [Fact]
        public void Invoke_No_AfterReadDelegate_Does_Not_Throw_When_ExecuteNonQuery_Returns_1()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResult = 1; //1 row affected
            };

            // Act
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text)))
                      .Should().NotThrow<DataException>();
        }

        [Fact]
        public void Invoke_AfterReadDelegate_Throws_When_ExecuteReader_Read_Returns_False()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), exceptionMessage: "Myentity entity was not added", afterReadDelegate: (entity, exception) => entity))
                      .Should().Throw<DataException>()
                      .WithMessage("MyEntity entity was not added");
        }

        [Fact]
        public void Invoke_Returns_ResultEntity_When_FinalizeDelegate_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new MyEntity { Property = "test1" } });

            // Act
            var result = connection.InvokeCommand(new MyEntity { Property = "test" }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), afterReadDelegate: (entity, reader) =>
            {
                entity.Property = reader.GetString("Property");
                return entity;
            });

            // Assert
            result.Should().NotBeNull();
            result.Property.Should().Be("test1");
        }

        [Fact]
        public void Invoke_Returns_FinalizeDelegate_Result_When_FinalizeDelegate_Is_Not_Null()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new MyEntity { Property = "test1" } });

            // Act
            var result = connection.InvokeCommand(new MyEntity { Property = "test" }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), afterReadDelegate: (entity, reader) =>
            {
                entity.Property = reader.GetString("Property");
                return entity;
            }, finalizeDelegate: (entity, exception) =>
            {
                entity.Property = "test2";
                return entity;
            });

            // Assert
            result.Should().NotBeNull();
            result.Property.Should().Be("test2");
        }

        [Fact]
        public void Invoke_Executes_FinalizeDelegate_And_Rethrows_When_Exception_Occurs_In_Database()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new Action<DataReader>(_ => throw new InvalidOperationException("Kaboom")), new[] { new MyEntity { Property = "test1" } });

            // Act
            connection.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), afterReadDelegate: (entity, reader) =>
            {
                entity.Property = reader.GetString("Property");
                return entity;
            }, finalizeDelegate: (entity, exception) =>
            {
                entity.Property = "test2";
                return entity;
            })).Should().Throw<InvalidOperationException>().WithMessage("Kaboom");
        }

        [Fact]
        public void FindOne_Throws_When_Command_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.FindOne(null, Map))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("command");
        }

        [Fact]
        public void FindOne_Throws_When_MapFunction_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.FindOne<MyEntity>(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("mapFunction");
        }

        [Fact]
        public void FindOne_Returns_MappedEntity_When_All_Goes_Well()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });

            // Act
            var actual = connection.FindOne(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), Map);

            // Assert
            actual.Should().NotBeNull();
            actual.Property.Should().Be("test");
        }

        [Fact]
        public void FindOne_Executes_FinalizeDelegate_On_Success()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
            var isCalled = false;

            // Act
            connection.FindOne(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), Map, (result, exception) => { isCalled = true; return result; });

            // Assert
            isCalled.Should().BeTrue();
        }

        [Fact]
        public void FindOne_Executes_FinalizeDelegate_And_Rethrows_When_Exception_Occurs_In_Database()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new Action<DataReader>(_ => throw new InvalidOperationException("Kaboom")), new[] { new MyEntity { Property = "test" } });
            var isCalled = false;

            // Act
            connection.Invoking(x => x.FindOne(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), Map, (result, exception) => { isCalled = true; return result; }))
                      .Should().Throw<InvalidOperationException>()
                      .And.Message.Should().Be("Kaboom");

            // Assert
            isCalled.Should().BeTrue();
        }

        [Fact]
        public void FindMany_Throws_When_Command_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.FindMany(null, Map))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("command");
        }

        [Fact]
        public void FindMany_Throws_When_MapFunction_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.FindMany<MyEntity>(new SqlDbCommand("SELECT 1 Property FROM MyEntity", DatabaseCommandType.Text), null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("mapFunction");
        }

        [Fact]
        public void FindMany_Returns_MappedEntities_When_All_Goes_Well()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[]
            {
                new MyEntity { Property = "test1" },
                new MyEntity { Property = "test2" }
            });

            // Act
            var actual = connection.FindMany(new SqlDbCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text), Map);

            // Assert
            actual.Should().NotBeNull().And.HaveCount(2);
            actual.First().Property.Should().Be("test1");
            actual.Last().Property.Should().Be("test2");
        }

        [Fact]
        public void FindMany_Executes_FinalizeDelegate_On_Success()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[]
            {
                new MyEntity { Property = "test1" },
                new MyEntity { Property = "test2" }
            });
            var isCalled = false;

            // Act
            connection.FindMany(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), Map, (result, exception) => { isCalled = true; return result; });

            // Assert
            isCalled.Should().BeTrue();
        }

        [Fact]
        public void FindMany_Executes_FinalizeDelegate_And_Rethrows_When_Exception_Occurs_In_Database()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(_ => throw new InvalidOperationException("Kaboom"), new[]
            {
                new MyEntity { Property = "test1" },
                new MyEntity { Property = "test2" }
            });
            connection.AddResultForDataReader(new Action<DataReader>(_ => throw new InvalidOperationException("Kaboom")), new[] { new MyEntity { Property = "test" } });
            var isCalled = false;

            // Act
            connection.Invoking(x => x.FindMany(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), Map, (result, exception) => { isCalled = true; return result; }))
                      .Should().Throw<InvalidOperationException>()
                      .And.Message.Should().Be("Kaboom");

            // Assert
            isCalled.Should().BeTrue();
        }

        [Fact]
        public void FindPaged_Throws_When_DataCommand_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.FindPaged(null, new Mock<IDatabaseCommand>().Object, 0, 10, Map))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("dataCommand");
        }

        [Fact]
        public void FindPaged_Throws_When_RecordCountCommand_Is_Null()
        {
            // Arrange
            using var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.FindPaged(new Mock<IDatabaseCommand>().Object, null, 0, 10, Map))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("recordCountCommand");
        }

        [Fact]
        public void FindPaged_Throws_When_MapFunction_Is_Null()
        {
            // Arrange
            var callback = new DbConnectionCallback();
            var connection = new DbConnection().AddCallback(callback);
            connection.AddResultForScalarCommand(1);

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.FindPaged<MyEntity>(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), new SqlDbCommand("SELECT Count(*) FROM MyEntity", DatabaseCommandType.Text), 0, 10, null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("mapFunction");
        }

        [Fact]
        public void FindPaged_Returns_MappedEntities_And_Other_Properties_When_All_Goes_Well()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[]
            {
                new MyEntity { Property = "test1" },
                new MyEntity { Property = "test2" }
            });
            connection.AddResultForScalarCommand(1);

            // Act
            var actual = connection.FindPaged(new SqlDbCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text), new SqlDbCommand("SELECT COUNT(*) FROM MyEntity", DatabaseCommandType.Text), 20, 10, Map);

            // Assert
            actual.Should().NotBeNull().And.HaveCount(2);
            actual.First().Property.Should().Be("test1");
            actual.Last().Property.Should().Be("test2");
            actual.TotalRecordCount.Should().Be(1);
            actual.Offset.Should().Be(20);
            actual.PageSize.Should().Be(10);
        }

        [Fact]
        public void FindPaged_Executes_FinalizeDelegate_On_Success()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(new[]
            {
                new MyEntity { Property = "test1" },
                new MyEntity { Property = "test2" }
            });
            connection.AddResultForScalarCommand(1);
            var isCalled = false;

            // Act
            connection.FindPaged(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), new SqlDbCommand("SELECT COUNT(*) FROM MyEntity", DatabaseCommandType.Text), 0, 10, Map, (result, exception) => { isCalled = true; return result; });

            // Assert
            isCalled.Should().BeTrue();
        }

        [Fact]
        public void FindPaged_Executes_FinalizeDelegate_And_Rethrows_When_Exception_Occurs_In_Database()
        {
            // Arrange
            using var connection = new DbConnection();
            connection.AddResultForDataReader(_ => throw new InvalidOperationException("Kaboom"), new[]
            {
                new MyEntity { Property = "test1" },
                new MyEntity { Property = "test2" }
            });
            connection.AddResultForDataReader(new Action<DataReader>(_ => throw new InvalidOperationException("Kaboom")), new[] { new MyEntity { Property = "test" } });
            connection.AddResultForScalarCommand(1);
            var isCalled = false;

            // Act
            connection.Invoking(x => x.FindPaged(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text), new SqlDbCommand("SELECT COUNT(*) FROM MyEntity", DatabaseCommandType.Text), 0, 10, Map, (result, exception) => { isCalled = true; return result; }))
                      .Should().Throw<InvalidOperationException>()
                      .And.Message.Should().Be("Kaboom");

            // Assert
            isCalled.Should().BeTrue();
        }

        private MyEntity Map(IDataReader reader)
            => new MyEntity
            {
                Property = reader.GetString("Property")
            };

        private class MyEntity
        {
            [Required]
            public string? Property { get; set; }
        }
    }
}
