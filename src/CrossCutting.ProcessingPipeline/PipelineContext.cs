namespace CrossCutting.ProcessingPipeline;

public class PipelineContext<TRequest>
{
    public PipelineContext(TRequest request)
    {
        ArgumentGuard.IsNotNull(request, nameof(request));

        Request = request;
    }

    public TRequest Request { get; }
}

public class PipelineContext<TRequest, TResponse> : PipelineContext<TRequest>
{
    public PipelineContext(TRequest request, TResponse response) : base(request)
    {
        ArgumentGuard.IsNotNull(response, nameof(response));

        Response = response;
    }

    public TResponse Response { get; }
}
