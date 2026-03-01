namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Extensions;

public static class IEvaluatableBuilderExtensions
{
    #region Generated code
    /// <summary>Creates a query condition builder with the Equals query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsEqualTo<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new EqualConditionBuilder(), value);

    /// <summary>Creates a query condition builder with the Equals query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsEqualTo<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new EqualConditionBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the GreaterOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsGreaterOrEqualThan<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new GreaterThanOrEqualConditionBuilder(), value);

    /// <summary>Creates a query condition builder with the GreaterOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsGreaterOrEqualThan<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new GreaterThanOrEqualConditionBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsGreaterThan<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new GreaterThanConditionBuilder(), value);

    /// <summary>Creates a query condition builder with the GreaterThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsGreaterThan<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new GreaterThanConditionBuilder(), valueDelegate);
    /// <summary>Creates a query condition builder with the IsNotNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNotNull(this IEvaluatableBuilder instance)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotNullConditionBuilder());

    /// <summary>Creates a query condition builder with the IsNull query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    public static IConditionBuilder IsNull(this IEvaluatableBuilder instance)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NullConditionBuilder());

    /// <summary>Creates a query condition builder with the LowerOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsSmallerOrEqualThan<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new SmallerThanOrEqualConditionBuilder(), value);

    /// <summary>Creates a query condition builder with the LowerOrEqualThan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsSmallerOrEqualThan<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new SmallerThanOrEqualConditionBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the LowerTHan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsSmallerThan<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new SmallerThanConditionBuilder(), value);

    /// <summary>Creates a query condition builder with the LowerTHan query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsSmallerThan<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new SmallerThanConditionBuilder(), valueDelegate);

    /// <summary>Creates a query condition builder with the NotEqual query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="value">The value.</param>
    public static IConditionBuilder IsNotEqualTo<T>(this IEvaluatableBuilder instance, T value)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotEqualConditionBuilder(), value);

    /// <summary>Creates a query condition builder with the NotEqual query operator, using the specified values.</summary>
    /// <param name="instance">The query expression builder instance.</param>
    /// <param name="valueDelegate">The value.</param>
    public static IConditionBuilder IsNotEqualTo<T>(this IEvaluatableBuilder instance, Func<T> valueDelegate)
        => ComposableEvaluatableBuilderHelper.Create(instance, new NotEqualConditionBuilder(), valueDelegate);
    #endregion
}
