using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace System.Data.Stub
{
    public sealed class DbParameterCollection : IDataParameterCollection
    {
        private readonly IDictionary<string, object> dictionary = new Dictionary<string, object>();

        public object this[string parameterName] { get => dictionary[parameterName]; set => dictionary[parameterName] = value; }

        public object this[int index] { get => dictionary[dictionary.Keys.ElementAt(index)]; set => dictionary[dictionary.Keys.ElementAt(index)] = value; }

        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        public int Count => dictionary.Count;

        public bool IsSynchronized => true;

        public object SyncRoot => this;

        public int Add(object value)
        {
            dictionary.Add("_" + (dictionary.Count + 1), value);
            return dictionary.Count;
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(string parameterName)
        {
            return dictionary.ContainsKey(parameterName);
        }

        public bool Contains(object value)
        {
            return dictionary.Values.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return dictionary.Select(kvp => (IDbDataParameter)kvp.Value).GetEnumerator();
        }

        public int IndexOf(string parameterName)
        {
            return dictionary.Keys.ToList().IndexOf(parameterName);
        }

        public int IndexOf(object value)
        {
            return dictionary.Values.ToList().IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            dictionary.Add("_" + index, value);
        }

        public void Remove(object value)
        {
            foreach (var kvp in dictionary.ToArray())
            {
                if (kvp.Value.Equals(value))
                {
                    dictionary.Remove(kvp.Key);
                    break;
                }
            }
        }

        public void RemoveAt(string parameterName)
        {
            dictionary.Remove(parameterName);
        }

        public void RemoveAt(int index)
        {
            var key = dictionary.Keys.ElementAt(index);
            dictionary.Remove(key);
        }
    }
}