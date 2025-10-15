namespace CrossCutting.ProcessingPipeline;

public class Pipeline<TRequest> : IPipeline<TRequest>
{
    private readonly IEnumerable<IPipelineComponent<TRequest>> _components;

    public Pipeline(IEnumerable<IPipelineComponent<TRequest>> components)
    {
        ArgumentGuard.IsNotNull(components, nameof(components));

        _components = components;
    }

    public async Task<Result> ProcessAsync(TRequest request, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(request, nameof(request));

        var pipelineContext = new PipelineContext<TRequest>(request);

        var results = new List<Result>();
        foreach (var component in _components)
        {
            var result = await component.ProcessAsync(pipelineContext, token).ConfigureAwait(false);
            results.Add(result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return Result.Aggregate
        (
            results,
            Result.Success(),
            errors => Result.Error(errors, "An error occured while processing the pipeline. See the inner results for more details.")
        );
    }
}

public class Pipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
{
    private readonly IEnumerable<IPipelineComponent<TRequest, TResponse>> _components;

    public Pipeline(IEnumerable<IPipelineComponent<TRequest, TResponse>> components)
    {
        ArgumentGuard.IsNotNull(components, nameof(components));
        
        _components = components;
    }

    public async Task<Result<TResponse>> ProcessAsync(TRequest request, TResponse seed, CancellationToken token)
    {
        ArgumentGuard.IsNotNull(request, nameof(request));

        var pipelineContext = new PipelineContext<TRequest, TResponse>(request, seed);

        var results = new List<Result>();
        foreach (var component in _components)
        {
            var result = await component.ProcessAsync(pipelineContext, token).ConfigureAwait(false);
            results.Add(result);
            if (!result.IsSuccessful())
            {
                break;
            }
        }

        return Result.Aggregate
        (
            results,
            Result.Success(seed),
            errors => Result.Error<TResponse>(errors, "An error occured while processing the pipeline. See the inner results for more details.")
        );
    }
}
