using System;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core.Commands
{
    public class TextCommand<T> : DatabaseCommand<T>
    {
        public TextCommand(string commandText, T instance, Func<T, object> commandParametersDelegate)
            : this(commandText, instance, DatabaseOperation.Unspecified, commandParametersDelegate)
        {
        }

        public TextCommand(string commandText, T instance, DatabaseOperation operation, Func<T, object> commandParametersDelegate)
            : base(commandText, DatabaseCommandType.Text, instance, operation, commandParametersDelegate)
        {
        }
    }

    public class SqlTextCommand : SqlDatabaseCommand
    {
        public SqlTextCommand(string commandText, object? commandParameters = null)
            : this(commandText, DatabaseOperation.Unspecified, commandParameters)
        {
        }

        public SqlTextCommand(string commandText, DatabaseOperation operation, object? commandParameters = null)
            : base(commandText, DatabaseCommandType.Text, operation, commandParameters)
        {
        }
    }
}
