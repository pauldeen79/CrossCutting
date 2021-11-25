using System;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;

namespace CrossCutting.Data.Core.CommandProviders
{
    public abstract class SelectDatabaseCommandProviderBase<TSettings> : IDatabaseCommandProvider
        where TSettings : IDatabaseEntityRetrieverSettings
    {
        protected TSettings Settings { get; }

        protected SelectDatabaseCommandProviderBase(TSettings settings)
        {
            Settings = settings;
        }

        public IDatabaseCommand Create(DatabaseOperation operation)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only Select operation is supported");
            }

            return new SelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where(Settings.DefaultWhere)
                .OrderBy(Settings.DefaultOrderBy)
                .Build();
        }
    }
}
