namespace CrossCutting.Commands.Abstractions;

#pragma warning disable CA1040 // Avoid empty interfaces
public interface ICommandHandler
#pragma warning restore CA1040 // Avoid empty interfaces
{

}

public interface IExecutable<in TCommand>
{
    Task<Result> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token);
}

public interface ICommandHandler<in TCommand> : ICommandHandler, IExecutable<TCommand>
{
}

public interface IExecutable<in TCommand, TResponse>
{
    Task<Result<TResponse>> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token);
}

public interface ICommandHandler<in TCommand, TResponse> : ICommandHandler, IExecutable<TCommand, TResponse>
{
}
