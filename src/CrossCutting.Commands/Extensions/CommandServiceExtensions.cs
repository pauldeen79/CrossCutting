namespace CrossCutting.Commands.Extensions;

public static class CommandServiceExtensions
{
    public static Task<Result> ExecuteAsync<TCommand>(this ICommandService instance, TCommand command)
        => instance.ExecuteAsync(command, CancellationToken.None);

    public static Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(this ICommandService instance, TCommand command)
        => instance.ExecuteAsync<TCommand, TResponse>(command, CancellationToken.None);
}
