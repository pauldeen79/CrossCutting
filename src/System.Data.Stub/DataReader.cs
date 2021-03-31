using System.Collections.Generic;
using System.Linq;

namespace System.Data.Stub
{
    public sealed class DataReader : IDataReader
    {
        private int currentIndex;
        private Dictionary<int, IDictionary<string, object>> dictionary = new Dictionary<int, IDictionary<string, object>>();

        public object this[int i] { get => dictionary[currentIndex][dictionary[currentIndex].Keys.ElementAt(i)]; }

        public object this[string name] { get => dictionary[currentIndex][name]; }

        public int Depth => 1;

        public bool IsClosed { get; set; }

        public int RecordsAffected { get; set; }

        public int FieldCount => currentIndex > dictionary.Count
            ? 0
            : dictionary[currentIndex].Count;

        public void Close()
        {
            IsClosed = true;
        }

        public void Dispose()
        {
            // Method intentionally left empty.
        }

        public bool GetBoolean(int i)
        {
            return Convert.ToBoolean(this[i]);
        }

        public byte GetByte(int i)
        {
            return Convert.ToByte(this[i]);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return Convert.ToChar(this[i]);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            return this[i].GetType().FullName;
        }

        public DateTime GetDateTime(int i)
        {
            return Convert.ToDateTime(this[i]);
        }

        public decimal GetDecimal(int i)
        {
            return Convert.ToDecimal(this[i]);
        }

        public double GetDouble(int i)
        {
            return Convert.ToDouble(this[i]);
        }

        public Type GetFieldType(int i)
        {
            return this[i].GetType();
        }

        public float GetFloat(int i)
        {
            return Convert.ToSingle(this[i]);
        }

        public Guid GetGuid(int i)
        {
            return new Guid(Convert.ToString(this[i]));
        }

        public short GetInt16(int i)
        {
            return Convert.ToInt16(this[i]);
        }

        public int GetInt32(int i)
        {
            return Convert.ToInt32(this[i]);
        }

        public long GetInt64(int i)
        {
            return Convert.ToInt64(this[i]);
        }

        public string GetName(int i)
        {
            return dictionary[currentIndex].Keys.ElementAt(i);
        }

        public int GetOrdinal(string name)
        {
            int index = 0;
            foreach (var key in dictionary[currentIndex].Keys)
            {
                if (key == name)
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            return Convert.ToString(this[i]);
        }

        public object GetValue(int i)
        {
            return this[i];
        }

        public int GetValues(object[] values)
        {
            var index = 0;
            foreach (var kvp in dictionary[currentIndex])
            {
                values[index] = kvp.Value;
                index++;
            }

            return dictionary.Count;
        }

        public bool IsDBNull(int i)
        {
            return this[i] == null || this[i] == DBNull.Value;
        }

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
                currentIndex = args.CurrentIndex ?? currentIndex;
                dictionary = args.Dictionary ?? dictionary;
            }

            return args.Result;
        }

        public bool Read()
        {
            if (currentIndex >= dictionary.Count)
            {
                return false;
            }
            currentIndex++;
            return true;
        }

        public void Add(object objectWithValues)
        {
            var localDict = new Dictionary<string, object>();
            foreach (var property in objectWithValues.GetType().GetProperties())
            {
                localDict.Add(property.Name, property.GetValue(objectWithValues));
            }
            dictionary.Add(dictionary.Count + 1, localDict);
        }

        public event EventHandler<NextResultCalledEventArgs> NextResultCalled;
    }
}