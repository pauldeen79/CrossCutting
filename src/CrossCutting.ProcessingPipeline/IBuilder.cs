namespace CrossCutting.ProcessingPipeline;

public interface IBuilder<out T>
{
    T Build();
}
