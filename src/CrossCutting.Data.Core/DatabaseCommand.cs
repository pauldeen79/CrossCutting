using System;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core
{
    public class DatabaseCommand<T> : IDatabaseCommand
    {
        private readonly T _instance;
        private readonly Func<T, object?> _commandParametersDelegate;

        public DatabaseCommand(string commandText, DatabaseCommandType commandType, T instance, Func<T, object?> commandParametersDelegate)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentOutOfRangeException(nameof(commandText), "CommandText may not be null or empty");
            }
            CommandText = commandText;
            CommandType = commandType;
            _instance = instance;
            _commandParametersDelegate = commandParametersDelegate;
        }

        public string CommandText { get; }

        public DatabaseCommandType CommandType { get; }

        public object? CommandParameters => _commandParametersDelegate == null
            ? null
            : _commandParametersDelegate(_instance);
    }

    public class SqlDbCommand : IDatabaseCommand
    {
        public SqlDbCommand(string commandText, DatabaseCommandType commandType, object? commandParameters = null)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentOutOfRangeException(nameof(commandText), "CommandText may not be null or empty");
            }
            CommandText = commandText;
            CommandType = commandType;
            CommandParameters = commandParameters;
        }

        public string CommandText { get; }

        public DatabaseCommandType CommandType { get; }

        public object? CommandParameters { get; }
    }
}
