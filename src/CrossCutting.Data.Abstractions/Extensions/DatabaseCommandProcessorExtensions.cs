namespace CrossCutting.Data.Abstractions.Extensions
{
    public static class DatabaseCommandProcessorExtensions
    {
        public static object ExecuteScalar<T>(this IDatabaseCommandProcessor<T> instance, IDatabaseCommand command)
            where T : class
            => instance.ExecuteScalar(command, DatabaseOperation.Unspecified);
        public static int ExecuteNonQuery<T>(this IDatabaseCommandProcessor<T> instance, IDatabaseCommand command)
            where T : class
            => instance.ExecuteNonQuery(command, DatabaseOperation.Unspecified);
        public static IDatabaseCommandResult<T> InvokeCommand<T>(this IDatabaseCommandProcessor<T> instance, T entityInstance)
            where T : class
            => instance.InvokeCommand(entityInstance, DatabaseOperation.Unspecified);
    }
}
