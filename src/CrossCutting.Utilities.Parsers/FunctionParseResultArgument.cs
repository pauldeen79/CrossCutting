namespace CrossCutting.Utilities.Parsers;

public abstract record FunctionParseResultArgument
{
    public abstract Result<object?> GetValue(IFunctionParseResultEvaluator evaluator);

    public Result<int> GetValueInt32(string argumentName, IFormatProvider formatProvider, IFunctionParseResultEvaluator evaluator)
    {
        var result = GetValue(evaluator);
        if (result.Value is string s)
        {
            return int.TryParse(s, NumberStyles.None, formatProvider, out var i)
                ? Result<int>.Success(i)
                : Result<int>.Invalid($"{argumentName} could not be parsed to an integer");
        }
        else if (result.Value is int i2)
        {
            return Result<int>.Success(i2);
        }
        else
        {
            return Result<int>.Invalid($"{argumentName} is not of type string or int");
        }
    }

    public Result<string> GetValueString(string argumentName, IFormatProvider formatProvider, IFunctionParseResultEvaluator evaluator)
        => GetValue(evaluator).Value is string s
            ? Result<string>.Success(s)
            : Result<string>.Invalid($"{argumentName} is not of type string");
}

public sealed record LiteralArgument : FunctionParseResultArgument
{
    public LiteralArgument(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override Result<object?> GetValue(IFunctionParseResultEvaluator evaluator) => Result<object?>.Success(Value);
}

public sealed record FunctionArgument : FunctionParseResultArgument
{
    public FunctionArgument(FunctionParseResult function)
    {
        Function = function;
    }

    public FunctionParseResult Function { get; }

    public override Result<object?> GetValue(IFunctionParseResultEvaluator evaluator)
        => evaluator.Evaluate(Function);
}
