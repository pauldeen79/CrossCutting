namespace CrossCutting.ProcessingPipeline;

public class PipelineContext<TRequest>
{
    public PipelineContext(TRequest request)
    {
        Request = request.IsNotNull(nameof(request));
    }

    public TRequest Request { get; }
}

public class PipelineContext<TRequest, TResponse> : PipelineContext<TRequest>
{
    public PipelineContext(TRequest request, TResponse response) : base(request)
    {
        Response = response.IsNotNull(nameof(response));
    }

    public TResponse Response { get; }
}
