using System;

namespace System.Data.Stub
{
    public class DbCommandCreatedEventArgs : EventArgs
    {
        public DbCommand DbCommand { get; }

        public DbCommandCreatedEventArgs(DbCommand dbCommand)
        {
            DbCommand = dbCommand;
        }
    }
}
