namespace CrossCutting.ProcessingPipeline;

public class PipelineBuilder<TRequest> : PipelineBuilderBase<IPipelineComponent<TRequest>, PipelineBuilder<TRequest>>, IPipelineBuilder<TRequest>
{
    public PipelineBuilder()
    {
    }

    public IPipeline<TRequest> Build()
        => new Pipeline<TRequest>(Initialize, Components.Select(x => x.Build()));

    protected virtual void Initialize(TRequest request, PipelineContext<TRequest> pipelineContext)
    {
    }
}

public class PipelineBuilder<TRequest, TResponse> : PipelineBuilderBase<IPipelineComponent<TRequest, TResponse>, PipelineBuilder<TRequest, TResponse>>, IPipelineBuilder<TRequest, TResponse>
    where TResponse : new()
{
    public PipelineBuilder()
    {
    }

    public IPipeline<TRequest, TResponse> Build()
        => new Pipeline<TRequest, TResponse>(Initialize, Components.Select(x => x.Build()));

    protected virtual void Initialize(TRequest request, PipelineContext<TRequest> pipelineContext)
    {
    }
}
