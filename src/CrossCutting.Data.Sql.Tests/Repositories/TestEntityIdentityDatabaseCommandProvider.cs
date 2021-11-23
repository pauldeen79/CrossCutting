using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Data.Sql.Builders;
using CrossCutting.Data.Sql.CommandProviders;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityIdentityDatabaseCommandProvider : PagedSelectDatabaseCommandProviderBase<TestEntityDatabaseEntityRetrieverSettings>, IPagedDatabaseCommandProvider<TestEntityIdentity>
    {
        public TestEntityIdentityDatabaseCommandProvider() : base(new TestEntityDatabaseEntityRetrieverSettings())
        {
        }

        public IDatabaseCommand Create(TestEntityIdentity source, DatabaseOperation operation)
        {
            return new SelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where("[Code] = @Code AND [CodeType] = @CodeType")
                .AppendParameters(source)
                .Build();
        }

        public IPagedDatabaseCommand CreatePaged(TestEntityIdentity source, DatabaseOperation operation, int offset, int pageSize)
        {
            throw new System.NotImplementedException();
        }
    }
}
