namespace CrossCutting.Utilities.Parsers.Builders;

public partial class PlaceholderSettingsBuilder
{
    partial void SetDefaultValues()
    {
        FormatProvider = CultureInfo.InvariantCulture;
    }
}
