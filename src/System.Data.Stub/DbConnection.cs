namespace System.Data.Stub
{
    public sealed class DbConnection : IDbConnection
    {
        public string ConnectionString { get; set; }

        public int ConnectionTimeout => 1;

        public string Database => "System.Data.Stub.DbConnection";

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
            // Method intentionally left empty.
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
            // Method intentionally left empty.
        }

        public void Open()
        {
            State = ConnectionState.Open;
        }

        public event EventHandler<DbCommandCreatedEventArgs> DbCommandCreated;
        public event EventHandler<DbTransactionCreatedEventArgs> DbTransactionCreated;
    }
}
