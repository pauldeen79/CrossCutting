namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models;

internal interface IFunctionEvaluatorSettings
{
    IFormatProvider FormatProvider { get; }
    [DefaultValue(true)] bool ValidateArgumentTypes { get; }
}
