using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Abstractions.Extensions;
using CrossCutting.Data.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Sql.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class DatabaseCommandProcessorTests : IDisposable
    {
        private DatabaseCommandProcessor<MyEntity> Sut { get; }
        private DbConnection Connection { get; }
        private Mock<IDatabaseCommandEntityProvider<MyEntity>> ProviderMock { get; }

        public DatabaseCommandProcessorTests()
        {
            Connection = new DbConnection();
            ProviderMock = new Mock<IDatabaseCommandEntityProvider<MyEntity>>();
            Sut = new DatabaseCommandProcessor<MyEntity>(Connection, ProviderMock.Object);
        }

        [Fact]
        public void ExecuteScalar_Returns_Correct_Value_When_Command_Is_Not_Null()
        {
            // Arrange
            Connection.AddResultForScalarCommand(12345);
            var command = new SqlDbCommand("Select 12345", DatabaseCommandType.Text);

            // Act
            var actual = Sut.ExecuteScalar(command);

            // Assert
            actual.Should().Be(12345);
        }

        [Fact]
        public void ExecuteNonQuery_Returns_Correct_Value_When_Command_Is_Not_Null()
        {
            // Arrange
            Connection.AddResultForNonQueryCommand(12345);
            var command = new SqlDbCommand("Select 12345", DatabaseCommandType.Text);

            // Act
            var actual = Sut.ExecuteNonQuery(command);

            // Assert
            actual.Should().Be(12345);
        }

        [Fact]
        public void InvokeCommand_Throws_When_ResultEntityDelegate_Returns_Null()
        {
            // Arrange
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));
#pragma warning disable CS8603 // Possible null reference return.
            ProviderMock.SetupGet(x => x.ResultEntityDelegate).Returns(_ => null);
#pragma warning restore CS8603 // Possible null reference return.

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = "filled" }))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("Instance should be supplied, or result entity delegate should deliver an instance");
        }

        [Fact]
        public void InvokeCommand_Does_Not_Throw_When_OperationValidationDelegate_Returns_True()
        {
            // Arrange
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = "filled" }))
               .Should().NotThrow<InvalidOperationException>();
        }

        [Fact]
        public void InvokeCommand_Throws_When_Instance_Validation_Fails()
        {
            // Arrange
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = null }))
               .Should().Throw<ValidationException>()
               .WithMessage("The Property field is required.");
        }

        [Fact]
        public void InvokeCommand_No_AfterReadDelegate_Throws_When_ExecuteNonQuery_Returns_0()
        {
            // Arrange
            Connection.AddResultForNonQueryCommand(0); // 0 rows affected
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added"))
               .Should().Throw<DataException>()
               .WithMessage("MyEntity entity was not added");
        }

        [Fact]
        public void InvokeCommand_No_AfterReadDelegate_Does_Not_Throw_When_ExecuteNonQuery_Returns_1()
        {
            // Arrange
            Connection.AddResultForNonQueryCommand(1); // 1 row affected
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }))
               .Should().NotThrow<DataException>();
        }

        [Fact]
        public void InvokeCommand_AfterReadDelegate_Throws_When_ExecuteReader_Read_Returns_False()
        {
            // Arrange
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));
            ProviderMock.SetupGet(x => x.AfterReadDelegate).Returns(new Func<MyEntity, IDataReader, MyEntity>((x, _) => x));

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added"))
               .Should().Throw<DataException>()
               .WithMessage("MyEntity entity was not added");
        }

        [Fact]
        public void InvokeCommand_AfterReadDelegate_Does_Not_Throw_When_ExecuteReader_Read_Returns_True()
        {
            // Arrange
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));
            ProviderMock.SetupGet(x => x.AfterReadDelegate).Returns(new Func<MyEntity, IDataReader, MyEntity>((x, _) => x));

            // Act
            var actual = Sut.InvokeCommand(new MyEntity { Property = "test" }).HandleResult("MyEntity entity was not added");

            // Assert
            actual.Property.Should().Be("test");
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        [ExcludeFromCodeCoverage]
        public class MyEntity
        {
            [Required]
            public string? Property { get; set; }
        }
    }
}
