using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using CrossCutting.Data.Core.Extensions;

namespace CrossCutting.Data.Core.Builders
{
    public class UpdateCommandBuilder
    {
        public IDictionary<string, object> CommandParameters { get; set; }
        public string Table { get; set; }
        public string TemporaryTable { get; set; }
        public List<string> FieldNames { get; set; }
        public List<string> FieldValues { get; set; }
        public List<string> OutputFields { get; set; }
        private readonly StringBuilder _whereBuilder;

        public UpdateCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            FieldNames = new List<string>();
            FieldValues = new List<string>();
            OutputFields = new List<string>();
            Table = string.Empty;
            TemporaryTable = string.Empty;
            _whereBuilder = new StringBuilder();
        }

        public UpdateCommandBuilder WithTable(string table)
            => this.Chain(() => Table = table);

        public UpdateCommandBuilder WithTemporaryTable(string temporaryTable)
            => this.Chain(() => TemporaryTable = temporaryTable);

        public UpdateCommandBuilder AddFieldNames(IEnumerable<string> fieldNames)
            => this.Chain(() => FieldNames.AddRange(fieldNames));

        public UpdateCommandBuilder AddFieldNames(params string[] fieldNames)
            => this.Chain(() => FieldNames.AddRange(fieldNames));

        public UpdateCommandBuilder AddFieldValues(IEnumerable<string> fieldValues)
            => this.Chain(() => FieldValues.AddRange(fieldValues));

        public UpdateCommandBuilder AddFieldValues(params string[] fieldValues)
            => this.Chain(() => FieldValues.AddRange(fieldValues));

        public UpdateCommandBuilder AddOutputFields(IEnumerable<string> outputFields)
            => this.Chain(() => OutputFields.AddRange(outputFields));

        public UpdateCommandBuilder AddOutputFields(params string[] outputFields)
            => this.Chain(() => OutputFields.AddRange(outputFields));

        public UpdateCommandBuilder Where(string value)
            => this.Chain(() =>
            {
                if (_whereBuilder.Length > 0)
                {
                    _whereBuilder.Append(" AND ");
                }
                _whereBuilder.Append(value);
            });

        public UpdateCommandBuilder And(string value)
            => Where(value);

        public UpdateCommandBuilder Or(string value)
            => this.Chain(() =>
            {
                if (_whereBuilder.Length == 0)
                {
                    throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
                }
                _whereBuilder.Append(" OR ").Append(value);
            });

        public UpdateCommandBuilder AppendParameter(string key, object value)
            => this.Chain(() => CommandParameters.Add(key, value));

        public UpdateCommandBuilder AppendParameters(object parameters)
            => this.Chain(() => CommandParameters.AddRange(parameters.ToExpandoObject()));

        public UpdateCommandBuilder Clear()
        {
            CommandParameters.Clear();
            FieldNames.Clear();
            FieldValues.Clear();
            OutputFields.Clear();
            Table = string.Empty;
            TemporaryTable = string.Empty;
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
                .Append(string.Join(", ", GenerateValues()))
                .AppendOutputFields(TemporaryTable, OutputFields);

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
