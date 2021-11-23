using System;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Data.Core.Commands;
using CrossCutting.Data.Sql.Builders;

namespace CrossCutting.Data.Sql.CommandProviders
{
    public abstract class PagedSelectDatabaseCommandProviderBase<TSettings> : IPagedDatabaseCommandProvider
        where TSettings : IDatabaseEntityRetrieverSettings
    {
        protected TSettings Settings { get; }

        protected PagedSelectDatabaseCommandProviderBase(TSettings settings)
        {
            Settings = settings;
        }
        
        public IDatabaseCommand Create(DatabaseOperation operation)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
            }

            return new SelectCommandBuilder().Select(Settings.Fields).From(Settings.TableName).Where(Settings.DefaultWhere).OrderBy(Settings.DefaultOrderBy).Build();
        }

        public IPagedDatabaseCommand CreatePaged(DatabaseOperation operation, int offset, int pageSize)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
            }

            var dataCommand = CreatePagedCommand(offset, pageSize, false);
            var recordCountCommand = CreatePagedCommand(offset, pageSize, true);
            return new PagedDatabaseCommand(dataCommand, recordCountCommand, offset, pageSize);
        }

        private IDatabaseCommand CreatePagedCommand(int? offset, int? pageSize, bool countOnly)
        {
            //if (countOnly)
            //{
            //    return new PagedSelectCommandBuilder()
            //        .Select("COUNT(*)")
            //        .From(Settings.TableName)
            //        .Where(Settings.DefaultWhere)
            //        .Build(countOnly);
            //}
            
            return new PagedSelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where(Settings.DefaultWhere)
                .OrderBy(Settings.DefaultOrderBy)
                .Offset(offset)
                .PageSize(pageSize)
                .Build(countOnly);
        }
    }
}
