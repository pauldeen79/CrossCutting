namespace CrossCutting.Utilities.QueryEvaluator.Core.Builders.Extensions;

public static partial class QueryBuilderExtensions
{
    public static ComposableEvaluatableFieldNameBuilderWrapper<T> Where<T>(this T instance, string fieldName)
        where T : IQueryBuilder
        => new ComposableEvaluatableFieldNameBuilderWrapper<T>(instance, fieldName);

    //TODO: Review why this causes an ambigious reference exception in tests
    // public static ComposableEvaluatableExpressionBuilderWrapper<T> Where<T>(this T instance, IEvaluatableBuilder expression)
    //     where T : IQueryBuilder
    //     => new ComposableEvaluatableExpressionBuilderWrapper<T>(instance, expression);

    public static ComposableEvaluatableFieldNameBuilderWrapper<T> Or<T>(this T instance, string fieldName)
        where T : IQueryBuilder
        => new ComposableEvaluatableFieldNameBuilderWrapper<T>(instance, fieldName, Combination.Or);

    public static ComposableEvaluatableFieldNameBuilderWrapper<T> And<T>(this T instance, string fieldName)
        where T : IQueryBuilder
        => new ComposableEvaluatableFieldNameBuilderWrapper<T>(instance, fieldName, Combination.And);
}