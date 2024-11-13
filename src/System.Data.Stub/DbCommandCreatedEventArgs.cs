namespace System.Data.Stub;

public class DbCommandCreatedEventArgs(DbCommand dbCommand) : EventArgs
{
    public DbCommand DbCommand { get; } = dbCommand;
}
