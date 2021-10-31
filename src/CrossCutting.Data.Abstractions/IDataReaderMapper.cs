using System.Data;

namespace CrossCutting.Data.Abstractions
{
    public interface IDataReaderMapper<out T>
    {
        T Map(IDataReader reader);
    }
}
