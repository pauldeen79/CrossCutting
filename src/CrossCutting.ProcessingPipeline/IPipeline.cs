namespace CrossCutting.ProcessingPipeline;

public interface IPipeline<TModel>
{
    Task<Result<TModel>> Process(TModel model, CancellationToken token);
}

public interface IPipeline<TModel, TContext>
{
    Task<Result<TModel>> Process(TModel model, TContext context, CancellationToken token);
}
