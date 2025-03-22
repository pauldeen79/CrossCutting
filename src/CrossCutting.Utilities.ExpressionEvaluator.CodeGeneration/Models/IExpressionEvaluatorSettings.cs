namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    StringComparison StringComparison { get; }
}
