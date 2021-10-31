using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql
{
    public class DatabaseCommandProcessor<T> : IDatabaseCommandProcessor<T>
        where T : class
    {
        private readonly IDbConnection _connection;
        private readonly IDataReaderMapper<T> _mapper;
        private readonly IDatabaseCommandProcessorSettings _settings;
        private readonly IDatabaseCommandEntityProvider<T> _provider;

        public DatabaseCommandProcessor(IDbConnection connection,
                                        IDataReaderMapper<T> mapper,
                                        IDatabaseCommandProcessorSettings settings,
                                        IDatabaseCommandEntityProvider<T> provider)
        {
            _connection = connection;
            _mapper = mapper;
            _settings = settings;
            _provider = provider;
        }

        public int ExecuteNonQuery(IDatabaseCommand command)
            => InvokeCommand(command, cmd => cmd.ExecuteNonQuery());

        public object ExecuteScalar(IDatabaseCommand command)
            => InvokeCommand(command, cmd => cmd.ExecuteScalar());

        public IReadOnlyCollection<T> FindMany(IDatabaseCommand command)
            => Find(cmd => cmd.FindMany(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters).ToList());

        public T? FindOne(IDatabaseCommand command)
            => Find(cmd => cmd.FindOne(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters));

        public IPagedResult<T> FindPaged(IDatabaseCommand dataCommand, IDatabaseCommand recordCountCommand, int offset, int pageSize)
        {
            var returnValue = default(IPagedResult<T>);

            OpenConnection();
            using (var cmd = _connection.CreateCommand())
            {
                using (var countCommand = _connection.CreateCommand())
                {
                    countCommand.FillCommand(recordCountCommand.CommandText, recordCountCommand.CommandType, recordCountCommand.CommandParameters);
                    var totalRecordCount = (int)countCommand.ExecuteScalar();
                    returnValue = new PagedResult<T>
                    (
                        cmd.FindMany(dataCommand.CommandText, dataCommand.CommandType, _mapper.Map, dataCommand.CommandParameters).ToList(),
                        totalRecordCount,
                        offset,
                        pageSize
                    );
                }
            }

            return returnValue;
        }

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

            OpenConnection();
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

        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        private TResult InvokeCommand<TResult>(IDatabaseCommand command, Func<IDbCommand, TResult> actionDelegate)
        {
            OpenConnection();
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

        private TResult Find<TResult>(Func<IDbCommand, TResult> findDelegate)
        {
            var returnValue = default(TResult);

            OpenConnection();
            using (var cmd = _connection.CreateCommand())
            {
                returnValue = findDelegate(cmd);
            }

            return returnValue;
        }

        private static void Nothing()
        {
            // Method intentionally left empty.
        }
    }
}
