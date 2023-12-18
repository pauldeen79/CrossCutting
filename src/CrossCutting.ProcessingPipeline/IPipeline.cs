namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<TModel>
{
    IReadOnlyCollection<IPipelineFeature<TModel>> Features { get; }
    
    Result<TModel> Process(TModel model);
}

public interface IPipeline<TModel, TContext>
{
    IReadOnlyCollection<IPipelineFeature<TModel, TContext>> Features { get; }

    Result<TModel> Process(TModel model, TContext context);
}
