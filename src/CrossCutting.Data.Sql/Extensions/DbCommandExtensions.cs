using System;
using System.Collections.Generic;
using System.Data;
using CrossCutting.Common.Extensions;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Abstractions.Extensions;

namespace CrossCutting.Data.Sql.Extensions
{
    public static class DbCommandExtensions
    {
        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static IDbDataParameter CreateParameter(this IDbCommand command, string name, object? value)
        {
            var param = command.CreateParameter();
            var parameter = CreateParameter(name, value);
            param.ParameterName = parameter.Key;
            param.Value = parameter.Value;

            return param;
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public static IDbCommand AddParameter(this IDbCommand command, string name, object? value)
        {
            var p = command.CreateParameter(name, value);
            command.Parameters.Add(p);
            return command;
        }

        /// <summary>Adds the parameters.</summary>
        /// <param name="command">The command.</param>
        /// <param name="keyValuePairs">The key value pairs.</param>
        public static IDbCommand AddParameters(this IDbCommand command, IEnumerable<KeyValuePair<string, object?>> keyValuePairs)
        {
            foreach (var keyValuePair in keyValuePairs)
            {
                command.AddParameter(keyValuePair.Key, keyValuePair.Value);
            }

            return command;
        }

        /// <summary>Fills the command.</summary>
        /// <param name="command">The command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandParameters">The command parameters.</param>
        public static IDbCommand FillCommand(this IDbCommand command,
                                             string commandText,
                                             DatabaseCommandType commandType,
                                             object? commandParameters = null)
        {
            command.CommandText = commandText;
            command.CommandType = commandType.ToCommandType();
            if (commandParameters != null)
            {
                foreach (var param in commandParameters.ToExpandoObject())
                {
                    command.AddParameter(param.Key, param.Value);
                }
            }
            return command;
        }

        public static T? FindOne<T>(this IDbCommand command,
                                    string commandText,
                                    DatabaseCommandType commandType,
                                    Func<IDataReader, T> mapFunction,
                                    object? commandParameters = null)
            where T : class
            => command.FillCommand(commandText, commandType, commandParameters)
                      .FindOne(mapFunction);

        /// <summary>Finds multiple entities using a data reader.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">The command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="mapFunction">The map function.</param>
        /// <param name="commandParameters">The command parameters.</param>
        public static IReadOnlyCollection<T> FindMany<T>(this IDbCommand command,
                                                         string commandText,
                                                         DatabaseCommandType commandType,
                                                         Func<IDataReader, T> mapFunction,
                                                         object? commandParameters = null)
            => command.FillCommand(commandText, commandType, commandParameters)
                      .FindMany(mapFunction);

        private static KeyValuePair<string, object> CreateParameter(string name, object? value)
            => new KeyValuePair<string, object>(name, value.FixNull());

        private static T? FindOne<T>(this IDbCommand command, Func<IDataReader, T> mapFunction)
            where T : class
        {
            using (var reader = command.ExecuteReader())
            {
                return reader.FindOne(mapFunction);
            }
        }

        private static IReadOnlyCollection<T> FindMany<T>(this IDbCommand command, Func<IDataReader, T> mapFunction)
        {
            using (var reader = command.ExecuteReader())
            {
                return reader.FindMany(mapFunction);
            }
        }
    }
}
