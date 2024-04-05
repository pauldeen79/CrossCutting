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

    public static IDbCommand AddParameter(this IDbCommand command, string name, object? value)
        => command.Chain(x => x.Parameters.Add(command.CreateParameter(name, value)));

    public static IDbCommand AddParameters(this IDbCommand command, IEnumerable<KeyValuePair<string, object?>> keyValuePairs)
    {
        foreach (var keyValuePair in keyValuePairs)
        {
            command.AddParameter(keyValuePair.Key, keyValuePair.Value);
        }

        return command;
    }

    public static IDbCommand FillCommand(this IDbCommand command,
                                         string commandText,
                                         DatabaseCommandType commandType,
                                         object? commandParameters)
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

    public static IDbCommand FillCommand(this IDbCommand command,
                                         FormattableString commandText,
                                         DatabaseCommandType commandType)
    {
        command.CommandText = commandText.Format;
        command.CommandType = commandType.ToCommandType();
        var index = 0;
        foreach (var arg in commandText.GetArguments())
        {
            command.AddParameter($"p{index}", arg);
            command.CommandText = command.CommandText.Replace("{" + index + "}", $"@p{index}");
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

    private static KeyValuePair<string, object> CreateParameter(string name, object? value)
        => new KeyValuePair<string, object>(name, value.FixNull());

    private static T? FindOne<T>(this IDbCommand command, Func<IDataReader, T> mapFunction)
        where T : class
    {
        using var reader = command.ExecuteReader();
        return reader.FindOne(mapFunction);
    }

    private static IReadOnlyCollection<T> FindMany<T>(this IDbCommand command, Func<IDataReader, T> mapFunction)
    {
        using var reader = command.ExecuteReader();
        return reader.FindMany(mapFunction);
    }
}
