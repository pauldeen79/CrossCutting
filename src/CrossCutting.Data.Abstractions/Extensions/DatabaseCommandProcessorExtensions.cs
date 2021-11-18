namespace CrossCutting.Data.Abstractions.Extensions
{
    public static class DatabaseCommandProcessorExtensions
    {
        public static IDatabaseCommandResult<T> InvokeCommand<T>(this IDatabaseCommandProcessor<T> instance, T entityInstance)
            where T : class
            => instance.InvokeCommand(entityInstance, DatabaseOperation.Unspecified);
    }
}
