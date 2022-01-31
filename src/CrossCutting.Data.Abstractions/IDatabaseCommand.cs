namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommand
{
    string CommandText { get; }
    DatabaseCommandType CommandType { get; }
    DatabaseOperation Operation { get; }
    object? CommandParameters { get; }
}
