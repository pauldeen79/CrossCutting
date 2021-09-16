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
                    return cmd.ExecuteScalar();
                }
            }
            finally
            {
                CloseConnectionIfNecessary(connection, closeConnection);
            }
        }

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
                    return cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                CloseConnectionIfNecessary(connection, closeConnection);
            }
        }

        /// <summary>Adds the specified entity to this connection.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="isAddDelegate">The is add delegate.</param>
        /// <param name="commandDelegate">The command delegate.</param>
        /// <param name="resultEntityDelegate">The result entity delegate.</param>
        /// <param name="afterReadDelegate">The after read delegate.</param>
        /// <param name="finalizeDelegate">The finalize delegate.</param>
        public static T Add<T>(this IDbConnection connection,
                               T instance,
                               Func<T, bool> isAddDelegate,
                               Func<T, IDatabaseCommand> commandDelegate,
                               Func<T, T>? resultEntityDelegate = null,
                               Func<T, IDataReader, T>? afterReadDelegate = null,
                               Func<T, Exception?, T>? finalizeDelegate = null)
        {
            if (commandDelegate == null)
            {
                throw new ArgumentNullException(nameof(commandDelegate));
            }

            if (isAddDelegate == null)
            {
                throw new ArgumentNullException(nameof(isAddDelegate));
            }

            var command = commandDelegate.Invoke(instance);
            return connection.ExecuteCommand(instance,
                                             command,
                                             i => isAddDelegate(i),
                                             typeof(T).Name + " entity was not added",
                                             resultEntityDelegate,
                                             typeof(T).Name + " entity cannot be added, because it's an existing item",
                                             afterReadDelegate,
                                             finalizeDelegate);
        }

        /// <summary>Updates the specified entity to this connection.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="isAddDelegate">The is add delegate.</param>
        /// <param name="resultEntityDelegate">The result entity delegate.</param>
        /// <param name="commandDelegate">The command delegate.</param>
        /// <param name="afterReadDelegate">The after read delegate.</param>
        /// <param name="finalizeDelegate">The finalize delegate.</param>
        public static T Update<T>(this IDbConnection connection,
                                  T instance,
                                  Func<T, bool> isAddDelegate,
                                  Func<T, IDatabaseCommand> commandDelegate,
                                  Func<T, T>? resultEntityDelegate = null,
                                  Func<T, IDataReader, T>? afterReadDelegate = null,
                                  Func<T, Exception?, T>? finalizeDelegate = null)
        {
            if (commandDelegate == null)
            {
                throw new ArgumentNullException(nameof(commandDelegate));
            }

            if (isAddDelegate == null)
            {
                throw new ArgumentNullException(nameof(isAddDelegate));
            }
            
            var command = commandDelegate.Invoke(instance);
            return connection.ExecuteCommand(instance,
                                             command,
                                             i => !isAddDelegate(i),
                                             typeof(T).Name + " entity was not updated",
                                             resultEntityDelegate,
                                             typeof(T).Name + " entity cannot be updated, because it's a new item",
                                             afterReadDelegate,
                                             finalizeDelegate);
        }

        /// <summary>Deletes the specified entity from this connection.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="isAddDelegate">The is add delegate.</param>
        /// <param name="commandDelegate">The command delegate.</param>
        /// <param name="resultEntityDelegate">The result entity delegate.</param>
        /// <param name="finalizeDelegate">The finalize delegate.</param>
        public static T Delete<T>(this IDbConnection connection,
                                  T instance,
                                  Func<T, bool> isAddDelegate,
                                  Func<T, IDatabaseCommand> commandDelegate,
                                  Func<T, T>? resultEntityDelegate = null,
                                  Func<T, Exception?, T>? finalizeDelegate = null)
        {
            if (commandDelegate == null)
            {
                throw new ArgumentNullException(nameof(commandDelegate));
            }

            if (isAddDelegate == null)
            {
                throw new ArgumentNullException(nameof(isAddDelegate));
            }

            var command = commandDelegate.Invoke(instance);
            return connection.ExecuteCommand(instance,
                                             command,
                                             i => !isAddDelegate(i),
                                             typeof(T).Name + " entity was not deleted",
                                             resultEntityDelegate,
                                             typeof(T).Name + " entity cannot be deleted, because it's a new item",
                                             null,
                                             finalizeDelegate);
        }

        /// <summary>Finds one entity on this connection.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="command">The sql command.</param>
        /// <param name="mapFunction">The map function.</param>
        /// <param name="finalizeDelegate">The finalize delegate.</param>
        public static T FindOne<T>(this IDbConnection connection,
                                   IDatabaseCommand command,
                                   Func<IDataReader, T> mapFunction,
                                   Func<T, Exception?, T>? finalizeDelegate = null)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var returnValue = default(T);

            try
            {
                connection.OpenIfNecessary();
                using (var cmd = connection.CreateCommand())
                {
                    returnValue = cmd.FindOne(command.CommandText, command.CommandType, mapFunction, command.CommandParameters);
                }

                return finalizeDelegate != null
                    ? finalizeDelegate.Invoke(returnValue, null)
                    : returnValue;
            }
            catch (Exception exception)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                finalizeDelegate?.Invoke(returnValue, exception);
#pragma warning restore CS8604 // Possible null reference argument.
                throw;
            }
        }

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
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IReadOnlyCollection<T> returnValue = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            try
            {
                connection.OpenIfNecessary();
                using (var cmd = connection.CreateCommand())
                {
                    returnValue = cmd.FindMany(command.CommandText, command.CommandType, mapFunction, command.CommandParameters).ToList();
                }

                return finalizeDelegate != null
                    ? finalizeDelegate.Invoke(returnValue, null) ?? returnValue
                    : returnValue;
            }
            catch (Exception exception)
            {
                finalizeDelegate?.Invoke(returnValue, exception);

                throw;
            }
        }

        private static T ExecuteCommand<T>(this IDbConnection connection,
                                           T instance,
                                           IDatabaseCommand command,
                                           Func<T, bool> operationValidationDelegate,
                                           string? exceptionMessage = null,
                                           Func<T, T>? resultEntityDelegate = null,
                                           string? invalidOperationErrorMessage = null,
                                           Func<T, IDataReader, T>? afterReadDelegate = null,
                                           Func<T, Exception?, T>? finalizeDelegate = null)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var resultEntity = resultEntityDelegate == null
                ? instance
                : resultEntityDelegate(instance);

            if (resultEntity == null)
            {
                throw new ArgumentException("Instance should be supplied, or result entity delegate should deliver an instance");
            }

            if (!operationValidationDelegate(resultEntity))
            {
                throw new ArgumentException(invalidOperationErrorMessage, nameof(instance));
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
                        if (cmd.ExecuteNonQuery() == 0 && !string.IsNullOrEmpty(exceptionMessage))
                        {
                            throw new DataException(exceptionMessage);
                        }
                    }
                    else
                    {
                        //Use ExecuteReader
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
