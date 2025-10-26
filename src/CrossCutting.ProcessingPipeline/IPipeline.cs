namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<in TCommand> : ICommandHandler<TCommand>
{
}
