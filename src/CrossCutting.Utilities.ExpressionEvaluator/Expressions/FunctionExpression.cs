namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class FunctionExpression : IExpression
{
    //private static readonly Regex _functionRegEx = new(@"\b(?<FunctionName>\w+)\s*(?<Generics><[\w\s,.<>]*>)?\s*\(", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));
    private static readonly Regex _functionRegEx = new(@"\b\w*\s*(?:<[\w\s,.<>]*>)?\s*\(\s*[^)]*\s*\)", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    private const string TemporaryDelimiter = "\uE002";

    public int Order => 20;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var functionCall = ParseFunctionCall(context);

        throw new NotImplementedException();
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var match = _functionRegEx.Match(context.Expression);
        if (!match.Success)
        {
            return new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue).WithExpressionType(typeof(FunctionExpression)).WithSourceExpression(context.Expression);
        }

        throw new NotImplementedException();
    }

    public static Result<FunctionCall> ParseFunctionCall(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        // First, do a RegEx probe. If this does not match, then don't bother.
        var match = _functionRegEx.Match(context.Expression);
        if (!match.Success)
        {
            return Result.NotFound<FunctionCall>();
        }

        if (context.Expression.Contains(TemporaryDelimiter))
        {
            return Result.NotSupported<FunctionCall>($"Input cannot contain {TemporaryDelimiter}, as this is used internally for formatting");
        }

        //var functionName = match.Groups["FunctionName"].Value;
        //var generics = match.Groups["Generics"].Success ? match.Groups["Generics"].Value : "";
        //var genericsSplit = generics.SplitDelimited(',', '\"', trimItems: true, leaveTextQualifier: true);
        var nameBuilder = new StringBuilder();
        var genericsBuilder = new StringBuilder();
        var argumentBuilder = new StringBuilder();
        var arguments = new List<string>();
        var nameComplete = false;
        var genericsStarted = false;
        var genericsComplete = false;
        var argumentsStarted = false;
        var argumentsComplete = false;
        var inQuotes = false;
        var index = 0;
        var bracketCount = 0;

        foreach (var c in context.Expression)
        {
            if (!nameComplete)
            {
                if (c == '<' || c == '(')
                {
                    nameComplete = true;
                    argumentsStarted = true;
                    bracketCount = c == '(' ? 1 : 0;
                }
                else if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
                {
                    nameBuilder.Append(c);
                }
                else
                {
                    return Result.Invalid<FunctionCall>("Function name may not contain whitespace");
                }
            }
            else if (!genericsStarted && !argumentsStarted)
            {
                if (c == '<')
                {
                    genericsStarted = true;
                }
                else if (c == '(')
                {
                    argumentsStarted = true;
                    bracketCount = 1;
                }
                else if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
                {
                    return Result.Invalid<FunctionCall>("Missing open bracket");
                }
            }
            else if (genericsStarted && !genericsComplete)
            {
                if (c == '>')
                {
                    genericsComplete = true;
                }
                else if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
                {
                    genericsBuilder.Append(c);
                }
            }
            else if (genericsComplete && !argumentsStarted)
            {
                if (c == '(')
                {
                    argumentsStarted = true;
                    bracketCount = 1;
                }
                else if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
                {
                    return Result.Invalid<FunctionCall>("Missing open bracket");
                }
            }
            else if (argumentsStarted && !argumentsComplete)
            {
                if (c == ')' && !inQuotes)
                {
                    bracketCount--;
                    if (bracketCount < 0)
                    {
                        return Result.Invalid<FunctionCall>("Too many close brackets found");
                    }
                    else if (bracketCount == 0)
                    {
                        arguments.Add(argumentBuilder.ToString());
                        argumentsComplete = true;
                    }
                    else
                    {
                        argumentBuilder.Append(c);
                    }
                }
                else if (c == '(' && !inQuotes)
                {
                    bracketCount++;
                    argumentBuilder.Append(c);
                }
                else if (c == '"')
                {
                    inQuotes = !inQuotes;
                    argumentBuilder.Append(c);
                }
                else if (c == ',' && !inQuotes)
                {
                    if (bracketCount == 1)
                    {
                        arguments.Add(argumentBuilder.ToString());
                        argumentBuilder.Clear();
                    }
                    else
                    {
                        argumentBuilder.Append(c);
                    }
                }
                else if (c != ' ' && c != '\r' && c != '\n' && c != '\t')
                {
                    argumentBuilder.Append(c);
                }
            }
            else
            {
                // Function name, generics and arguments are all complete
                // If we still find something, then the format is not valid
                if (index + 1 < context.Expression.Length)
                {
                    // remaining characters at the end, like MyFunction(a) ILLEGAL
                    return Result.Invalid<FunctionCall>("Input has additional characters after last close bracket");
                }
                break;
            }

            index++;
        }

        if (!nameComplete)
        {
            return Result.Invalid<FunctionCall>("Missing open bracket");
        }
        else if (genericsStarted && !genericsComplete)
        {
            return Result.Invalid<FunctionCall>("Generic type name is not properly ended");
        }
        else if (!argumentsComplete)
        {
            return Result.Invalid<FunctionCall>("Missing close bracket");
        }

        var generics = genericsBuilder.ToString().SplitDelimited(',', trimItems: true);
        var genericTypeArgumentsResult = GetTypeArguments(generics);
        if (!genericTypeArgumentsResult.IsSuccessful())
        {
            return Result.FromExistingResult<FunctionCall>(genericTypeArgumentsResult);
        }

        return Result.Success(new FunctionCallBuilder()
            .WithName(nameBuilder.ToString().Trim())
            .AddArguments(arguments)
            .AddTypeArguments(genericTypeArgumentsResult.Value!)
            .Build());
    }

    //private static string ExtractArguments(string input, int startIndex)
    //{
    //    int openBrackets = 1;
    //    int endIndex = startIndex;

    //    while (endIndex < input.Length && openBrackets > 0)
    //    {
    //        char c = input[endIndex];

    //        if (c == '(') openBrackets++;
    //        else if (c == ')') openBrackets--;

    //        endIndex++;
    //    }

    //    // Extract the arguments part
    //    return input.Substring(startIndex, endIndex - startIndex - 1);
    //}

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
}
