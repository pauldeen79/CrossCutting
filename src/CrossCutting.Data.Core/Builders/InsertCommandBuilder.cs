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
        public string Table { get; set; }
        public string TemporaryTable { get; set; }
        public List<string> FieldNames { get; set; }
        public List<string> OutputFields { get; set; }
        public List<string> FieldValues { get; set; }

        public InsertCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            Table = string.Empty;
            TemporaryTable = string.Empty;
            FieldNames = new List<string>();
            OutputFields = new List<string>();
            FieldValues = new List<string>();
        }

        public InsertCommandBuilder Into(string table)
        {
            Table = table;
            return this;
        }

        public InsertCommandBuilder WithTemporaryTable(string temporaryTable)
        {
            TemporaryTable = temporaryTable;
            return this;
        }

        public InsertCommandBuilder WithFieldName(string fieldName)
        {
            FieldNames.Add(fieldName);
            return this;
        }

        public InsertCommandBuilder WithFieldNames(IEnumerable<string> fieldNames)
        {
            FieldNames.AddRange(fieldNames);
            return this;
        }

        public InsertCommandBuilder WithFieldNames(params string[] fieldNames)
        {
            FieldNames.AddRange(fieldNames);
            return this;
        }

        public InsertCommandBuilder WithFieldValue(string fieldValue)
        {
            FieldValues.Add(fieldValue);
            return this;
        }

        public InsertCommandBuilder WithFieldValues(IEnumerable<string> fieldValues)
        {
            FieldValues.AddRange(fieldValues);
            return this;
        }

        public InsertCommandBuilder WithFieldValues(params string[] fieldValues)
        {
            FieldValues.AddRange(fieldValues);
            return this;
        }

        public InsertCommandBuilder WithOutputField(string outputField)
        {
            OutputFields.Add(outputField);
            return this;
        }

        public InsertCommandBuilder WithOutputFields(IEnumerable<string> outputFields)
        {
            OutputFields.AddRange(outputFields);
            return this;
        }

        public InsertCommandBuilder WithOutputFields(params string[] outputFields)
        {
            OutputFields.AddRange(outputFields);
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
            Table = string.Empty;
            TemporaryTable = string.Empty;
            FieldNames.Clear();
            OutputFields.Clear();
            FieldValues.Clear();
            return this;
        }

        public IDatabaseCommand Build()
            => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Insert, CommandParameters);

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

            var builder = new StringBuilder();

            builder.Append("INSERT INTO ")
                   .Append(Table)
                   .Append("(")
                   .Append(string.Join(", ", FieldNames))
                   .Append(")");

            if (OutputFields.Count > 0)
            {
                builder.Append(" OUTPUT ")
                       .Append(string.Join(", ", OutputFields));
            }

            if (!string.IsNullOrEmpty(TemporaryTable))
            {
                builder.Append(" INTO ")
                       .Append(TemporaryTable);
            }

            return builder.Append(" VALUES(")
                          .Append(string.Join(", ", FieldValues))
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
