namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class ExpressionParser : IExpressionParser
{
    public Result<IExpression> Parse(ICollection<ExpressionToken> tokens)
        => ParseLogicalOr(new ExpressionParserState(tokens));

    private static Result<IExpression> ParseLogicalOr(ExpressionParserState state)
    {
        var expr = ParseLogicalAnd(state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Or))
        {
            var op = Previous(state);
            var right = ParseLogicalAnd(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(new BinaryExpression(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IExpression> ParseLogicalAnd(ExpressionParserState state)
    {
        var expr = ParseEquality(state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.And))
        {
            var op = Previous(state);
            var right = ParseEquality(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(new BinaryExpression(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IExpression> ParseEquality(ExpressionParserState state)
    {
        var expr = ParseComparison(state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Equal, ExpressionTokenType.NotEqual))
        {
            var op = Previous(state);
            var right = ParseAdditive(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(new BinaryExpression(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IExpression> ParseComparison(ExpressionParserState state)
    {
        var expr = ParseAdditive(state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Less, ExpressionTokenType.LessOrEqual, ExpressionTokenType.Greater, ExpressionTokenType.GreaterOrEqual))
        {
            var op = Previous(state);
            var right = ParseAdditive(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(new BinaryExpression(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IExpression> ParseAdditive(ExpressionParserState state)
    {
        var expr = ParseMultiplicative(state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Plus, ExpressionTokenType.Minus))
        {
            var op = Previous(state);
            var right = ParseMultiplicative(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(new BinaryExpression(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IExpression> ParseMultiplicative(ExpressionParserState state)
    {
        var expr = ParseUnary(state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Multiply, ExpressionTokenType.Divide, ExpressionTokenType.Modulo))
        {
            var op = Previous(state);
            var right = ParseUnary(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(new BinaryExpression(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IExpression> ParseUnary(ExpressionParserState state)
    {
        if (Match(state, ExpressionTokenType.Bang))
        {
            var right = ParseUnary(state); // right-associative
            if (!right.IsSuccessful())
            {
                return right;
            }

            return Result.Success<IExpression>(new UnaryExpression(right));
        }

        return ParsePrimary(state);
    }

    private static Result<IExpression> ParsePrimary(ExpressionParserState state)
    {
        if (Match(state, ExpressionTokenType.Other))
        {
            var peek = Peek(state);
            if (peek.Type == ExpressionTokenType.LeftParenthesis)
            {
                return ParseFunction(state, ref peek);
            }
            else if (peek.Type == ExpressionTokenType.Less
                && PeekNext(state, 1).Type == ExpressionTokenType.Other
                && PeekNext(state, 2).Type == ExpressionTokenType.Greater
                && PeekNext(state, 3).Type == ExpressionTokenType.LeftParenthesis)
            {
                return ParseGenericFunction(state);
            }

            return Result.Success<IExpression>(new OtherExpression(Previous(state).Value));
        }

        if (Match(state, ExpressionTokenType.LeftParenthesis))
        {
            var childState = new ExpressionParserState(state.Tokens.Skip(state.Position).ToList());
            var operatorResult = ParseLogicalOr(childState);
            if (!operatorResult.IsSuccessful())
            {
                return operatorResult;
            }

            state.Position += childState.Position;
            var result = Consume(state, ExpressionTokenType.RightParenthesis, "Expect ')' after expression.");
            if (!result.IsSuccessful())
            {
                return Result.FromExistingResult<IExpression>(result);
            }

            return operatorResult;
        }

        return Result.Invalid<IExpression>("Unexpected token");
    }

    private static Result<IExpression> ParseFunction(ExpressionParserState state, ref ExpressionToken peek)
    {
        if (PeekNext(state, 1).Type == ExpressionTokenType.EOF)
        {
            return Result.Invalid<IExpression>("Missing right parenthesis");
        }

        var builder = new StringBuilder();
        builder.Append(Previous(state).Value);
        builder.Append(peek.Value);

        while (peek.Type != ExpressionTokenType.RightParenthesis && peek.Type != ExpressionTokenType.EOF)
        {
            Advance(state);
            peek = Peek(state);
            builder.Append(peek.Value);
        }

        // Also consume the last right parenthesis
        if (!IsAtEnd(state))
        {
            Advance(state);
        }

        return Result.Success<IExpression>(new OtherExpression(builder.ToString()));
    }

    private static Result<IExpression> ParseGenericFunction(ExpressionParserState state)
    {
        //format: function<type>(
        //a.k.a. other, less, other, greater, left parenthesis
        var afterParenthesis = PeekNext(state, 4);
        if (afterParenthesis.Type == ExpressionTokenType.EOF)
        {
            return Result.Invalid<IExpression>("Missing right parenthesis");
        }
        else if (afterParenthesis.Type == ExpressionTokenType.Other && PeekNext(state, 5).Type == ExpressionTokenType.EOF)
        {
            return Result.Invalid<IExpression>("Missing right parenthesis");
        }
        else
        {
            var numberOfItemsToTake = afterParenthesis.Type == ExpressionTokenType.Other
                ? 7
                : 6;
            var result = Result.Success<IExpression>(new OtherExpression(string.Concat(state.Tokens.Skip(state.Position - 1).Take(numberOfItemsToTake).Select(x => x.Value))));
            state.Position += numberOfItemsToTake - 1;
            return result;
        }
    }

    private static bool Match(ExpressionParserState state, params ExpressionTokenType[] types)
    {
        if (types.Any(x => Check(state, x)))
        {
            Advance(state);
            return true;
        }

        return false;
    }

    private static bool Check(ExpressionParserState state, ExpressionTokenType type)
    {
        if (IsAtEnd(state))
        {
            return false;
        }

        return Peek(state).Type == type;
    }

    private static Result<ExpressionToken> Advance(ExpressionParserState state)
    {
        if (!IsAtEnd(state))
        {
            state.Position++;
        }

        return Result.Success(Previous(state));
    }

    private static bool IsAtEnd(ExpressionParserState state) => Peek(state).Type == ExpressionTokenType.EOF;
    private static ExpressionToken Peek(ExpressionParserState state) => state.Tokens.ElementAt(state.Position);
    private static ExpressionToken Previous(ExpressionParserState state) => state.Tokens.ElementAt(state.Position - 1);
    private static ExpressionToken PeekNext(ExpressionParserState state, int increase) => state.Tokens.ElementAt(state.Position + increase);

    private static Result<ExpressionToken> Consume(ExpressionParserState state, ExpressionTokenType type, string message)
    {
        if (!Check(state, type))
        {
            return Result.Invalid<ExpressionToken>(message);
        }

        return Advance(state);
    }
}
