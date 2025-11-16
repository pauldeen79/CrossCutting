namespace CrossCutting.ProcessingPipeline.Abstractions;

public interface IPipelineResponseGenerator
{
    Result<T> Generate<T>(object command);
}
