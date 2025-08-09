namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models;

internal interface IExpressionStringEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    [DefaultValue(true)] bool ValidateArgumentTypes { get; }
}
