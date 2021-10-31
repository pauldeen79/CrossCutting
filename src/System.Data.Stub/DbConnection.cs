namespace System.Data.Stub
{
    public sealed class DbConnection : IDbConnection
    {
        public string ConnectionString { get; set; } = string.Empty;

        public int ConnectionTimeout => 1;

        public string Database { get; private set; } = "System.Data.Stub.DbConnection";

        public ConnectionState State { get; set; }

        public IDbTransaction BeginTransaction()
        {
            var result = new DbTransaction(this);
            DbTransactionCreated?.Invoke(this, new DbTransactionCreatedEventArgs(result));
            return result;
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            var result = new DbTransaction(this) { IsolationLevel = il };
            DbTransactionCreated?.Invoke(this, new DbTransactionCreatedEventArgs(result));
            return result;
        }

        public void ChangeDatabase(string databaseName)
        {
            Database = databaseName;
        }

        public void Close()
        {
            State = ConnectionState.Closed;
        }

        public IDbCommand CreateCommand()
        {
            var result = new DbCommand();
            DbCommandCreated?.Invoke(this, new DbCommandCreatedEventArgs(result));
            return result;
        }

        public void Dispose()
        {
            Close();
        }

        public void Open()
        {
            State = ConnectionState.Open;
        }

        public event EventHandler<DbCommandCreatedEventArgs>? DbCommandCreated;
        public event EventHandler<DbTransactionCreatedEventArgs>? DbTransactionCreated;
    }
}
