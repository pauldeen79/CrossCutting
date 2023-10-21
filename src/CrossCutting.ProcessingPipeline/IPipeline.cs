namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<in TModel>
{
    Result Process(TModel model);
}

public interface IPipeline<TModel, TContext>
{
    Result<TContext> Process(TModel model, TContext context);
}
