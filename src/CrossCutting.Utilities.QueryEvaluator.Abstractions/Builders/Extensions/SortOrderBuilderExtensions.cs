namespace CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders.Extensions;

public static partial class SortOrderBuilderExtensions
{
    public static ISortOrderBuilder WithPropertyName(this ISortOrderBuilder instance, string propertyName)
        => instance.WithExpression(new PropertyNameEvaluatableBuilder(propertyName));
}
