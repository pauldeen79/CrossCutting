namespace CrossCutting.Data.Core.Tests.CommandProviders;

public class SelectDatabaseCommandProviderTests : TestBase<SelectDatabaseCommandProvider>
{
    private IDatabaseEntityRetrieverSettings SettingsMock => Fixture.Freeze<IDatabaseEntityRetrieverSettings>();

    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public async Task CreateAsync_Returns_Invalid_On_Unsupported_DatabaseOperation(DatabaseOperation operation)
    {
        // Act
        var result = await Sut.CreateAsync<TestEntity>(operation, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public async Task CreateAsync_Returns_Error_On_Unsupported_DatabaseEntityRetrieverSettings()
    {
        // Arrange
        var sut = new SelectDatabaseCommandProvider([]);

        // Act
        var result = await sut.CreateAsync<TestEntity>(DatabaseOperation.Select, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Could not obtain database entity retriever settings for type [CrossCutting.Data.Core.Tests.TestFixtures.TestEntity]");
    }

    [Fact]
    public async Task CreateAsync_Returns_Correct_Command_On_Select_DatabaseOperation()
    {
        // Arrange
        const string Sql = "SELECT Id, Active, Field1, Field2, Field3 FROM MyTable WHERE Active = 1 ORDER BY Field1";
        SettingsMock.DefaultOrderBy.Returns("Field1");
        SettingsMock.DefaultWhere.Returns("Active = 1");
        SettingsMock.Fields.Returns("Id, Active, Field1, Field2, Field3");
        SettingsMock.TableName.Returns("MyTable");
        Fixture.Freeze<IDatabaseEntityRetrieverSettingsProvider>()
               .Get<TestEntity>()
               .Returns(_ => Result.Success(SettingsMock));

        // Act
        var actual = (await Sut.CreateAsync<TestEntity>(DatabaseOperation.Select, CancellationToken.None)).EnsureValue().GetValueOrThrow();

        // Assert
        actual.CommandText.ShouldBe(Sql);
    }
}
