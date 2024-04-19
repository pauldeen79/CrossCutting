namespace CrossCutting.Data.Abstractions.Extensions;

public static class DatabaseCommandProcessorExtensions
{
    public static Task<IDatabaseCommandResult<TOutput>> ExecuteCommandAsync<TInput, TOutput>(this IDatabaseCommandProcessor<TInput, TOutput> processor, IDatabaseCommand command, TInput instance)
        where TOutput : class
        => processor.ExecuteCommandAsync(command, instance, CancellationToken.None);

    public static Task<object> ExecuteScalarAsync(this IDatabaseCommandProcessor processor, IDatabaseCommand command)
        => processor.ExecuteScalarAsync(command, CancellationToken.None);

    public static Task<int> ExecuteNonQueryAsync(this IDatabaseCommandProcessor processor, IDatabaseCommand command)
        => processor.ExecuteNonQueryAsync(command, CancellationToken.None);
}
