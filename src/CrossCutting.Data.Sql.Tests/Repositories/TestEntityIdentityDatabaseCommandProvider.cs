using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityIdentityDatabaseCommandProvider : IDatabaseCommandProvider<TestEntityIdentity>
    {
        public TestEntityDatabaseEntityRetrieverSettings Settings { get; }

        public TestEntityIdentityDatabaseCommandProvider(TestEntityDatabaseEntityRetrieverSettings settings)
        {
            Settings = settings;
        }

        public IDatabaseCommand Create(TestEntityIdentity source, DatabaseOperation operation)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only select is supported");
            }
            return new SelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where("[Code] = @Code AND [CodeType] = @CodeType")
                .AppendParameters(source)
                .Build();
        }
    }
}
