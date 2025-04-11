namespace CrossCutting.Utilities.ExpressionEvaluator;

internal sealed class OperatorExpressionParser : IOperatorExpressionParser
{
    public Result<IOperator> Parse(ICollection<OperatorExpressionToken> tokens)
        => ParseLogicalOr(new OperatorExpressionParserState(tokens));

    private static Result<IOperator> ParseLogicalOr(OperatorExpressionParserState state)
    {
        var expr = ParseLogicalAnd(state);

        while (Match(state, OperatorExpressionTokenType.OrOr))
        {
            var op = Previous(state);
            var right = ParseLogicalAnd(state);
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private static Result<IOperator> ParseLogicalAnd(OperatorExpressionParserState state)
    {
        var expr = ParseEquality(state);

        while (Match(state, OperatorExpressionTokenType.AndAnd))
        {
            var op = Previous(state);
            var right = ParseEquality(state);
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private static Result<IOperator> ParseEquality(OperatorExpressionParserState state)
    {
        var expr = ParseComparison(state);

        while (Match(state, OperatorExpressionTokenType.EqualEqual, OperatorExpressionTokenType.NotEqual))
        {
            var op = Previous(state);
            var right = ParseAdditive(state);
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private static Result<IOperator> ParseComparison(OperatorExpressionParserState state)
    {
        var expr = ParseAdditive(state);

        while (Match(state, OperatorExpressionTokenType.Less, OperatorExpressionTokenType.LessEqual, OperatorExpressionTokenType.Greater, OperatorExpressionTokenType.GreaterEqual))
        {
            var op = Previous(state);
            var right = ParseAdditive(state);
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private static Result<IOperator> ParseAdditive(OperatorExpressionParserState state)
    {
        var expr = ParseMultiplicative(state);

        while (Match(state, OperatorExpressionTokenType.Plus, OperatorExpressionTokenType.Minus))
        {
            var op = Previous(state);
            var right = ParseMultiplicative(state);
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private static Result<IOperator> ParseMultiplicative(OperatorExpressionParserState state)
    {
        var expr = ParseUnary(state);

        while (Match(state, OperatorExpressionTokenType.Multiply, OperatorExpressionTokenType.Divide))
        {
            var op = Previous(state);
            var right = ParseUnary(state);
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right));
        }

        return expr;
    }

    private static Result<IOperator> ParseUnary(OperatorExpressionParserState state)
    {
        if (Match(state, OperatorExpressionTokenType.Bang))
        {
            var operatorToken = Previous(state);
            var right = ParseUnary(state); // right-associative
            return Result.Success<IOperator>(new UnaryOperator(operatorToken.Type, right));
        }

        return ParsePrimary(state);
    }

    private static Result<IOperator> ParsePrimary(OperatorExpressionParserState state)
    {
        if (Match(state, OperatorExpressionTokenType.Expression))
        {
            return Result.Success<IOperator>(new ExpressionOperator(Previous(state).Value));
        }

        if (Match(state, OperatorExpressionTokenType.LeftParenthesis))
        {
            var childState = new OperatorExpressionParserState(state.Tokens.Skip(state.Position).ToList());
            var operatorResult = ParseLogicalOr(childState);
            if (!operatorResult.IsSuccessful())
            {
                return operatorResult;
            }

            state.Position += childState.Position;
            var result = Consume(state, OperatorExpressionTokenType.RightParenthesis, "Expect ')' after expression.");
            if (!result.IsSuccessful())
            {
                return Result.FromExistingResult<IOperator>(result);
            }

            return operatorResult;
        }

        return Result.Invalid<IOperator>("Unexpected token");
    }

    private static bool Match(OperatorExpressionParserState state, params OperatorExpressionTokenType[] types)
    {
        if (types.Any(x => Check(state, x)))
        {
            Advance(state);
            return true;
        }

        return false;
    }

    private static bool Check(OperatorExpressionParserState state, OperatorExpressionTokenType type)
    {
        if (IsAtEnd(state))
        {
            return false;
        }

        return Peek(state).Type == type;
    }

    private static Result<OperatorExpressionToken> Advance(OperatorExpressionParserState state)
    {
        if (!IsAtEnd(state))
        {
            state.Position++;
        }

        return Result.Success(Previous(state));
    }

    private static bool IsAtEnd(OperatorExpressionParserState state) => Peek(state).Type == OperatorExpressionTokenType.EOF;
    private static OperatorExpressionToken Peek(OperatorExpressionParserState state) => state.Tokens.ElementAt(state.Position);
    private static OperatorExpressionToken Previous(OperatorExpressionParserState state) => state.Tokens.ElementAt(state.Position - 1);

    private static Result<OperatorExpressionToken> Consume(OperatorExpressionParserState state, OperatorExpressionTokenType type, string message)
    {
        if (!Check(state, type))
        {
            return Result.Invalid<OperatorExpressionToken>(message);
        }

        return Advance(state);
    }
}
