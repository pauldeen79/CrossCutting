namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionParser;

internal sealed class ExpressionParser
{
    private readonly List<ExpressionToken> _tokens;
    private int _position;

    public ExpressionParser(List<ExpressionToken> tokens)
    {
        _tokens = tokens;
    }

    public Result<IOperator> Parse()
    {
        return ParseLogicalOr();
    }

    private Result<IOperator> ParseLogicalOr()
    {
        var expr = ParseLogicalAnd();

        while (Match(ExpressionTokenType.OrOr))
        {
            var op = Previous();
            var right = ParseLogicalAnd();
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperator> ParseLogicalAnd()
    {
        var expr = ParseEquality();

        while (Match(ExpressionTokenType.AndAnd))
        {
            var op = Previous();
            var right = ParseEquality();
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperator> ParseEquality()
    {
        var expr = ParseComparison();

        while (Match(ExpressionTokenType.EqualEqual, ExpressionTokenType.NotEqual))
        {
            var op = Previous();
            var right = ParseAdditive();
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperator> ParseComparison()
    {
        var expr = ParseAdditive();

        while (Match(ExpressionTokenType.Less, ExpressionTokenType.LessEqual, ExpressionTokenType.Greater, ExpressionTokenType.GreaterEqual))
        {
            var op = Previous();
            var right = ParseAdditive();
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperator> ParseAdditive()
    {
        var expr = ParseMultiplicative();

        while (Match(ExpressionTokenType.Plus, ExpressionTokenType.Minus))
        {
            var op = Previous();
            var right = ParseMultiplicative();
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperator> ParseMultiplicative()
    {
        var expr = ParseUnary();

        while (Match(ExpressionTokenType.Multiply, ExpressionTokenType.Divide))
        {
            var op = Previous();
            var right = ParseUnary();
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperator> ParseUnary()
    {
        if (Match(ExpressionTokenType.Bang))
        {
            var operatorToken = Previous();
            var right = ParseUnary(); // right-associative
            return Result.Success<IOperator>(new UnaryOPerator(operatorToken.Type, right));
        }

        return ParsePrimary();
    }

    private Result<IOperator> ParsePrimary()
    {
        if (Match(ExpressionTokenType.Other))
        {
            return Result.Success<IOperator>(new ExpressionOperator(Previous().Value));
        }

        if (Match(ExpressionTokenType.LeftParen))
        {
            var expr = Parse();
            _ = Consume(ExpressionTokenType.RightParen, "Expect ')' after expression.");
            return expr;
        }

        return Result.Invalid<IOperator>("Unexpected token");
    }

    private bool Match(params ExpressionTokenType[] types)
    {
        if (types.Any(Check))
        {
            Advance();
            return true;
        }

        return false;
    }

    private bool Check(ExpressionTokenType type)
    {
        if (IsAtEnd())
        {
            return false;
        }

        return Peek().Type == type;
    }

    private Result<ExpressionToken> Advance()
    {
        if (!IsAtEnd())
        {
            _position++;
        }

        return Result.Success(Previous());
    }

    private bool IsAtEnd() => Peek().Type == ExpressionTokenType.EOF;
    private ExpressionToken Peek() => _tokens[_position];
    private ExpressionToken Previous() => _tokens[_position - 1];

    private Result<ExpressionToken> Consume(ExpressionTokenType type, string message)
    {
        if (!Check(type))
        {
            return Result.Invalid<ExpressionToken>(message);
        }

        return Advance();
    }
}
