namespace CrossCutting.ProcessingPipeline;

public static class PipelineExtensions
{
    public static Task<Result> ExecuteAsync<TCommand>(this IPipeline<TCommand> pipeline, TCommand command)
        => pipeline.ExecuteAsync(command, CancellationToken.None);
}
