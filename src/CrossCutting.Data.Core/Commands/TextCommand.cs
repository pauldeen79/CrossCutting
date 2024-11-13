namespace CrossCutting.Data.Core.Commands;

public class TextCommand<T>(string commandText,
                   T instance,
                   DatabaseOperation operation,
                   Func<T, object?>? commandParametersDelegate) : DatabaseCommand<T>(commandText, DatabaseCommandType.Text, instance, operation, commandParametersDelegate)
{
    public TextCommand(string commandText,
                       T instance,
                       Func<T, object?>? commandParametersDelegate)
        : this(commandText, instance, DatabaseOperation.Unspecified, commandParametersDelegate)
    {
    }
}

public class SqlTextCommand(string commandText,
                      DatabaseOperation operation,
                      object? commandParameters = null) : SqlDatabaseCommand(commandText, DatabaseCommandType.Text, operation, commandParameters)
{
    public SqlTextCommand(string commandText,
                          object? commandParameters = null)
        : this(commandText, DatabaseOperation.Unspecified, commandParameters)
    {
    }
}
