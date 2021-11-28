using System;
using System.Collections.Generic;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;

namespace CrossCutting.Data.Core.Builders
{
    public class InsertSelectCommandBuilder
    {
        public IDictionary<string, object> CommandParameters { get; set; }
        private string _table;
        private string _temporaryTable;
        private SelectCommandBuilder SelectCommand { get; set; }
        private readonly List<string> _fieldNames;
        private readonly List<string> _outputFields;

        public InsertSelectCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            _table = string.Empty;
            _temporaryTable = string.Empty;
            _fieldNames = new List<string>();
            _outputFields = new List<string>();
            SelectCommand = new SelectCommandBuilder();
        }

        public InsertSelectCommandBuilder Into(string table)
        {
            _table = table;
            return this;
        }

        public InsertSelectCommandBuilder TemporaryTable(string temporaryTable)
        {
            _temporaryTable = temporaryTable;
            return this;
        }

        public InsertSelectCommandBuilder WithFieldName(string fieldName)
        {
            _fieldNames.Add(fieldName);
            return this;
        }

        public InsertSelectCommandBuilder WithFieldNames(IEnumerable<string> fieldNames)
        {
            _fieldNames.AddRange(fieldNames);
            return this;
        }

        public InsertSelectCommandBuilder WithFieldNames(params string[] fieldNames)
        {
            _fieldNames.AddRange(fieldNames);
            return this;
        }

        public InsertSelectCommandBuilder WithSelectCommand(SelectCommandBuilder builder)
        {
            SelectCommand = builder;
            return this;
        }

        public InsertSelectCommandBuilder WithOutputField(string outputField)
        {
            _outputFields.Add(outputField);
            return this;
        }

        public InsertSelectCommandBuilder WithOutputFields(IEnumerable<string> outputFields)
        {
            _outputFields.AddRange(outputFields);
            return this;
        }

        public InsertSelectCommandBuilder WithOutputFields(params string[] outputFields)
        {
            _outputFields.AddRange(outputFields);
            return this;
        }

        public InsertSelectCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public InsertSelectCommandBuilder AppendParameters(object parameters)
        {
            foreach (var param in parameters.ToExpandoObject())
            {
                CommandParameters.Add(param.Key, param.Value);
            }
            return this;
        }

        public InsertSelectCommandBuilder Clear()
        {
            CommandParameters.Clear();
            _table = string.Empty;
            _temporaryTable = string.Empty;
            _fieldNames.Clear();
            _outputFields.Clear();
            SelectCommand.Clear();
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Insert, CommandParameters);

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

            var builder = new StringBuilder();

            builder.Append("INSERT INTO ")
                   .Append(_table)
                   .Append("(")
                   .Append(string.Join(", ", _fieldNames))
                   .Append(")");

            if (_outputFields.Count > 0)
            {
                builder.Append(" OUTPUT ")
                       .Append(string.Join(", ", _outputFields));
            }

            if (!string.IsNullOrEmpty(_temporaryTable))
            {
                builder.Append(" INTO ")
                       .Append(_temporaryTable);
            }

            return builder.Append(" ")
                          .Append(SelectCommand.Build().CommandText)
                          .ToString();
        }
/*
DECLARE @NewValues TABLE ([Code] varchar(3), [CodeType] varchar(3), [CreatedBy] varchar(128), [CreatedOn] datetime, [UpdatedBy] varchar(128), [UpdatedOn] datetime, [DeletedBy] varchar(128), [DeletedOn] datetime)
INSERT INTO [Code] ([Code], [CodeType], [Description], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn]) OUTPUT INSERTED.[Code], INSERTED.[CodeType], INSERTED.[CreatedBy], INSERTED.[CreatedOn], INSERTED.[UpdatedBy], INSERTED.[UpdatedOn], INSERTED.[DeletedBy], INSERTED.[DeletedOn] INTO @NewValues VALUES (@Code, @CodeType, @Description, COALESCE(@CreatedBy, SUSER_SNAME()), COALESCE(@CreatedOn, GETDATE()), @UpdatedBy, @UpdatedOn, @DeletedBy, @DeletedOn)
INSERT INTO [Code_Hist] ([Code], [CodeType], [Description], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn]) OUTPUT INSERTED.[Code], INSERTED.[CodeType], INSERTED.[CreatedBy], INSERTED.[CreatedOn], INSERTED.[UpdatedBy], INSERTED.[UpdatedOn], INSERTED.[DeletedBy], INSERTED.[DeletedOn] SELECT [Code], [CodeType], @Description, COALESCE(@CreatedBy, SUSER_SNAME()), COALESCE(@CreatedOn, GETDATE()), [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn] FROM @NewValues
*/
    }
}
