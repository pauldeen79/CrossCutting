using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Core.CommandProviders;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntitySelectDatabaseCommandProvider : SelectDatabaseCommandProviderBase<TestEntityDatabaseEntityRetrieverSettings>
    {
        public TestEntitySelectDatabaseCommandProvider() : base(new TestEntityDatabaseEntityRetrieverSettings())
        {
        }
    }
}
