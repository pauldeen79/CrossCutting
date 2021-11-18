using System;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core
{
    public class StoredProcedureCommand<T> : DatabaseCommand<T>
    {
        public StoredProcedureCommand(string commandText,
                                      T instance,
                                      Func<T, object> commandParametersDelegate)
            : this(commandText, instance, DatabaseOperation.Unspecified, commandParametersDelegate)
        {
        }

        public StoredProcedureCommand(string commandText,
                                      T instance,
                                      DatabaseOperation operation,
                                      Func<T, object> commandParametersDelegate)
            : base(commandText, DatabaseCommandType.StoredProcedure, instance, operation, commandParametersDelegate)
        {
        }
    }

    public class SqlStoredProcedureCommand : SqlDatabaseCommand
    {
        public SqlStoredProcedureCommand(string commandText,
                                         object? commandParameters = null)
            : this(commandText, DatabaseOperation.Unspecified, commandParameters)
        {
        }

        public SqlStoredProcedureCommand(string commandText,
                                         DatabaseOperation operation,
                                         object? commandParameters = null)
            : base(commandText, DatabaseCommandType.StoredProcedure, operation, commandParameters)
        {
        }
    }
}
