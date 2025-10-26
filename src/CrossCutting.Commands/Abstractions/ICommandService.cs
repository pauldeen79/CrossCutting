namespace CrossCutting.Commands.Abstractions;

public interface ICommandService
{
    Task<Result> ExecuteAsync<TCommand>(TCommand command, CancellationToken token);
    Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(TCommand command, CancellationToken token);
}
