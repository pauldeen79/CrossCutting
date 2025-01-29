namespace CrossCutting.CodeGeneration.Models;

internal interface IFunctionEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    [DefaultValue(true)] bool ValidateArgumentTypes { get; }
}
