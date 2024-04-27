namespace CrossCutting.ProcessingPipeline;

public class Pipeline<TRequest> : PipelineBase<TRequest>, IPipeline<TRequest>
{
    private readonly Action<TRequest, PipelineContext<TRequest>> _validationDelegate;

    public Pipeline(Action<TRequest, PipelineContext<TRequest>> validationDelegate, IEnumerable<IPipelineComponent<TRequest>> features) : base(features.IsNotNull(nameof(features)))
    {
        _validationDelegate = validationDelegate.IsNotNull(nameof(validationDelegate));
    }

    public async Task<Result> Process(TRequest request, CancellationToken token)
    {
        var pipelineContext = new PipelineContext<TRequest>(ArgumentGuard.IsNotNull(request, nameof(request)));

        _validationDelegate(request, pipelineContext);

        var results = await Task.WhenAll(Components.Select(x => x.Process(pipelineContext, token))).ConfigureAwait(false);
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
    private readonly Action<TRequest, PipelineContext<TRequest, TResponse>> _validationDelegate;

    public Pipeline(Action<TRequest, PipelineContext<TRequest, TResponse>> validationDelegate, IEnumerable<IPipelineComponent<TRequest, TResponse>> features) : base(features.IsNotNull(nameof(features)))
    {
        _validationDelegate = validationDelegate.IsNotNull(nameof(validationDelegate));
    }

    public async Task<Result<TResponse>> Process(TRequest request, TResponse seed, CancellationToken token)
    {
        var pipelineContext = new PipelineContext<TRequest, TResponse>(request.IsNotNull(nameof(request)), seed);

        _validationDelegate(request, pipelineContext);

        var results = await Task.WhenAll(Components.Select(x => x.Process(pipelineContext, token))).ConfigureAwait(false);
        return Result.Aggregate
        (
            results,
            Result.Success(seed),
            errors => Result.Error<TResponse>(errors, "An error occured while processing the pipeline. See the inner results for more details.")
        );
    }
}
