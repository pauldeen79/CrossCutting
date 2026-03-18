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

    public static EqualOperatorEvaluatableBuilder IsEqualTo(this IEvaluatableBuilder instance, IEvaluatableBuilder other)
        => new EqualOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(other);

    public static EqualOperatorEvaluatableBuilder IsEqualTo(this IEvaluatableBuilder instance, object? value)
        => new EqualOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(new LiteralEvaluatableBuilder(value));

    public static GreaterOrEqualOperatorEvaluatableBuilder IsGreaterOrEqualThan(this IEvaluatableBuilder instance, IEvaluatableBuilder other)
        => new GreaterOrEqualOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(other);

    public static GreaterOrEqualOperatorEvaluatableBuilder IsGreaterOrEqualThan(this IEvaluatableBuilder instance, object? value)
        => new GreaterOrEqualOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(new LiteralEvaluatableBuilder(value));

    public static GreaterOperatorEvaluatableBuilder IsGreaterThan(this IEvaluatableBuilder instance, IEvaluatableBuilder other)
        => new GreaterOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(other);

    public static GreaterOperatorEvaluatableBuilder IsGreaterThan(this IEvaluatableBuilder instance, object? value)
        => new GreaterOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(new LiteralEvaluatableBuilder(value));

    public static SmallerOrEqualOperatorEvaluatableBuilder IsSmallerOrEqualThan(this IEvaluatableBuilder instance, IEvaluatableBuilder other)
        => new SmallerOrEqualOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(other);

    public static SmallerOrEqualOperatorEvaluatableBuilder IsSmallerOrEqualThan(this IEvaluatableBuilder instance, object? value)
        => new SmallerOrEqualOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(new LiteralEvaluatableBuilder(value));

    public static SmallerOperatorEvaluatableBuilder IsSmallerThan(this IEvaluatableBuilder instance, IEvaluatableBuilder other)
        => new SmallerOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(other);

    public static SmallerOperatorEvaluatableBuilder IsSmallerThan(this IEvaluatableBuilder instance, object? value)
        => new SmallerOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(new LiteralEvaluatableBuilder(value));

    public static NotEqualOperatorEvaluatableBuilder IsNotEqualTo(this IEvaluatableBuilder instance, IEvaluatableBuilder other)
        => new NotEqualOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(other);

    public static NotEqualOperatorEvaluatableBuilder IsNotEqualTo(this IEvaluatableBuilder instance, object? value)
        => new NotEqualOperatorEvaluatableBuilder()
            .WithLeftOperand(instance)
            .WithRightOperand(new LiteralEvaluatableBuilder(value));

    public static NotNullOperatorEvaluatableBuilder IsNotNull(this IEvaluatableBuilder instance)
        => new NotNullOperatorEvaluatableBuilder()
            .WithOperand(instance);

    public static NullOperatorEvaluatableBuilder IsNull(this IEvaluatableBuilder instance)
        => new NullOperatorEvaluatableBuilder()
            .WithOperand(instance);

}