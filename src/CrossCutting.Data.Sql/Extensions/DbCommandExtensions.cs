namespace CrossCutting.Data.Sql.Extensions;

public static class DbCommandExtensions
{
    public static IDbDataParameter CreateParameter(this IDbCommand command, string name, object? value)
    {
        var dbDataParameter = command.CreateParameter();
        var parameterKeyValuePair = CreateParameter(name, value);
        dbDataParameter.ParameterName = parameterKeyValuePair.Key;
        dbDataParameter.Value = parameterKeyValuePair.Value;

        return dbDataParameter;
    }

    public static T AddParameter<T>(this T command, string name, object? value)
        where T : IDbCommand
        => command.Chain(x => x.Parameters.Add(command.CreateParameter(name, value)));

    public static T AddParameters<T>(this T command, IEnumerable<KeyValuePair<string, object?>> keyValuePairs)
        where T : IDbCommand
    {
        foreach (var keyValuePair in keyValuePairs)
        {
            command.AddParameter(keyValuePair.Key, keyValuePair.Value);
        }

        return command;
    }

    public static T FillCommand<T>(this T command,
                                   string commandText,
                                   DatabaseCommandType commandType,
                                   object? commandParameters)
        where T : IDbCommand
    {
        command.CommandText = commandText;
        command.CommandType = commandType.ToCommandType();
        if (commandParameters is not null)
        {
            foreach (var param in commandParameters.ToExpandoObject())
            {
                command.AddParameter(param.Key, param.Value);
            }
        }
        return command;
    }

    public static T FillCommand<T>(this T command,
                                   FormattableString commandText,
                                   DatabaseCommandType commandType)
        where T : IDbCommand
    {
        command.CommandText = commandText.Format;
        command.CommandType = commandType.ToCommandType();
        var index = 0;
        foreach (var arg in commandText.GetArguments())
        {
            command.AddParameter($"p{index}", arg);
            command.CommandText = command.CommandText.Replace($"{{{index}}}", $"@p{index}");
            index++;
        }
        return command;
    }

    public static T? FindOne<T>(this IDbCommand command,
                                string commandText,
                                DatabaseCommandType commandType,
                                Func<IDataReader, T> mapFunction,
                                object? commandParameters)
        where T : class
        => command.FillCommand(commandText, commandType, commandParameters)
                  .FindOne(mapFunction);

    public static T? FindOne<T>(this IDbCommand command,
                                FormattableString commandText,
                                DatabaseCommandType commandType,
                                Func<IDataReader, T> mapFunction)
        where T : class
        => command.FillCommand(commandText, commandType)
                  .FindOne(mapFunction);

    public static async Task<T?> FindOneAsync<T>(this DbCommand command,
                                                 string commandText,
                                                 DatabaseCommandType commandType,
                                                 CancellationToken cancellationToken,
                                                 Func<IDataReader, T> mapFunction,
                                                 object? commandParameters)
        where T : class
        => await command.FillCommand(commandText, commandType, commandParameters)
                        .FindOneAsync(cancellationToken, mapFunction);

    public static async Task<T?> FindOneAsync<T>(this DbCommand command,
                                                 FormattableString commandText,
                                                 DatabaseCommandType commandType,
                                                 CancellationToken cancellationToken,
                                                 Func<IDataReader, T> mapFunction)
        where T : class
        => await command.FillCommand(commandText, commandType)
                        .FindOneAsync(cancellationToken, mapFunction);

    public static IReadOnlyCollection<T> FindMany<T>(this IDbCommand command,
                                                     string commandText,
                                                     DatabaseCommandType commandType,
                                                     Func<IDataReader, T> mapFunction,
                                                     object? commandParameters)
        => command.FillCommand(commandText, commandType, commandParameters)
                  .FindMany(mapFunction);

    public static IReadOnlyCollection<T> FindMany<T>(this IDbCommand command,
                                                     FormattableString commandText,
                                                     DatabaseCommandType commandType,
                                                     Func<IDataReader, T> mapFunction)
        => command.FillCommand(commandText, commandType)
                  .FindMany(mapFunction);

    public static async Task<IReadOnlyCollection<T>> FindManyAsync<T>(this DbCommand command,
                                                                      string commandText,
                                                                      DatabaseCommandType commandType,
                                                                      CancellationToken cancellationToken,
                                                                      Func<IDataReader, T> mapFunction,
                                                                      object? commandParameters)
        => await command.FillCommand(commandText, commandType, commandParameters)
                        .FindManyAsync(cancellationToken, mapFunction);

    public static async Task<IReadOnlyCollection<T>> FindManyAsync<T>(this DbCommand command,
                                                                      FormattableString commandText,
                                                                      DatabaseCommandType commandType,
                                                                      CancellationToken cancellationToken,
                                                                      Func<IDataReader, T> mapFunction)
        => await command.FillCommand(commandText, commandType)
                        .FindManyAsync(cancellationToken, mapFunction);

    private static KeyValuePair<string, object> CreateParameter(string name, object? value)
        => new KeyValuePair<string, object>(name, value.FixNull());

    private static T? FindOne<T>(this IDbCommand command, Func<IDataReader, T> mapFunction)
        where T : class
    {
        using var reader = command.ExecuteReader();
        return reader.FindOne(mapFunction);
    }

    private static async Task<T?> FindOneAsync<T>(this DbCommand command, CancellationToken cancellationToken, Func<IDataReader, T> mapFunction)
        where T : class
    {
        using var reader = await command.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken);
        return await reader.FindOneAsync(cancellationToken, mapFunction);
    }

    private static IReadOnlyCollection<T> FindMany<T>(this IDbCommand command, Func<IDataReader, T> mapFunction)
    {
        using var reader = command.ExecuteReader();
        return reader.FindMany(mapFunction);
    }

    private static async Task<IReadOnlyCollection<T>> FindManyAsync<T>(this DbCommand command, CancellationToken cancellationToken, Func<IDataReader, T> mapFunction)
    {
        using var reader = await command.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken);
        return await reader.FindManyAsync(cancellationToken, mapFunction);
    }
}
