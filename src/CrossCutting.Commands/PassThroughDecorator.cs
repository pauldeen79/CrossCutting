namespace CrossCutting.Commands;

public class PassThroughDecorator : ICommandDecorator
{
    public Task<Result> ExecuteAsync<TCommand>(ICommandHandler<TCommand> handler, TCommand command, ICommandService commandService, CancellationToken token)
    {
        handler = ArgumentGuard.IsNotNull(handler, nameof(handler));

        return handler.ExecuteAsync(command, commandService, token);
    }

    public Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> handler, TCommand command, ICommandService commandService, CancellationToken token)
    {
        handler = ArgumentGuard.IsNotNull(handler, nameof(handler));

        return handler.ExecuteAsync(command, commandService, token);
    }
}
