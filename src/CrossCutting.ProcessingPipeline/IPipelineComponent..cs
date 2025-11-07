namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponent<in TCommand> : IExecutable<TCommand>
{
}

public interface IPipelineComponent<TCommand, TResponse>
{
    Task<Result> ExecuteAsync(TCommand commmand, TResponse response, ICommandService commandService, CancellationToken token);
}
