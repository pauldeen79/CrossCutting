using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.CommandProviders;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityDatabaseCommandProvider : SelectDatabaseCommandProviderBase<TestEntityDatabaseEntityRetrieverSettings>, IDatabaseCommandProvider<TestEntity>
    {
        public TestEntityDatabaseCommandProvider() : base(new TestEntityDatabaseEntityRetrieverSettings())
        {
        }

        public IDatabaseCommand Create(TestEntity source, DatabaseOperation operation)
        {
            throw new NotImplementedException();
        }
    }
}
