using System.Collections;
using System.Linq;

namespace System.Data.Stub.Extensions
{
    public static class DbConnectionExtensions
    {
        public static DbConnection AddResultForDataReader(this DbConnection instance, IEnumerable data)
            => instance.AddResultForDataReader(null, null, data.OfType<object>().ToArray());

        public static DbConnection AddResultForDataReader(this DbConnection instance, params object[] data)
            => instance.AddResultForDataReader(null, null, data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Action<DataReader> callback, IEnumerable data)
            => instance.AddResultForDataReader(null, callback, data.OfType<object>().ToArray());

        public static DbConnection AddResultForDataReader(this DbConnection instance, Action<DataReader> callback, params object[] data)
            => instance.AddResultForDataReader(null, callback, data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<DbCommand, bool> predicate, IEnumerable data)
            => instance.AddResultForDataReader(predicate, null, data.OfType<object>().ToArray());

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<DbCommand, bool> predicate, params object[] data)
            => instance.AddResultForDataReader(predicate, null, data);

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<DbCommand, bool> predicate, Action<DataReader> callback, IEnumerable data)
            => instance.AddResultForDataReader(predicate, callback, data.OfType<object>().ToArray());

        public static DbConnection AddResultForDataReader(this DbConnection instance, Func<DbCommand, bool> predicate, Action<DataReader> callback, params object[] data)
        {
            instance.DbCommandCreated += (sender, args) =>
            {
                args.DbCommand.DataReaderCreated += (sender2, args2) =>
                {
                    if (predicate == null || predicate((DbCommand)sender2))
                    {
                        foreach (var obj in data)
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
    }
}
