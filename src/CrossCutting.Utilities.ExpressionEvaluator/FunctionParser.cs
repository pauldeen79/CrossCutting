namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionParser : IFunctionParser
{
    private static readonly Regex _functionRegEx = new(@"\b\w*\s*(?:<[\w\s,.<>]*>)?\s*\(\s*[^)]*\s*\)", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    private static readonly Func<FunctionParseState, Result>[] _processors =
        [
            ProcessNameSection
        ];

    public Result<FunctionCall> Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        // First, do a RegEx probe. If this does not match, then don't bother.
        var match = _functionRegEx.Match(context.Expression);
        if (!match.Success)
        {
            return Result.NotFound<FunctionCall>();
        }

        var state = new FunctionParseState();
        foreach (var c in context.Expression)
        {
            state.CurrentCharacter = c;
            var result = _processors
                .Select(x => x.Invoke(state))
                .TakeWhileWithFirstNonMatching(x => x.Status == ResultStatus.Continue)
                .Last();

            if (!result.IsSuccessful())
            {
                return Result.FromExistingResult<FunctionCall>(result);
            }

            // TODO: Remove this
            if (result.Status != ResultStatus.Continue)
            {
                continue;
            }

            // TODO: Move to process methods like ProcessNameSection
            if (state.GenericsStarted && !state.GenericsComplete)
            {
                // Type arguments section
                if (c == '>')
                {
                    state.GenericsComplete = true;
                }
                else if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
                {
                    state.GenericsBuilder.Append(c);
                }
            }
            else if (state.GenericsComplete && !state.ArgumentsStarted)
            {
                // Type arguments finished, looking for start of arguments section
                if (c == '(')
                {
                    state.ArgumentsStarted = true;
                    state.BracketCount = 1;
                }
            }
            else if (!state.ArgumentsComplete)
            {
                // Arguments section
                if (c == ')' && !state.InQuotes)
                {
                    state.BracketCount--;
                    if (state.BracketCount == 0)
                    {
                        var arg = state.ArgumentBuilder.ToString().Trim();
                        if (!string.IsNullOrEmpty(arg))
                        {
                            state.Arguments.Add(arg);
                        }
                        state.ArgumentsComplete = true;
                    }
                    else
                    {
                        state.ArgumentBuilder.Append(c);
                    }
                }
                else if (c == '(' && !state.InQuotes)
                {
                    state.BracketCount++;
                    state.ArgumentBuilder.Append(c);
                }
                else if (c == '"')
                {
                    state.InQuotes = !state.InQuotes;
                    state.ArgumentBuilder.Append(c);
                }
                else if (c == ',' && !state.InQuotes)
                {
                    if (state.BracketCount == 1)
                    {
                        state.Arguments.Add(state.ArgumentBuilder.ToString().Trim());
                        state.ArgumentBuilder.Clear();
                    }
                }
                else if ((c != ' ' && c != '\r' && c != '\n' && c != '\t') || state.InQuotes)
                {
                    state.ArgumentBuilder.Append(c);
                }
            }
            else if (state.Index < context.Expression.Length)
            {
                // remaining characters at the end, like MyFunction(a) ILLEGAL
                return Result.Invalid<FunctionCall>("Input has additional characters after last close bracket");
            }

            state.Index++;
        }

        var generics = state.GenericsBuilder.ToString().SplitDelimited(',', trimItems: true);
        var genericTypeArgumentsResult = GetTypeArguments(generics);
        if (!genericTypeArgumentsResult.IsSuccessful())
        {
            return Result.FromExistingResult<FunctionCall>(genericTypeArgumentsResult);
        }

        return Result.Success<FunctionCall>(new FunctionCallBuilder()
            .WithName(state.NameBuilder.ToString().Trim())
            .AddArguments(state.Arguments)
            .AddTypeArguments(genericTypeArgumentsResult.GetValueOrThrow()));
    }

    private static Result<List<Type>> GetTypeArguments(string[] generics)
    {
        var typeArguments = new List<Type>();

        foreach (var typeArgument in generics)
        {
            var type = Type.GetType(typeArgument, false);
            if (type is null)
            {
                return Result.Invalid<List<Type>>($"Unknown type: {typeArgument}");
            }

            typeArguments.Add(type);
        }

        return Result.Success(typeArguments);
    }

    private static Result ProcessNameSection(FunctionParseState state)
    {
        if (state.NameComplete)
        {
            return Result.Continue();
        }

        if (state.CurrentCharacter == '<' || state.CurrentCharacter == '(')
        {
            state.NameComplete = true;
            state.ArgumentsStarted = state.CurrentCharacter == '(';
            state.GenericsStarted = state.CurrentCharacter == '<';
            state.BracketCount = state.CurrentCharacter == '(' ? 1 : 0;
        }
        else if (state.CurrentCharacter != ' ' && state.CurrentCharacter != '\r' && state.CurrentCharacter != '\n' && state.CurrentCharacter != '\t')
        {
            state.NameBuilder.Append(state.CurrentCharacter);
        }
        else
        {
            return Result.Invalid<FunctionCall>("Function name may not contain whitespace");
        }

        return Result.NoContent();
    }
}
