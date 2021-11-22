using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.CommandProviders;
using CrossCutting.Data.Core.Commands;

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
            switch (operation)
            {
                case DatabaseOperation.Insert:
                    return new SqlTextCommand("INSERT INTO...", DatabaseOperation.Insert);
                case DatabaseOperation.Update:
                    return new SqlTextCommand("UPDATE...", DatabaseOperation.Update);
                case DatabaseOperation.Delete:
                    return new SqlTextCommand("DELETE...", DatabaseOperation.Delete);
                default:
                    throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}");
            }
        }
    }
}
