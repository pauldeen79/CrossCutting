using System;
using System.Collections.Generic;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;

namespace CrossCutting.Data.Core.Builders
{
    public class DeleteCommandBuilder
    {
        public DatabaseCommandType CommandType { get; set; }
        public IDictionary<string, object> CommandParameters { get; set; }
        private string _table;
        private readonly StringBuilder _whereBuilder;

        public DeleteCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            _table = string.Empty;
            _whereBuilder = new StringBuilder();
        }

        public DeleteCommandBuilder From(string table)
        {
            _table = table;
            return this;
        }

        public DeleteCommandBuilder Where(string value)
        {
            if (_whereBuilder.Length > 0)
            {
                _whereBuilder.Append(" AND ");
            }
            _whereBuilder.Append(value);
            return this;
        }

        public DeleteCommandBuilder And(string value)
            => Where(value);

        public DeleteCommandBuilder Or(string value)
        {
            if (_whereBuilder.Length == 0)
            {
                throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
            }
            _whereBuilder.Append(" OR ").Append(value);
            return this;
        }

        public DeleteCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public DeleteCommandBuilder AppendParameters(object parameters)
        {
            foreach (var param in parameters.ToExpandoObject())
            {
                CommandParameters.Add(param.Key, param.Value);
            }
            return this;
        }

        public DeleteCommandBuilder Clear()
        {
            CommandParameters.Clear();
            _table = string.Empty;
            _whereBuilder.Clear();
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Delete, CommandParameters);

        private string BuildSql()
        {
            if (string.IsNullOrWhiteSpace(_table))
            {
                throw new InvalidOperationException("table name is missing");
            }

            var builder = new StringBuilder()
                .Append("DELETE FROM ")
                .Append(_table);

            if (_whereBuilder.Length > 0)
            {
                builder.Append(" WHERE ").Append(_whereBuilder);
            }

            return builder.ToString();
        }
    }
}
