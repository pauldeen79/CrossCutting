using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql
{
    public class DatabaseEntityRetriever<T> : IDatabaseEntityRetriever<T>
        where T : class
    {
        private readonly IDbConnection _connection;
        private readonly IDataReaderMapper<T> _mapper;

        public DatabaseEntityRetriever(IDbConnection connection, IDataReaderMapper<T> mapper)
        {
            _connection = connection;
            _mapper = mapper;
        }

        public T? FindOne(IDatabaseCommand command)
            => Find(cmd => cmd.FindOne(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters));

        public IReadOnlyCollection<T> FindMany(IDatabaseCommand command)
            => Find(cmd => cmd.FindMany(command.CommandText, command.CommandType, _mapper.Map, command.CommandParameters).ToList());

        public IPagedResult<T> FindPaged(IDatabaseCommand dataCommand, IDatabaseCommand recordCountCommand, int offset, int pageSize)
        {
            var returnValue = default(IPagedResult<T>);

            _connection.OpenIfNecessary();
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

        private TResult Find<TResult>(Func<IDbCommand, TResult> findDelegate)
        {
            var returnValue = default(TResult);

            _connection.OpenIfNecessary();
            using (var cmd = _connection.CreateCommand())
            {
                returnValue = findDelegate(cmd);
            }

            return returnValue;
        }
    }
}
