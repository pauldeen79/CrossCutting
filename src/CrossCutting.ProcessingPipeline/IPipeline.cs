namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<in TCommand> : ICommandHandler<TCommand>
{
}

public interface IPipeline<in TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
{
}
