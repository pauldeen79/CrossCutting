namespace CrossCutting.CodeGeneration.Models;

internal interface IExpressionStringEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    [DefaultValue(true)] bool ValidateArgumentTypes { get; }
}
