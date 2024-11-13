namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityIdentityDatabaseCommandProvider(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : IdentityDatabaseCommandProviderBase<TestEntityIdentity>(settingsProviders)
{
    protected override IEnumerable<IdentityDatabaseCommandProviderField> GetFields()
    {
        yield return new IdentityDatabaseCommandProviderField("Code");
        yield return new IdentityDatabaseCommandProviderField("CodeType");
    }
}
