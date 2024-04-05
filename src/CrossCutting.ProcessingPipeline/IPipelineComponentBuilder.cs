namespace CrossCutting.ProcessingPipeline;

public interface IPipelineComponentBuilder<TModel> : IBuilder<IPipelineComponent<TModel>>
{
}

public interface IPipelineComponentBuilder<TModel, TContext> : IBuilder<IPipelineComponent<TModel, TContext>>
{
}
