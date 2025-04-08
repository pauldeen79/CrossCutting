namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IExpressionEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    StringComparison StringComparison { get; }
    [DefaultValue(10)] int MaximumRecursion { get; }
    [DefaultValue(true)] bool EscapeBraces { get; }
    [DefaultValue(true)] bool ValidateArgumentTypes { get; }
}
