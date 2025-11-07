namespace CrossCutting.ProcessingPipeline;

public static class PipelineExtensions
{
    public static Task<Result> ExecuteAsync<TCommand>(this IPipeline<TCommand> pipeline, TCommand command, ICommandService commandService)
        => pipeline.ExecuteAsync(command, commandService, CancellationToken.None);

    public static Task<Result<TResponse>> ExecuteAsync<TCommand, TResponse>(this IPipeline<TCommand, TResponse> pipeline, TCommand command, ICommandService commandService)
        => pipeline.ExecuteAsync(command, commandService, CancellationToken.None);
}
