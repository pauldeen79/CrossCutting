namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityIdentityDatabaseCommandProvider : IdentityDatabaseCommandProviderBase<TestEntityIdentity>
{
    public TestEntityIdentityDatabaseCommandProvider(TestEntityDatabaseEntityRetrieverSettings settings) : base(settings)
    {
    }

    protected override IEnumerable<IdentityDatabaseCommandProviderField> GetFields()
    {
        yield return new IdentityDatabaseCommandProviderField("Code");
        yield return new IdentityDatabaseCommandProviderField("CodeType");
    }
}
