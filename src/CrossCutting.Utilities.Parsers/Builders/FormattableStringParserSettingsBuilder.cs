namespace CrossCutting.Utilities.Parsers.Builders;

public partial class FormattableStringParserSettingsBuilder
{
    partial void SetDefaultValues()
    {
        // Can't put braces in default value, as this will let the code generation crash. (format string parser)
        // But this is okay for now.
        _placeholderStart = "{";
        _placeholderEnd = "}";
        _formatProvider = CultureInfo.InvariantCulture;
    }
}
