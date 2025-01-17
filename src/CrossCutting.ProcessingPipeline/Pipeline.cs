namespace CrossCutting.ProcessingPipeline;

public class Pipeline<TRequest> : PipelineBase<TRequest>, IPipeline<TRequest>
{
    public Pipeline(IEnumerable<IPipelineComponent<TRequest>> components) : base(components)
    {
        ArgumentGuard.IsNotNull(components, nameof(components)); //note that the base class allows null
    }

    public async Task<Result> ProcessAsync(TRequest request, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(request, nameof(request));

        var pipelineContext = new PipelineContext<TRequest>(request);

        var results = await Task.WhenAll(Components.Select(x => x.ProcessAsync(pipelineContext, token))).ConfigureAwait(false);

        return Result.Aggregate
        (
            results,
            Result.Success(),
            errors => Result.Error(errors, "An error occured while processing the pipeline. See the inner results for more details.")
        );
    }
}

public class Pipeline<TRequest, TResponse> : PipelineBase<TRequest, TResponse>, IPipeline<TRequest, TResponse>
{
    public Pipeline(IEnumerable<IPipelineComponent<TRequest, TResponse>> components) : base(components)
    {
        ArgumentGuard.IsNotNull(components, nameof(components)); //note that the base class allows null
    }

    public async Task<Result<TResponse>> ProcessAsync(TRequest request, TResponse seed, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(request, nameof(request));

        var pipelineContext = new PipelineContext<TRequest, TResponse>(request, seed);

        var results = await Task.WhenAll(Components.Select(x => x.ProcessAsync(pipelineContext, token))).ConfigureAwait(false);

        return Result.Aggregate
        (
            results,
            Result.Success(seed),
            errors => Result.Error<TResponse>(errors, "An error occured while processing the pipeline. See the inner results for more details.")
        );
    }
}
