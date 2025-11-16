namespace CrossCutting.Commands.Abstractions;

public interface ICommandInterceptor
{
    Task<Result> ExecuteAsync<TCommand>(TCommand command, ICommandService commandService, Func<Task<Result>> next, CancellationToken token);
    Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(TCommand command, ICommandService commandService, Func<Task<Result<TResponse>>> next, CancellationToken token);
}
