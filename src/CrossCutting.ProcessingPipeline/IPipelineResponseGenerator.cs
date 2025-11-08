namespace CrossCutting.ProcessingPipeline;

public interface IPipelineResponseGenerator
{
    Result<T> Generate<T>(object command);
}
