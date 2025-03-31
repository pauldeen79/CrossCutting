using CrossCutting.Utilities.ExpressionEvaluator.Mathematics.Validators;

namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExpressionEvaluator(this IServiceCollection services)
        => services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<IExpressionEvaluator, ExpressionEvaluator>()
            .AddSingleton<IExpression, PrimitiveExpression>()
            .AddSingleton<IExpression, StringExpression>()
            .AddSingleton<IExpression, TypeOfExpression>()
            .AddSingleton<IExpression, ComparisonOperatorExpression>()
            .AddSingleton<IComparisonConditionGroupParser, ComparisonConditionGroupParser>()
            .AddSingleton<IComparisonConditionGroupEvaluator, ComparisonConditionGroupEvaluator>()
            .AddSingleton<IOperator, EqualOperator>()
            .AddSingleton<IOperator, GreaterOrEqualThanOperator>()
            .AddSingleton<IOperator, GreaterThanOperator>()
            .AddSingleton<IOperator, NotEqualOperator>()
            .AddSingleton<IOperator, SmallerOrEqualThanOperator>()
            .AddSingleton<IOperator, SmallerThanOperator>()
            .AddSingleton<IExpression, BinaryOperatorExpression>()
            .AddSingleton<IBinaryConditionGroupParser, BinaryConditionGroupParser>()
            .AddSingleton<IBinaryConditionGroupEvaluator, BinaryConditionGroupEvaluator>()
            .AddSingleton<IExpression, FunctionExpression>()
            .AddSingleton<IFunctionCallArgumentValidator, FunctionCallArgumentValidator>()
            .AddSingleton<IFunctionDescriptorMapper, FunctionDescriptorMapper>()
            .AddSingleton<IFunctionDescriptorProvider, FunctionDescriptorProvider>()
            .AddSingleton<IFunctionParser, FunctionParser>()
            .AddSingleton<IFunctionResolver, FunctionResolver>()
            .AddSingleton<IExpression, MathematicExpression>()
            .AddSingleton<IMathematicExpression, Validate>()
            .AddSingleton<IMathematicExpression, Recursion>()
            .AddSingleton<IMathematicExpression, MathematicOperators>()
            .AddSingleton<IMathematicExpressionValidator, NullOrEmptyValidator>()
            .AddSingleton<IMathematicExpressionValidator, TemporaryDelimiterValidator>()
            .AddSingleton<IMathematicExpressionValidator, StartWithOperatorValidator>()
            .AddSingleton<IMathematicExpressionValidator, EndWithOperatorValidator>()
            .AddSingleton<IMathematicExpressionValidator, EmptyValuePartValidator>()
            .AddSingleton<IMathematicExpressionValidator, BraceValidator>()
            .AddSingleton<IExpression, NumericExpression>();
}
