namespace System.Data.Stub;

public sealed class DbConnection : Common.DbConnection
{
    private string _database = "System.Data.Stub.DbConnection";
    private ConnectionState _state;

    public override string ConnectionString { get; set; } = string.Empty;

    public override int ConnectionTimeout => 1;

    public override string Database => _database;

    public override ConnectionState State => _state;

    public override string DataSource => "System.Data.Stub.DbConnection";

    public override string ServerVersion => "3.0.0";

    protected override Common.DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
        var result = new DbTransaction(this);
        result.SetIsolationLevel(isolationLevel);
        DbTransactionCreated?.Invoke(this, new DbTransactionCreatedEventArgs(result));
        return result;
    }

    public override void ChangeDatabase(string databaseName) => _database = databaseName;

    public override void Close() => _state = ConnectionState.Closed;

    public override void Open() => _state = ConnectionState.Open;

    protected override Common.DbCommand CreateDbCommand()
    {
        var result = new DbCommand();
        DbCommandCreated?.Invoke(this, new DbCommandCreatedEventArgs(result));
        return result;
    }

    public event EventHandler<DbCommandCreatedEventArgs>? DbCommandCreated;
    public event EventHandler<DbTransactionCreatedEventArgs>? DbTransactionCreated;
}
