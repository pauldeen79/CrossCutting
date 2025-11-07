namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponent<TCommand>
{
    Task<Result> ExecuteAsync(TCommand commmand, ICommandService commandService, CancellationToken token);
}

public interface IPipelineComponent<TCommand, TResponse>
{
    Task<Result> ExecuteAsync(TCommand commmand, TResponse response, ICommandService commandService, CancellationToken token);
}
