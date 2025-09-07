namespace CrossCutting.Data.Core.Tests.CommandProviders;

public class SelectDatabaseCommandProviderTests : TestBase<SelectDatabaseCommandProvider>
{
    private IDatabaseEntityRetrieverSettings SettingsMock => Fixture.Freeze<IDatabaseEntityRetrieverSettings>();

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void Create_Returns_Invalid_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Act
        var result = Sut.Create<TestEntity>(operation);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void Create_Returns_Error_On_Unsupported_DatabaseEntityRetrieverSettings()
    {
        // Act
        var result = Sut.Create<TestEntity>(DatabaseOperation.Select);

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Could not obtain database entity retriever settings for type [CrossCutting.Data.Core.Tests.TestFixtures.TestEntity]");
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
        var actual = Sut.Create<TestEntity>(DatabaseOperation.Select).EnsureValue().GetValueOrThrow();

        // Assert
        actual.CommandText.ShouldBe(Sql);
    }
}
