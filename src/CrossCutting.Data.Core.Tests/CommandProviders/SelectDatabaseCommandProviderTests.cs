namespace CrossCutting.Data.Core.Tests.CommandProviders;

public class SelectDatabaseCommandProviderTests : TestBase<SelectDatabaseCommandProvider>
{
    private Mock<IDatabaseEntityRetrieverSettings> SettingsMock => Fixture.Freeze<Mock<IDatabaseEntityRetrieverSettings>>();

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void Create_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Act & Assert
        Sut.Invoking(x => x.Create<TestEntity>(operation))
           .Should().ThrowExactly<ArgumentOutOfRangeException>().And.ParamName.Should().Be("operation");
    }

    [Fact]
    public void Create_Throws_On_Unsupported_DatabaseEntityRetrieverSettings()
    {
        // Act & Assert
        Sut.Invoking(x => x.Create<TestEntity>(DatabaseOperation.Select))
           .Should().ThrowExactly<InvalidOperationException>()
           .WithMessage("Could not obtain database entity retriever settings for type [CrossCutting.Data.Core.Tests.TestFixtures.TestEntity]");
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
        var settings = SettingsMock.Object;
        Fixture.Freeze<Mock<IDatabaseEntityRetrieverSettingsProvider>>()
               .Setup(x => x.TryGet<TestEntity>(out settings))
               .Returns(true);

        // Act
        var actual = Sut.Create<TestEntity>(DatabaseOperation.Select);

        // Assert
        actual.CommandText.Should().Be(Sql);
    }
}
