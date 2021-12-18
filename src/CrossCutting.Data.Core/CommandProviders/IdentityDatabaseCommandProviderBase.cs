using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Builders;

namespace CrossCutting.Data.Core.CommandProviders
{
    public abstract class IdentityDatabaseCommandProviderBase<T> : IDatabaseCommandProvider<T>
        where T : class
    {
        public IPagedDatabaseEntityRetrieverSettings Settings { get; }

        protected IdentityDatabaseCommandProviderBase(IPagedDatabaseEntityRetrieverSettings settings)
        {
            Settings = settings;
        }

        public IDatabaseCommand Create(T source, DatabaseOperation operation)
        {
            if (operation != DatabaseOperation.Select)
            {
                throw new ArgumentOutOfRangeException(nameof(operation), "Only select is supported");
            }
            return new SelectCommandBuilder()
                .Select(Settings.Fields)
                .From(Settings.TableName)
                .Where(string.Join(" AND ", GetFields().Select(x => $"[{x.FieldName}] = @{x.ParameterName}")))
                .AppendParameters(source)
                .Build();
        }

        protected abstract IEnumerable<IdentityDatabaseCommandProviderField> GetFields();
    }
}
