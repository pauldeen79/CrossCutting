namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommandProcessor<T> where T : class
    {
        object ExecuteScalar(IDatabaseCommand command, DatabaseOperation operation);
        int ExecuteNonQuery(IDatabaseCommand command, DatabaseOperation operation);
        IDatabaseCommandResult<T> InvokeCommand(T instance, DatabaseOperation operation);
    }
}
