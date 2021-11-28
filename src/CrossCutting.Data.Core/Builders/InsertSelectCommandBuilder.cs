using System;
using System.Collections.Generic;
using System.Text;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using CrossCutting.Data.Core.Extensions;

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
            => this.Chain(() => Table = table);

        public InsertSelectCommandBuilder WithTemporaryTable(string temporaryTable)
            => this.Chain(() => TemporaryTable = temporaryTable);

        public InsertSelectCommandBuilder WithFieldName(string fieldName)
            => this.Chain(() => FieldNames.Add(fieldName));

        public InsertSelectCommandBuilder WithFieldNames(IEnumerable<string> fieldNames)
            => this.Chain(() => FieldNames.AddRange(fieldNames));

        public InsertSelectCommandBuilder WithFieldNames(params string[] fieldNames)
            => this.Chain(() => FieldNames.AddRange(fieldNames));

        public InsertSelectCommandBuilder WithSelectCommand(SelectCommandBuilder builder)
            => this.Chain(() => SelectCommand = builder);

        public InsertSelectCommandBuilder WithOutputField(string outputField)
            => this.Chain(() => OutputFields.Add(outputField));

        public InsertSelectCommandBuilder WithOutputFields(IEnumerable<string> outputFields)
            => this.Chain(() => OutputFields.AddRange(outputFields));

        public InsertSelectCommandBuilder WithOutputFields(params string[] outputFields)
            => this.Chain(() => OutputFields.AddRange(outputFields));

        public InsertSelectCommandBuilder AppendParameter(string key, object value)
            => this.Chain(() => CommandParameters.Add(key, value));

        public InsertSelectCommandBuilder AppendParameters(object parameters)
            => this.Chain(() => CommandParameters.AddRange(parameters.ToExpandoObject()));

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

            return new StringBuilder()
                .AppendInsert(Table, TemporaryTable, FieldNames, OutputFields)
                .Append(" ")
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
