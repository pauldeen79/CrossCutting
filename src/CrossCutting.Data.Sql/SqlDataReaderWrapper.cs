namespace CrossCutting.Data.Sql;

public sealed class SqlDataReaderWrapper : IDataReader
{
    private readonly IDataReader _reader;
    private readonly Func<IDataReader, CancellationToken, Task<bool>> _readAsyncDelegate;
    private readonly Func<IDataReader, CancellationToken, Task<bool>> _nextResultAsyncDelegate;
    private readonly Func<IDataReader, Task> _closeAsyncDelegate;

    public SqlDataReaderWrapper(
        IDataReader reader,
        Func<IDataReader, CancellationToken, Task<bool>> readAsyncDelegate,
        Func<IDataReader, CancellationToken, Task<bool>> nextResultAsyncDelegate,
        Func<IDataReader, Task> closeAsyncDelegate)
    {
        _reader = reader;
        _readAsyncDelegate = readAsyncDelegate;
        _nextResultAsyncDelegate = nextResultAsyncDelegate;
        _closeAsyncDelegate = closeAsyncDelegate;
    }

    public object this[int i] => _reader[i];
    public object this[string name] => _reader[name];
    public int Depth => _reader.Depth;
    public bool IsClosed => _reader.IsClosed;
    public int RecordsAffected => _reader.RecordsAffected;
    public int FieldCount => _reader.FieldCount;
    public void Close() => _reader.Close();
    public void Dispose() => _reader.Dispose();
    public bool GetBoolean(int i) => _reader.GetBoolean(i);
    public byte GetByte(int i) => _reader.GetByte(i);
    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => _reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
    public char GetChar(int i) => _reader.GetChar(i);
    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => _reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
    public IDataReader GetData(int i) => _reader.GetData(i);
    public string GetDataTypeName(int i) => _reader.GetDataTypeName(i);
    public DateTime GetDateTime(int i) => _reader.GetDateTime(i);
    public decimal GetDecimal(int i) => _reader.GetDecimal(i);
    public double GetDouble(int i) => _reader.GetDouble(i);
    public Type GetFieldType(int i) => _reader.GetFieldType(i);
    public float GetFloat(int i) => _reader.GetFloat(i);
    public Guid GetGuid(int i) => _reader.GetGuid(i);
    public short GetInt16(int i) => _reader.GetInt16(i);
    public int GetInt32(int i) => _reader.GetInt32(i);
    public long GetInt64(int i) => _reader.GetInt64(i);
    public string GetName(int i) => _reader.GetName(i);
    public int GetOrdinal(string name) => _reader.GetOrdinal(name);
    public DataTable GetSchemaTable() => _reader.GetSchemaTable();
    public string GetString(int i) => _reader.GetString(i);
    public object GetValue(int i) => _reader.GetValue(i);
    public int GetValues(object[] values) => _reader.GetValues(values);
    public bool IsDBNull(int i) => _reader.IsDBNull(i);
    public bool NextResult() => _reader.NextResult();
    public bool Read() => _reader.Read();

    public Task<bool> ReadAsync(CancellationToken cancellationToken) => _readAsyncDelegate(_reader, cancellationToken);
    public Task<bool> NextResultAsync(CancellationToken cancellationToken) => _nextResultAsyncDelegate(_reader, cancellationToken);
    public Task CloseAsync() => _closeAsyncDelegate(_reader);
}
