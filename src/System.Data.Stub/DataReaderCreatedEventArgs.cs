namespace System.Data.Stub;

public class DataReaderCreatedEventArgs(DataReader dataReader) : EventArgs
{
    public DataReader DataReader { get; } = dataReader;
}
