namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunctionParser
{
    Result<FunctionCall> Parse(ExpressionEvaluatorContext context);
}
