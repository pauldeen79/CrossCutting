namespace CrossCutting.Data.Core.Commands;

public class SqlDatabaseCommand : IDatabaseCommand
{
    public SqlDatabaseCommand(string commandText,
                              DatabaseCommandType commandType,
                              DatabaseOperation operation = DatabaseOperation.Unspecified,
                              object? commandParameters = null)
    {
        ArgumentGuard.IsNotNullOrEmpty(commandText, nameof(commandText));

        CommandText = commandText;
        CommandType = commandType;
        Operation = operation;
        CommandParameters = commandParameters;
    }

    public string CommandText { get; }
    public DatabaseCommandType CommandType { get; }
    public DatabaseOperation Operation { get; }
    public object? CommandParameters { get; }
}
