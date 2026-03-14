namespace CrossCutting.Utilities.ExpressionEvaluator.Sql;

public static class DatabaseCommandExtensions
{
    public static IPagedDatabaseCommand ToPagedCommand(this IDatabaseCommand instance, int offset, int pageSize)
    {
        if (instance.Operation != DatabaseOperation.Select)
        {
            throw new ArgumentException($"Database command operation {instance.Operation} is not Select", nameof(instance));
        }

        var countCommandBuilder = new DatabaseCommandBuilder();
        countCommandBuilder.CommandParameters.AddRange(instance.CommandParameters.ToExpandoObject());
        countCommandBuilder.CommandType = instance.CommandType;
        countCommandBuilder.Operation = instance.Operation;
        countCommandBuilder.Append(Regex.Replace(
            instance.CommandText,
            @"(?i)\bSELECT\b\s+.*?\s+\bFROM\b",
            "SELECT COUNT(*) FROM"));

        return new PagedDatabaseCommand(instance, countCommandBuilder.Build(), offset, pageSize);
    }
}