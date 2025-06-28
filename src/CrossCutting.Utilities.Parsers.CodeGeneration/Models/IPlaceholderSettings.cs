namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models;

internal interface IPlaceholderSettings
{
    IFormatProvider FormatProvider { get; }
    [DefaultValue(true)] bool ValidateArgumentTypes { get; }
}
