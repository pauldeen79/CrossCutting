namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class Int32ExpressionComponent : IExpressionComponent
{
    private static readonly Regex _wholeNumberRegEx = new("^[+-]?[0-9]*$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    
    public int Order => 20;

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.Run(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(context.Expression));

            if (isWholeNumber.Value && int.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out var i))
            {
                return Result.Success<object?>(i);
            }

            return Result.Continue<object?>();
        }, token);

    public Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.Run<ExpressionParseResult>(() =>
        {
            context = ArgumentGuard.IsNotNull(context, nameof(context));

            var isWholeNumber = new Lazy<bool>(() => _wholeNumberRegEx.IsMatch(context.Expression));

            var type = default(Type?);

            if (isWholeNumber.Value && int.TryParse(context.Expression, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, context.Settings.FormatProvider, out _))
            {
                type = typeof(int);
            }

            return new ExpressionParseResultBuilder()
                .WithStatus(type is null
                    ? ResultStatus.Continue
                    : ResultStatus.Ok)
                .WithExpressionComponentType(typeof(Int32ExpressionComponent))
                .WithSourceExpression(context.Expression)
                .WithResultType(type);
        }, token);
}
