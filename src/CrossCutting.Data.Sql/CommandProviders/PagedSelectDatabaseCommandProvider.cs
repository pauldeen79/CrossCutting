using System;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using CrossCutting.Data.Sql.Builders;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql.CommandProviders
{
    public class PagedSelectDatabaseCommandProvider : IPagedDatabaseCommandProvider
    {
        private IPagedDatabaseEntityRetrieverSettings Settings { get; }

        public PagedSelectDatabaseCommandProvider(IPagedDatabaseEntityRetrieverSettings settings)
        {
            Settings = settings;
        }
        
        public IPagedDatabaseCommand CreatePaged(DatabaseOperation operation, int offset, int pageSize)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
            }

            var dataCommand = CreatePagedCommand(offset, pageSize, false);
            var recordCountCommand = CreatePagedCommand(offset, pageSize, true);
            return new PagedDatabaseCommand(dataCommand, recordCountCommand, offset, ((int?)pageSize).IfNotGreaterThan(Settings.OverridePageSize));
        }

        private IDatabaseCommand CreatePagedCommand(int? offset, int? pageSize, bool countOnly)
            => new PagedSelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where(Settings.DefaultWhere)
                .OrderBy(Settings.DefaultOrderBy)
                .Offset(offset)
                .PageSize(pageSize.IfNotGreaterThan(Settings.OverridePageSize))
                .Build(countOnly);
    }
}
