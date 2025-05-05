namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class FunctionExpressionComponent : IExpressionComponent
{
    private readonly IFunctionParser _functionParser;
    private readonly IMemberResolver _memberResolver;

    public FunctionExpressionComponent(IFunctionParser functionParser, IMemberResolver memberResolver)
    {
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));
        ArgumentGuard.IsNotNull(memberResolver, nameof(memberResolver));

        _functionParser = functionParser;
        _memberResolver = memberResolver;
    }

    public int Order => 20;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return _functionParser
            .Parse(context)
            .IgnoreNotFound()
            .Transform(functionCall =>
            {
                var functionCallContext = new FunctionCallContext(functionCall, context);

                return _memberResolver
                    .Resolve(functionCallContext)
                    .Transform(result => EvaluateFunction(result, functionCallContext));
            });
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var functionCallResult = _functionParser.Parse(context);
        if (functionCallResult.Status == ResultStatus.NotFound)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Continue)
                .WithExpressionComponentType(typeof(FunctionExpressionComponent))
                .WithSourceExpression(context.Expression);
        }
        else if (!functionCallResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder()
                .FillFromResult(functionCallResult)
                .WithExpressionComponentType(typeof(FunctionExpressionComponent))
                .WithSourceExpression(context.Expression);
        }

        var functionCallContext = new FunctionCallContext(functionCallResult.GetValueOrThrow(), context);
        var resolveResult = _memberResolver.Resolve(functionCallContext);

        if (!resolveResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder()
                .FillFromResult(resolveResult)
                .AddPartResults(resolveResult.ValidationErrors.Select(validationError => new ExpressionParsePartResultBuilder()
                    .WithErrorMessage(validationError.ErrorMessage)
                    .WithPartName(validationError.MemberNames.First())
                    .WithSourceExpression(context.Expression)
                    .WithExpressionComponentType(typeof(FunctionExpressionComponent))))
                .WithExpressionComponentType(typeof(FunctionExpressionComponent))
                .WithSourceExpression(context.Expression);
        }

        return new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.Ok)
            .WithExpressionComponentType(typeof(FunctionExpressionComponent))
            .WithSourceExpression(context.Expression)
            .WithResultType(resolveResult.GetValueOrThrow().ReturnValueType);
    }

    private static Result<object?> EvaluateFunction(MemberAndTypeDescriptor result, FunctionCallContext functionCallContext)
        => result.Member switch
        {
            null => Result.Invalid<object?>("Member is null"),
            IGenericFunction genericFunction => functionCallContext.Evaluate(genericFunction),
            INonGenericMember nonGenericMember => nonGenericMember.Evaluate(functionCallContext),
            _ => Result.NotSupported<object?>($"Unsupported member type: {result.Member.GetType().FullName}")
        };
}
