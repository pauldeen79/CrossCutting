namespace CrossCutting.ProcessingPipeline;

public interface IPipelineResponseGeneratorComponent
{
    Result<T> Generate<T>(object command);
}
