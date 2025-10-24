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

    public int Order => 30;

    public async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var parseResult = _functionParser.Parse(context);

        if (parseResult.Status == ResultStatus.NotFound)
        {
            return Result.Continue<object?>();
        }

        if (!parseResult.IsSuccessful())
        {
            return parseResult;
        }

        var functionCallContext = new FunctionCallContext(parseResult.Value!, context);
        var resolveResult = await _memberResolver.ResolveAsync(functionCallContext, token).ConfigureAwait(false);
        if (!resolveResult.IsSuccessful())
        {
            return resolveResult;
        }

        return await EvaluateFunction(resolveResult.Value!, functionCallContext, token).ConfigureAwait(false);
    }

    public async Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var functionCallResult = _functionParser.Parse(context).EnsureNotNull().EnsureValue();
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

        var functionCallContext = new FunctionCallContext(functionCallResult.Value!, context);
        var resolveResult = await _memberResolver.ResolveAsync(functionCallContext, token).ConfigureAwait(false);

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

    private static async Task<Result<object?>> EvaluateFunction(MemberAndTypeDescriptor result, FunctionCallContext functionCallContext, CancellationToken token)
        => result.Member switch
        {
            null => Result.Invalid<object?>("Member is null"),
            IGenericFunction genericFunction => await functionCallContext.EvaluateAsync(genericFunction, token).ConfigureAwait(false),
            INonGenericMember nonGenericMember => await nonGenericMember.EvaluateAsync(functionCallContext, token).ConfigureAwait(false),
            _ => Result.NotSupported<object?>($"Unsupported member type: {result.Member.GetType().FullName}")
        };
}
