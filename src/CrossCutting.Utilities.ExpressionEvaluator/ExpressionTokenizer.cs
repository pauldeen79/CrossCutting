namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class ExpressionTokenizer : IExpressionTokenizer
{
    private static readonly char[] TokenSigns = ['+', '-', '*', '/', '(', ')', '=', '!', '<', '>', '&', '|', '%', '^'];
    private static readonly Dictionary<char, Func<ExpressionTokenizerState, Result>> Operators = new Dictionary<char, Func<ExpressionTokenizerState, Result>>
    {
        { '+', ProcessPlus },
        { '-', ProcessMinus },
        { '*', ProcessMultiply },
        { '/', ProcessDivide },
        { '(', ProcessLeftParenthesis },
        { ')', ProcessRightParenthesis },
        { '=', ProcessEqual },
        { '!', ProcessExclamation },
        { '<', ProcessSmallerThan },
        { '>', ProcessGreaterThan },
        { '&', ProcessAmpersand },
        { '|', ProcessPipe },
        { '^', ProcessCaret },
        { '%', ProcessPercent },
    };

    public Result<List<ExpressionToken>> Tokenize(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var state = new ExpressionTokenizerState(context.Expression);

        while (state.Position < state.Input.Length)
        {
            char current = state.Input[state.Position];

            if (current == '"')
            {
                state.InQuotes = !state.InQuotes;

                if (state.InQuotes)
                {
                    state.Tokens.Add(ReadOther(state));
                    state.InQuotes = false;
                    continue;
                }
                //TODO: Review if we can get here
            }

            if (char.IsWhiteSpace(current) && !state.InQuotes)
            {
                state.Position++;
                continue;
            }

            if (Operators.TryGetValue(current, out var action))
            {
                var result = action.Invoke(state);
                if (!result.IsSuccessful())
                {
                    return Result.FromExistingResult(result, state.Tokens);
                }
            }
            else
            {
                state.Tokens.Add(ReadOther(state));
            }
        }

        state.Tokens.Add(new ExpressionToken(ExpressionTokenType.EOF));
        return Result.Success(state.Tokens);
    }

    private static Result ProcessPlus(ExpressionTokenizerState state)
    {
        if (char.IsNumber(Peek(state)))
        {
            state.Tokens.Add(ReadOtherFromPlusOrMinus(state));
        }
        else
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Plus, "+"));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessMinus(ExpressionTokenizerState state)
    {
        if (char.IsNumber(Peek(state)))
        {
            state.Tokens.Add(ReadOtherFromPlusOrMinus(state));
        }
        else
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Minus, "-"));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessMultiply(ExpressionTokenizerState state)
    {
        state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Multiply, "*"));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessDivide(ExpressionTokenizerState state)
    {
        state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Divide, "/"));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessLeftParenthesis(ExpressionTokenizerState state)
    {
        state.Tokens.Add(new ExpressionToken(ExpressionTokenType.LeftParenthesis, "("));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessRightParenthesis(ExpressionTokenizerState state)
    {
        state.Tokens.Add(new ExpressionToken(ExpressionTokenType.RightParenthesis, ")"));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessEqual(ExpressionTokenizerState state)
    {
        if (Match(state, '='))
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Equal, "=="));
            return Result.Success();
        }
        else
        {
            return Result.Invalid("Unexpected '='");
        }
    }

    private static Result ProcessExclamation(ExpressionTokenizerState state)
    {
        if (Match(state, '='))
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.NotEqual, "!="));
        }
        else
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Bang, "!"));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessSmallerThan(ExpressionTokenizerState state)
    {
        if (Match(state, '='))
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.LessOrEqual, "<="));
        }
        else
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Less, "<"));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessGreaterThan(ExpressionTokenizerState state)
    {
        if (Match(state, '='))
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.GreaterOrEqual, ">="));
        }
        else
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Greater, ">"));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessAmpersand(ExpressionTokenizerState state)
    {
        if (Match(state, '&'))
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.And, "&&"));
            return Result.Success();
        }
        else
        {
            return Result.Invalid("Single '&' is not supported.");
        }
    }

    private static Result ProcessPipe(ExpressionTokenizerState state)
    {
        if (Match(state, '|'))
        {
            state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Or, "||"));
            return Result.Success();
        }
        else
        {
            return Result.Invalid("Single '|' is not supported.");
        }
    }

    private static Result ProcessCaret(ExpressionTokenizerState state)
    {
        state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Exponentiation, "^"));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessPercent(ExpressionTokenizerState state)
    {
        state.Tokens.Add(new ExpressionToken(ExpressionTokenType.Modulo, "%"));
        state.Position++;

        return Result.Success();
    }

    private static ExpressionToken ReadOther(ExpressionTokenizerState state)
    {
        var start = state.Position;
        while (state.Position < state.Input.Length && !IsTokenSign(state.Input[state.Position], state.InQuotes))
        {
            if (state.Input[state.Position] == '"' && state.Position != start)
            {
                state.InQuotes = !state.InQuotes;
            }

            state.Position++;
        }

        var value = state.Input.Substring(start, state.Position - start).Trim(' ', '\t', '\r', '\n');
        return new ExpressionToken(ExpressionTokenType.Other, value);
    }

    private static ExpressionToken ReadOtherFromPlusOrMinus(ExpressionTokenizerState state)
    {
        var start = state.Position;
        while (state.Position < state.Input.Length && (state.Position == start || !IsTokenSign(state.Input[state.Position], state.InQuotes)))
        {
            state.Position++;
        }

        var value = state.Input.Substring(start, state.Position - start).Trim(' ', '\t', '\r', '\n');
        return new ExpressionToken(ExpressionTokenType.Other, value);
    }

    private static bool IsTokenSign(char current, bool inQuotes)
    {
        if (inQuotes)
        {
            return false;
        }

        return TokenSigns.Contains(current);
    }

    private static bool Match(ExpressionTokenizerState state, char expected)
    {
        if (state.Position + 1 >= state.Input.Length || state.Input[state.Position + 1] != expected)
        {
            return false;
        }

        state.Position += 2;
        return true;
    }

    private static char Peek(ExpressionTokenizerState state)
    {
        if (state.Position + 1 >= state.Input.Length)
        {
            // note we are currently returning a space.
            // this does not really matter, because the calling method will check for numeric values ;-)
            return ' ';
        }

        return state.Input[state.Position + 1];
    }
}
