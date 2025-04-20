namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class OperatorExpressionParser : IOperatorExpressionParser
{
    public Result<IOperator> Parse(ICollection<OperatorExpressionToken> tokens)
        => ParseLogicalOr(new OperatorExpressionParserState(tokens));

    private static Result<IOperator> ParseLogicalOr(OperatorExpressionParserState state)
    {
        var expr = ParseLogicalAnd(state);

        while (expr.IsSuccessful() && Match(state, OperatorExpressionTokenType.Or))
        {
            var op = Previous(state);
            var right = ParseLogicalAnd(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IOperator> ParseLogicalAnd(OperatorExpressionParserState state)
    {
        var expr = ParseEquality(state);

        while (expr.IsSuccessful() && Match(state, OperatorExpressionTokenType.And))
        {
            var op = Previous(state);
            var right = ParseEquality(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IOperator> ParseEquality(OperatorExpressionParserState state)
    {
        var expr = ParseComparison(state);

        while (expr.IsSuccessful() && Match(state, OperatorExpressionTokenType.Equal, OperatorExpressionTokenType.NotEqual))
        {
            var op = Previous(state);
            var right = ParseAdditive(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IOperator> ParseComparison(OperatorExpressionParserState state)
    {
        var expr = ParseAdditive(state);

        while (expr.IsSuccessful() && Match(state, OperatorExpressionTokenType.Less, OperatorExpressionTokenType.LessOrEqual, OperatorExpressionTokenType.Greater, OperatorExpressionTokenType.GreaterOrEqual))
        {
            var op = Previous(state);
            var right = ParseAdditive(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IOperator> ParseAdditive(OperatorExpressionParserState state)
    {
        var expr = ParseMultiplicative(state);

        while (expr.IsSuccessful() && Match(state, OperatorExpressionTokenType.Plus, OperatorExpressionTokenType.Minus))
        {
            var op = Previous(state);
            var right = ParseMultiplicative(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IOperator> ParseMultiplicative(OperatorExpressionParserState state)
    {
        var expr = ParseUnary(state);

        while (expr.IsSuccessful() && Match(state, OperatorExpressionTokenType.Multiply, OperatorExpressionTokenType.Divide, OperatorExpressionTokenType.Modulo))
        {
            var op = Previous(state);
            var right = ParseUnary(state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IOperator>(new BinaryOperator(expr, op.Type, right, op.Value));
        }

        return expr;
    }

    private static Result<IOperator> ParseUnary(OperatorExpressionParserState state)
    {
        if (Match(state, OperatorExpressionTokenType.Bang))
        {
            var right = ParseUnary(state); // right-associative
            if (!right.IsSuccessful())
            {
                return right;
            }

            return Result.Success<IOperator>(new UnaryOperator(right));
        }

        return ParsePrimary(state);
    }

    private static Result<IOperator> ParsePrimary(OperatorExpressionParserState state)
    {
        if (Match(state, OperatorExpressionTokenType.Other))
        {
            var peek = Peek(state);
            if (peek.Type == OperatorExpressionTokenType.LeftParenthesis)
            {
                return ParseFunction(state, ref peek);
            }
            else if (peek.Type == OperatorExpressionTokenType.Less
                && PeekNext(state, 1).Type == OperatorExpressionTokenType.Other
                && PeekNext(state, 2).Type == OperatorExpressionTokenType.Greater
                && PeekNext(state, 3).Type == OperatorExpressionTokenType.LeftParenthesis)
            {
                return ParseGenericFunction(state);
            }

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

    private static Result<IOperator> ParseFunction(OperatorExpressionParserState state, ref OperatorExpressionToken peek)
    {
        if (PeekNext(state, 1).Type == OperatorExpressionTokenType.EOF)
        {
            return Result.Invalid<IOperator>("Missing right parenthesis");
        }

        var builder = new StringBuilder();
        builder.Append(Previous(state).Value);
        builder.Append(peek.Value);

        while (peek.Type != OperatorExpressionTokenType.RightParenthesis && peek.Type != OperatorExpressionTokenType.EOF)
        {
            Advance(state);
            peek = Peek(state);
            builder.Append(peek.Value);
        }

        return Result.Success<IOperator>(new ExpressionOperator(builder.ToString()));
    }

    private static Result<IOperator> ParseGenericFunction(OperatorExpressionParserState state)
    {
        //format: function<type>(
        //a.k.a. other, less, other, greater, left parenthesis
        var afterParenthesis = PeekNext(state, 4);
        if (afterParenthesis.Type == OperatorExpressionTokenType.EOF)
        {
            return Result.Invalid<IOperator>("Missing right parenthesis");
        }
        else if (afterParenthesis.Type == OperatorExpressionTokenType.Other && PeekNext(state, 5).Type == OperatorExpressionTokenType.EOF)
        {
            return Result.Invalid<IOperator>("Missing right parenthesis");
        }
        else
        {
            var numberOfItemsToTake = afterParenthesis.Type == OperatorExpressionTokenType.Other
                ? 7
                : 6;
            var result = Result.Success<IOperator>(new ExpressionOperator(string.Concat(state.Tokens.Skip(state.Position - 1).Take(numberOfItemsToTake).Select(x => x.Value))));
            state.Position += numberOfItemsToTake - 1;
            return result;
        }
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
    private static OperatorExpressionToken PeekNext(OperatorExpressionParserState state, int increase) => state.Tokens.ElementAt(state.Position + increase);

    private static Result<OperatorExpressionToken> Consume(OperatorExpressionParserState state, OperatorExpressionTokenType type, string message)
    {
        if (!Check(state, type))
        {
            return Result.Invalid<OperatorExpressionToken>(message);
        }

        return Advance(state);
    }
}
