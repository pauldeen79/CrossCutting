namespace CrossCutting.ProcessingPipeline;

public class PipelineBase<TRequest>(IEnumerable<IPipelineComponent<TRequest>>? features) : AbstractPipelineBase<IPipelineComponent<TRequest>>(features)
{
}

public class PipelineBase<TRequest, TResponse>(IEnumerable<IPipelineComponent<TRequest, TResponse>>? features) : AbstractPipelineBase<IPipelineComponent<TRequest, TResponse>>(features)
{
}
