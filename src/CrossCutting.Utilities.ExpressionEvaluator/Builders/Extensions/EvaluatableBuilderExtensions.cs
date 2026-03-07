namespace CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;

public static partial class EvaluatableBuilderExtensions
{
    public static BinaryAndOperatorEvaluatableBuilder And(this IEvaluatableBuilder instance, IEvaluatableBuilder other)
        => new BinaryAndOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(other);

    public static BinaryOrOperatorEvaluatableBuilder Or(this IEvaluatableBuilder instance, IEvaluatableBuilder other)
        => new BinaryOrOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(other);
}