namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionParseResult
{
    [Required]
    public string FunctionName { get; }
    [Required]
    public IReadOnlyCollection<IFunctionParseResultArgument> Arguments { get; }
    [Required]
    public IFormatProvider FormatProvider { get; }
    public object? Context { get; }
}
