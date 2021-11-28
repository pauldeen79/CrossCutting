using System;
using System.Collections.Generic;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;

namespace CrossCutting.Data.Core.Builders
{
    public class InsertCommandBuilder
    {
        public IDictionary<string, object> CommandParameters { get; set; }
        private string _table;
        private string _temporaryTable;
        private readonly List<string> _fieldNames;
        private readonly List<string> _outputFields;
        private readonly List<string> _fieldValues;

        public InsertCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            _table = string.Empty;
            _temporaryTable = string.Empty;
            _fieldNames = new List<string>();
            _outputFields = new List<string>();
            _fieldValues = new List<string>();
        }

        public InsertCommandBuilder Into(string table)
        {
            _table = table;
            return this;
        }

        public InsertCommandBuilder TemporaryTable(string temporaryTable)
        {
            _temporaryTable = temporaryTable;
            return this;
        }

        public InsertCommandBuilder WithFieldName(string fieldName)
        {
            _fieldNames.Add(fieldName);
            return this;
        }

        public InsertCommandBuilder WithFieldNames(IEnumerable<string> fieldNames)
        {
            _fieldNames.AddRange(fieldNames);
            return this;
        }

        public InsertCommandBuilder WithFieldNames(params string[] fieldNames)
        {
            _fieldNames.AddRange(fieldNames);
            return this;
        }

        public InsertCommandBuilder WithFieldValue(string fieldValue)
        {
            _fieldValues.Add(fieldValue);
            return this;
        }

        public InsertCommandBuilder WithFieldValues(IEnumerable<string> fieldValues)
        {
            _fieldValues.AddRange(fieldValues);
            return this;
        }

        public InsertCommandBuilder WithFieldValues(params string[] fieldValues)
        {
            _fieldValues.AddRange(fieldValues);
            return this;
        }

        public InsertCommandBuilder WithOutputField(string outputField)
        {
            _outputFields.Add(outputField);
            return this;
        }

        public InsertCommandBuilder WithOutputFields(IEnumerable<string> outputFields)
        {
            _outputFields.AddRange(outputFields);
            return this;
        }

        public InsertCommandBuilder WithOutputFields(params string[] outputFields)
        {
            _outputFields.AddRange(outputFields);
            return this;
        }

        public InsertCommandBuilder AppendParameter(string key, object value)
        {
            CommandParameters.Add(key, value);
            return this;
        }

        public InsertCommandBuilder AppendParameters(object parameters)
        {
            foreach (var param in parameters.ToExpandoObject())
            {
                CommandParameters.Add(param.Key, param.Value);
            }
            return this;
        }

        public InsertCommandBuilder Clear()
        {
            CommandParameters.Clear();
            _table = string.Empty;
            _temporaryTable = string.Empty;
            _fieldNames.Clear();
            _outputFields.Clear();
            _fieldValues.Clear();
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

            if (_fieldValues.Count == 0)
            {
                throw new InvalidOperationException("field values are missing");
            }

            if (_fieldNames.Count != _fieldValues.Count)
            {
                throw new InvalidOperationException("field name count should be equal to field value count");
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

            return builder.Append(" VALUES(")
                          .Append(string.Join(", ", _fieldValues))
                          .Append(")")
                          .ToString();
        }
/*
DECLARE @NewValues TABLE ([Code] varchar(3), [CodeType] varchar(3), [CreatedBy] varchar(128), [CreatedOn] datetime, [UpdatedBy] varchar(128), [UpdatedOn] datetime, [DeletedBy] varchar(128), [DeletedOn] datetime)
INSERT INTO [Code] ([Code], [CodeType], [Description], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn]) OUTPUT INSERTED.[Code], INSERTED.[CodeType], INSERTED.[CreatedBy], INSERTED.[CreatedOn], INSERTED.[UpdatedBy], INSERTED.[UpdatedOn], INSERTED.[DeletedBy], INSERTED.[DeletedOn] INTO @NewValues VALUES (@Code, @CodeType, @Description, COALESCE(@CreatedBy, SUSER_SNAME()), COALESCE(@CreatedOn, GETDATE()), @UpdatedBy, @UpdatedOn, @DeletedBy, @DeletedOn)
INSERT INTO [Code_Hist] ([Code], [CodeType], [Description], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn]) OUTPUT INSERTED.[Code], INSERTED.[CodeType], INSERTED.[CreatedBy], INSERTED.[CreatedOn], INSERTED.[UpdatedBy], INSERTED.[UpdatedOn], INSERTED.[DeletedBy], INSERTED.[DeletedOn] SELECT [Code], [CodeType], @Description, COALESCE(@CreatedBy, SUSER_SNAME()), COALESCE(@CreatedOn, GETDATE()), [UpdatedBy], [UpdatedOn], [DeletedBy], [DeletedOn] FROM @NewValues
*/
    }
}
