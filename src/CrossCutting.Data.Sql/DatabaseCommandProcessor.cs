namespace CrossCutting.Data.Sql;

public class DatabaseCommandProcessor<TEntity> : DatabaseCommandProcessor<TEntity, TEntity>
    where TEntity : class
{
    public DatabaseCommandProcessor(
        DbConnection connection,
        IDatabaseCommandEntityProvider<TEntity, TEntity> provider)
        : base(connection, provider)
    {
    }
}

public class DatabaseCommandProcessor<TEntity, TBuilder> : IDatabaseCommandProcessor<TEntity>
    where TEntity : class
    where TBuilder : class
{
    private readonly DbConnection _connection;
    private readonly IDatabaseCommandEntityProvider<TEntity, TBuilder> _provider;

    public DatabaseCommandProcessor(
        DbConnection connection,
        IDatabaseCommandEntityProvider<TEntity, TBuilder> provider)
    {
        _connection = connection;
        _provider = provider;
    }

    public int ExecuteNonQuery(IDatabaseCommand command)
        => InvokeCommand(command, cmd => cmd.ExecuteNonQuery());

    public async Task<int> ExecuteNonQueryAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => await InvokeCommandAsync(command, async cmd => await cmd.ExecuteNonQueryAsync(cancellationToken));

    public object ExecuteScalar(IDatabaseCommand command)
        => InvokeCommand(command, cmd => cmd.ExecuteScalar());

    public async Task<object?> ExecuteScalarAsync(IDatabaseCommand command, CancellationToken cancellationToken)
        => await InvokeCommandAsync(command, async cmd => await cmd.ExecuteScalarAsync(cancellationToken));

    public IDatabaseCommandResult<TEntity> ExecuteCommand(IDatabaseCommand command, TEntity instance)
    {
        var resultEntity = CreateResultEntity(command, instance);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);

            if (_provider.AfterRead is null)
            {
                //Use ExecuteNonQuery
                return ExecuteNonQuery(cmd, resultEntity);
            }
            else
            {
                //Use ExecuteReader
                return ExecuteReader(cmd, command.Operation, _provider.AfterRead, resultEntity);
            }
        }
    }

    public async Task<IDatabaseCommandResult<TEntity>> ExecuteCommandAsync(IDatabaseCommand command, TEntity instance, CancellationToken cancellationToken)
    {
        var resultEntity = CreateResultEntity(command, instance);

        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
        {
            cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);

            if (_provider.AfterRead is null)
            {
                //Use ExecuteNonQuery
                return await ExecuteNonQueryAsync(cmd, resultEntity, cancellationToken);
            }
            else
            {
                //Use ExecuteReader
                return await ExecuteReaderAsync(cmd, command.Operation, cancellationToken, _provider.AfterRead, resultEntity);
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
            builder = _provider.CreateBuilder is not null
                ? _provider.CreateBuilder.Invoke(instance)
                : default;
        }

        if (builder is null)
        {
            throw new InvalidOperationException("Builder instance was not constructed, create builder delegate should deliver an instance");
        }

        var resultEntity = _provider.CreateResultEntity is null
            ? builder
            : _provider.CreateResultEntity.Invoke(builder, command.Operation);

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

    private async Task<IDatabaseCommandResult<TEntity>> ExecuteNonQueryAsync(DbCommand cmd, TBuilder result, CancellationToken cancellationToken)
        => new DatabaseCommandResult<TEntity>(await cmd.ExecuteNonQueryAsync(cancellationToken) != 0, CreateEntityFromBuilder(result));

    private TEntity CreateEntityFromBuilder(TBuilder result)
    {
        if (_provider.CreateEntity is not null)
        {
            return _provider.CreateEntity.Invoke(result);
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
        AfterReadHandler<TBuilder> afterReadDelegate,
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
        DbCommand cmd,
        DatabaseOperation operation,
        CancellationToken cancellationToken,
        AfterReadHandler<TBuilder> afterReadDelegate,
        TBuilder resultEntity)
    {
        var success = false;
        using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken))
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

    private async Task<TResult> InvokeCommandAsync<TResult>(IDatabaseCommand command, Func<DbCommand, Task<TResult>> actionDelegate)
    {
        _connection.OpenIfNecessary();
        using (var cmd = _connection.CreateCommand())
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
