namespace CrossCutting.Data.Core.Commands;

public class DatabaseCommand<T> : IDatabaseCommand
{
    private readonly T _instance;
    private readonly Func<T, object?>? _commandParametersDelegate;

    public DatabaseCommand(string commandText,
                           DatabaseCommandType commandType,
                           T instance,
                           Func<T, object?>? commandParametersDelegate)
        : this(commandText, commandType, instance, DatabaseOperation.Unspecified, commandParametersDelegate)
    {
    }

    public DatabaseCommand(string commandText,
                           DatabaseCommandType commandType,
                           T instance,
                           DatabaseOperation operation,
                           Func<T, object?>? commandParametersDelegate)
    {
        if (string.IsNullOrEmpty(commandText))
        {
            throw new ArgumentOutOfRangeException(nameof(commandText), "CommandText may not be null or empty");
        }
        CommandText = commandText;
        CommandType = commandType;
        Operation = operation;
        _instance = instance;
        _commandParametersDelegate = commandParametersDelegate;
    }

    public string CommandText { get; }
    public DatabaseCommandType CommandType { get; }
    public DatabaseOperation Operation { get; }

    public object? CommandParameters
        => _commandParametersDelegate is null
            ? null
            : _commandParametersDelegate(_instance);
}
