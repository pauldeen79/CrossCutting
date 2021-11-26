using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Sql.CommandProviders;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityPagedSelectDatabaseCommandProvider : PagedSelectDatabaseCommandProviderBase<TestEntityDatabaseEntityRetrieverSettings>
    {
        public TestEntityPagedSelectDatabaseCommandProvider() : base(new TestEntityDatabaseEntityRetrieverSettings())
        {
        }
    }
}
