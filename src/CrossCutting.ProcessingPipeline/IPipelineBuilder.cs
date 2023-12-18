namespace CrossCutting.ProcessingPipeline;

public interface IPipelineBuilder<TModel> : IValidatableObject
{
    public IPipeline<TModel> Build();
}

public interface IPipelineBuilder<TModel, TContext> : IValidatableObject
{
    public IPipeline<TModel, TContext> Build();
}
