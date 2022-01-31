namespace CrossCutting.Data.Abstractions.Extensions;

public static class DatabaseCommandTypeExtensions
{
    public static CommandType ToCommandType(this DatabaseCommandType instance)
        => instance switch
        {
            DatabaseCommandType.StoredProcedure => CommandType.StoredProcedure,
            DatabaseCommandType.Text => CommandType.Text,
            _ => throw new ArgumentOutOfRangeException(nameof(instance), $"Unknown DbCommandType: {instance}"),
        };
}
