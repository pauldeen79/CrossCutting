namespace CrossCutting.Utilities.Parsers.ExpressionStrings;

public class TypeOfExpressionString : IExpressionString
{
    private static readonly Regex _typeOfRegEx = new(@"^=typeof\((?<typename>.+?)\)$", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public Result<object?> Evaluate(ExpressionStringEvaluatorContext context)
    {
        context = context.IsNotNull(nameof(context));

        var match = _typeOfRegEx.Match(context.Input);
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

    public Result<Type> Validate(ExpressionStringEvaluatorContext context)
    {
        context = context.IsNotNull(nameof(context));

        var match = _typeOfRegEx.Match(context.Input);
        if (match.Success)
        {
            var typename = match.Groups["typename"].Value;

            var type = Type.GetType(typename, false);
            if (type is null)
            {
                return Result.Invalid<Type>($"Unknown type: {typename}");
            }

            return Result.NoContent<Type>();
        }
        else
        {
            return Result.Continue<Type>();
        }
    }
}
