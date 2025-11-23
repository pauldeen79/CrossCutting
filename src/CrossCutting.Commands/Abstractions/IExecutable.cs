namespace CrossCutting.Commands.Abstractions;

public interface IExecutable<in TCommand>
{
    Task<Result> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token);
}

public interface IExecutable<in TCommand, TResponse>
{
    Task<Result<TResponse>> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token);
}
