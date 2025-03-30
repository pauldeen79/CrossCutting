namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class FunctionExpression : IExpression
{
    private readonly IFunctionCallArgumentValidator _functionCallArgumentValidator;
    private readonly IEnumerable<IFunction> _functions;
    private readonly IEnumerable<IGenericFunction> _genericFunctions;

    private static readonly Func<FunctionParseState, Result>[] _processors =
        [
            ProcessNameSection
        ];

    public FunctionExpression(IFunctionDescriptorProvider functionDescriptorProvider, IFunctionCallArgumentValidator functionCallArgumentValidator, IEnumerable<IFunction> functions, IEnumerable<IGenericFunction> genericFunctions)
    {
        ArgumentGuard.IsNotNull(functionDescriptorProvider, nameof(functionDescriptorProvider));
        ArgumentGuard.IsNotNull(functionCallArgumentValidator, nameof(functionCallArgumentValidator));
        ArgumentGuard.IsNotNull(functions, nameof(functions));
        ArgumentGuard.IsNotNull(genericFunctions, nameof(genericFunctions));

        _functionCallArgumentValidator = functionCallArgumentValidator;
        _functions = functions;
        _genericFunctions = genericFunctions;
        _descriptors = new Lazy<IReadOnlyCollection<FunctionDescriptor>>(() => functionDescriptorProvider.GetAll());
    }

    private static readonly Regex _functionRegEx = new(@"\b\w*\s*(?:<[\w\s,.<>]*>)?\s*\(\s*[^)]*\s*\)", RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(250));

    private readonly Lazy<IReadOnlyCollection<FunctionDescriptor>> _descriptors;

    public IReadOnlyCollection<FunctionDescriptor> Descriptors => _descriptors.Value;

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

        var functionCallContext = new FunctionCallContext(functionCallResult.GetValueOrThrow(), context);

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

        var functionCallContext = new FunctionCallContext(functionCallResult.GetValueOrThrow(), context);
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
            .WithResultType(resolveResult.GetValueOrThrow().ReturnValueType);
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
            0 => Result.NotFound<FunctionAndTypeDescriptor>($"No overload of the {functionCallContext.FunctionCall.Name} function takes {functionCallContext.FunctionCall.Arguments.Count} arguments"),
            1 => GetFunctionByDescriptor(functionCallContext, functionsWithRightArgumentCount[0]),
            _ => Result.NotFound<FunctionAndTypeDescriptor>($"Function {functionCallContext.FunctionCall.Name} with {functionCallContext.FunctionCall.Arguments.Count} arguments could not be identified uniquely")
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
            return EvaluateGenericFunction(result.GenericFunction, functionCallContext);
        }

        // We can safely assume that Function is not null, because the c'tor has verified this
        return result.Function!.Evaluate(functionCallContext);
    }

    private static Result<object?> EvaluateGenericFunction(IGenericFunction genericFunction, FunctionCallContext functionCallContext)
    {
        try
        {
            var method = genericFunction.GetType().GetMethod(nameof(IGenericFunction.EvaluateGeneric))!.MakeGenericMethod(functionCallContext.FunctionCall.TypeArguments.ToArray());

            return (Result<object?>)method.Invoke(genericFunction, [functionCallContext]);
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

    private sealed class FunctionParseState
    {
        public StringBuilder NameBuilder { get; } = new StringBuilder();
        public StringBuilder GenericsBuilder { get; } = new StringBuilder();
        public StringBuilder ArgumentBuilder { get; } = new StringBuilder();
        public List<string> Arguments { get; } = new List<string>();
        public bool NameComplete { get; set; }
        public bool GenericsStarted { get; set; }
        public bool GenericsComplete { get; set; }
        public bool ArgumentsStarted { get; set; }
        public bool ArgumentsComplete { get; set; }
        public bool InQuotes { get; set; }
        public int Index { get; set; }
        public char CurrentCharacter { get; set; }
        public int BracketCount { get; set; }
    }
}
