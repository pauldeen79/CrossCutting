namespace CrossCutting.Data.Core.Builders;

public class DeleteCommandBuilder : IBuilder<IDatabaseCommand>
{
    public IDictionary<string, object> CommandParameters { get; set; }
    public string Table { get; set; }
    public string TemporaryTable { get; set; }
    public List<string> OutputFields { get; set; }
    private readonly StringBuilder _whereBuilder;

    public DeleteCommandBuilder()
    {
        CommandParameters = new Dictionary<string, object>();
        OutputFields = [];
        Table = string.Empty;
        TemporaryTable = string.Empty;
        _whereBuilder = new StringBuilder();
    }

    public DeleteCommandBuilder From(string table)
        => this.Chain(() => Table = table);

    public DeleteCommandBuilder WithTemporaryTable(string temporaryTable)
        => this.Chain(() => TemporaryTable = temporaryTable);

    public DeleteCommandBuilder Where(string value)
        => this.Chain(() =>
        {
            if (_whereBuilder.Length > 0)
            {
                _whereBuilder.Append(" AND ");
            }
            _whereBuilder.Append(value);
        });

    public DeleteCommandBuilder And(string value)
        => Where(value);

    public DeleteCommandBuilder Or(string value)
        => this.Chain(() =>
        {
            if (_whereBuilder.Length == 0)
            {
                throw new InvalidOperationException("There is no WHERE clause to combine the current value with");
            }
            _whereBuilder.Append(" OR ").Append(value);
        });

    public DeleteCommandBuilder AddOutputFields(IEnumerable<string> outputFields)
        => this.Chain(() => OutputFields.AddRange(outputFields));

    public DeleteCommandBuilder AddOutputFields(params string[] outputFields)
        => this.Chain(() => OutputFields.AddRange(outputFields));

    public DeleteCommandBuilder AppendParameter(string key, object value)
        => this.Chain(() => CommandParameters.Add(key, value));

    public DeleteCommandBuilder AppendParameters(object parameters)
        => this.Chain(() => CommandParameters.AddRange(parameters.ToExpandoObject()));

    public DeleteCommandBuilder Clear()
        => this.Chain(() =>
        {
            CommandParameters.Clear();
            OutputFields.Clear();
            Table = string.Empty;
            TemporaryTable = string.Empty;
            _whereBuilder.Clear();
        });

    public IDatabaseCommand Build()
        => BuildTyped();

    public SqlDatabaseCommand BuildTyped()
        => new SqlDatabaseCommand(BuildSql(), DatabaseCommandType.Text, DatabaseOperation.Delete, CommandParameters);

    public static implicit operator SqlDatabaseCommand(DeleteCommandBuilder instance)
        => instance.BuildTyped();

    private string BuildSql()
    {
        if (string.IsNullOrWhiteSpace(Table))
        {
            throw new InvalidOperationException("table name is missing");
        }

        var builder = new StringBuilder()
            .Append("DELETE FROM ")
            .Append(Table)
            .AppendOutputFields(TemporaryTable, OutputFields);

        if (_whereBuilder.Length > 0)
        {
            builder.Append(" WHERE ").Append(_whereBuilder);
        }

        return builder.ToString();
    }
}
