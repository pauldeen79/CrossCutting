namespace System.Data.Stub;

public sealed class DbCommand : IDbCommand
{
    public string CommandText { get; set; } = string.Empty;
    public int CommandTimeout { get; set; }
    public CommandType CommandType { get; set; }
    public IDbConnection? Connection { get; set; }

    public IDataParameterCollection Parameters { get; } = new DbParameterCollection();

    public IDbTransaction? Transaction { get; set; }
    public UpdateRowSource UpdatedRowSource { get; set; }

    public void Cancel()
    {
        // Method intentionally left empty.
    }

    public IDbDataParameter CreateParameter()
    {
        return new DbDataParameter();
    }

    public void Dispose()
    {
        // Method intentionally left empty.
    }

    public int ExecuteNonQuery()
    {
        if (ExecuteNonQueryResultDelegate is not null
            && (ExecuteNonQueryResultPredicate is null || ExecuteNonQueryResultPredicate(this)))
        {
            return ExecuteNonQueryResultDelegate(this);
        }
        return ExecuteNonQueryResult;
    }

    public IDataReader ExecuteReader()
    {
        var result = new DataReader(CommandBehavior.Default);
        DataReaderCreated?.Invoke(this, new DataReaderCreatedEventArgs(result));
        return result;
    }

    public IDataReader ExecuteReader(CommandBehavior behavior)
    {
        var result = new DataReader(behavior);
        DataReaderCreated?.Invoke(this, new DataReaderCreatedEventArgs(result));
        return result;
    }

    public object? ExecuteScalar()
    {
        if (ExecuteScalarResultDelegate is not null
            && (ExecuteScalarResultPredicate is null || ExecuteScalarResultPredicate(this)))
        {
            return ExecuteScalarResultDelegate(this);
        }
        return ExecuteScalarResult;
    }

    public void Prepare()
    {
        // Method intentionally left empty.
    }

    public Func<DbCommand, bool>? ExecuteNonQueryResultPredicate { get; set; }
    public Func<DbCommand, int>? ExecuteNonQueryResultDelegate { get; set; }
    public int ExecuteNonQueryResult { get; set; }
    public Func<DbCommand, bool>? ExecuteScalarResultPredicate { get; set; }
    public Func<DbCommand, object>? ExecuteScalarResultDelegate { get; set; }
    public object? ExecuteScalarResult { get; set; }
    public event EventHandler<DataReaderCreatedEventArgs>? DataReaderCreated;
}
