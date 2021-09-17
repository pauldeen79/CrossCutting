using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Sql.Extensions
{
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Executes the scalar command.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="command">The sql command.</param>
        /// <param name="closeConnection">if set to <c>true</c> [close connection].</param>
        /// <returns>
        /// Result of ExecuteScalar method.
        /// </returns>
        public static object ExecuteScalar(this IDbConnection connection,
                                           IDatabaseCommand command,
                                           bool closeConnection = false)
            => connection.InvokeCommand(command, closeConnection, cmd => cmd.ExecuteScalar());

        /// <summary>
        /// Executes the non query command.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="command">The sql command.</param>
        /// <param name="closeConnection">if set to <c>true</c> [close connection].</param>
        /// <returns>
        /// Number of rows affected.
        /// </returns>
        public static int ExecuteNonQuery(this IDbConnection connection,
                                          IDatabaseCommand command,
                                          bool closeConnection = false)
            => connection.InvokeCommand(command, closeConnection, cmd => cmd.ExecuteNonQuery());

        /// <summary>Finds one entity on this connection.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="command">The sql command.</param>
        /// <param name="mapFunction">The map function.</param>
        /// <param name="finalizeDelegate">The finalize delegate.</param>
        public static T FindOne<T>(this IDbConnection connection,
                                   IDatabaseCommand command,
                                   Func<IDataReader, T> mapFunction,
                                   Func<T?, Exception?, T?>? finalizeDelegate = null) where T : class
            => connection.Find(command,
                               cmd => cmd.FindOne(command.CommandText, command.CommandType, mapFunction, command.CommandParameters),
                               finalizeDelegate);

        /// <summary>Finds multiple entities on this connection.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="command">The sql command.</param>
        /// <param name="mapFunction">The map function.</param>
        /// <param name="finalizeDelegate">The finalize delegate.</param>
        public static IReadOnlyCollection<T> FindMany<T>(this IDbConnection connection,
                                                         IDatabaseCommand command,
                                                         Func<IDataReader, T> mapFunction,
                                                         Func<IReadOnlyCollection<T>?, Exception?, IReadOnlyCollection<T>?>? finalizeDelegate = null)
            => connection.Find(command,
                               cmd => cmd.FindMany(command.CommandText, command.CommandType, mapFunction, command.CommandParameters).ToList(),
                               finalizeDelegate);

        /// <summary>
        /// Invokes the command on the connection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection</param>
        /// <param name="instance">Input and output entity</param>
        /// <param name="commandDelegate">The command delegat</param>
        /// <param name="exceptionMessage">Optional exception message (applied to result of command execution)</param>
        /// <param name="resultEntityDelegate">Optional pre processing delegate</param>
        /// <param name="afterReadDelegate">Optional post processing delegate</param>
        /// <param name="finalizeDelegate">Optional finalization delegate</param>
        /// <returns></returns>
        public static T InvokeCommand<T>(this IDbConnection connection,
                                         T instance,
                                         Func<T, IDatabaseCommand> commandDelegate,
                                         string? exceptionMessage = null,
                                         Func<T, T>? resultEntityDelegate = null,
                                         Func<T, IDataReader, T>? afterReadDelegate = null,
                                         Func<T, Exception?, T>? finalizeDelegate = null)
        {
            if (commandDelegate == null)
            {
                throw new ArgumentNullException(nameof(commandDelegate));
            }

            var command = commandDelegate.Invoke(instance);
            if (command == null)
            {
                throw new ArgumentException("CommandDelegate result was null", nameof(commandDelegate));
            }

            var resultEntity = resultEntityDelegate == null
                ? instance
                : resultEntityDelegate(instance);

            if (resultEntity == null)
            {
                throw new ArgumentException("Instance should be supplied, or result entity delegate should deliver an instance");
            }

            resultEntity.Validate();

            try
            {
                connection.OpenIfNecessary();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);

                    if (afterReadDelegate == null)
                    {
                        //Use ExecuteNonQuery
                        ExecuteNonQuery(cmd, exceptionMessage);
                    }
                    else
                    {
                        //Use ExecuteReader
                        resultEntity = ExecuteReader(cmd, exceptionMessage, afterReadDelegate, resultEntity);
                    }
                }

                return finalizeDelegate == null
                    ? resultEntity
                    : finalizeDelegate.Invoke(resultEntity, null);
            }
            catch (Exception exception)
            {
                finalizeDelegate?.Invoke(resultEntity, exception);
                throw;
            }
        }

        private static void ExecuteNonQuery(IDbCommand cmd, string? exceptionMessage)
        {
            if (cmd.ExecuteNonQuery() == 0 && !string.IsNullOrEmpty(exceptionMessage))
            {
                throw new DataException(exceptionMessage);
            }
        }

        private static T ExecuteReader<T>(IDbCommand cmd,
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

        private static T InvokeCommand<T>(this IDbConnection connection,
                                          IDatabaseCommand command,
                                          bool closeConnection,
                                          Func<IDbCommand, T> actionDelegate)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            try
            {
                connection.OpenIfNecessary();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.FillCommand(command.CommandText, command.CommandType, command.CommandParameters);
                    return actionDelegate(cmd);
                }
            }
            finally
            {
                CloseConnectionIfNecessary(connection, closeConnection);
            }
        }

        private static TResult Find<TResult>(this IDbConnection connection,
                                             IDatabaseCommand command,
                                             Func<IDbCommand, TResult> findDelegate,
                                             Func<TResult?, Exception?, TResult?>? finalizeDelegate = null)
            where TResult : class
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var returnValue = default(TResult);

            try
            {
                connection.OpenIfNecessary();
                using (var cmd = connection.CreateCommand())
                {
                    returnValue = findDelegate(cmd);
                }

#pragma warning disable CS8603 // Possible null reference return.
                return finalizeDelegate != null
                    ? finalizeDelegate.Invoke(returnValue, null)
                    : returnValue;
#pragma warning restore CS8603 // Possible null reference return.
            }
            catch (Exception exception)
            {
                finalizeDelegate?.Invoke(returnValue, exception);
                throw;
            }
        }

        public static void OpenIfNecessary(this IDbConnection connection)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        private static void CloseConnectionIfNecessary(IDbConnection connection, bool closeConnection)
        {
            if (!closeConnection)
            {
                return;
            }

            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Dispose();
        }

        private static void Nothing()
        {
            // Method intentionally left empty.
        }
    }
}
