using System;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core
{
    public class TextCommand<T> : DatabaseCommand<T>
    {
        public TextCommand(string commandText, T instance, Func<T, object> commandParametersDelegate) : base(commandText, DatabaseCommandType.Text, instance, commandParametersDelegate)
        {
        }
    }

    public class SqlTextCommand : SqlDbCommand
    {
        public SqlTextCommand(string commandText, object? commandParameters = null) : base(commandText, DatabaseCommandType.Text, commandParameters)
        {
        }
    }
}
