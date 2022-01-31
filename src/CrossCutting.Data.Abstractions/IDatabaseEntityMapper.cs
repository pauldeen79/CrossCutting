namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseEntityMapper<out T>
    {
        T Map(IDataReader reader);
    }
}
