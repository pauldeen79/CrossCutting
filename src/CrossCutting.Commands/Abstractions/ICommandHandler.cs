namespace CrossCutting.Commands.Abstractions;

#pragma warning disable CA1040 // Avoid empty interfaces
public interface ICommandHandler
#pragma warning restore CA1040 // Avoid empty interfaces
{

}

public interface ICommandHandler<in TCommand> : ICommandHandler
{
    Task<Result> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token);
}

public interface ICommandHandler<in TCommand, TResponse> : ICommandHandler
{
    Task<Result<TResponse>> ExecuteAsync(TCommand command, ICommandService commandService, CancellationToken token);
}
