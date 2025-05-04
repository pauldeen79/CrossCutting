namespace CrossCutting.Utilities.ExpressionEvaluator;

public sealed class DotExpressionComponentState
{
    private readonly IFunctionParser _functionParser;

    private static readonly Regex _propertyNameRegEx = new Regex("^[A-Za-z]+$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

    public ExpressionEvaluatorContext Context { get; }
    public StringBuilder CurrentExpression { get; }

    public Type? ResultType { get; internal set; }
    public Result<Type> CurrentParseResult { get; internal set; }

    public object Value { get; internal set; }
    public Result<object?> CurrentEvaluateResult { get; internal set; }

    private string _part;
    public string Part
    {
        get
        {
            return _part;
        }
        internal set
        {
            _part = value;
            _functionParseResult = null;
            _type = null;
        }
    }

    public string PreviousPart { get; internal set; }

    private DotExpressionType? _type;
    public DotExpressionType Type
    {
        get
        {
            if (_type is null)
            {
                if (_propertyNameRegEx.IsMatch(_part))
                {
                    _type = DotExpressionType.Property;
                }
                else if (FunctionParseResult.IsSuccessful() || FunctionParseResult.Status != ResultStatus.NotFound)
                {
                    _type = DotExpressionType.Method;
                }
                else
                {
                    _type = DotExpressionType.Unknown;
                }
            }

            return _type.Value;
        }
    }

    public string Name => Type == DotExpressionType.Method
        ? FunctionParseResult.Value!.Name
        : Part;

    private Result<FunctionCall>? _functionParseResult;
    public Result<FunctionCall> FunctionParseResult
    {
        get
        {
            if (_functionParseResult is null)
            {
                _functionParseResult = _functionParser.Parse(Context.CreateChildContext(_part.IndexOf('(') == -1 ? $"{_part}()" : _part));
            }

            return _functionParseResult;
        }
    }

    public string TypeDisplayName =>
        Type == DotExpressionType.Unknown
            ? "expression"
            : Type.ToString().ToLowerInvariant();

    public DotExpressionComponentState(ExpressionEvaluatorContext context, IFunctionParser functionParser, Result<object?> result, string firstPart, Type? resultType = null)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));
        ArgumentGuard.IsNotNull(result, nameof(result));
        ArgumentGuard.IsNotNull(firstPart, nameof(firstPart));

        Context = context;
        _functionParser = functionParser;
        CurrentExpression = new StringBuilder(firstPart);
        _part = string.Empty;
        Value = default!;
        CurrentParseResult = default!;
        CurrentEvaluateResult = result;
        PreviousPart = string.Empty;
        ResultType = resultType;
    }

    public void AppendPart() => CurrentExpression.Append('.').Append(Part);
}
