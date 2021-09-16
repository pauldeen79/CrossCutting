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
            var connection = new DbConnection();

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
            var connection = new DbConnection();
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
            var connection = new DbConnection();

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
            var connection = new DbConnection();
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
        public void Add_Throws_When_CommandDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.Add(new MyEntity { Property = "filled" }, _ => true, null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("commandDelegate");
        }

        [Fact]
        public void Add_Throws_When_IsAddDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.Add(new MyEntity { Property = "filled" }, null, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("isAddDelegate");
        }

        [Fact]
        public void Add_Throws_When_CommandDelegate_Returns_Null()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
#pragma warning disable CS8603 // Possible null reference return.
            connection.Invoking(x => x.Add(new MyEntity { Property = "filled" }, _ => true, _ => null))
#pragma warning restore CS8603 // Possible null reference return.
                      .Should().Throw<ArgumentException>()
                      .And.ParamName.Should().Be("commandDelegate");
        }

        [Fact]
        public void Add_Throws_When_ResultEntityDelegate_Returns_Null()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.Add(new MyEntity { Property = "filled" }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), _ => null))
                      .Should().Throw<ArgumentException>()
                      .WithMessage("Instance should be supplied, or result entity delegate should deliver an instance");
        }

        [Fact]
        public void Add_Throws_When_OperationValidationDelegate_Returns_False()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.Add(new MyEntity { Property = "filled" }, _ => false, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), x => x))
                      .Should().Throw<ArgumentException>()
                      .And.Message.Should().StartWith("MyEntity entity cannot be added, because it's an existing item");
        }

        [Fact]
        public void Add_Does_Not_Throw_When_OperationValidationDelegate_Returns_True()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.Add(new MyEntity { Property = "filled" }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), x => x))
                      .Should().NotThrow<ArgumentException>();
        }

        [Fact]
        public void Add_Throws_When_Instance_Validation_Fails()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.Add(new MyEntity { Property = null }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text)))
                      .Should().Throw<ValidationException>()
                      .WithMessage("The Property field is required.");
        }

        [Fact]
        public void Add_No_AfterReadDelegate_Throws_When_ExecuteNonQuery_Returns_0()
        {
            // Arrange
            var connection = new DbConnection();
            connection.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResult = 0; //0 rows affected
            };

            // Act
            connection.Invoking(x => x.Add(new MyEntity { Property = "test" }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text)))
                      .Should().Throw<DataException>()
                      .WithMessage("MyEntity entity was not added");
        }

        [Fact]
        public void Add_No_AfterReadDelegate_Does_Not_Throw_When_ExecuteNonQuery_Returns_1()
        {
            // Arrange
            var connection = new DbConnection();
            connection.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResult = 1; //1 row affected
            };

            // Act
            connection.Invoking(x => x.Add(new MyEntity { Property = "test" }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text)))
                      .Should().NotThrow<DataException>();
        }

        [Fact]
        public void Add_AfterReadDelegate_Throws_When_ExecuteReader_Read_Returns_False()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
            connection.Invoking(x => x.Add(new MyEntity { Property = "test" }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), afterReadDelegate: (entity, exception) => entity))
                      .Should().Throw<DataException>()
                      .WithMessage("MyEntity entity was not added");
        }

        [Fact]
        public void Add_Returns_ResultEntity_When_FinalizeDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new MyEntity { Property = "test1" } });

            // Act
            var result = connection.Add(new MyEntity { Property = "test" }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), afterReadDelegate: (entity, reader) =>
            {
                entity.Property = reader.GetString("Property");
                return entity;
            });

            // Assert
            result.Should().NotBeNull();
            result.Property.Should().Be("test1");
        }

        [Fact]
        public void Add_Returns_FinalizeDelegate_Result_When_FinalizeDelegate_Is_Not_Null()
        {
            // Arrange
            var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new MyEntity { Property = "test1" } });

            // Act
            var result = connection.Add(new MyEntity { Property = "test" }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), afterReadDelegate: (entity, reader) =>
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
        public void Add_Executes_FinalizeDelegate_And_Rethrows_When_Exception_Occurs_In_Database()
        {
            // Arrange
            var connection = new DbConnection();
            connection.AddResultForDataReader(new Action<DataReader>(_ => throw new InvalidOperationException("Kaboom")), new[] { new MyEntity { Property = "test1" } });

            // Act
            connection.Invoking(x => x.Add(new MyEntity { Property = "test" }, _ => true, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), afterReadDelegate: (entity, reader) =>
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
        public void Update_Throws_When_CommandDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.Update(new MyEntity { Property = "filled" }, _ => false, null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("commandDelegate");
        }

        [Fact]
        public void Update_Throws_When_IsAddDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.Update(new MyEntity { Property = "filled" }, null, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("isAddDelegate");
        }

        [Fact]
        public void Update_Returns_ResultEntity_When_FinalizeDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();
            connection.AddResultForDataReader(new[] { new MyEntity { Property = "test1" } });

            // Act
            var result = connection.Update(new MyEntity { Property = "test" }, _ => false, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text), afterReadDelegate: (entity, reader) =>
            {
                entity.Property = reader.GetString("Property");
                return entity;
            });

            // Assert
            result.Should().NotBeNull();
            result.Property.Should().Be("test1");
        }

        [Fact]
        public void Delete_Throws_When_CommandDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.Delete(new MyEntity { Property = "filled" }, _ => false, null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("commandDelegate");
        }

        [Fact]
        public void Delete_Throws_When_IsAddDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            connection.Invoking(x => x.Delete(new MyEntity { Property = "filled" }, null, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                      .Should().Throw<ArgumentNullException>()
                      .And.ParamName.Should().Be("isAddDelegate");
        }

        [Fact]
        public void Delete_Returns_ResultEntity_When_FinalizeDelegate_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();
            // Note that Delete doesn't have an after read delegate... If you wnt this, you probably use a soft delete, and need to use the Update method ;-)
            connection.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResult = 1; //1 row affected
            };

            // Act
            var result = connection.Delete(new MyEntity { Property = "test" }, _ => false, _ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));

            // Assert
            result.Should().NotBeNull();
            result.Property.Should().Be("test");
        }

        [Fact]
        public void FindOne_Throws_When_Command_Is_Null()
        {
            // Arrange
            var connection = new DbConnection();

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
            var connection = new DbConnection();

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
            var connection = new DbConnection();
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
            var connection = new DbConnection();
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
            var connection = new DbConnection();
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
            var connection = new DbConnection();

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
            var connection = new DbConnection();

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
            var connection = new DbConnection();
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
            var connection = new DbConnection();
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
            var connection = new DbConnection();
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
