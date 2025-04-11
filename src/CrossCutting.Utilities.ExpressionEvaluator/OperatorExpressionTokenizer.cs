namespace CrossCutting.Utilities.ExpressionEvaluator;

internal sealed class OperatorExpressionTokenizer : IOperatorExpressionTokenizer
{
    private static readonly char[] TokenSigns = ['+', '-', '*', '/', '(', ')', '=', '!', '<', '>', '&', '|'];
    private static readonly Dictionary<char, Func<OperatorExpressionTokenizerState, Result>> Processors = new Dictionary<char, Func<OperatorExpressionTokenizerState, Result>>
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
    };

    public Result<List<OperatorExpressionToken>> Tokenize(string input)
    {
        var state = new OperatorExpressionTokenizerState(input);

        while (state.Position < state.Input.Length)
        {
            char current = state.Input[state.Position];

            if (current == '"')
            {
                state.InQuotes = !state.InQuotes;
            }

            if (char.IsWhiteSpace(current) && !state.InQuotes)
            {
                state.Position++;
                continue;
            }

            if (state.InQuotes)
            {
                state.Tokens.Add(ReadOther(state, true));
                state.InQuotes = false;
                continue;
            }

            if (Processors.TryGetValue(current, out var action))
            {
                var result = action.Invoke(state);
                if (!result.IsSuccessful())
                {
                    return Result.FromExistingResult(result, state.Tokens);
                }
            }
            else
            {
                state.Tokens.Add(ReadOther(state, false));
            }
        }

        state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.EOF));
        return Result.Success(state.Tokens);
    }

    private static Result ProcessPlus(OperatorExpressionTokenizerState state)
    {
        if (char.IsNumber(Peek(state)))
        {
            state.Tokens.Add(ReadOtherFromPlusOrMinus(state));
        }
        else
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.Plus));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessMinus(OperatorExpressionTokenizerState state)
    {
        if (char.IsNumber(Peek(state)))
        {
            state.Tokens.Add(ReadOtherFromPlusOrMinus(state));
        }
        else
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.Minus));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessMultiply(OperatorExpressionTokenizerState state)
    {
        state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.Multiply));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessDivide(OperatorExpressionTokenizerState state)
    {
        state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.Divide));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessLeftParenthesis(OperatorExpressionTokenizerState state)
    {
        state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.LeftParenthesis));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessRightParenthesis(OperatorExpressionTokenizerState state)
    {
        state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.RightParenthesis));
        state.Position++;

        return Result.Success();
    }

    private static Result ProcessEqual(OperatorExpressionTokenizerState state)
    {
        if (Match(state, '='))
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.EqualEqual));
            return Result.Success();
        }
        else
        {
            return Result.Invalid("Unexpected '='");
        }
    }

    private static Result ProcessExclamation(OperatorExpressionTokenizerState state)
    {
        if (Match(state, '='))
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.NotEqual));
        }
        else
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.Bang));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessSmallerThan(OperatorExpressionTokenizerState state)
    {
        if (Match(state, '='))
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.LessEqual));
        }
        else
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.Less));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessGreaterThan(OperatorExpressionTokenizerState state)
    {
        if (Match(state, '='))
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.GreaterEqual));
        }
        else
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.Greater));
            state.Position++;
        }

        return Result.Success();
    }

    private static Result ProcessAmpersand(OperatorExpressionTokenizerState state)
    {
        if (Match(state, '&'))
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.AndAnd));
            return Result.Success();
        }
        else
        {
            return Result.Invalid("Single '&' is not supported.");
        }
    }

    private static Result ProcessPipe(OperatorExpressionTokenizerState state)
    {
        if (Match(state, '|'))
        {
            state.Tokens.Add(new OperatorExpressionToken(OperatorExpressionTokenType.OrOr));
            return Result.Success();
        }
        else
        {
            return Result.Invalid("Single '|' is not supported.");
        }
    }

    private static OperatorExpressionToken ReadOther(OperatorExpressionTokenizerState state, bool inQuotes)
    {
        var start = state.Position;
        while (state.Position < state.Input.Length && !IsTokenSign(state.Input[state.Position], inQuotes))
        {
            state.Position++;
        }

        var value = state.Input.Substring(start, state.Position - start);
        return new OperatorExpressionToken(OperatorExpressionTokenType.Expression, value);
    }

    private static OperatorExpressionToken ReadOtherFromPlusOrMinus(OperatorExpressionTokenizerState state)
    {
        var start = state.Position;
        while (state.Position < state.Input.Length && (state.Position == start || !IsTokenSign(state.Input[state.Position], false)))
        {
            state.Position++;
        }

        var value = state.Input.Substring(start, state.Position - start);
        return new OperatorExpressionToken(OperatorExpressionTokenType.Expression, value);
    }

    private static bool IsTokenSign(char current, bool inQuotes)
    {
        if (inQuotes)
        {
            return false;
        }

        return TokenSigns.Contains(current);
    }

    private static bool Match(OperatorExpressionTokenizerState state, char expected)
    {
        if (state.Position + 1 >= state.Input.Length || state.Input[state.Position + 1] != expected)
        {
            return false;
        }

        state.Position += 2;
        return true;
    }

    private static char Peek(OperatorExpressionTokenizerState state)
    {
        if (state.Position + 1 >= state.Input.Length)
        {
            return ' ';
        }

        return state.Input[state.Position + 1];
    }
}
