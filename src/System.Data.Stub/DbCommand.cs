namespace System.Data.Stub
{
    public sealed class DbCommand : IDbCommand
    {
        public string CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public IDbConnection Connection { get; set; }

        public IDataParameterCollection Parameters { get; } = new DbParameterCollection();

        public IDbTransaction Transaction { get; set; }
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
            return ExecuteNonQueryResult;
        }

        public IDataReader ExecuteReader()
        {
            var result = new DataReader();
            DataReaderCreated?.Invoke(this, new DataReaderCreatedEventArgs(result));
            return result;
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            var result = new DataReader();
            DataReaderCreated?.Invoke(this, new DataReaderCreatedEventArgs(result));
            return result;
        }

        public object ExecuteScalar()
        {
            return ExecuteScalarResult;
        }

        public void Prepare()
        {
            // Method intentionally left empty.
        }

        public int ExecuteNonQueryResult { get; set; }
        public object ExecuteScalarResult { get; set; }
        public event EventHandler<DataReaderCreatedEventArgs> DataReaderCreated;
    }
}