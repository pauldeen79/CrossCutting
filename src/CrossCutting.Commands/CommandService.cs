namespace CrossCutting.Commands;

public class CommandService : ICommandService
{
    private readonly ICommandDecorator _decorator;
    private readonly IEnumerable<ICommandHandler> _handlers;

    public CommandService(ICommandDecorator decorator, IEnumerable<ICommandHandler> handlers)
    {
        ArgumentGuard.IsNotNull(decorator, nameof(decorator));
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));

        _decorator = decorator;
        _handlers = handlers;
    }

    public async Task<Result> ExecuteAsync<TCommand>(TCommand command, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(command, nameof(command));

        var handlers = _handlers.OfType<ICommandHandler<TCommand>>().ToArray();

        return handlers.Length switch
        {
            0 => Result.NotSupported($"No command handler is known for command type {typeof(TCommand).FullName}"),
            1 => await _decorator.ExecuteAsync(handlers[0], command, token).ConfigureAwait(false),
            _ => Result.NotSupported($"{handlers.Length} command handlers are known for command type {typeof(TCommand).FullName}, only 1 can be present"),
        };
    }

    public async Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(TCommand command, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(command, nameof(command));

        var handlers = _handlers.OfType<ICommandHandler<TCommand, TResponse>>().ToArray();

        return handlers.Length switch
        {
            0 => Result.NotSupported<TResponse>($"No command handler is known for command type {typeof(TCommand).FullName}"),
            1 => await _decorator.ExecuteAsync(handlers[0], command, token).ConfigureAwait(false),
            _ => Result.NotSupported<TResponse>($"{handlers.Length} command handlers are known for command type {typeof(TCommand).FullName}, only 1 can be present"),
        };
    }
}
