using System;
using System.Data;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql
{
    public class DatabaseCommandProcessor<T> : IDatabaseCommandProcessor<T> where T : class
    {
        private readonly IDbConnection _connection;
        private readonly IDatabaseCommandEntityProvider<T> _provider;

        public DatabaseCommandProcessor(IDbConnection connection, IDatabaseCommandEntityProvider<T> provider)
        {
            _connection = connection;
            _provider = provider;
        }

        public int ExecuteNonQuery(IDatabaseCommand command)
            => InvokeCommand(command, cmd => cmd.ExecuteNonQuery());

        public object ExecuteScalar(IDatabaseCommand command)
            => InvokeCommand(command, cmd => cmd.ExecuteScalar());

        public IDatabaseCommandResult<T> InvokeCommand(T instance)
        {
            var command = _provider.CommandDelegate.Invoke(instance);
            var resultEntity = _provider.ResultEntityDelegate == null
                ? instance
                : _provider.ResultEntityDelegate.Invoke(instance);

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
                    return ExecuteReader(cmd, _provider.AfterReadDelegate, resultEntity);
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

        private IDatabaseCommandResult<T> ExecuteNonQuery(IDbCommand cmd, T result)
            => new DatabaseCommandResult<T>(cmd.ExecuteNonQuery() != 0, result);

        private IDatabaseCommandResult<T> ExecuteReader(IDbCommand cmd,
                                                        Func<T, IDataReader, T> afterReadDelegate,
                                                        T resultEntity)
        {
            var success = false;
            using (var reader = cmd.ExecuteReader())
            {
                var result = reader.Read();
                do { Nothing(); } while ((reader.FieldCount == 0 || !result) && reader.NextResult());
                if (result)
                {
                    resultEntity = afterReadDelegate(resultEntity, reader);
                    success = true;
                }
            }

            return new DatabaseCommandResult<T>(success, resultEntity);
        }

        private static void Nothing()
        {
            // Method intentionally left empty.
        }
    }
}
