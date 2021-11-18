using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityDatabaseCommandEntityProviderTests
    {
        [Theory]
        [InlineData(DatabaseOperation.Insert, "[InsertEntity]")]
        [InlineData(DatabaseOperation.Update, "[UpdateEntity]")]
        [InlineData(DatabaseOperation.Delete, "[DeleteEntity]")]
        public void CommandDelegate_Returns_Correct_Command(DatabaseOperation operation, string expectedCommandText)
        {
            // Arrange
            var sut = new TestEntityDatabaseCommandEntityProvider();
            var entity = new TestEntity("", "", "");

            // Act
            var actual = sut.CommandDelegate.Invoke(entity, operation);

            // Assert
            actual.CommandText.Should().Be(expectedCommandText);
        }

        [Theory]
        [InlineData(DatabaseOperation.Unspecified)]
        [InlineData(DatabaseOperation.Select)]
        public void CommandDelegate_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
        {
            // Arrange
            var sut = new TestEntityDatabaseCommandEntityProvider();
            var entity = new TestEntity("", "", "");

            // Act
            sut.Invoking(x => x.CommandDelegate.Invoke(entity, operation))
               .Should().Throw<ArgumentOutOfRangeException>()
               .And.ParamName.Should().Be("operation");
        }

        [Theory]
        [InlineData(DatabaseOperation.Insert)]
        [InlineData(DatabaseOperation.Update)]
        [InlineData(DatabaseOperation.Delete)]
        public void ResultEntityDelegate_Returns_Correct_ResultEntity(DatabaseOperation operation)
        {
            // Arrange
            var sut = new TestEntityDatabaseCommandEntityProvider();
            var entity = new TestEntity("", "", "");

            // Act
            sut.ResultEntityDelegate.Should().NotBeNull();
            if (sut.ResultEntityDelegate != null)
            {
                var actual = sut.ResultEntityDelegate.Invoke(entity, operation);

                // Assert
                actual.Should().Be(entity);
            }
        }

        [Theory]
        [InlineData(DatabaseOperation.Unspecified)]
        [InlineData(DatabaseOperation.Select)]
        public void ResultEntityDelegate_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
        {
            // Arrange
            var sut = new TestEntityDatabaseCommandEntityProvider();
            var entity = new TestEntity("", "", "");

            // Act
            sut.ResultEntityDelegate.Should().NotBeNull();
            if (sut.ResultEntityDelegate != null)
            {
                sut.Invoking(x => x.ResultEntityDelegate?.Invoke(entity, operation))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.ParamName.Should().Be("operation");
            }
        }

        [Theory]
        [InlineData(DatabaseOperation.Insert)]
        [InlineData(DatabaseOperation.Update)]
        [InlineData(DatabaseOperation.Delete)]
        public void AfterReadDelegate_Returns_Correct_ResultEntity(DatabaseOperation operation)
        {
            // Arrange
            var sut = new TestEntityDatabaseCommandEntityProvider();
            var entity = new TestEntity("", "", "");
            var readerMock = new Mock<IDataReader>();
            readerMock.SetupGet(x => x.FieldCount).Returns(3);
            readerMock.Setup(x => x.GetName(It.IsAny<int>())).Returns<int>(index =>
            {
                switch (index)
                {
                    case 0:
                        return "Code";
                    case 1:
                        return "CodeType";
                    case 2:
                        return "Description";
                    default:
                        return string.Empty;
                }
            });
            readerMock.Setup(x => x.GetOrdinal(It.IsAny<string>())).Returns<string>(name =>
            {
                switch (name)
                {
                    case "Code":
                        return 0;
                    case "CodeType":
                        return 1;
                    case "Description":
                        return 2;
                    default:
                        return -1;
                }
            });
            readerMock.Setup(x => x.GetString(It.IsAny<int>())).Returns<int>(index =>
            {
                switch (index)
                {
                    case 0:
                        return "new code";
                    case 1:
                        return "new code type";
                    case 2:
                        return "new description";
                    default:
                        return string.Empty;
                }
            });

            // Act
            sut.AfterReadDelegate.Should().NotBeNull();
            if (sut.AfterReadDelegate != null)
            {
                var actual = sut.AfterReadDelegate.Invoke(entity, operation, readerMock.Object);

                // Assert
                actual.Should().Be(entity);
                if (operation != DatabaseOperation.Delete)
                {
                    actual.Code.Should().Be("new code");
                    actual.CodeType.Should().Be("new code type");
                    actual.Description.Should().Be("new description");
                }
            }
        }

        [Theory]
        [InlineData(DatabaseOperation.Unspecified)]
        [InlineData(DatabaseOperation.Select)]
        public void AfterReadDelegate_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
        {
            // Arrange
            var sut = new TestEntityDatabaseCommandEntityProvider();
            var entity = new TestEntity("", "", "");
            var readerMock = new Mock<IDataReader>();

            // Act
            sut.AfterReadDelegate.Should().NotBeNull();
            if (sut.AfterReadDelegate != null)
            {
                sut.Invoking(x => x.AfterReadDelegate?.Invoke(entity, operation, readerMock.Object))
                   .Should().Throw<ArgumentOutOfRangeException>()
                   .And.ParamName.Should().Be("operation");
            }
        }
    }
}
