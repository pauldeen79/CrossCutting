namespace CrossCutting.ProcessingPipeline.Abstractions;

public interface IPipelineResponseGeneratorComponent
{
    Result<T> Generate<T>(object command);
}
