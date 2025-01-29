namespace CrossCutting.Utilities.Parsers.Builders;

public partial class ExpressionStringEvaluatorSettingsBuilder
{
    partial void SetDefaultValues()
    {
        FormatProvider = CultureInfo.InvariantCulture;
    }
}
