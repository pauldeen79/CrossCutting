namespace CrossCutting.Utilities.ExpressionEvaluator.Builders;

public partial class ExpressionEvaluatorSettingsBuilder
{
    partial void SetDefaultValues()
    {
        _placeholderStart = "{";
        _placeholderEnd = "}";
    }
}
