namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommandProcessor<T> : IDatabaseCommandProcessor<T, T> where T : class
{
}

public interface IDatabaseCommandProcessor<TInput, TOutput> : IDatabaseCommandProcessor
    where TOutput : class
{
    IDatabaseCommandResult<TOutput> ExecuteCommand(IDatabaseCommand command, TInput instance);
    Task<IDatabaseCommandResult<TOutput>> ExecuteCommandAsync(IDatabaseCommand command, TInput instance, CancellationToken token);
}

public interface IDatabaseCommandProcessor
{
    object ExecuteScalar(IDatabaseCommand command);
    Task<object?> ExecuteScalarAsync(IDatabaseCommand command, CancellationToken token);
    int ExecuteNonQuery(IDatabaseCommand command);
    Task<int> ExecuteNonQueryAsync(IDatabaseCommand command, CancellationToken token);
}
