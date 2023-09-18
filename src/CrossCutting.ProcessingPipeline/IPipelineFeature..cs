namespace CrossCutting.ProcessingPipeline;

public interface IPipelineFeature<TModel, TContext>
{
    void Process(PipelineContext<TModel, TContext> context);
}
