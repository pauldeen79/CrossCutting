using System;
using System.Data;

namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommandEntityProvider<T>
    {
        public Func<T, IDatabaseCommand> CommandDelegate { get; }
        public Func<T, T>? ResultEntityDelegate { get; }
        public Func<T, IDataReader, T>? AfterReadDelegate { get; }
    }
}
