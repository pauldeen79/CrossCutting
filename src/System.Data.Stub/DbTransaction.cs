namespace System.Data.Stub
{
    public sealed class DbTransaction : IDbTransaction
    {
        private readonly DbConnection dbConnection;

        public DbTransaction(DbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IDbConnection Connection => dbConnection;

        public IsolationLevel IsolationLevel { get; set; }

        public void Commit()
        {
            Committed?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            // Method intentionally left empty.
        }

        public void Rollback()
        {
            RolledBack?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Committed;
        public event EventHandler RolledBack;
    }
}