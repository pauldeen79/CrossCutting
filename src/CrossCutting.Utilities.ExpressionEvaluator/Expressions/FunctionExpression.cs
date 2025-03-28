namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class FunctionExpression : IExpression
{
    private readonly IFunctionDescriptorProvider _functionDescriptorProvider;
    private readonly IFunctionCallArgumentValidator _functionCallArgumentValidator;
    private readonly IEnumerable<IFunction> _functions;
    private readonly IEnumerable<IGenericFunction> _genericFunctions;

    public FunctionExpression(IFunctionDescriptorProvider functionDescriptorProvider, IFunctionCallArgumentValidator functionCallArgumentValidator, IEnumerable<IFunction> functions, IEnumerable<IGenericFunction> genericFunctions)
    {
        ArgumentGuard.IsNotNull(functionDescriptorProvider, nameof(functionDescriptorProvider));
        ArgumentGuard.IsNotNull(functionCallArgumentValidator, nameof(functionCallArgumentValidator));
        ArgumentGuard.IsNotNull(functions, nameof(functions));
        ArgumentGuard.IsNotNull(genericFunctions, nameof(genericFunctions));

        _functionDescriptorProvider = functionDescriptorProvider;
        _functionCallArgumentValidator = functionCallArgumentValidator;
        _functions = functions;
        _genericFunctions = genericFunctions;
    }

    private static readonly Regex _functionRegEx = new(@"\b\w*\s*(?:<[\w\s,.<>]*>)?\s*\(\s*[^)]*\s*\)", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    private IReadOnlyCollection<FunctionDescriptor>? _descriptors;
    private IReadOnlyCollection<FunctionDescriptor> Descriptors
    {
        get
        {
            if (_descriptors is null)
            {
                _descriptors = _functionDescriptorProvider.GetAll();
            }
            return _descriptors;
        }
    }

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

        var functionCallContext = new FunctionCallContext(functionCallResult.Value!, context);

        return ResolveFunction(functionCallContext).Transform(result => EvaluateFunction(result, functionCallContext));
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

        var functionCallContext = new FunctionCallContext(functionCallResult.Value!, context);
        var resolveResult = ResolveFunction(functionCallContext);

        if (!resolveResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(resolveResult.Status)
                .WithErrorMessage(resolveResult.ErrorMessage)
                .AddValidationErrors(resolveResult.ValidationErrors)
                .WithExpressionType(typeof(FunctionExpression))
                .WithSourceExpression(context.Expression);
        }

        return new ExpressionParseResultBuilder()
            .WithStatus(ResultStatus.Ok)
            .WithExpressionType(typeof(FunctionExpression))
            .WithSourceExpression(context.Expression)
            .WithResultType(resolveResult.Value!.ReturnValueType);
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

        //TODO: Create private state class with these variables.
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

        //TODO: Refactor to context.Expression.Select((character, index) => _states.Select(x => x(character, index)).TakeWhileWithNonFirstMatching(x => x.Status != ResultStatus.Continue).Last()
        //move each if/else if to a state
        foreach (var c in context.Expression)
        {
            if (!nameComplete)
            {
                // Name section
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
            else if (genericsStarted && !genericsComplete)
            {
                // Type arguments section
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
                // Type arguments finished, looking for start of arguments section
                if (c == '(')
                {
                    argumentsStarted = true;
                    bracketCount = 1;
                }
            }
            else if (!argumentsComplete)
            {
                // Arguments section
                if (c == ')' && !inQuotes)
                {
                    bracketCount--;
                    if (bracketCount == 0)
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
                }
                else if ((c != ' ' && c != '\r' && c != '\n' && c != '\t') || inQuotes)
                {
                    argumentBuilder.Append(c);
                }
            }
            else if (index < context.Expression.Length)
            {
                // remaining characters at the end, like MyFunction(a) ILLEGAL
                return Result.Invalid<FunctionCall>("Input has additional characters after last close bracket");
            }

            index++;
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

    private Result<FunctionAndTypeDescriptor> ResolveFunction(FunctionCallContext functionCallContext)
    {
        var functionsByName = Descriptors
            .Where(x => x.Name.Equals(functionCallContext.FunctionCall.Name, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        if (functionsByName.Length == 0)
        {
            return Result.NotFound<FunctionAndTypeDescriptor>($"Unknown function: {functionCallContext.FunctionCall.Name}");
        }

        var functionsWithRightArgumentCount = functionsByName.Length == 1
            ? functionsByName.Where(x => functionCallContext.FunctionCall.Arguments.Count >= x.Arguments.Count(x => x.IsRequired)).ToArray()
            : functionsByName.Where(x => functionCallContext.FunctionCall.Arguments.Count == x.Arguments.Count).ToArray();

        return functionsWithRightArgumentCount.Length switch
        {
            0 => Result.Invalid<FunctionAndTypeDescriptor>($"No overload of the {functionCallContext.FunctionCall.Name} function takes {functionCallContext.FunctionCall.Arguments.Count} arguments"),
            1 => GetFunctionByDescriptor(functionCallContext, functionsWithRightArgumentCount[0]),
            _ => Result.Invalid<FunctionAndTypeDescriptor>($"Function {functionCallContext.FunctionCall.Name} with {functionCallContext.FunctionCall.Arguments.Count} arguments could not be identified uniquely")
        };
    }

    private Result<FunctionAndTypeDescriptor> GetFunctionByDescriptor(FunctionCallContext functionCallContext, FunctionDescriptor functionDescriptor)
    {
        IGenericFunction? genericFunction = null;

        var function = _functions.FirstOrDefault(x => x.GetType() == functionDescriptor.FunctionType);

        if (function is null)
        {
            genericFunction = _genericFunctions.FirstOrDefault(x => x.GetType() == functionDescriptor.FunctionType);
            if (genericFunction is null)
            {
                return Result.NotFound<FunctionAndTypeDescriptor>($"Could not find function with type name {functionDescriptor.FunctionType.FullName}");
            }
        }

        var arguments = functionDescriptor.Arguments.Zip(functionCallContext.FunctionCall.Arguments, (descriptor, call) => new { DescriptorArgument = descriptor, CallArgument = call });

        var errors = new List<Result>();
        foreach (var argument in arguments)
        {
            var validationResult = _functionCallArgumentValidator.Validate(argument.DescriptorArgument, argument.CallArgument, functionCallContext);
            if (!validationResult.Status.IsSuccessful())
            {
                errors.Add(validationResult.ToResult());
            }
        }

        if (errors.Count > 0)
        {
            return Result.Invalid<FunctionAndTypeDescriptor>($"Validation of function {functionCallContext.FunctionCall.Name} failed, see inner results for more details", errors);
        }

        return Result.Success(new FunctionAndTypeDescriptor(function, genericFunction, functionDescriptor.ReturnValueType));
    }

    private static Result<object?> EvaluateFunction(FunctionAndTypeDescriptor result, FunctionCallContext functionCallContext)
    {
        if (result.GenericFunction is not null)
        {
            return EvaluateGenericFunction(result, functionCallContext);
        }

        // We can safely assume that Function is not null, because the c'tor has verified this
        return result.Function!.Evaluate(functionCallContext);
    }

    private static Result<object?> EvaluateGenericFunction(FunctionAndTypeDescriptor result, FunctionCallContext functionCallContext)
    {
        try
        {
            var method = result.GenericFunction!.GetType().GetMethod(nameof(IGenericFunction.EvaluateGeneric))!.MakeGenericMethod(functionCallContext.FunctionCall.TypeArguments.ToArray());

            return (Result<object?>)method.Invoke(result.GenericFunction, [functionCallContext]);
        }
        catch (ArgumentException argException)
        {
            //The type or method has 1 generic parameter(s), but 0 generic argument(s) were provided. A generic argument must be provided for each generic parameter.
            return Result.Invalid<object?>(argException.Message);
        }
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
