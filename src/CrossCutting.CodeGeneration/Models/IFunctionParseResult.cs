namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionParseResult
{
    [Required][ValidateObject] string FunctionName { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionParseResultArgument> Arguments { get; }
    [Required] IFormatProvider FormatProvider { get; }
    object? Context { get; }
}
