using System;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core
{
    public class SqlDatabaseCommand : IDatabaseCommand
    {
        public SqlDatabaseCommand(string commandText,
                                  DatabaseCommandType commandType,
                                  DatabaseOperation operation = DatabaseOperation.Unspecified,
                                  object? commandParameters = null)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentOutOfRangeException(nameof(commandText), "CommandText may not be null or empty");
            }
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
}
