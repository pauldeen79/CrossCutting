namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models;

internal interface IFunctionEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    [DefaultValue(true)] bool ValidateArgumentTypes { get; }
}
