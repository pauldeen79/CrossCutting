namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponentBuilder<TRequest> : IBuilder<IPipelineComponent<TRequest>>
{
}

public interface IPipelineComponentBuilder<TRequest, TResponse> : IBuilder<IPipelineComponent<TRequest, TResponse>>
{
}
