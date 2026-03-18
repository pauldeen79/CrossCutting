namespace CrossCutting.Utilities.Parsers.Builders;

public partial class FunctionEvaluatorSettingsBuilder
{
    partial void SetDefaultValues()
    {
        _formatProvider = CultureInfo.InvariantCulture;
    }
}
