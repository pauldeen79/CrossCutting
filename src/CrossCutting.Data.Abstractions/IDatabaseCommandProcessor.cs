namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommandProcessor<T> : IDatabaseCommandProcessor<T, T> where T : class
{
}

public interface IDatabaseCommandProcessor<TInput, out TOutput> : IDatabaseCommandProcessor
    where TOutput : class
{
    IDatabaseCommandResult<TOutput> ExecuteCommand(IDatabaseCommand command, TInput instance);
}

public interface IDatabaseCommandProcessor
{
    object ExecuteScalar(IDatabaseCommand command);
    int ExecuteNonQuery(IDatabaseCommand command);
}
