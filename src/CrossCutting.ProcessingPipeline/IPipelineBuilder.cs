namespace CrossCutting.ProcessingPipeline;

public interface IPipelineBuilder<TModel>
{
    IList<IBuilder<IPipelineComponent<TModel>>> Components { get; }

    public IPipeline<TModel> Build();
}

public interface IPipelineBuilder<TModel, TContext>
{
    IList<IBuilder<IPipelineComponent<TModel, TContext>>> Components { get; }

    public IPipeline<TModel, TContext> Build();
}
