namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionParser;

internal sealed class ExpressionParser
{
    private readonly List<ExpressionToken> _tokens;
    private int _position;

    public ExpressionParser(List<ExpressionToken> tokens)
    {
        _tokens = tokens;
    }

    public Result<IOperatorExpression> Parse()
    {
        return ParseLogicalOr();
    }

    private Result<IOperatorExpression> ParseLogicalOr()
    {
        var expr = ParseLogicalAnd();

        while (Match(ExpressionTokenType.OrOr))
        {
            var op = Previous();
            var right = ParseLogicalAnd();
            expr = Result.Success<IOperatorExpression>(new BinaryOperatorExpression(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperatorExpression> ParseLogicalAnd()
    {
        var expr = ParseEquality();

        while (Match(ExpressionTokenType.AndAnd))
        {
            var op = Previous();
            var right = ParseEquality();
            expr = Result.Success<IOperatorExpression>(new BinaryOperatorExpression(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperatorExpression> ParseEquality()
    {
        var expr = ParseComparison();

        while (Match(ExpressionTokenType.EqualEqual, ExpressionTokenType.NotEqual))
        {
            var op = Previous();
            var right = ParseAdditive();
            expr = Result.Success<IOperatorExpression>(new BinaryOperatorExpression(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperatorExpression> ParseComparison()
    {
        var expr = ParseAdditive();

        while (Match(ExpressionTokenType.Less, ExpressionTokenType.LessEqual, ExpressionTokenType.Greater, ExpressionTokenType.GreaterEqual))
        {
            var op = Previous();
            var right = ParseAdditive();
            expr = Result.Success<IOperatorExpression>(new BinaryOperatorExpression(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperatorExpression> ParseAdditive()
    {
        var expr = ParseMultiplicative();

        while (Match(ExpressionTokenType.Plus, ExpressionTokenType.Minus))
        {
            var op = Previous();
            var right = ParseMultiplicative();
            expr = Result.Success<IOperatorExpression>(new BinaryOperatorExpression(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperatorExpression> ParseMultiplicative()
    {
        var expr = ParsePrimary();

        while (Match(ExpressionTokenType.Multiply, ExpressionTokenType.Divide))
        {
            var op = Previous();
            var right = ParsePrimary();
            expr = Result.Success<IOperatorExpression>(new BinaryOperatorExpression(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IOperatorExpression> ParsePrimary()
    {
        if (Match(ExpressionTokenType.Other))
        {
            return Result.Success<IOperatorExpression>(new NonBinaryOperatorExpression(Previous().Value));
        }

        if (Match(ExpressionTokenType.LeftParen))
        {
            var expr = Parse();
            _ = Consume(ExpressionTokenType.RightParen, "Expect ')' after expression.");
            return expr;
        }

        return Result.Invalid<IOperatorExpression>("Unexpected token");
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
