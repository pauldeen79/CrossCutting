namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommandResult<out T>
        where T : class
    {
        bool Success { get; }
        T? Data { get; }
    }
}
