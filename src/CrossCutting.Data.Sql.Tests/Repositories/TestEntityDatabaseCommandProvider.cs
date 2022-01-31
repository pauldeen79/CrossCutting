using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.CommandProviders;
using CrossCutting.Data.Core.Commands;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityDatabaseCommandProvider : SelectDatabaseCommandProvider, IDatabaseCommandProvider<TestEntity>
    {
        public TestEntityDatabaseCommandProvider() : base(new TestEntityDatabaseEntityRetrieverSettings())
        {
        }

        public IDatabaseCommand Create(TestEntity source, DatabaseOperation operation)
        {
            return operation switch
            {
                DatabaseOperation.Insert => new SqlTextCommand("INSERT INTO...", DatabaseOperation.Insert),
                DatabaseOperation.Update => new SqlTextCommand("UPDATE...", DatabaseOperation.Update),
                DatabaseOperation.Delete => new SqlTextCommand("DELETE...", DatabaseOperation.Delete),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}"),
            };
        }
    }
}
