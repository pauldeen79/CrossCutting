namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<TModel>
{
    IReadOnlyCollection<IPipelineComponent<TModel>> Components { get; }
    
    Result<TModel> Process(TModel model);
}

public interface IPipeline<TModel, TContext>
{
    IReadOnlyCollection<IPipelineComponent<TModel, TContext>> Components { get; }

    Result<TModel> Process(TModel model, TContext context);
}
