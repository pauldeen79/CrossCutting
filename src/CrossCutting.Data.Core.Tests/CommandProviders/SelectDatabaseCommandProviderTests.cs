namespace CrossCutting.Data.Core.Tests.CommandProviders;

public class SelectDatabaseCommandProviderTests : TestBase<SelectDatabaseCommandProvider>
{
    private IDatabaseEntityRetrieverSettings SettingsMock => Fixture.Freeze<IDatabaseEntityRetrieverSettings>();

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void Create_Throws_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Act & Assert
        Sut.Invoking(x => x.Create<TestEntity>(operation))
           .Should().ThrowExactly<ArgumentOutOfRangeException>()
           .And.ParamName.Should().Be("operation");
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
        SettingsMock.DefaultOrderBy.Returns("Field1");
        SettingsMock.DefaultWhere.Returns("Active = 1");
        SettingsMock.Fields.Returns("Id, Active, Field1, Field2, Field3");
        SettingsMock.TableName.Returns("MyTable");
        Fixture.Freeze<IDatabaseEntityRetrieverSettingsProvider>()
               .TryGet<TestEntity>(out Arg.Any<IDatabaseEntityRetrieverSettings?>())
               .Returns(x => { x[0] = SettingsMock; return true; });

        // Act
        var actual = Sut.Create<TestEntity>(DatabaseOperation.Select);

        // Assert
        actual.CommandText.Should().Be(Sql);
    }
}
