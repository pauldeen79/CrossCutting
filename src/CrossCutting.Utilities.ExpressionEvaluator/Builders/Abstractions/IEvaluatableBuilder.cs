namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions;

public partial interface IEvaluatableBuilder : IBuilder<IEvaluatable>
{
}

public partial interface IEvaluatableBuilder<T>
{
    IEvaluatable<T> BuildTyped();
}