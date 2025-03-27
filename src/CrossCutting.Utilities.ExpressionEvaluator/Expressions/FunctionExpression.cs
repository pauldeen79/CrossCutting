namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class FunctionExpression : IExpression
{
    //TODO: Add IFunction instances as read-only private member, inject using constructor injection.

    private static readonly Regex _functionRegEx = new(@"\b\w*\s*(?:<[\w\s,.<>]*>)?\s*\(\s*[^)]*\s*\)", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    public int Order => 20;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var functionCallResult = ParseFunctionCall(context);
        if (functionCallResult.Status == ResultStatus.NotFound)
        {
            return Result.Continue<object?>();
        }
        else if (!functionCallResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(functionCallResult);
        }

        //TODO: Find the right function, and return the result of the Evaluate method. Return NotFound when it couldn't be resolved.
        throw new NotImplementedException();
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var functionCallResult = ParseFunctionCall(context);
        if (functionCallResult.Status == ResultStatus.NotFound)
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Continue)
                .WithExpressionType(typeof(FunctionExpression))
                .WithSourceExpression(context.Expression);
        }
        else if (!functionCallResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(functionCallResult.Status)
                .WithErrorMessage(functionCallResult.ErrorMessage)
                .AddValidationErrors(functionCallResult.ValidationErrors)
                .WithExpressionType(typeof(FunctionExpression))
                .WithSourceExpression(context.Expression);
        }

        //TODO: Find the right function, and use the ResultType property of the result of the Parse method. Return NotFound when it couldn't be resolved.
        return new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.Ok)
            .WithExpressionType(typeof(FunctionExpression))
            .WithSourceExpression(context.Expression);
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
                    argumentsStarted = c == '(';
                    genericsStarted = c == '<';
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
            else if (!argumentsComplete)
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
                        var arg = argumentBuilder.ToString().Trim();
                        if (!string.IsNullOrEmpty(arg))
                        {
                            arguments.Add(arg);
                        }
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
                        arguments.Add(argumentBuilder.ToString().Trim());
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
            else if (index + 1 < context.Expression.Length)
            {
                // remaining characters at the end, like MyFunction(a) ILLEGAL
                return Result.Invalid<FunctionCall>("Input has additional characters after last close bracket");
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

        return Result.Success<FunctionCall>(new FunctionCallBuilder()
            .WithName(nameBuilder.ToString().Trim())
            .AddArguments(arguments)
            .AddTypeArguments(genericTypeArgumentsResult.Value!));
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
}
