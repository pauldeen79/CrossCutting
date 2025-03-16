namespace CrossCutting.Utilities.Parsers.Expressions;

public class TypeOfExpression : IExpression
{
    private static readonly Regex _typeOfRegEx = new(@"^typeof\((?<typename>.+?)\)$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var match = _typeOfRegEx.Match(context.Expression);
        if (match.Success)
        {
            var typename = match.Groups["typename"].Value;

            var type = Type.GetType(typename, false);
            if (type is null)
            {
                return Result.Invalid<object?>($"Unknown type: {typename}");
            }

            return Result.Success<object?>(type!);
        }
        else
        {
            return Result.Continue<object?>();
        }
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var match = _typeOfRegEx.Match(context.Expression);
        if (match.Success)
        {
            var typename = match.Groups["typename"].Value;

            var type = Type.GetType(typename, false);
            if (type is null)
            {
                return Result.Invalid<Type>($"Unknown type: {typename}");
            }

            return Result.Success(typeof(Type));
        }
        else
        {
            return Result.Continue<Type>();
        }
    }
}
