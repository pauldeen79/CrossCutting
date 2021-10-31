namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommandProcessorSettings
    {
        string? ExceptionMessage { get; }
    }
}
