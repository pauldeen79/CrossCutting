namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponent<TCommand>
{
    Task<Result> ExecuteAsync(TCommand commmand, CancellationToken token);
}
