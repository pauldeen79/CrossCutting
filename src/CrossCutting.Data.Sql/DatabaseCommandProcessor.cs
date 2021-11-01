using System;
using System.Data;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql
{
    public class DatabaseCommandProcessor<T> : IDatabaseCommandProcessor<T> where T : class
    {
        private readonly IDbConnection _connection;
        private readonly IDatabaseCommandEntityProvider<T> _provider;
        private readonly IDatabaseCommandProcessorSettings _settings;

        public DatabaseCommandProcessor(IDbConnection connection,
                                        IDatabaseCommandEntityProvider<T> provider,
                                        IDatabaseCommandProcessorSettings settings)
        {
            _connection = connection;
            _provider = provider;
            _settings = settings;
        }

        public int ExecuteNonQuery(IDatabaseCommand command)
            => InvokeCommand(command, cmd => cmd.ExecuteNonQuery());

        public object ExecuteScalar(IDatabaseCommand command)
            => InvokeCommand(command, cmd => cmd.ExecuteScalar());

        public T InvokeCommand(T instance)
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
                    ExecuteNonQuery(cmd, _settings.ExceptionMessage);
                }
                else
                {
                    //Use ExecuteReader
                    resultEntity = ExecuteReader(cmd, _settings.ExceptionMessage, _provider.AfterReadDelegate, resultEntity);
                }
            }

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

        private T ExecuteReader(IDbCommand cmd,
                                string? exceptionMessage,
                                Func<T, IDataReader, T> afterReadDelegate,
                                T resultEntity)
        {
            using (var reader = cmd.ExecuteReader())
            {
                var result = reader.Read();
                do { Nothing(); } while ((reader.FieldCount == 0 || !result) && reader.NextResult());
                if (result)
                {
                    resultEntity = afterReadDelegate(resultEntity, reader);
                }
                else if (!string.IsNullOrEmpty(exceptionMessage))
                {
                    throw new DataException(exceptionMessage);
                }
            }

            return resultEntity;
        }

        private static void ExecuteNonQuery(IDbCommand cmd, string? exceptionMessage)
        {
            if (cmd.ExecuteNonQuery() == 0 && !string.IsNullOrEmpty(exceptionMessage))
            {
                throw new DataException(exceptionMessage);
            }
        }

        private static void Nothing()
        {
            // Method intentionally left empty.
        }
    }
}
