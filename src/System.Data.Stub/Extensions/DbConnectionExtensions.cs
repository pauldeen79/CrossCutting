using System.Collections;

namespace System.Data.Stub.Extensions
{
    public static class DbConnectionExtensions
    {
        public static DbConnection AddResultForDataReader(this DbConnection instance, IEnumerable data)
            => instance.AddResultForDataReader(null, null, () => data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<IEnumerable> data)
            => instance.AddResultForDataReader(null, null, data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Action<DataReader>? callback, IEnumerable data)
            => instance.AddResultForDataReader(null, callback, () => data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Action<DataReader>? callback, Func<IEnumerable> data)
            => instance.AddResultForDataReader(null, callback, data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<DbCommand, bool>? predicate, IEnumerable data)
            => instance.AddResultForDataReader(predicate, null, () => data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<DbCommand, bool>? predicate, Func<IEnumerable> data)
            => instance.AddResultForDataReader(predicate, null, data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<DbCommand, bool>? predicate, Action<DataReader>? callback, IEnumerable data)
            => instance.AddResultForDataReader(predicate, callback, () => data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<DbCommand, bool>? predicate, Action<DataReader>? callback, Func<IEnumerable> data)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.DataReaderCreated += (sender2, args2) =>
                {
                    if (predicate == null || predicate((DbCommand)sender2))
                    {
                        foreach (var obj in data())
                        {
                            args2.DataReader.Add(obj);
                        }
                    }

                    callback?.Invoke(args2.DataReader);
                };
            };

            return instance;
        }

        public static DbConnection AddCallback(this DbConnection instance, DbConnectionCallback callback)
        {
            instance.DbCommandCreated += (sender, args) => callback.AddCommand(args.DbCommand);
            instance.DbTransactionCreated += (sender, args) => callback.AddTransaction(args.DbTransaction);
            return instance;
        }

        public static DbConnection AddResultForNonQueryCommand(this DbConnection instance, int result)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResult = result;
            };
            return instance;
        }

        public static DbConnection AddResultForNonQueryCommand(this DbConnection instance, Func<DbCommand, bool> predicate, int result)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResultPredicate = predicate;
                args.DbCommand.ExecuteNonQueryResultDelegate = _ => result;
            };
            return instance;
        }

        public static DbConnection AddResultForNonQueryCommand(this DbConnection instance, Func<DbCommand, int> resultDelegate)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResultDelegate = resultDelegate;
            };
            return instance;
        }

        public static DbConnection AddResultForNonQueryCommand(this DbConnection instance, Func<DbCommand, bool> predicate, Func<DbCommand, int> resultDelegate)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteNonQueryResultPredicate = predicate;
                args.DbCommand.ExecuteNonQueryResultDelegate = resultDelegate;
            };
            return instance;
        }

        public static DbConnection AddResultForScalarCommand(this DbConnection instance, int result)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteScalarResult = result;
            };
            return instance;
        }

        public static DbConnection AddResultForScalarCommand(this DbConnection instance, Func<DbCommand, bool> predicate, int result)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteScalarResultPredicate = predicate;
                args.DbCommand.ExecuteScalarResultDelegate = _ => result;
            };
            return instance;
        }

        public static DbConnection AddResultForScalarCommand(this DbConnection instance, Func<DbCommand, object> resultDelegate)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteScalarResultDelegate = resultDelegate;
            };
            return instance;
        }

        public static DbConnection AddResultForScalarCommand(this DbConnection instance, Func<DbCommand, bool> predicate, Func<DbCommand, object> resultDelegate)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.ExecuteScalarResultPredicate = predicate;
                args.DbCommand.ExecuteScalarResultDelegate = resultDelegate;
            };
            return instance;
        }
    }
}
