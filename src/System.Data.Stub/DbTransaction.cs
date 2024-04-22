namespace System.Data.Stub;

public sealed class DbTransaction : Common.DbTransaction
{
    private readonly DbConnection _dbConnection;
    private IsolationLevel _isolationLevel = IsolationLevel.Unspecified;

    public DbTransaction(DbConnection dbConnection) => _dbConnection = dbConnection;

    public override IsolationLevel IsolationLevel => _isolationLevel;

    protected override Common.DbConnection DbConnection => _dbConnection;

    public override void Commit() => Committed?.Invoke(this, EventArgs.Empty);

    public override void Rollback() => RolledBack?.Invoke(this, EventArgs.Empty);

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            RolledBack?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? Committed;
    public event EventHandler? RolledBack;

    public void SetIsolationLevel(IsolationLevel isolationLevel)
    {
        _isolationLevel = isolationLevel;
    }
}
