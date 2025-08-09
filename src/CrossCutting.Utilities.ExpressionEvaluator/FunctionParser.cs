namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionParser : IFunctionParser
{
    private static readonly Regex _functionRegEx = new(@"(?<!\.)\b[a-zA-Z_][a-zA-Z0-9_]*\s*(?:<[\w\s,.<>]*>)?\s*\(\s*[^)]*\s*\)", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    private static readonly Func<FunctionParserState, Result>[] _processors =
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
        var match = _functionRegEx.Match(context.Expression.Trim());
        if (!match.Success)
        {
            return Result.NotFound<FunctionCall>();
        }

        var expression = context.Expression.StartsWith("new ")
            ? context.Expression.Substring(4)
            : context.Expression;

        var state = new FunctionParserState(expression);

        foreach (var c in expression)
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

        var genericTypeArguments = genericTypeArgumentsResult.GetValueOrThrow();
        return Result.Success<FunctionCall>(new FunctionCallBuilder()
            .WithName(state.NameBuilder.ToString().Trim())
            .WithMemberType(GetMemberType(context.Expression, genericTypeArguments.Count, state.IsLanguageFunction))
            .AddArguments(state.Arguments)
            .AddTypeArguments(genericTypeArguments));
    }

    private static MemberType GetMemberType(string expression, int genericArgumentCount, bool isLanguageFunction)
    {
        if (expression.StartsWith("new "))
        {
            return MemberType.Constructor;
        }

        if (isLanguageFunction)
        {
            return genericArgumentCount > 0
                ? MemberType.GenericLanguageFunction
                : MemberType.LanguageFunction;
        }

        if (genericArgumentCount > 0)
        {
            return MemberType.GenericFunction;
        }

        return MemberType.Function;
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

    private static Result ProcessNameSection(FunctionParserState state)
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
            state.IsLanguageFunction = true;
            state.Arguments.Add(state.Expression.Substring(0, state.Index));
            state.NameBuilder.Clear();
        }

        return Result.Success();
    }

    private static Result ProcessGenericsSection(FunctionParserState state)
    {
        if (!state.GenericsStarted || state.GenericsComplete)
        {
            return Result.Continue();
        }

        if (state.CurrentCharacter == '<')
        {
            state.GenericsCount++;
        }

        if (state.CurrentCharacter == '>')
        {
            state.GenericsCount--;
            if (state.GenericsCount == 0)
            {
                state.GenericsComplete = true;
            }
            else
            {
                state.GenericsBuilder.Append(state.CurrentCharacter);
            }
        }
        else if (!state.IsWhiteSpace())
        {
            state.GenericsBuilder.Append(state.CurrentCharacter);
        }

        return Result.Success();
    }

    private static Result ProcessPostGenericsSection(FunctionParserState state)
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

    private static Result ProcessArgumentsSection(FunctionParserState state)
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

    private static Result ProcessPostArgumentsSection(FunctionParserState state)
        // remaining characters at the end, like MyFunction(a) ILLEGAL
        => Result.Invalid("Input has additional characters after last close bracket");
}
