namespace CrossCutting.Data.Core.Tests.CommandProviders;

public class IdentityDatabaseCommandProviderBaseTests
{
    [Theory]
    [InlineData(DatabaseOperation.Delete)]
    [InlineData(DatabaseOperation.Insert)]
    [InlineData(DatabaseOperation.Unspecified)]
    [InlineData(DatabaseOperation.Update)]
    public void Create_Returns_Invalid_On_Wrong_DatabaseOperation(DatabaseOperation operation)
    {
        // Arrange
        var sut = new IdentityDatabaseCommandProviderMock([new PagedDatabaseEntityRetrieverSettingsProviderMock()]);

        // Act
        var result = sut.Create(new TestEntityIdentity("A", "B"), operation);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void Create_Returns_Error_On_Unsupported_PagedDatabaseEntityRetrieverSettings()
    {
        // Arrange
        var sut = new IdentityDatabaseCommandProviderMock([]);

        // Act
        var result = sut.Create(new TestEntityIdentity("NOTIMPLEMENTED", "NOTIMPLEMENTED"), DatabaseOperation.Select);

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Could not obtain paged database entity retriever settings for type [CrossCutting.Data.Core.Tests.TestFixtures.TestEntityIdentity]");
    }

    [Fact]
    public void Create_Generates_Where_Statement_For_Both_Simple_Fields_And_Fields_With_Different_Name_In_Database()
    {
        // Arrange
        var sut = new IdentityDatabaseCommandProviderMock([new PagedDatabaseEntityRetrieverSettingsProviderMock()]);

        // Act
        var actual = sut.Create(new TestEntityIdentity("A", "B"), DatabaseOperation.Select).EnsureValue().GetValueOrThrow();

        // Assert
        actual.CommandText.ShouldBe(@"SELECT A, B, C FROM Table WHERE [Field1] = @Field1 AND [Field2] = @Field2Alias");
    }

    private sealed class IdentityDatabaseCommandProviderMock(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IdentityDatabaseCommandProviderBase<TestEntityIdentity>(settingsProviders)
    {
        protected override IEnumerable<IdentityDatabaseCommandProviderField> GetFields()
        {
            yield return new IdentityDatabaseCommandProviderField("Field1");
            yield return new IdentityDatabaseCommandProviderField("Field2Alias", "Field2");
        }
    }

    private sealed class PagedDatabaseEntityRetrieverSettingsProviderMock : IPagedDatabaseEntityRetrieverSettingsProvider
    {
        public Result<IPagedDatabaseEntityRetrieverSettings> Get<TSource>()
        {
            if (typeof(TSource) == typeof(TestEntityIdentity))
            {
                return Result.Success<IPagedDatabaseEntityRetrieverSettings>(new PagedDatabaseEntityRetrieverSettingsMock());
            }

            return Result.Continue<IPagedDatabaseEntityRetrieverSettings>();
        }
    }

    private sealed class PagedDatabaseEntityRetrieverSettingsMock : IPagedDatabaseEntityRetrieverSettings
    {
        public int? OverridePageSize => null;
        public string TableName => "Table";
        public string Fields => "A, B, C";
        public string DefaultOrderBy => string.Empty;
        public string DefaultWhere => string.Empty;
    }
}
