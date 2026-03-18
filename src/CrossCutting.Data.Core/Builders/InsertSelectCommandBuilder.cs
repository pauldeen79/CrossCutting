namespace CrossCutting.Data.Core.Builders;

public class InsertSelectCommandBuilder : IBuilder<IDatabaseCommand>
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
        FieldNames = [];
        OutputFields = [];
        SelectCommand = new SelectCommandBuilder();
        Table = string.Empty;
        TemporaryTable = string.Empty;
    }

    public InsertSelectCommandBuilder Into(string table)
        => this.Chain(() => Table = table);

    public InsertSelectCommandBuilder WithTemporaryTable(string temporaryTable)
        => this.Chain(() => TemporaryTable = temporaryTable);

    public InsertSelectCommandBuilder WithFieldNames(IEnumerable<string> fieldNames)
        => this.Chain(() => FieldNames.AddRange(fieldNames));

    public InsertSelectCommandBuilder WithFieldNames(params string[] fieldNames)
        => this.Chain(() => FieldNames.AddRange(fieldNames));

    public InsertSelectCommandBuilder WithSelectCommand(SelectCommandBuilder builder)
        => this.Chain(() => SelectCommand = builder);

    public InsertSelectCommandBuilder AddOutputFields(IEnumerable<string> outputFields)
        => this.Chain(() => OutputFields.AddRange(outputFields));

    public InsertSelectCommandBuilder AddOutputFields(params string[] outputFields)
        => this.Chain(() => OutputFields.AddRange(outputFields));

    public InsertSelectCommandBuilder AppendParameter(string key, object value)
        => this.Chain(() => CommandParameters.Add(key, value));

    public InsertSelectCommandBuilder AppendParameters(object parameters)
        => this.Chain(() => CommandParameters.AddRange(parameters.ToExpandoObject()));

    public InsertSelectCommandBuilder Clear()
    {
        CommandParameters.Clear();
        FieldNames.Clear();
        OutputFields.Clear();
        SelectCommand.Clear();
        Table = string.Empty;
        TemporaryTable = string.Empty;
        return this;
    }

    public IDatabaseCommand Build()
        => BuildTyped();

    public SqlDatabaseCommand BuildTyped()
        => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Insert, CommandParameters);

    public static implicit operator SqlDatabaseCommand(InsertSelectCommandBuilder instance)
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

        return new StringBuilder()
            .AppendInsert(Table, TemporaryTable, FieldNames, OutputFields)
            .Append(" ")
            .Append(SelectCommand.Build().CommandText)
            .ToString();
    }
}
