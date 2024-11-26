namespace CrossCutting.CodeGeneration.Models;

internal interface IFormattableStringParserSettings
{
    IFormatProvider FormatProvider { get; }
    [Required(AllowEmptyStrings = false)][MatchingCharacters] string PlaceholderStart { get; }
    [Required(AllowEmptyStrings = false)][MatchingCharacters] string PlaceholderEnd { get; }
}
