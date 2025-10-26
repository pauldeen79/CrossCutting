namespace CrossCutting.Commands.Abstractions;

public interface ICommandDecorator
{
    Task<Result> ExecuteAsync<TCommand>(ICommandHandler<TCommand> handler, TCommand command, CancellationToken token);
    Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> handler, TCommand command, CancellationToken token);
}
