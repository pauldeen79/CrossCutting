namespace CrossCutting.Common.Abstractions;

public interface IBuildableEntity<out T>
{
    T ToBuilder();
}
