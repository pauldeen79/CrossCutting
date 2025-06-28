namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models;

internal interface IExpressionEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    StringComparison StringComparison { get; }
}
