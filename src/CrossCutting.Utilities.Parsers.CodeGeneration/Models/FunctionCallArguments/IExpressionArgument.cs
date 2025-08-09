namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models.FunctionCallArguments;

internal interface IExpressionArgument : IFunctionCallArgumentBase
{
    [Required] string Expression { get; }
}
