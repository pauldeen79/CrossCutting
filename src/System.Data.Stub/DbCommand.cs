namespace System.Data.Stub;

public sealed class DbCommand : Common.DbCommand
{
    public override string CommandText { get; set; } = string.Empty;
    public override int CommandTimeout { get; set; }
    public override CommandType CommandType { get; set; }
    public override UpdateRowSource UpdatedRowSource { get; set; }
    public override bool DesignTimeVisible { get; set; }

    protected override Common.DbConnection DbConnection { get; set; } = default!;
    protected override Common.DbTransaction DbTransaction { get; set; } = default!;
    protected override Common.DbParameterCollection DbParameterCollection { get; } = new DbParameterCollection();
    protected override Common.DbParameter CreateDbParameter() => new DbDataParameter();
    protected override Common.DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
        var result = new DataReader(behavior);
        DataReaderCreated?.Invoke(this, new DataReaderCreatedEventArgs(result));
        return result;
    }
    protected override Task<Common.DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
        => Task.FromResult(ExecuteDbDataReader(behavior));

    public override void Cancel()
    {
        // Method intentionally left empty.
    }

    public override int ExecuteNonQuery()
    {
        if (ExecuteNonQueryResultDelegate is not null
            && (ExecuteNonQueryResultPredicate is null || ExecuteNonQueryResultPredicate(this)))
        {
            return ExecuteNonQueryResultDelegate(this);
        }
        return ExecuteNonQueryResult;
    }

    public override object? ExecuteScalar()
    {
        if (ExecuteScalarResultDelegate is not null
            && (ExecuteScalarResultPredicate is null || ExecuteScalarResultPredicate(this)))
        {
            return ExecuteScalarResultDelegate(this);
        }
        return ExecuteScalarResult;
    }

    public override void Prepare()
    {
        // Method intentionally left empty.
    }

    public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        => Task.FromResult(ExecuteNonQuery());

    public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
        => Task.FromResult(ExecuteScalar()!);

    public Func<DbCommand, bool>? ExecuteNonQueryResultPredicate { get; set; }
    public Func<DbCommand, int>? ExecuteNonQueryResultDelegate { get; set; }
    public int ExecuteNonQueryResult { get; set; }
    public Func<DbCommand, bool>? ExecuteScalarResultPredicate { get; set; }
    public Func<DbCommand, object>? ExecuteScalarResultDelegate { get; set; }
    public object? ExecuteScalarResult { get; set; }
    public event EventHandler<DataReaderCreatedEventArgs>? DataReaderCreated;
}
