namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    StringComparison StringComparison { get; }
    //[DefaultValue(true)] bool ValidateArgumentTypes { get; }
    [DefaultValue(10)] int MaximumRecursion { get; }
}
