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
        public string Table { get; set; }
        public string TemporaryTable { get; set; }
        public SelectCommandBuilder SelectCommand { get; set; }
        public List<string> FieldNames { get; set; }
        public List<string> OutputFields { get; set; }

        public InsertSelectCommandBuilder()
        {
            CommandParameters = new Dictionary<string, object>();
            Table = string.Empty;
            TemporaryTable = string.Empty;
            FieldNames = new List<string>();
            OutputFields = new List<string>();
            SelectCommand = new SelectCommandBuilder();
        }

        public InsertSelectCommandBuilder Into(string table)
        {
            Table = table;
            return this;
        }

        public InsertSelectCommandBuilder WithTemporaryTable(string temporaryTable)
        {
            TemporaryTable = temporaryTable;
            return this;
        }

        public InsertSelectCommandBuilder WithFieldName(string fieldName)
        {
            FieldNames.Add(fieldName);
            return this;
        }

        public InsertSelectCommandBuilder WithFieldNames(IEnumerable<string> fieldNames)
        {
            FieldNames.AddRange(fieldNames);
            return this;
        }

        public InsertSelectCommandBuilder WithFieldNames(params string[] fieldNames)
        {
            FieldNames.AddRange(fieldNames);
            return this;
        }

        public InsertSelectCommandBuilder WithSelectCommand(SelectCommandBuilder builder)
        {
            SelectCommand = builder;
            return this;
        }

        public InsertSelectCommandBuilder WithOutputField(string outputField)
        {
            OutputFields.Add(outputField);
            return this;
        }

        public InsertSelectCommandBuilder WithOutputFields(IEnumerable<string> outputFields)
        {
            OutputFields.AddRange(outputFields);
            return this;
        }

        public InsertSelectCommandBuilder WithOutputFields(params string[] outputFields)
        {
            OutputFields.AddRange(outputFields);
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
            Table = string.Empty;
            TemporaryTable = string.Empty;
            FieldNames.Clear();
            OutputFields.Clear();
            SelectCommand.Clear();
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
