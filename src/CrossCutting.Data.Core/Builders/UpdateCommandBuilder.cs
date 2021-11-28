using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;

namespace CrossCutting.Data.Core.Builders
{
    public class UpdateCommandBuilder
    {
        public IDictionary<string, object> CommandParameters { get; set; }
        private string _table;
        private readonly List<string> _fieldNames;
        private readonly List<string> _fieldValues;
        private readonly StringBuilder _whereBuilder;

        public UpdateCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            _table = string.Empty;
            _fieldNames = new List<string>();
            _fieldValues = new List<string>();
            _whereBuilder = new StringBuilder();
        }

        public UpdateCommandBuilder Table(string table)
        {
            _table = table;
            return this;
        }

        public UpdateCommandBuilder WithFieldName(string fieldName)
        {
            _fieldNames.Add(fieldName);
            return this;
        }

        public UpdateCommandBuilder WithFieldNames(IEnumerable<string> fieldNames)
        {
            _fieldNames.AddRange(fieldNames);
            return this;
        }

        public UpdateCommandBuilder WithFieldNames(params string[] fieldNames)
        {
            _fieldNames.AddRange(fieldNames);
            return this;
        }

        public UpdateCommandBuilder WithFieldValue(string fieldValue)
        {
            _fieldValues.Add(fieldValue);
            return this;
        }

        public UpdateCommandBuilder WithFieldValues(IEnumerable<string> fieldValues)
        {
            _fieldValues.AddRange(fieldValues);
            return this;
        }

        public UpdateCommandBuilder WithFieldValues(params string[] fieldValues)
        {
            _fieldValues.AddRange(fieldValues);
            return this;
        }

        public UpdateCommandBuilder Where(string value)
        {
            if (_whereBuilder.Length > 0)
            {
                _whereBuilder.Append(" AND ");
            }
            _whereBuilder.Append(value);
            return this;
        }

        public UpdateCommandBuilder And(string value)
            => Where(value);

        public UpdateCommandBuilder Or(string value)
        {
            if (_whereBuilder.Length == 0)
            {
                throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
            }
            _whereBuilder.Append(" OR ").Append(value);
            return this;
        }

        public UpdateCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public UpdateCommandBuilder AppendParameters(object parameters)
        {
            foreach (var param in parameters.ToExpandoObject())
            {
                CommandParameters.Add(param.Key, param.Value);
            }
            return this;
        }

        public UpdateCommandBuilder Clear()
        {
            CommandParameters.Clear();
            _table = string.Empty;
            _fieldNames.Clear();
            _fieldValues.Clear();
            _whereBuilder.Clear();
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Update, CommandParameters);

        private string BuildSql()
        {
            if (string.IsNullOrWhiteSpace(_table))
            {
                throw new InvalidOperationException("table name is missing");
            }

            if (_fieldNames.Count == 0)
            {
                throw new InvalidOperationException("field names are missing");
            }

            if (_fieldValues.Count == 0)
            {
                throw new InvalidOperationException("field values are missing");
            }

            if (_fieldNames.Count != _fieldValues.Count)
            {
                throw new InvalidOperationException("field name count should be equal to field value count");
            }

            var builder = new StringBuilder()
                .Append("UPDATE ")
                .Append(_table)
                .Append(" SET ")
                .Append(string.Join(", ", GenerateValues()));

            if (_whereBuilder.Length > 0)
            {
                builder.Append(" WHERE ").Append(_whereBuilder);
            }

            return builder.ToString();
        }

        private IEnumerable<string> GenerateValues()
            => _fieldNames.Zip(_fieldValues, (name, value) => $"{name} = {value}");
    }
}
