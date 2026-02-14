namespace CrossCutting.Utilities.Parsers.Builders;

public partial class ExpressionEvaluatorSettingsBuilder
{
    partial void SetDefaultValues()
    {
        _formatProvider = CultureInfo.InvariantCulture;
    }
}
