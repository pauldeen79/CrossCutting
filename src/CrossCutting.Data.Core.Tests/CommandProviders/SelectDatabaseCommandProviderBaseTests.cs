using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.CommandProviders;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Core.Tests.CommandProviders
{
    [ExcludeFromCodeCoverage]
    public class SelectDatabaseCommandProviderBaseTests
    {
        private Mock<IDatabaseEntityRetrieverSettings> SettingsMock { get; }
        private TestSelectDatabaseCommandProvider Sut { get; }

        public SelectDatabaseCommandProviderBaseTests()
        {
            SettingsMock = new Mock<IDatabaseEntityRetrieverSettings>();
            Sut = new TestSelectDatabaseCommandProvider(SettingsMock.Object);
        }

        [Theory]
        [InlineData(DatabaseOperation.Delete)]
        [InlineData(DatabaseOperation.Insert)]
        [InlineData(DatabaseOperation.Unspecified)]
        [InlineData(DatabaseOperation.Update)]
        public void Create_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
        {
            // Act
            Sut.Invoking(x => x.Create(operation))
               .Should().ThrowExactly<ArgumentOutOfRangeException>().And.ParamName.Should().Be("operation");
        }

        [Fact]
        public void Create_Returns_Correct_Command_On_Select_DatabaseOperation()
        {
            // Arrange
            const string Sql = "SELECT Id, Active, Field1, Field2, Field3 FROM MyTable WHERE Active = 1 ORDER BY Field1";
            SettingsMock.SetupGet(x => x.DefaultOrderBy).Returns("Field1");
            SettingsMock.SetupGet(x => x.DefaultWhere).Returns("Active = 1");
            SettingsMock.SetupGet(x => x.Fields).Returns("Id, Active, Field1, Field2, Field3");
            SettingsMock.SetupGet(x => x.TableName).Returns("MyTable");

            // Act
            var actual = Sut.Create(DatabaseOperation.Select);

            // Assert
            actual.CommandText.Should().Be(Sql);
        }
    }

    [ExcludeFromCodeCoverage]
    public class TestSelectDatabaseCommandProvider : SelectDatabaseCommandProviderBase<IDatabaseEntityRetrieverSettings>
    {
        public TestSelectDatabaseCommandProvider(IDatabaseEntityRetrieverSettings settings) : base(settings)
        {
        }
    }
}
