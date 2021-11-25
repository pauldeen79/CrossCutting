using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Sql.CommandProviders;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityGenericDatabaseCommandProvider : PagedSelectDatabaseCommandProviderBase<TestEntityDatabaseEntityRetrieverSettings>
    {
        public TestEntityGenericDatabaseCommandProvider() : base(new TestEntityDatabaseEntityRetrieverSettings())
        {
        }
    }
}
