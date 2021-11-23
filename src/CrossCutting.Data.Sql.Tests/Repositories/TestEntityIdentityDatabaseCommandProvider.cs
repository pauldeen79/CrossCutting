using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Data.Core.Commands;
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
            => new SelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where("[Code] = @Code AND [CodeType] = @CodeType")
                .AppendParameters(source)
                .Build();

        public IPagedDatabaseCommand CreatePaged(TestEntityIdentity source, DatabaseOperation operation, int offset, int pageSize)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
            }

            var dataCommand = CreatePagedCommand(source, offset, pageSize, false);
            var recordCountCommand = CreatePagedCommand(source, offset, pageSize, true);
            return new PagedDatabaseCommand(dataCommand, recordCountCommand, offset, pageSize);
        }

        private IDatabaseCommand CreatePagedCommand(TestEntityIdentity source, int? offset, int? pageSize, bool countOnly)
            => new PagedSelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where("[Code] = @Code AND [CodeType] = @CodeType")
                .AppendParameters(source)
                .OrderBy(Settings.DefaultOrderBy)
                .Offset(offset)
                .PageSize(pageSize)
                .Build(countOnly);
    }
}
