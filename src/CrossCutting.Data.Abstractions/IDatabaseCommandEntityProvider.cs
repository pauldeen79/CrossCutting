using System;
using System.Data;

namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommandEntityProvider<T>
    {
        public Func<T, DatabaseOperation, IDatabaseCommand> CommandDelegate { get; }
        public Func<T, DatabaseOperation, T>? ResultEntityDelegate { get; }
        public Func<T, DatabaseOperation, IDataReader, T>? AfterReadDelegate { get; }
    }
}
