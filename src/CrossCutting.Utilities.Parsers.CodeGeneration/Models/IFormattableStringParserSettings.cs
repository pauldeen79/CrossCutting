namespace CrossCutting.Utilities.Parsers.CodeGeneration.Models;

internal interface IFormattableStringParserSettings
{
    IFormatProvider FormatProvider { get; }
    [DefaultValue(true)] bool ValidateArgumentTypes { get; }
    [Required(AllowEmptyStrings = false)] string PlaceholderStart { get; }
    [Required(AllowEmptyStrings = false)] string PlaceholderEnd { get; }
    [DefaultValue(true)] bool EscapeBraces { get; }
    [DefaultValue(10)] int MaximumRecursion { get; }
}
