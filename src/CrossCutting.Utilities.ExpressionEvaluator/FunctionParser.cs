namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionParser : IFunctionParser
{
    private static readonly Regex _functionRegEx = new(@"\b\w*\s*(?:<[\w\s,.<>]*>)?\s*\(\s*[^)]*\s*\)", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    private static readonly Func<FunctionParseState, Result>[] _processors =
        [
            ProcessNameSection,
            ProcessGenericsSection,
            ProcessPostGenericsSection,
            ProcessArgumentsSection,
            ProcessPostArgumentsSection
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

        var state = new FunctionParseState(context.Expression);

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
            state.CompleteName();
        }
        else if (!state.IsWhiteSpace())
        {
            state.NameBuilder.Append(state.CurrentCharacter);
        }
        else
        {
            return Result.Invalid<FunctionCall>("Function name may not contain whitespace");
        }

        return Result.Success();
    }

    private static Result ProcessGenericsSection(FunctionParseState state)
    {
        if (!state.GenericsStarted || state.GenericsComplete)
        {
            return Result.Continue();
        }

        if (state.CurrentCharacter == '>')
        {
            state.GenericsComplete = true;
        }
        else if (!state.IsWhiteSpace())
        {
            state.GenericsBuilder.Append(state.CurrentCharacter);
        }

        return Result.Success();
    }

    private static Result ProcessPostGenericsSection(FunctionParseState state)
    {
        if (!state.GenericsComplete || state.ArgumentsStarted)
        {
            return Result.Continue();
        }

        // Type arguments finished, looking for start of arguments section
        if (state.CurrentCharacter == '(')
        {
            state.ArgumentsStarted = true;
            state.BracketCount = 1;
        }

        return Result.Success();
    }

    private static Result ProcessArgumentsSection(FunctionParseState state)
    {
        if (state.ArgumentsComplete)
        {
            return Result.Continue();
        }

        if (state.CurrentCharacter == ')' && !state.InQuotes)
        {
            state.CloseBracket();
        }
        else if (state.CurrentCharacter == '(' && !state.InQuotes)
        {
            state.OpenBracket();
        }
        else if (state.CurrentCharacter == '"')
        {
            state.InQuotes = !state.InQuotes;
            state.ArgumentBuilder.Append(state.CurrentCharacter);
        }
        else if (state.CurrentCharacter == ',' && !state.InQuotes)
        {
            if (state.BracketCount == 1)
            {
                state.Arguments.Add(state.ArgumentBuilder.ToString().Trim());
                state.ArgumentBuilder.Clear();
            }
        }
        else if (!state.IsWhiteSpace() || state.InQuotes)
        {
            state.ArgumentBuilder.Append(state.CurrentCharacter);
        }

        return Result.Success();
    }

    private static Result ProcessPostArgumentsSection(FunctionParseState state)
    {
        if (state.Index < state.Expression.Length)
        {
            // remaining characters at the end, like MyFunction(a) ILLEGAL
            return Result.Invalid<FunctionCall>("Input has additional characters after last close bracket");
        }

        return Result.Success();
    }
}
