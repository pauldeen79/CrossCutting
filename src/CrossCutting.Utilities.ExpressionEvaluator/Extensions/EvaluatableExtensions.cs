namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class EvaluatableExtensions
{
    public static IEnumerable<IEvaluatable> GetContainedEvaluatables(this IEvaluatable instance, bool recurse)
    {
        if (instance is not IChildEvaluatablesContainer childEvaluatablesContainer)
        {
            yield return instance;
            yield break;
        }

        if (!recurse)
        {
            foreach (var evaluatable in childEvaluatablesContainer.GetChildEvaluatables())
            {
                yield return evaluatable;
            }
            yield break;
        }

        foreach (var evaluatable in childEvaluatablesContainer.GetChildEvaluatables())
        {
            foreach(var childEvaluatable in GetContainedEvaluatables(evaluatable, true))
            {
                yield return childEvaluatable;
            }
        }
    }

    public static BinaryAndOperatorEvaluatable And(this IEvaluatable instance, IEvaluatable other)
        => new BinaryAndOperatorEvaluatable(instance, other);

    public static BinaryOrOperatorEvaluatable Or(this IEvaluatable instance, IEvaluatable other)
        => new BinaryOrOperatorEvaluatable(instance, other);

    public static EqualOperatorEvaluatable IsEqualTo(this IEvaluatable instance, IEvaluatable other)
        => new EqualOperatorEvaluatable(instance, other);

    public static EqualOperatorEvaluatable IsEqualTo(this IEvaluatable instance, object? value)
        => new EqualOperatorEvaluatable(instance, new LiteralEvaluatable(value));

    public static GreaterOrEqualOperatorEvaluatable IsGreaterOrEqualThan(this IEvaluatable instance, IEvaluatable other)
        => new GreaterOrEqualOperatorEvaluatable(instance, other);

    public static GreaterOrEqualOperatorEvaluatable IsGreaterOrEqualThan(this IEvaluatable instance, object? value)
        => new GreaterOrEqualOperatorEvaluatable(instance, new LiteralEvaluatable(value));

    public static GreaterOperatorEvaluatable IsGreaterThan(this IEvaluatable instance, IEvaluatable other)
        => new GreaterOperatorEvaluatable(instance, other);

    public static GreaterOperatorEvaluatable IsGreaterThan(this IEvaluatable instance, object? value)
        => new GreaterOperatorEvaluatable(instance, new LiteralEvaluatable(value));

    public static SmallerOrEqualOperatorEvaluatable IsSmallerOrEqualThan(this IEvaluatable instance, IEvaluatable other)
        => new SmallerOrEqualOperatorEvaluatable(instance, other);

    public static SmallerOrEqualOperatorEvaluatable IsSmallerOrEqualThan(this IEvaluatable instance, object? value)
        => new SmallerOrEqualOperatorEvaluatable(instance, new LiteralEvaluatable(value));

    public static SmallerOperatorEvaluatable IsSmallerThan(this IEvaluatable instance, IEvaluatable other)
        => new SmallerOperatorEvaluatable(instance, other);

    public static SmallerOperatorEvaluatable IsSmallerThan(this IEvaluatable instance, object? value)
        => new SmallerOperatorEvaluatable(instance, new LiteralEvaluatable(value));

    public static NotEqualOperatorEvaluatable IsNotEqualTo(this IEvaluatable instance, IEvaluatable other)
        => new NotEqualOperatorEvaluatable(instance, other);

    public static NotEqualOperatorEvaluatable IsNotEqualTo(this IEvaluatable instance, object? value)
        => new NotEqualOperatorEvaluatable(instance, new LiteralEvaluatable(value));

    public static NotNullOperatorEvaluatable IsNotNull(this IEvaluatable instance)
        => new NotNullOperatorEvaluatable(instance);

    public static NullOperatorEvaluatable IsNull(this IEvaluatable instance)
        => new NullOperatorEvaluatable(instance);

    public static StringStartsWithOperatorEvaluatable StartsWith(this IEvaluatable<string> instance, IEvaluatable<string> other, StringComparison stringComparison = default)
        => new StringStartsWithOperatorEvaluatable(instance, other, stringComparison);

    public static StringStartsWithOperatorEvaluatable StartsWith(this IEvaluatable<string> instance, string value, StringComparison stringComparison = default)
        => new StringStartsWithOperatorEvaluatable(instance, new LiteralEvaluatable<string>(value), stringComparison);

    public static StringEndsWithOperatorEvaluatable EndsWith(this IEvaluatable<string> instance, IEvaluatable<string> other, StringComparison stringComparison = default)
        => new StringEndsWithOperatorEvaluatable(instance, other, stringComparison);

    public static StringEndsWithOperatorEvaluatable EndsWith(this IEvaluatable<string> instance, string value, StringComparison stringComparison = default)
        => new StringEndsWithOperatorEvaluatable(instance, new LiteralEvaluatable<string>(value), stringComparison);

    public static StringContainsOperatorEvaluatable Contains(this IEvaluatable<string> instance, IEvaluatable<string> other, StringComparison stringComparison = default)
        => new StringContainsOperatorEvaluatable(instance, other, stringComparison);

    public static StringContainsOperatorEvaluatable Contains(this IEvaluatable<string> instance, string value, StringComparison stringComparison = default)
        => new StringContainsOperatorEvaluatable(instance, new LiteralEvaluatable<string>(value), stringComparison);

    public static UnaryNegateOperatorEvaluatable DoesNotStartWith(this IEvaluatable<string> instance, IEvaluatable<string> other, StringComparison stringComparison = default)
        => new UnaryNegateOperatorEvaluatable(new StringStartsWithOperatorEvaluatable(instance, other, stringComparison));

    public static UnaryNegateOperatorEvaluatable DoesNotStartWith(this IEvaluatable<string> instance, string value, StringComparison stringComparison = default)
        => new UnaryNegateOperatorEvaluatable(new StringStartsWithOperatorEvaluatable(instance, new LiteralEvaluatable<string>(value), stringComparison));

    public static UnaryNegateOperatorEvaluatable DoesNotEndWith(this IEvaluatable<string> instance, IEvaluatable<string> other, StringComparison stringComparison = default)
        => new UnaryNegateOperatorEvaluatable(new StringEndsWithOperatorEvaluatable(instance, other, stringComparison));

    public static UnaryNegateOperatorEvaluatable DoesNotEndWith(this IEvaluatable<string> instance, string value, StringComparison stringComparison = default)
        => new UnaryNegateOperatorEvaluatable(new StringEndsWithOperatorEvaluatable(instance, new LiteralEvaluatable<string>(value), stringComparison));

    public static UnaryNegateOperatorEvaluatable DoesNotContain(this IEvaluatable<string> instance, IEvaluatable<string>other, StringComparison stringComparison = default)
        => new UnaryNegateOperatorEvaluatable(new StringContainsOperatorEvaluatable(instance, other, stringComparison));

    public static UnaryNegateOperatorEvaluatable DoesNotContain(this IEvaluatable<string> instance, string value, StringComparison stringComparison = default)
        => new UnaryNegateOperatorEvaluatable(new StringContainsOperatorEvaluatable(instance, new LiteralEvaluatable<string>(value), stringComparison));
}