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
        return Array.Find(results, x => !x.IsSuccessful())
            ?? Result.Success();
    }
}

public class Pipeline<TRequest, TResponse> : PipelineBase<TRequest, TResponse>, IPipeline<TRequest, TResponse>
    where TResponse : new()
{
    private readonly Action<TRequest, PipelineContext<TRequest, TResponse>> _validationDelegate;

    public Pipeline(Action<TRequest, PipelineContext<TRequest, TResponse>> validationDelegate, IEnumerable<IPipelineComponent<TRequest, TResponse>> features) : base(features.IsNotNull(nameof(features)))
    {
        _validationDelegate = validationDelegate.IsNotNull(nameof(validationDelegate));
    }

    public async Task<Result<TResponse>> Process(TRequest request, CancellationToken token)
    {
        var response = new TResponse();
        var pipelineContext = new PipelineContext<TRequest, TResponse>(request.IsNotNull(nameof(request)), response);

        _validationDelegate(request, pipelineContext);

        var results = await Task.WhenAll(Components.Select(x => x.Process(pipelineContext, token))).ConfigureAwait(false);
        var error = Array.Find(results, x => !x.IsSuccessful());
        if (error is not null)
        {
            return Result.FromExistingResult<TResponse>(error);
        }

        return Result.Success(response);
    }
}
