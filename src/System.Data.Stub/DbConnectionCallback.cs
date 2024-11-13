namespace System.Data.Stub;

public class DbConnectionCallback
{
    private readonly Collection<DbCommand> _commands = [];
    private readonly Collection<DbTransaction> _transactions = [];

    public IEnumerable<DbCommand> Commands => _commands;
    public IEnumerable<DbTransaction> Transactions => _transactions;

    internal void AddCommand(DbCommand command) => _commands.Add(command);

    internal void AddTransaction(DbTransaction transaction) => _transactions.Add(transaction);
}
