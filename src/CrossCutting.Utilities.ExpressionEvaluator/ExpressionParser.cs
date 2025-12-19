namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class ExpressionParser : IExpressionParser
{
    public Result<IExpression> Parse(ExpressionEvaluatorContext context, ICollection<ExpressionToken> tokens)
        => ParseLogicalOr(context, new ExpressionParserState(tokens));

    private Result<IExpression> ParseLogicalOr(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        var expr = ParseLogicalAnd(context, state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Or))
        {
            var op = Previous(state);
            var right = ParseLogicalAnd(context, state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(op.Type == ExpressionTokenType.And
                ? new BinaryAndOperatorExpression(expr, right, op.Value)
                : new BinaryOrOperatorExpression(expr, right, op.Value));
        }

        return expr;
    }

    private Result<IExpression> ParseLogicalAnd(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        var expr = ParseEquality(context, state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.And))
        {
            var op = Previous(state);
            var right = ParseEquality(context, state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(op.Type == ExpressionTokenType.Equal
                ? new BinaryAndOperatorExpression(expr, right, op.Value)
                : new BinaryOrOperatorExpression(expr, right, op.Value));
        }

        return expr;
    }

    private Result<IExpression> ParseEquality(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        var expr = ParseComparison(context, state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Equal, ExpressionTokenType.NotEqual))
        {
            var op = Previous(state);
            var right = ParseAdditive(context, state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(op.Type == ExpressionTokenType.Equal
                ? new EqualOperatorExpression(expr, right, op.Value)
                : new NotEqualOperatorExpression(expr, right, op.Value));
        }

        return expr;
    }

    private Result<IExpression> ParseComparison(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        var expr = ParseAdditive(context, state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Less, ExpressionTokenType.LessOrEqual, ExpressionTokenType.Greater, ExpressionTokenType.GreaterOrEqual))
        {
            var op = Previous(state);
            var right = ParseAdditive(context, state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(op.Type switch
            {
                ExpressionTokenType.Less => new SmallerOperatorExpression(expr, right, op.Value),
                ExpressionTokenType.LessOrEqual => new SmallerOrEqualOperatorExpression(expr, right, op.Value),
                ExpressionTokenType.Greater => new GreaterOperatorExpression(expr, right, op.Value),
                //ExpressionTokenType.GreaterOrEqual
                _ => new GreaterOrEqualOperatorExpression(expr, right, op.Value)
            });
        }

        return expr;
    }

    private Result<IExpression> ParseAdditive(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        var expr = ParseMultiplicative(context, state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Plus, ExpressionTokenType.Minus))
        {
            var op = Previous(state);
            var right = ParseMultiplicative(context, state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(op.Type == ExpressionTokenType.Plus
                ? new AddOperatorExpression(expr, right, op.Value)
                : new SubtractOperatorExpression(expr, right, op.Value));
        }

        return expr;
    }

    private Result<IExpression> ParseMultiplicative(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        var expr = ParseExponent(context, state);

        while (expr.IsSuccessful() && Match(state, ExpressionTokenType.Multiply, ExpressionTokenType.Divide, ExpressionTokenType.Modulus))
        {
            var op = Previous(state);
            var right = ParseExponent(context, state);
            if (!right.IsSuccessful())
            {
                return right;
            }
            expr = Result.Success<IExpression>(op.Type switch
            {
                ExpressionTokenType.Multiply => new MultiplyOperatorExpression(expr, right, op.Value),
                ExpressionTokenType.Divide => new DivideOperatorExpression(expr, right, op.Value),
                //ExpressionTokenType.Modulus
                _  => new ModulusOperatorExpression(expr, right, op.Value),
            });
        }

        return expr;
    }

    private Result<IExpression> ParseExponent(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        var left = ParseUnary(context, state);

        if (!left.IsSuccessful())
            return left;

        if (Match(state, ExpressionTokenType.Exponentiation))
        {
            var op = Previous(state);
            var right = ParseExponent(context, state); // recursion = right-associative

            if (!right.IsSuccessful())
            {
                return right;
            }

            return Result.Success<IExpression>(new PowerOperatorExpression(left, right, op.Value));
        }

        return left;
    }

    private Result<IExpression> ParseUnary(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        if (Match(state, ExpressionTokenType.Bang))
        {
            var right = ParseUnary(context, state); // right-associative
            if (!right.IsSuccessful())
            {
                return right;
            }

            return Result.Success<IExpression>(new UnaryNegateOperatorExpression(right, context.Expression));
        }

        return ParsePrimary(context, state);
    }

    private Result<IExpression> ParsePrimary(ExpressionEvaluatorContext context, ExpressionParserState state)
    {
        if (Match(state, ExpressionTokenType.Other))
        {
            var peek = Peek(state);
            if (peek.Type == ExpressionTokenType.LeftParenthesis)
            {
                return ParseFunction(context, state, ref peek);
            }
            else if (peek.Type == ExpressionTokenType.Less
                && PeekNext(state, 1).Type == ExpressionTokenType.Other
                && PeekNext(state, 2).Type == ExpressionTokenType.Greater
                && PeekNext(state, 3).Type == ExpressionTokenType.LeftParenthesis)
            {
                return ParseGenericFunction(context, state);
            }

            return Result.Success<IExpression>(new OtherExpression(context, Previous(state).Value));
        }

        if (Match(state, ExpressionTokenType.LeftParenthesis))
        {
            var childState = new ExpressionParserState(state.Tokens.Skip(state.Position).ToList());
            var operatorResult = ParseLogicalOr(context, childState);
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

    private static Result<IExpression> ParseFunction(ExpressionEvaluatorContext context, ExpressionParserState state, ref ExpressionToken peek)
    {
        if (PeekNext(state, 1).Type == ExpressionTokenType.EOF)
        {
            return Result.Invalid<IExpression>("Missing right parenthesis");
        }

        var builder = new StringBuilder();
        builder.Append(Previous(state).Value);
        builder.Append(peek.Value);

        var counter = 1;
        while (peek.Type != ExpressionTokenType.EOF)
        {
            Advance(state);
            peek = Peek(state);
            builder.Append(peek.Value);

            if (peek.Type == ExpressionTokenType.LeftParenthesis)
            {
                counter++;
            }
            else if (peek.Type == ExpressionTokenType.RightParenthesis)
            {
                counter--;
                if (counter == 0)
                {
                    break;
                }
            }
        }

        // Also consume the last right parenthesis
        if (!IsAtEnd(state))
        {
            Advance(state);
        }

        return Result.Success<IExpression>(new OtherExpression(context, builder.ToString()));
    }

    private static Result<IExpression> ParseGenericFunction(ExpressionEvaluatorContext context, ExpressionParserState state)
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
            var result = Result.Success<IExpression>(new OtherExpression(context, string.Concat(state.Tokens.Skip(state.Position - 1).Take(numberOfItemsToTake).Select(x => x.Value))));
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
