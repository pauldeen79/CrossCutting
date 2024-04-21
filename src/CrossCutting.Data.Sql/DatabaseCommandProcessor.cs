namespace CrossCutting.Data.Sql;

public class DatabaseCommandProcessor<TEntity> : DatabaseCommandProcessor<TEntity, TEntity>
    where TEntity : class
{
    public DatabaseCommandProcessor(
        IDbConnection connection,
        IDatabaseCommandEntityProvider<TEntity, TEntity> provider,
        ISqlConnectionWrapperFactory sqlConnectionWrapperFactory,
        ISqlCommandWrapperFactory sqlCommandWrapperFactory)
        : base(connection, provider, sqlConnectionWrapperFactory, sqlCommandWrapperFactory)
    {
    }
}

public class DatabaseCommandProcessor<TEntity, TBuilder> : IDatabaseCommandProcessor<TEntity>
    where TEntity : class
    where TBuilder : class
{
    private readonly IDbConnection _connection;
    private readonly IDatabaseCommandEntityProvider<TEntity, TBuilder> _provider;
    private readonly ISqlConnectionWrapperFactory _sqlConnectionWrapperFactory;
    private readonly ISqlCommandWrapperFactory _sqlCommandWrapperFactory;

    public DatabaseCommandProcessor(
        IDbConnection connection,
        IDatabaseCommandEntityProvider<TEntity, TBuilder> provider,
        ISqlConnectionWrapperFactory sqlConnectionWrapperFactory,
        ISqlCommandWrapperFactory sqlCommandWrapperFactory)
    {
        _connection = connection;
        _provider = provider;
        _sqlConnectionWrapperFactory = sqlConnectionWrapperFactory;
        _sqlCommandWrapperFactory = sqlCommandWrapperFactory;
    }

    public int ExecuteNonQuery(IDatabaseCommand command)
        => InvokeCommand(command, cmd => cmd.ExecuteNonQuery());

    public async Task<int> ExecuteNonQueryAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => await InvokeCommandAsync(_sqlConnectionWrapperFactory.Create(_connection), command, async cmd => await cmd.ExecuteNonQueryAsync(cancellationToken));

    public object ExecuteScalar(IDatabaseCommand command)
        => InvokeCommand(command, cmd => cmd.ExecuteScalar());

    public async Task<object> ExecuteScalarAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => await InvokeCommandAsync(_sqlConnectionWrapperFactory.Create(_connection), command, async cmd => await cmd.ExecuteScalarAsync(cancellationToken));

    public IDatabaseCommandResult<TEntity> ExecuteCommand(IDatabaseCommand command, TEntity instance)
    {
        var resultEntity = CreateResultEntity(command, instance);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);

            if (_provider.AfterReadDelegate is null)
            {
                //Use ExecuteNonQuery
                return ExecuteNonQuery(cmd, resultEntity);
            }
            else
            {
                //Use ExecuteReader
                return ExecuteReader(cmd, command.Operation, _provider.AfterReadDelegate, resultEntity);
            }
        }
    }

    public async Task<IDatabaseCommandResult<TEntity>> ExecuteCommandAsync(IDatabaseCommand command, TEntity instance, CancellationToken cancellationToken)
    {
        var resultEntity = CreateResultEntity(command, instance);

        var sqlConnection = _sqlConnectionWrapperFactory.Create(_connection);
        sqlConnection.OpenIfNecessary();
        using (var cmd = sqlConnection.CreateCommand())
        {
            cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);

            if (_provider.AfterReadDelegate is null)
            {
                //Use ExecuteNonQuery
                return await ExecuteNonQueryAsync(cmd, resultEntity, cancellationToken);
            }
            else
            {
                //Use ExecuteReader
                return await ExecuteReaderAsync(cmd, command.Operation, cancellationToken, _provider.AfterReadDelegate, resultEntity);
            }
        }
    }

    private TBuilder CreateResultEntity(IDatabaseCommand command, TEntity instance)
    {
        TBuilder? builder = default;
        if (instance is TBuilder x)
        {
            builder = x;
        }
        else
        {
            builder = _provider.CreateBuilderDelegate is not null
                ? _provider.CreateBuilderDelegate.Invoke(instance)
                : default;
        }

        if (builder is null)
        {
            throw new InvalidOperationException("Builder instance was not constructed, create builder delegate should deliver an instance");
        }

        var resultEntity = _provider.ResultEntityDelegate is null
            ? builder
            : _provider.ResultEntityDelegate.Invoke(builder, command.Operation);

        if (resultEntity is null)
        {
            throw new InvalidOperationException("Instance should be supplied, or result entity delegate should deliver an instance");
        }

        resultEntity.Validate();
        return resultEntity;
    }

    private TResult InvokeCommand<TResult>(IDatabaseCommand command, Func<IDbCommand, TResult> actionDelegate)
    {
        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);
            return actionDelegate(cmd);
        }
    }

    private IDatabaseCommandResult<TEntity> ExecuteNonQuery(IDbCommand cmd, TBuilder result)
        => new DatabaseCommandResult<TEntity>(cmd.ExecuteNonQuery() != 0, CreateEntityFromBuilder(result));

    private async Task<IDatabaseCommandResult<TEntity>> ExecuteNonQueryAsync(IDbCommand cmd, TBuilder result, CancellationToken cancellationToken)
    {
        var sqlCommand = _sqlCommandWrapperFactory.Create(cmd);
        return new DatabaseCommandResult<TEntity>(await (sqlCommand.ExecuteNonQueryAsync(cancellationToken)) != 0, CreateEntityFromBuilder(result));
    }

    private TEntity CreateEntityFromBuilder(TBuilder result)
    {
        if (_provider.CreateEntityDelegate is not null)
        {
            return _provider.CreateEntityDelegate.Invoke(result);
        }

        if (result is TEntity x)
        {
            return x;
        }

        throw new InvalidOperationException($"Could not cast type [{result.GetType().FullName}] to [{typeof(TEntity).FullName}]");
    }

    private IDatabaseCommandResult<TEntity> ExecuteReader(
        IDbCommand cmd,
        DatabaseOperation operation,
        Func<TBuilder, DatabaseOperation, IDataReader, TBuilder> afterReadDelegate,
        TBuilder resultEntity)
    {
        var success = false;
        using (var reader = cmd.ExecuteReader())
        {
            var result = reader.Read();
            do { Nothing(); } while ((reader.FieldCount == 0 || !result) && reader.NextResult());
            if (result)
            {
                resultEntity = afterReadDelegate(resultEntity, operation, reader);
                success = true;
            }
        }

        return new DatabaseCommandResult<TEntity>(success, CreateEntityFromBuilder(resultEntity));
    }

    private async Task<IDatabaseCommandResult<TEntity>> ExecuteReaderAsync(
        IDbCommand cmd,
        DatabaseOperation operation,
        CancellationToken cancellationToken,
        Func<TBuilder, DatabaseOperation, IDataReader, TBuilder> afterReadDelegate,
        TBuilder resultEntity)
    {
        var success = false;
        var sqlCommand = _sqlCommandWrapperFactory.Create(cmd);
        using (var reader = await sqlCommand.ExecuteReaderAsync(cancellationToken))
        {
            var result = await reader.ReadAsync(cancellationToken);
            do { Nothing(); } while (!cancellationToken.IsCancellationRequested && (reader.FieldCount == 0 || !result) && await reader.NextResultAsync(cancellationToken));
            if (result)
            {
                resultEntity = afterReadDelegate(resultEntity, operation, reader);
                success = true;
            }
        }

        return new DatabaseCommandResult<TEntity>(success, CreateEntityFromBuilder(resultEntity));
    }

    private async Task<TResult> InvokeCommandAsync<TResult>(SqlConnectionWrapper sqlConnection, IDatabaseCommand command, Func<SqlCommandWrapper, Task<TResult>> actionDelegate)
    {
        sqlConnection.OpenIfNecessary();
        using (var cmd = _sqlCommandWrapperFactory.Create(sqlConnection.CreateCommand()))
        {
            cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);
            return await actionDelegate(cmd);
        }
    }

    private static void Nothing()
    {
        // Method intentionally left empty.
    }
}
