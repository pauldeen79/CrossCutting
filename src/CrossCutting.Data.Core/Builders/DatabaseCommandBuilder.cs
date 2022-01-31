namespace CrossCutting.Data.Core.Builders;

public class DatabaseCommandBuilder
{
    public DatabaseCommandType CommandType { get; set; }
    public DatabaseOperation Operation { get; set; }
    public IDictionary<string, object> CommandParameters { get; set; }
    private readonly StringBuilder _commandTextBuilder;

    public DatabaseCommandBuilder()
    {
        CommandParameters = new Dictionary<string, object>();
        _commandTextBuilder = new StringBuilder();
        Operation = DatabaseOperation.Unspecified;
    }

    public DatabaseCommandBuilder Append(string value)
        => this.Chain(() => _commandTextBuilder.Append(value));

    public DatabaseCommandBuilder AppendParameter(string key, object value)
        => this.Chain(() => CommandParameters.Add(key, value));

    public DatabaseCommandBuilder WithOperation(DatabaseOperation operation)
        => this.Chain(() => Operation = operation);

    public DatabaseCommandBuilder Clear()
        => this.Chain(() =>
        {
            _commandTextBuilder.Clear();
            CommandParameters.Clear();
            Operation = DatabaseOperation.Unspecified;
        });

    public IDatabaseCommand Build()
        => new SqlDatabaseCommand(_commandTextBuilder.ToString(), CommandType, Operation, CommandParameters);
}
