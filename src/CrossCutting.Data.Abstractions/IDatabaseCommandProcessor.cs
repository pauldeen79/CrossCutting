namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommandProcessor<T> where T : class
    {
        object ExecuteScalar(IDatabaseCommand command);
        int ExecuteNonQuery(IDatabaseCommand command);
        IDatabaseCommandResult<T> InvokeCommand(T instance);
    }
}
