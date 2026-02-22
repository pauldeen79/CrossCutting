namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Extensions;

public static class IEvaluatableBuilderExtensions
{
    // public static IEvaluatableBuilder<T> Cast<T>(this IEvaluatableBuilder builder)
    //     => new CastExpressionBuilder<T>().WithSourceExpression(builder);

    // public static IEvaluatableBuilder<TTarget> Cast<TSource, TTarget>(this IEvaluatableBuilder<TSource> builder)
    //     => new CastExpressionBuilder<TTarget>()
    //         .WithSourceExpression(builder);

    #region Generated code
/*
    /// <summary>Creates a query condition builder with the Contains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder Contains(this IEvaluatableBuilder instance, string value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new StringContainsOperatorBuilder(), value);

    /// <summary>Creates a query condition builder with the Contains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder Contains(this IEvaluatableBuilder instance, Func<string> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new StringContainsOperatorBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the Contains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="expression">The expression for the right part of the operator.</param>
    public static IConditionBuilder Contains<T>(this IEvaluatableBuilder instance, T expression)
        where T : IEvaluatableBuilder, IEvaluatableBuilder<string>
        => ComposableEvaluatableBuilderHelper.Create(instance, new StringContainsOperatorBuilder(), expression);

    /// <summary>Creates a query condition builder with the EndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder EndsWith(this IEvaluatableBuilder instance, string value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new EndsWithOperatorBuilder(), value);

    /// <summary>Creates a query condition builder with the EndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder EndsWith(this IEvaluatableBuilder instance, Func<string> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new EndsWithOperatorBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the EndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="expression">The expression for the right part of the operator.</param>
    public static IConditionBuilder EndsWith<T>(this IEvaluatableBuilder instance, T expression)
        where T : IEvaluatableBuilder, IEvaluatableBuilder<string>
        => ComposableEvaluatableBuilderHelper.Create(instance, new EndsWithOperatorBuilder(), expression);
*/
    /// <summary>Creates a query condition builder with the Equals query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsEqualTo<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new EqualOperatorEvaluatableBuilder(), value);

    /// <summary>Creates a query condition builder with the Equals query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsEqualTo<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new EqualOperatorEvaluatableBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the GreaterOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsGreaterOrEqualThan<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new GreaterOrEqualOperatorEvaluatableBuilder(), value);

    /// <summary>Creates a query condition builder with the GreaterOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsGreaterOrEqualThan<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new GreaterOrEqualOperatorEvaluatableBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsGreaterThan<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new GreaterOperatorEvaluatableBuilder(), value);

    /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsGreaterThan<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new GreaterOperatorEvaluatableBuilder(), valueDelegate);
/*
    /// <summary>Creates a query condition builder with the IsNotNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNotNull(this IEvaluatableBuilder instance)
        => ComposableEvaluatableBuilderHelper.Create(instance, new IsNotNullOperatorBuilder());

    /// <summary>Creates a query condition builder with the IsNotNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNotNullOrEmpty(this IEvaluatableBuilder instance)
        => ComposableEvaluatableBuilderHelper.Create(instance, new IsNotNullOrEmptyOperatorBuilder());

    /// <summary>Creates a query condition builder with the IsNotNullOrWhiteSpace query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNotNullOrWhiteSpace(this IEvaluatableBuilder instance)
        => ComposableEvaluatableBuilderHelper.Create(instance, new IsNotNullOrWhiteSpaceOperatorBuilder());

    /// <summary>Creates a query condition builder with the IsNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNull(this IEvaluatableBuilder instance)
        => ComposableEvaluatableBuilderHelper.Create(instance, new IsNullOperatorBuilder());

    /// <summary>Creates a query condition builder with the IsNullOrEmpty query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNullOrEmpty(this IEvaluatableBuilder instance)
        => ComposableEvaluatableBuilderHelper.Create(instance, new IsNullOrEmptyOperatorBuilder());

    /// <summary>Creates a query condition builder with the IsNullOrWhiteSpace query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNullOrWhiteSpace(this IEvaluatableBuilder instance)
        => ComposableEvaluatableBuilderHelper.Create(instance, new IsNullOrWhiteSpaceOperatorBuilder());
*/
    /// <summary>Creates a query condition builder with the LowerOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsSmallerOrEqualThan<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new SmallerOrEqualOperatorEvaluatableBuilder(), value);

    /// <summary>Creates a query condition builder with the LowerOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsSmallerOrEqualThan<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new SmallerOrEqualOperatorEvaluatableBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the LowerTHan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsSmallerThan<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new SmallerOperatorEvaluatableBuilder(), value);

    /// <summary>Creates a query condition builder with the LowerTHan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsSmallerThan<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new SmallerOperatorEvaluatableBuilder(), valueDelegate);
/*
    /// <summary>Creates a query condition builder with the NotContains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesNotContain(this IEvaluatableBuilder instance, string value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new StringNotContainsOperatorBuilder(), value);

    /// <summary>Creates a query condition builder with the NotContains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder DoesNotContain(this IEvaluatableBuilder instance, Func<string> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new StringNotContainsOperatorBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the NotContains query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="expression">The expression for the right part of the operator.</param>
    public static IConditionBuilder DoesNotContain<T>(this IEvaluatableBuilder instance, T expression)
        where T : IEvaluatableBuilder, IEvaluatableBuilder<string>
        => ComposableEvaluatableBuilderHelper.Create(instance, new StringNotContainsOperatorBuilder(), expression);

    /// <summary>Creates a query condition builder with the NotEndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesNotEndWith(this IEvaluatableBuilder instance, string value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotEndsWithOperatorBuilder(), value);

    /// <summary>Creates a query condition builder with the NotEndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder DoesNotEndWith(this IEvaluatableBuilder instance, Func<string> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotEndsWithOperatorBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the NotEndsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="expression">The expression for the right part of the operator.</param>
    public static IConditionBuilder DoesNotEndWith<T>(this IEvaluatableBuilder instance, T expression)
        where T : IEvaluatableBuilder, IEvaluatableBuilder<string>
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotEndsWithOperatorBuilder(), expression);

    /// <summary>Creates a query condition builder with the NotEqual query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsNotEqualTo<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotEqualsOperatorBuilder(), value);

    /// <summary>Creates a query condition builder with the NotEqual query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsNotEqualTo<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotEqualsOperatorBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the NotStartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder DoesNotStartWith(this IEvaluatableBuilder instance, string value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotStartsWithOperatorBuilder(), value);

    /// <summary>Creates a query condition builder with the NotStartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder DoesNotStartWith(this IEvaluatableBuilder instance, Func<string> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotStartsWithOperatorBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the NotStartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="expression">The expression for the right part of the operator.</param>
    public static IConditionBuilder DoesNotStartWith<T>(this IEvaluatableBuilder instance, T expression)
        where T : IEvaluatableBuilder, IEvaluatableBuilder<string>
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotStartsWithOperatorBuilder(), expression);

    /// <summary>Creates a query condition builder with the StartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder StartsWith(this IEvaluatableBuilder instance, string value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new StartsWithOperatorBuilder(), value);

    /// <summary>Creates a query condition builder with the StartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder StartsWith(this IEvaluatableBuilder instance, Func<string> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new StartsWithOperatorBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the StartsWith query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="expression">The expression for the right part of the operator.</param>
    public static IConditionBuilder StartsWith<T>(this IEvaluatableBuilder instance, T expression)
        where T : IEvaluatableBuilder, IEvaluatableBuilder<string>
        => ComposableEvaluatableBuilderHelper.Create(instance, new StartsWithOperatorBuilder(), expression);
*/
    #endregion

    /*#region Built-in functions
    /// <summary>Gets the length of this expression.</summary>
    public static IEvaluatableBuilder<int> Len(this IEvaluatableBuilder<string> instance)
        => new StringLengthExpressionBuilder().WithExpression(instance);

    /// <summary>Trims the value of this expression.</summary>
    public static IEvaluatableBuilder<string> Trim(this IEvaluatableBuilder<string> instance)
        => new TrimExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the upper-cased value of this expression.</summary>
    public static IEvaluatableBuilder<string> Upper(this IEvaluatableBuilder<string> instance)
        => new ToUpperCaseExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the lower-cased value of this expression.</summary>
    public static IEvaluatableBuilder<string> Lower(this IEvaluatableBuilder<string> instance)
        => new ToLowerCaseExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the left part of this expression.</summary>
    public static IEvaluatableBuilder<string> Left(this IEvaluatableBuilder<string> instance, int length)
        => new LeftExpressionBuilder().WithExpression(instance).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));

    /// <summary>Gets the right part of this expression.</summary>
    public static IEvaluatableBuilder<string> Right(this IEvaluatableBuilder<string> instance, int length)
        => new RightExpressionBuilder().WithExpression(instance).WithLengthExpression(new TypedConstantExpressionBuilder<int>().WithValue(length));

    /// <summary>Gets the year of this date expression.</summary>
    public static IEvaluatableBuilder<int> Year(this IEvaluatableBuilder<DateTime> instance)
        => new YearExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the month of this date expression.</summary>
    public static IEvaluatableBuilder<int> Month(this IEvaluatableBuilder<DateTime> instance)
        => new MonthExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the day of this date expression.</summary>
    public static IEvaluatableBuilder<int> Day(this IEvaluatableBuilder<DateTime> instance)
        => new DayExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the count of this expression.</summary>
    public static IEvaluatableBuilder<int> Count(this IEvaluatableBuilder<IEnumerable> instance)
        => new CountExpressionBuilder().WithExpression(instance);

    /// <summary>Gets the sum of this expression.</summary>
    public static ExpressionBuilder Sum(this IEvaluatableBuilder<IEnumerable> instance)
        => new SumExpressionBuilder().WithExpression(instance);
    #endregion*/
}
