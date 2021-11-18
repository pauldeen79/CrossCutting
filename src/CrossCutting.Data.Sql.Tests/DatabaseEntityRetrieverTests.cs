using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Stub;
using System.Data.Stub.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Sql.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class DatabaseEntityRetrieverTests : IDisposable
    {
        private DatabaseEntityRetriever<MyEntity> Sut { get; }
        private DbConnection Connection { get; }
        private Mock<IDataReaderMapper<MyEntity>> MapperMock { get; }

        public DatabaseEntityRetrieverTests()
        {
            Connection = new DbConnection();
            MapperMock = new Mock<IDataReaderMapper<MyEntity>>();
            Sut = new DatabaseEntityRetriever<MyEntity>(Connection, MapperMock.Object);
        }

        [Fact]
        public void FindOne_Returns_MappedEntity_When_All_Goes_Well()
        {
            // Arrange
            Connection.AddResultForDataReader(new[] { new MyEntity { Property = "test" } });
            InitializeMapper();

            // Act
            var actual = Sut.FindOne(new SqlDatabaseCommand("SELECT TOP 1 Property FROM MyEntity", DatabaseCommandType.Text));

            // Assert
            actual.Should().NotBeNull();
            if (actual != null)
            {
                actual.Property.Should().Be("test");
            }
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
            var actual = Sut.FindMany(new SqlDatabaseCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text));

            // Assert
            actual.Should().NotBeNull().And.HaveCount(2);
            actual.First().Should().NotBeNull();
            actual.First().Property.Should().Be("test1");
            actual.Last().Should().NotBeNull();
            actual.Last().Property.Should().Be("test2");
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
            var command = new PagedDatabaseCommand(new SqlDatabaseCommand("SELECT Property FROM MyEntity", DatabaseCommandType.Text),
                                                   new SqlDatabaseCommand("SELECT COUNT(*) FROM MyEntity", DatabaseCommandType.Text),
                                                   20,
                                                   10);

            // Act
            var actual = Sut.FindPaged(command);

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
