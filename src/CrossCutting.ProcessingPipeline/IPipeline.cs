namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<TModel>
{
    Task<Result> Process(TModel model, CancellationToken token);
}

public interface IPipeline<TModel, TContext>
{
    Task<Result> Process(TModel model, TContext context, CancellationToken token);
}
