namespace CrossCutting.Data.Abstractions.Extensions;

public static class DatabaseCommandResultExtensions
{
    public static Result<T> HandleResult<T>(this IDatabaseCommandResult<T> instance, string exceptionMessage)
        where T : class
    {
        if (!instance.Success || instance.Data is null)
        {
            return Result.Error<T>(exceptionMessage);
        }

        return Result.Success(instance.Data!);
    }
}
