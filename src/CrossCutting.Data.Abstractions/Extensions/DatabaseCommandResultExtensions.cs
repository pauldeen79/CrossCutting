namespace CrossCutting.Data.Abstractions.Extensions;

public static class DatabaseCommandResultExtensions
{
    public static T HandleResult<T>(this IDatabaseCommandResult<T> instance, string exceptionMessage)
        where T : class
    {
        if (!instance.Success || instance.Data == default)
        {
            throw new DataException(exceptionMessage);
        }

        return instance.Data;
    }
}
