namespace CrossCutting.Commands;

public class CommandService(
    IEnumerable<ICommandInterceptor> interceptors,
    IEnumerable<ICommandHandler> handlers) : ICommandService
{
    private readonly ICommandInterceptor[] _interceptors = ArgumentGuard.IsNotNull(interceptors, nameof(interceptors))
        .OrderBy(x => (x as IOrderContainer)?.Order)
        .ToArray();

    private readonly ICommandHandler[] _handlers = ArgumentGuard.IsNotNull(handlers, nameof(handlers)).ToArray();

    public async Task<Result> ExecuteAsync<TCommand>(TCommand command, CancellationToken token)
        => await Result.EnsureNotNull<TCommand>(command, nameof(command))
            .OnSuccessAsync(async () =>
            {
                var commandHandlers = _handlers.OfType<ICommandHandler<TCommand>>().ToArray();

                return commandHandlers.Length switch
                {
                    0 => Result.NotSupported($"No command handler is known for command type {typeof(TCommand).FullName}"),
                    1 => await DoExecute(commandHandlers[0], command, this, token).ConfigureAwait(false),
                    _ => Result.NotSupported($"{commandHandlers.Length} command handlers are known for command type {typeof(TCommand).FullName}, only 1 can be present"),
                };
            }).ConfigureAwait(false);

    public async Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(TCommand command, CancellationToken token)
        => await Result.EnsureNotNull<TResponse>(command, nameof(command))
            .OnSuccessAsync(async () =>
            {
                var commandHandlers = _handlers.OfType<ICommandHandler<TCommand, TResponse>>().ToArray();

                return commandHandlers.Length switch
                {
                    0 => Result.NotSupported<TResponse>($"No command handler is known for command type {typeof(TCommand).FullName}"),
                    1 => await DoExecute(commandHandlers[0], command, this, token).ConfigureAwait(false),
                    _ => Result.NotSupported<TResponse>($"{commandHandlers.Length} command handlers are known for command type {typeof(TCommand).FullName}, only 1 can be present"),
                };
            }).ConfigureAwait(false);

    private async Task<Result> DoExecute<TCommand>(ICommandHandler<TCommand> commandHandler, TCommand command, CommandService commandService, CancellationToken token)
    {
        var index = 0;

        Task<Result> Next()
        {
            if (index < _interceptors.Length)
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
            if (index < _interceptors.Length)
            {
                return _interceptors[index++].ExecuteAsync(command, commandService, Next, token);
            }

            return commandHandler.ExecuteAsync(command, commandService, token);
        }

        return await Next().ConfigureAwait(false);
    }
}
