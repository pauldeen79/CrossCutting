namespace System.Data.Stub;

public sealed class DataReader : IDataReader
{
    public int CurrentIndex { get; private set; }
    private CultureInfo CultureInfo { get; }
    public Dictionary<int, IDictionary<string, object>> Dictionary { get; private set; }
        = new Dictionary<int, IDictionary<string, object>>();

    public DataReader(CommandBehavior commandBehavior, CultureInfo? cultureInfo = null)
    {
        CommandBehavior = commandBehavior;
        CultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;
    }

    public object this[int i] { get => Dictionary[CurrentIndex][Dictionary[CurrentIndex].Keys.ElementAt(i)]; }

    public object this[string name] { get => Dictionary[CurrentIndex][name]; }

    public int Depth => 1;

    public bool IsClosed { get; set; }

    public int RecordsAffected { get; set; }

    public int FieldCount
        => CurrentIndex > Dictionary.Count || Dictionary.Count == 0 || !Dictionary.ContainsKey(CurrentIndex)
            ? 0
            : Dictionary[CurrentIndex].Count;

    public CommandBehavior CommandBehavior { get; }

    public void Close() => IsClosed = true;

    public void Dispose() => Close();

    public bool GetBoolean(int i) => Convert.ToBoolean(this[i], CultureInfo);

    public byte GetByte(int i) => Convert.ToByte(this[i], CultureInfo);

    public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => throw new NotImplementedException();

    public char GetChar(int i) => Convert.ToChar(this[i], CultureInfo);

    public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => throw new NotImplementedException();

    public IDataReader GetData(int i) => throw new NotImplementedException();

    public string GetDataTypeName(int i) => this[i].GetType().FullName;

    public DateTime GetDateTime(int i) => Convert.ToDateTime(this[i], CultureInfo);

    public decimal GetDecimal(int i) => Convert.ToDecimal(this[i], CultureInfo);

    public double GetDouble(int i) => Convert.ToDouble(this[i], CultureInfo);

    public Type GetFieldType(int i) => this[i].GetType();

    public float GetFloat(int i) => Convert.ToSingle(this[i], CultureInfo);

    public Guid GetGuid(int i) => new Guid(Convert.ToString(this[i], CultureInfo));

    public short GetInt16(int i) => Convert.ToInt16(this[i], CultureInfo);

    public int GetInt32(int i) => Convert.ToInt32(this[i], CultureInfo);

    public long GetInt64(int i) => Convert.ToInt64(this[i], CultureInfo);

    public string GetName(int i) => Dictionary[CurrentIndex].Keys.ElementAt(i);

    public int GetOrdinal(string name)
    {
        int index = 0;
        foreach (var key in Dictionary[CurrentIndex].Keys)
        {
            if (key == name)
            {
                return index;
            }
            index++;
        }

        return -1;
    }

    public DataTable GetSchemaTable() => throw new NotImplementedException();

    public string GetString(int i) => Convert.ToString(this[i], CultureInfo);

    public object GetValue(int i) => this[i];

    public int GetValues(object[] values)
    {
        var index = 0;
        foreach (var kvp in Dictionary[CurrentIndex])
        {
            values[index] = kvp.Value;
            index++;
        }

        return Dictionary.Count;
    }

    public bool IsDBNull(int i) => this[i] == null || this[i] == DBNull.Value;

    public bool NextResult()
    {
        if (NextResultCalled == null)
        {
            return false;
        }

        var args = new NextResultCalledEventArgs();
        NextResultCalled.Invoke(this, args);
        if (args.Result)
        {
            CurrentIndex = args.CurrentIndex ?? CurrentIndex;
            Dictionary = args.Dictionary ?? Dictionary;
        }

        return args.Result;
    }

    public bool Read()
    {
        if (CurrentIndex >= Dictionary.Count)
        {
            return false;
        }
        CurrentIndex++;
        return true;
    }

    public void Add(object objectWithValues)
    {
        var localDict = new Dictionary<string, object>();
        foreach (var property in objectWithValues.GetType().GetProperties())
        {
            localDict.Add(property.Name, property.GetValue(objectWithValues));
        }
        Dictionary.Add(Dictionary.Count + 1, localDict);
    }

    public event EventHandler<NextResultCalledEventArgs>? NextResultCalled;
}
