using System;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Core
{
    public class StoredProcedureCommand<T> : DatabaseCommand<T>
    {
        public StoredProcedureCommand(string commandText, T instance, Func<T, object> commandParametersDelegate) : base(commandText, DatabaseCommandType.StoredProcedure, instance, commandParametersDelegate)
        {
        }
    }

    public class SqlStoredProcedureCommand : SqlDbCommand
    {
        public SqlStoredProcedureCommand(string commandText, object? commandParameters = null) : base(commandText, DatabaseCommandType.StoredProcedure, commandParameters)
        {
        }
    }
}
