namespace CrossCutting.Commands;

public class CommandService : ICommandService
{
    private readonly List<ICommandInterceptor> _interceptors;
    private readonly IEnumerable<ICommandHandler> _handlers;

    public CommandService(IEnumerable<ICommandInterceptor> interceptors, IEnumerable<ICommandHandler> handlers)
    {
        ArgumentGuard.IsNotNull(interceptors, nameof(interceptors));
        ArgumentGuard.IsNotNull(handlers, nameof(handlers));

        _interceptors = interceptors.OrderBy(x => (x as IOrderContainer)?.Order).ToList();
        _handlers = handlers;
    }

    public async Task<Result> ExecuteAsync<TCommand>(TCommand command, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(command, nameof(command));

        var handlers = _handlers.OfType<ICommandHandler<TCommand>>().ToArray();

        return handlers.Length switch
        {
            0 => Result.NotSupported($"No command handler is known for command type {typeof(TCommand).FullName}"),
            1 => await DoExecute(handlers[0], command, this, token).ConfigureAwait(false),
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
            1 => await DoExecute(handlers[0], command, this, token).ConfigureAwait(false),
            _ => Result.NotSupported<TResponse>($"{handlers.Length} command handlers are known for command type {typeof(TCommand).FullName}, only 1 can be present"),
        };
    }

    private async Task<Result> DoExecute<TCommand>(ICommandHandler<TCommand> commandHandler, TCommand command, CommandService commandService, CancellationToken token)
    {
        var index = 0;

        Task<Result> Next()
        {
            if (index < _interceptors.Count)
            {
                return _interceptors[index++].ExecuteAsync(command, commandService, Next, token);
            }

            return commandHandler.ExecuteAsync(command, commandService, token);
        }

        return await Next().ConfigureAwait(false);
    }

    private async Task<Result<TResponse>> DoExecute<TCommand, TResponse>(ICommandHandler<TCommand, TResponse> commandHandler, TCommand command, CommandService commandService, CancellationToken token)
    {
        var index = 0;

        Task<Result<TResponse>> Next()
        {
            if (index < _interceptors.Count)
            {
                return _interceptors[index++].ExecuteAsync(command, commandService, Next, token);
            }

            return commandHandler.ExecuteAsync(command, commandService, token);
        }

        return await Next().ConfigureAwait(false);
    }
}
