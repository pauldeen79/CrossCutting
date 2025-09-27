namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Evaluatables;

public partial class PropertyNameEvaluatableBuilder : IEvaluatableBuilder
{
    IEvaluatable IBuilder<IEvaluatable>.Build()
        => new PropertyNameEvaluatable(SourceExpression?.Build()!, PropertyName);

    partial void SetDefaultValues()
    {
        SourceExpression = new ContextEvaluatableBuilder();
    }
}
