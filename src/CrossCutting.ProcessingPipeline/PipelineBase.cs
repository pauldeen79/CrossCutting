namespace CrossCutting.ProcessingPipeline;

public class PipelineBase<TRequest> : AbstractPipelineBase<IPipelineComponent<TRequest>>
{
    public PipelineBase(IEnumerable<IPipelineComponent<TRequest>>? features) : base(features)
    {
    }
}

public class PipelineBase<TRequest, TResponse> : AbstractPipelineBase<IPipelineComponent<TRequest, TResponse>>
{
    public PipelineBase(IEnumerable<IPipelineComponent<TRequest, TResponse>>? features) : base(features)
    {
    }
}
