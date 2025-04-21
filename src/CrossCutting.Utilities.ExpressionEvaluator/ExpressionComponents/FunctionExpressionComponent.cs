namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class FunctionExpressionComponent : IExpressionComponent
{
    private readonly IFunctionParser _functionParser;
    private readonly IFunctionResolver _functionResolver;

    public FunctionExpressionComponent(IFunctionParser functionParser, IFunctionResolver functionResolver)
    {
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));
        ArgumentGuard.IsNotNull(functionResolver, nameof(functionResolver));

        _functionParser = functionParser;
        _functionResolver = functionResolver;
    }

    public int Order => 20;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var functionCallResult = _functionParser.Parse(context);
        if (functionCallResult.Status == ResultStatus.NotFound)
        {
            return Result.Continue<object?>();
        }
        else if (!functionCallResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(functionCallResult);
        }

        var functionCallContext = new FunctionCallContext(functionCallResult.GetValueOrThrow(), context);

        return _functionResolver.Resolve(functionCallContext).Transform(result => EvaluateFunction(result, functionCallContext));
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
        var resolveResult = _functionResolver.Resolve(functionCallContext);

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

    private static Result<object?> EvaluateFunction(FunctionAndTypeDescriptor result, FunctionCallContext functionCallContext)
    {
        if (result.GenericFunction is not null)
        {
            return EvaluateGenericFunction(result.GenericFunction, functionCallContext);
        }

        // We can safely assume that Function is not null, because the c'tor has verified this
        return result.Function!.Evaluate(functionCallContext);
    }

    private static Result<object?> EvaluateGenericFunction(IGenericFunction genericFunction, FunctionCallContext functionCallContext)
    {
        try
        {
            var method = genericFunction
                .GetType()
                .GetMethod(nameof(IGenericFunction.EvaluateGeneric))!
                .MakeGenericMethod(functionCallContext.FunctionCall.TypeArguments.ToArray());

            return (Result<object?>)method.Invoke(genericFunction, [functionCallContext]);
        }
        catch (ArgumentException argException)
        {
            //The type or method has 1 generic parameter(s), but 0 generic argument(s) were provided. A generic argument must be provided for each generic parameter.
            return Result.Invalid<object?>(argException.Message);
        }
    }
}
