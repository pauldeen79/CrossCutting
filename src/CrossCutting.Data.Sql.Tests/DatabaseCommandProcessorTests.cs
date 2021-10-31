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

namespace CrossCutting.Data.Sql.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class DatabaseCommandProcessorTests : IDisposable
    {
        private DatabaseCommandProcessor<MyEntity> Sut { get; }
        private DbConnection Connection { get; }
        private Mock<IDataReaderMapper<MyEntity>> MapperMock { get; }
        private Mock<IDatabaseCommandProcessorSettings> SettingsMock { get; }
        private Mock<IDatabaseCommandEntityProvider<MyEntity>> ProviderMock { get; }

        public DatabaseCommandProcessorTests()
        {
            Connection = new DbConnection();
            MapperMock = new Mock<IDataReaderMapper<MyEntity>>();
            SettingsMock = new Mock<IDatabaseCommandProcessorSettings>();
            ProviderMock = new Mock<IDatabaseCommandEntityProvider<MyEntity>>();
            Sut = new DatabaseCommandProcessor<MyEntity>(Connection, MapperMock.Object, SettingsMock.Object, ProviderMock.Object);
        }

        [Fact]
        public void ExecuteScalar_Throws_When_Command_Is_Null()
        {
            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Sut.Invoking(x => x.ExecuteScalar(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("command");
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
        public void ExecuteNonQuery_Throws_When_Command_Is_Null()
        {
            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Sut.Invoking(x => x.ExecuteNonQuery(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("command");
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
        public void InvokeCommand_Throws_When_CommandDelegate_Returns_Null()
        {
            // Arrange
#pragma warning disable CS8603 // Possible null reference return.
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => null);
#pragma warning restore CS8603 // Possible null reference return.

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = "filled" }))
               .Should().Throw<InvalidOperationException>()
               .WithMessage("CommandDelegate resulted in null");
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
            SettingsMock.SetupGet(x => x.ExceptionMessage).Returns("MyEntity entity was not added");
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }))
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
            SettingsMock.SetupGet(x => x.ExceptionMessage).Returns("MyEntity entity was not added");
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));
            ProviderMock.SetupGet(x => x.AfterReadDelegate).Returns(new Func<MyEntity, IDataReader, MyEntity>((x, _) => x));

            // Act
            Sut.Invoking(x => x.InvokeCommand(new MyEntity { Property = "test" }))
               .Should().Throw<DataException>()
               .WithMessage("MyEntity entity was not added");
        }

        [Fact]
        public void InvokeCommand_AfterReadDelegate_Does_Not_Throw_When_ExecuteReader_Read_Returns_True()
        {
            // Arrange
            InitializeMapper();
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
            SettingsMock.SetupGet(x => x.ExceptionMessage).Returns("MyEntity entity was not added");
            ProviderMock.SetupGet(x => x.CommandDelegate).Returns(_ => new SqlDbCommand("INSERT INTO ...", DatabaseCommandType.Text));
            ProviderMock.SetupGet(x => x.AfterReadDelegate).Returns(new Func<MyEntity, IDataReader, MyEntity>((x, _) => x));

            // Act
            var actual = Sut.InvokeCommand(new MyEntity { Property = "test" });

            // Assert
            actual.Should().NotBeNull();
            actual.Property.Should().Be("test");
        }

        [Fact]
        public void FindOne_Throws_When_Command_Is_Null()
        {
            // Arrange
            InitializeMapper();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Sut.Invoking(x => x.FindOne(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("command");
        }

        [Fact]
        public void FindOne_Returns_MappedEntity_When_All_Goes_Well()
        {
            // Arrange
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
            InitializeMapper();

            // Act
            var actual = Sut.FindOne(new SqlDbCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text));

            // Assert
            actual.Should().NotBeNull();
            actual.Property.Should().Be("test");
        }

        [Fact]
        public void FindMany_Throws_When_Command_Is_Null()
        {
            // Arrange
            InitializeMapper();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Sut.Invoking(x => x.FindMany(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("command");
        }

        [Fact]
        public void FindMany_Returns_MappedEntities_When_All_Goes_Well()
        {
            // Arrange
            Connection.AddResultForDataReader(new[]
            {
                new MyEntity { Property = "test1" },
                new MyEntity { Property = "test2" }
            });
            InitializeMapper();

            // Act
            var actual = Sut.FindMany(new SqlDbCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text));

            // Assert
            actual.Should().NotBeNull().And.HaveCount(2);
            actual.First().Should().NotBeNull();
            actual.First().Property.Should().Be("test1");
            actual.Last().Should().NotBeNull();
            actual.Last().Property.Should().Be("test2");
        }

        [Fact]
        public void FindPaged_Throws_When_DataCommand_Is_Null()
        {
            // Arrange
            InitializeMapper();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Sut.Invoking(x => x.FindPaged(null, new Mock<IDatabaseCommand>().Object, 0, 10))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("dataCommand");
        }

        [Fact]
        public void FindPaged_Throws_When_RecordCountCommand_Is_Null()
        {
            // Arrange
            InitializeMapper();

            // Act
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            Sut.Invoking(x => x.FindPaged(new Mock<IDatabaseCommand>().Object, null, 0, 10))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
               .Should().Throw<ArgumentNullException>()
               .And.ParamName.Should().Be("recordCountCommand");
        }

        [Fact]
        public void FindPaged_Returns_MappedEntities_And_Other_Properties_When_All_Goes_Well()
        {
            // Arrange
            Connection.AddResultForDataReader(new[]
            {
                new MyEntity { Property = "test1" },
                new MyEntity { Property = "test2" }
            });
            Connection.AddResultForScalarCommand(1);
            InitializeMapper();

            // Act
            var actual = Sut.FindPaged(new SqlDbCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text),
                                       new SqlDbCommand("SELECT COUNT(*) FROM MyEntity", DatabaseCommandType.Text),
                                       20,
                                       10);

            // Assert
            actual.Should().NotBeNull().And.HaveCount(2);
            actual.First().Should().NotBeNull();
            actual.First().Property.Should().Be("test1");
            actual.Last().Should().NotBeNull();
            actual.Last().Property.Should().Be("test2");
            actual.TotalRecordCount.Should().Be(1);
            actual.Offset.Should().Be(20);
            actual.PageSize.Should().Be(10);
        }

        private void InitializeMapper()
        {
            MapperMock.Setup(x => x.Map(It.IsAny<IDataReader>()))
                      .Returns<IDataReader>(reader => new MyEntity { Property = reader.GetString("Property") });
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
