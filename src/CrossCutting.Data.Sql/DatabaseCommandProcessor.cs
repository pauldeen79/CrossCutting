using System;
using System.Data;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql
{
    public class DatabaseCommandProcessor<TEntity> : DatabaseCommandProcessor<TEntity, TEntity>
        where TEntity : class
    {
        public DatabaseCommandProcessor(IDbConnection connection, IDatabaseCommandEntityProvider<TEntity, TEntity> provider)
            : base(connection, provider)
        {
        }
    }

    public class DatabaseCommandProcessor<TEntity, TBuilder> : IDatabaseCommandProcessor<TEntity>
        where TEntity : class
        where TBuilder : class
    {
        private readonly IDbConnection _connection;
        private readonly IDatabaseCommandEntityProvider<TEntity, TBuilder> _provider;

        public DatabaseCommandProcessor(IDbConnection connection, IDatabaseCommandEntityProvider<TEntity, TBuilder> provider)
        {
            _connection = connection;
            _provider = provider;
        }

        public int ExecuteNonQuery(IDatabaseCommand command)
            => InvokeCommand(command, cmd => cmd.ExecuteNonQuery());

        public object ExecuteScalar(IDatabaseCommand command)
            => InvokeCommand(command, cmd => cmd.ExecuteScalar());

        public IDatabaseCommandResult<TEntity> ExecuteCommand(IDatabaseCommand command, TEntity instance)
        {
            TBuilder? builder = default;
            if (instance is TBuilder x)
            {
                builder = x;
            }
            else
            {
                builder = _provider.CreateBuilderDelegate != null
                    ? _provider.CreateBuilderDelegate.Invoke(instance)
                    : default;
            }

            if (builder == null)
            {
                throw new InvalidOperationException("Builder instance was not constructed, create builder delegate should deliver an instance");
            }

            var resultEntity = _provider.ResultEntityDelegate == null
                ? builder
                : _provider.ResultEntityDelegate.Invoke(builder, command.Operation);

            if (resultEntity == null)
            {
                throw new InvalidOperationException("Instance should be supplied, or result entity delegate should deliver an instance");
            }

            resultEntity.Validate();

            _connection.OpenIfNecessary();
            using (var cmd = _connection.CreateCommand())
            {
                cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);

                if (_provider.AfterReadDelegate == null)
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

        private TEntity CreateEntityFromBuilder(TBuilder result)
        {
            if (_provider.CreateEntityDelegate != null)
            {
                return _provider.CreateEntityDelegate.Invoke(result);
            }

            if (result is TEntity x)
            {
                return x;
            }

            throw new InvalidOperationException($"Could not cast type [{result.GetType().FullName}] to [{typeof(TEntity).FullName}]");
        }

        private IDatabaseCommandResult<TEntity> ExecuteReader(IDbCommand cmd,
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

        private static void Nothing()
        {
            // Method intentionally left empty.
        }
    }
}
