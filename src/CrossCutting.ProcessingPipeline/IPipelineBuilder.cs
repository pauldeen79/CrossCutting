namespace CrossCutting.ProcessingPipeline;

public interface IPipelineBuilder<TModel>
{
    public IPipeline<TModel> Build();
}

public interface IPipelineBuilder<TModel, TContext>
{
    public IPipeline<TModel, TContext> Build();
}
