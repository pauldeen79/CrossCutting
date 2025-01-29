namespace CrossCutting.Utilities.Parsers.Builders;

public partial class FunctionEvaluatorSettingsBuilder
{
    partial void SetDefaultValues()
    {
        FormatProvider = CultureInfo.InvariantCulture;
    }
}
