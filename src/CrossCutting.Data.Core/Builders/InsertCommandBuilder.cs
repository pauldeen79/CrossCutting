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
            FieldNames = new List<string>();
            OutputFields = new List<string>();
            FieldValues = new List<string>();
            Table = string.Empty;
            TemporaryTable = string.Empty;
        }

        public InsertCommandBuilder Into(string table)
            => this.Chain(() => Table = table);

        public InsertCommandBuilder WithTemporaryTable(string temporaryTable)
            => this.Chain(() => TemporaryTable = temporaryTable);

        public InsertCommandBuilder AddFieldName(string fieldName)
            => this.Chain(() => FieldNames.Add(fieldName));

        public InsertCommandBuilder AddFieldNames(IEnumerable<string> fieldNames)
            => this.Chain(() => FieldNames.AddRange(fieldNames));

        public InsertCommandBuilder AddFieldNames(params string[] fieldNames)
            => this.Chain(() => FieldNames.AddRange(fieldNames));

        public InsertCommandBuilder AddFieldValue(string fieldValue)
            => this.Chain(() => FieldValues.Add(fieldValue));

        public InsertCommandBuilder AddFieldValues(IEnumerable<string> fieldValues)
            => AddFieldValues(fieldValues.ToArray());

        public InsertCommandBuilder AddFieldValues(params string[] fieldValues)
            => this.Chain(() => FieldValues.AddRange(fieldValues));

        public InsertCommandBuilder AddOutputField(string outputField)
            => this.Chain(() => OutputFields.Add(outputField));

        public InsertCommandBuilder AddOutputFields(IEnumerable<string> outputFields)
            => this.Chain(() => OutputFields.AddRange(outputFields));

        public InsertCommandBuilder AddOutputFields(params string[] outputFields)
            => this.Chain(() => OutputFields.AddRange(outputFields));

        public InsertCommandBuilder AppendParameter(string key, object value)
            => this.Chain(() => CommandParameters.Add(key, value));

        public InsertCommandBuilder AppendParameters(object parameters)
            => this.Chain(() => CommandParameters.AddRange(parameters.ToExpandoObject()));

        public InsertCommandBuilder Clear()
        {
            CommandParameters.Clear();
            FieldNames.Clear();
            OutputFields.Clear();
            FieldValues.Clear();
            Table = string.Empty;
            TemporaryTable = string.Empty;
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

            return new StringBuilder()
                .AppendInsert(Table, TemporaryTable, FieldNames, OutputFields)
                .Append(" VALUES(")
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
