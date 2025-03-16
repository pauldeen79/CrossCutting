namespace CrossCutting.Utilities.Parsers.Expressions;

public class OperatorExpression : IExpression
{
    private const string Pattern = @"(==|!=|<=|>=|<|>)";
    private static readonly Regex _operatorRegEx = new(Pattern, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
    {
        var matches = _operatorRegEx.Matches(expression);

        // Ensure there's exactly one operator
        if (matches.Count != 1)
        {
            return Result.Continue<object?>();
        }

        string[] parts = Regex.Split(expression, matches[0].Value);
        if (parts.Length != 2)
        {
            // More than one operator
            return Result.Continue<object?>();
        }

        //TODO: Operands need to be converted to objects using a callback on the expression evaluator
        string leftOperand = parts[0].Trim();
        string operatorSymbol = matches[0].Value;
        string rightOperand = parts[1].Trim();
        return operatorSymbol switch
        {
            //TODO: What StringComparison do we use?
            "==" => Equal.Evaluate(leftOperand, rightOperand, StringComparison.InvariantCulture).Transform<object?>(x => x),
            _ => GreaterThan.Evaluate(leftOperand, rightOperand).Transform<object?>(x => x)
        };
    }

    public Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context)
        => Result.Continue<Type>(); //TODO: Implement this
}
