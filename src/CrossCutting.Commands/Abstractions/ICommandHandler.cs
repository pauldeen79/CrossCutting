namespace CrossCutting.Commands.Abstractions;

#pragma warning disable CA1040 // Avoid empty interfaces
public interface ICommandHandler
#pragma warning restore CA1040 // Avoid empty interfaces
{

}

public interface ICommandHandler<in TCommand> : ICommandHandler, IExecutable<TCommand>
{
}

public interface ICommandHandler<in TCommand, TResponse> : ICommandHandler, IExecutable<TCommand, TResponse>
{
}
