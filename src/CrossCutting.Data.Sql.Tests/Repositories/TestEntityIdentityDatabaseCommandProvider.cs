namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityIdentityDatabaseCommandProvider : IdentityDatabaseCommandProviderBase<TestEntityIdentity>
{
    public TestEntityIdentityDatabaseCommandProvider(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders)
        : base(settingsProviders)
    {
    }

    protected override IEnumerable<IdentityDatabaseCommandProviderField> GetFields()
    {
        yield return new IdentityDatabaseCommandProviderField("Code");
        yield return new IdentityDatabaseCommandProviderField("CodeType");
    }
}
