using System;

namespace System.Data.Stub
{
    public class DataReaderCreatedEventArgs : EventArgs
    {
        public DataReader DataReader { get; }

        public DataReaderCreatedEventArgs(DataReader dataReader)
        {
            DataReader = dataReader;
        }
    }
}