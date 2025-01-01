namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionCall
{
    [Required] string FunctionName { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionCallArgument> Arguments { get; }
    [Required] IFormatProvider FormatProvider { get; }
    object? Context { get; }
}
