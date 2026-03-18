namespace CrossCutting.Data.Core.Builders;

public class InsertCommandBuilder : IBuilder<IDatabaseCommand>
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
        FieldNames = [];
        OutputFields = [];
        FieldValues = [];
        Table = string.Empty;
        TemporaryTable = string.Empty;
    }

    public InsertCommandBuilder Into(string table)
        => this.Chain(() => Table = table);

    public InsertCommandBuilder WithTemporaryTable(string temporaryTable)
        => this.Chain(() => TemporaryTable = temporaryTable);

    public InsertCommandBuilder AddFieldNames(IEnumerable<string> fieldNames)
        => this.Chain(() => FieldNames.AddRange(fieldNames));

    public InsertCommandBuilder AddFieldNames(params string[] fieldNames)
        => this.Chain(() => FieldNames.AddRange(fieldNames));

    public InsertCommandBuilder AddFieldValues(IEnumerable<string> fieldValues)
        => AddFieldValues(fieldValues.ToArray());

    public InsertCommandBuilder AddFieldValues(params string[] fieldValues)
        => this.Chain(() => FieldValues.AddRange(fieldValues));

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
        => BuildTyped();

    public SqlDatabaseCommand BuildTyped()
        => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Insert, CommandParameters);

    public static implicit operator SqlDatabaseCommand(InsertCommandBuilder instance)
        => instance.BuildTyped();

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
}
