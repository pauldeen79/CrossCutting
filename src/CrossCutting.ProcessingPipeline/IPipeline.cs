namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<TModel>
{
    Result<TModel> Process(TModel model);
}

public interface IPipeline<TModel, TContext>
{
    Result<TModel> Process(TModel model, TContext context);
}
