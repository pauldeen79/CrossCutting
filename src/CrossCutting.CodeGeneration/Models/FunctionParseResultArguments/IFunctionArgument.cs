namespace CrossCutting.CodeGeneration.Models.FunctionParseResultArguments;

public interface IFunctionArgument : IFunctionParseResultArgument
{
    [Required][ValidateObject] IFunctionParseResult Function { get; }
}
