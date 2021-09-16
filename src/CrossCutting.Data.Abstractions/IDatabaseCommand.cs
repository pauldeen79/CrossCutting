namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommand
    {
        string CommandText { get; }
        DatabaseCommandType CommandType { get; }
        object? CommandParameters { get; }
    }
}
