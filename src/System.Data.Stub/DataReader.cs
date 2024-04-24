namespace System.Data.Stub;

public sealed class DataReader : Common.DbDataReader
{
    public int CurrentIndex { get; private set; }
    private CultureInfo _cultureInfo { get; }
    private bool _isClosed;

    public Dictionary<int, IDictionary<string, object>> Dictionary { get; private set; }
        = new Dictionary<int, IDictionary<string, object>>();

    public DataReader(CommandBehavior commandBehavior, CultureInfo? cultureInfo = null)
    {
        CommandBehavior = commandBehavior;
        _cultureInfo = cultureInfo ?? CultureInfo.CurrentCulture;
    }

    public override object this[int i] => Dictionary[CurrentIndex][Dictionary[CurrentIndex].Keys.ElementAt(i)];

    public override object this[string name] => Dictionary[CurrentIndex][name];

    public override int Depth => 1;

    public override bool IsClosed => _isClosed;

    public override int RecordsAffected { get; }

    public override int FieldCount
        => CurrentIndex > Dictionary.Count || Dictionary.Count == 0 || !Dictionary.ContainsKey(CurrentIndex)
            ? 0
            : Dictionary[CurrentIndex].Count;

    public CommandBehavior CommandBehavior { get; }

    public override bool HasRows => throw new NotImplementedException();

    public override void Close() => _isClosed = true;

    public override bool GetBoolean(int ordinal) => Convert.ToBoolean(this[ordinal], _cultureInfo);

    public override byte GetByte(int ordinal) => Convert.ToByte(this[ordinal], _cultureInfo);

    public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length) => throw new NotImplementedException();

    public override char GetChar(int ordinal) => Convert.ToChar(this[ordinal], _cultureInfo);

    public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length) => throw new NotImplementedException();

    public override string GetDataTypeName(int ordinal) => this[ordinal].GetType().FullName;

    public override DateTime GetDateTime(int ordinal) => Convert.ToDateTime(this[ordinal], _cultureInfo);

    public override decimal GetDecimal(int ordinal) => Convert.ToDecimal(this[ordinal], _cultureInfo);

    public override double GetDouble(int ordinal) => Convert.ToDouble(this[ordinal], _cultureInfo);

    public override Type GetFieldType(int ordinal) => this[ordinal].GetType();

    public override float GetFloat(int ordinal) => Convert.ToSingle(this[ordinal], _cultureInfo);

    public override Guid GetGuid(int ordinal) => new Guid(Convert.ToString(this[ordinal], _cultureInfo));

    public override short GetInt16(int ordinal) => Convert.ToInt16(this[ordinal], _cultureInfo);

    public override int GetInt32(int ordinal) => Convert.ToInt32(this[ordinal], _cultureInfo);

    public override long GetInt64(int ordinal) => Convert.ToInt64(this[ordinal], _cultureInfo);

    public override string GetName(int ordinal) => Dictionary[CurrentIndex].Keys.ElementAt(ordinal);

    public override int GetOrdinal(string name)
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

    public override DataTable GetSchemaTable() => throw new NotImplementedException();

    public override string GetString(int ordinal) => Convert.ToString(this[ordinal], _cultureInfo);

    public override object GetValue(int ordinal) => this[ordinal];

    public override int GetValues(object[] values)
    {
        var index = 0;
        foreach (var kvp in Dictionary[CurrentIndex])
        {
            values[index] = kvp.Value;
            index++;
        }

        return Dictionary.Count;
    }

    public override bool IsDBNull(int ordinal) => this[ordinal] is null || this[ordinal] == DBNull.Value;

    public override bool NextResult()
    {
        if (NextResultCalled is null)
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

    public override bool Read()
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

    public override IEnumerator GetEnumerator() => Dictionary.Select(x => x.Value).GetEnumerator();

    public event EventHandler<NextResultCalledEventArgs>? NextResultCalled;
}
