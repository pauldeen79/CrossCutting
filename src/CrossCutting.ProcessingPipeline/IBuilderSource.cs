namespace CrossCutting.ProcessingPipeline;

public interface IBuilderSource<out T>
{
    IBuilder<T> ToBuilder();
}
