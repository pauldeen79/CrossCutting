namespace CrossCutting.CodeGeneration.Models;

public interface IFunctionCall
{
    [Required][ValidateObject] string FunctionName { get; }
    [Required][ValidateObject] IReadOnlyCollection<IFunctionCallArgument> Arguments { get; }
    [Required] IFormatProvider FormatProvider { get; }
    object? Context { get; }
}
