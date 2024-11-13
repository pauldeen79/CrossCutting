namespace System.Data.Stub;

public class DbTransactionCreatedEventArgs(DbTransaction dbTransaction) : EventArgs
{
    public DbTransaction DbTransaction { get; } = dbTransaction;
}
