namespace CrossCutting.Common.Abstractions;

public interface IBuilder<out T>
{
    T Build();
}
