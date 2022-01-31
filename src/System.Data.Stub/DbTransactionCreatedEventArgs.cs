namespace System.Data.Stub;

public class DbTransactionCreatedEventArgs : EventArgs
{
    public DbTransaction DbTransaction { get; }

    public DbTransactionCreatedEventArgs(DbTransaction dbTransaction) => DbTransaction = dbTransaction;
}
