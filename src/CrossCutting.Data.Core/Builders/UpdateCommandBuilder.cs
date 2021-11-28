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
        public string Table { get; set; }
        public List<string> FieldNames { get; set; }
        public List<string> FieldValues { get; set; }
        private readonly StringBuilder _whereBuilder;

        public UpdateCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            Table = string.Empty;
            FieldNames = new List<string>();
            FieldValues = new List<string>();
            _whereBuilder = new StringBuilder();
        }

        public UpdateCommandBuilder WithTable(string table)
        {
            Table = table;
            return this;
        }

        public UpdateCommandBuilder WithFieldName(string fieldName)
        {
            FieldNames.Add(fieldName);
            return this;
        }

        public UpdateCommandBuilder WithFieldNames(IEnumerable<string> fieldNames)
        {
            FieldNames.AddRange(fieldNames);
            return this;
        }

        public UpdateCommandBuilder WithFieldNames(params string[] fieldNames)
        {
            FieldNames.AddRange(fieldNames);
            return this;
        }

        public UpdateCommandBuilder WithFieldValue(string fieldValue)
        {
            FieldValues.Add(fieldValue);
            return this;
        }

        public UpdateCommandBuilder WithFieldValues(IEnumerable<string> fieldValues)
        {
            FieldValues.AddRange(fieldValues);
            return this;
        }

        public UpdateCommandBuilder WithFieldValues(params string[] fieldValues)
        {
            FieldValues.AddRange(fieldValues);
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
            Table = string.Empty;
            FieldNames.Clear();
            FieldValues.Clear();
            _whereBuilder.Clear();
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Update, CommandParameters);

        private string BuildSql()
        {
            if (string.IsNullOrWhiteSpace(Table))
            {
                throw new InvalidOperationException("table name is missing");
            }

            if (FieldNames.Count == 0)
            {
                throw new InvalidOperationException("field names are missing");
            }

            if (FieldValues.Count == 0)
            {
                throw new InvalidOperationException("field values are missing");
            }

            if (FieldNames.Count != FieldValues.Count)
            {
                throw new InvalidOperationException("field name count should be equal to field value count");
            }

            var builder = new StringBuilder()
                .Append("UPDATE ")
                .Append(Table)
                .Append(" SET ")
                .Append(string.Join(", ", GenerateValues()));

            if (_whereBuilder.Length > 0)
            {
                builder.Append(" WHERE ").Append(_whereBuilder);
            }

            return builder.ToString();
        }

        private IEnumerable<string> GenerateValues()
            => FieldNames.Zip(FieldValues, (name, value) => $"{name} = {value}");
    }
}
