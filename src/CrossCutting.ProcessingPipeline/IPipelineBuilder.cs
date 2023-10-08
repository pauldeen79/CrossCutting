namespace CrossCutting.ProcessingPipeline;

public interface IPipelineBuilder<TModel> : IValidatableObject
{
    public Pipeline<TModel> Build();
}

public interface IPipelineBuilder<TModel, TContext> : IValidatableObject
{
    public Pipeline<TModel, TContext> Build();
}
