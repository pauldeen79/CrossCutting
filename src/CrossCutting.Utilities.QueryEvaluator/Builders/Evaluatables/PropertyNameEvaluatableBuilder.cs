namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Evaluatables;

public partial class PropertyNameEvaluatableBuilder : IEvaluatableBuilder
{
    partial void SetDefaultValues()
    {
        SourceExpression = new ContextEvaluatableBuilder();
    }
}
