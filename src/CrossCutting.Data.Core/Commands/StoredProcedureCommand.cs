namespace CrossCutting.Data.Core.Commands;

public class StoredProcedureCommand<T>(string commandText,
                              T instance,
                              DatabaseOperation operation,
                              Func<T, object?>? commandParametersDelegate) : DatabaseCommand<T>(commandText, DatabaseCommandType.StoredProcedure, instance, operation, commandParametersDelegate)
{
    public StoredProcedureCommand(string commandText,
                                  T instance,
                                  Func<T, object?>? commandParametersDelegate)
        : this(commandText, instance, DatabaseOperation.Unspecified, commandParametersDelegate)
    {
    }
}

public class SqlStoredProcedureCommand(string commandText,
                                 DatabaseOperation operation,
                                 object? commandParameters = null) : SqlDatabaseCommand(commandText, DatabaseCommandType.StoredProcedure, operation, commandParameters)
{
    public SqlStoredProcedureCommand(string commandText,
                                     object? commandParameters = null)
        : this(commandText, DatabaseOperation.Unspecified, commandParameters)
    {
    }
}
