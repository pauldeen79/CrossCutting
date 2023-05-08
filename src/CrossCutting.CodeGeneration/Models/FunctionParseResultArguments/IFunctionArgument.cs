namespace CrossCutting.CodeGeneration.Models.FunctionParseResultArguments;

public interface IFunctionArgument : IFunctionParseResultArgument
{
    [Required]
    IFunctionParseResult Function { get; }
}
