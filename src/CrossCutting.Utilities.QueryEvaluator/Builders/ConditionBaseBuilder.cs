namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders;

public partial class ConditionBaseBuilder : IEvaluatableBuilder<bool>
{
    IEvaluatable IBuilder<IEvaluatable>.Build() => Build();

#pragma warning disable CA1033 // Interface methods should be callable by child types
    IEvaluatable<bool> IEvaluatableBuilder<bool>.BuildTyped() => BuildTypedCore();
#pragma warning restore CA1033 // Interface methods should be callable by child types
    protected abstract IEvaluatable<bool> BuildTypedCore();

    // public abstract IEvaluatable<bool> BuildTyped();
}
