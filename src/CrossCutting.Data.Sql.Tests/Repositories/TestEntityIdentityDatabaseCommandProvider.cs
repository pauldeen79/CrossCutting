using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Data.Core.CommandProviders;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityIdentityDatabaseCommandProvider : SelectDatabaseCommandProviderBase<TestEntityDatabaseEntityRetrieverSettings>, IDatabaseCommandProvider<TestEntityIdentity>
    {
        public TestEntityIdentityDatabaseCommandProvider() : base(new TestEntityDatabaseEntityRetrieverSettings())
        {
        }

        public IDatabaseCommand Create(TestEntityIdentity source, DatabaseOperation operation)
            => new SelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where("[Code] = @Code AND [CodeType] = @CodeType")
                .AppendParameters(source)
                .Build();
    }
}
