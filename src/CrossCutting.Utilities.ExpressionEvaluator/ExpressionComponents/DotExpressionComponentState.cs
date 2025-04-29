namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;

public class DotExpressionComponentState
{
    private readonly IFunctionParser _functionParser;

    private static readonly Regex _propertyNameRegEx = new Regex("^[A-Za-z]+$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));

    public ExpressionEvaluatorContext Context { get; }
    public StringBuilder CurrentExpression { get; }

    private string _part;
    public string Part
    {
        get
        {
            return _part;
        }
        set
        {
            _part = value;
            _functionParseResult = null;
        }
    }

    public object Value { get; set; }
    public Type? ResultType { get; set; }
    public DotExpressionType Type
    {
        get
        {
            if (_propertyNameRegEx.IsMatch(_part))
            {
                return DotExpressionType.Property;
            }
            else if (FunctionParseResult.IsSuccessful() || FunctionParseResult.Status != ResultStatus.NotFound)
            {
                return DotExpressionType.Method;
            }
            else
            {
                return DotExpressionType.Unknown;
            }
        }
    }

    private Result<FunctionCall>? _functionParseResult;
    public Result<FunctionCall> FunctionParseResult
    {
        get
        {
            if (_functionParseResult is null)
            {
                _functionParseResult = _functionParser.Parse(Context.CreateChildContext(_part));
            }

            return _functionParseResult;
        }
    }

    public DotExpressionComponentState(ExpressionEvaluatorContext context, IFunctionParser functionParser, string firstPart)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(functionParser, nameof(functionParser));
        ArgumentGuard.IsNotNull(firstPart, nameof(firstPart));

        Context = context;
        _functionParser = functionParser;
        CurrentExpression = new StringBuilder(firstPart);
        _part = string.Empty;
        Value = default!;
    }

    public void AppendPart() => CurrentExpression.Append('.').Append(Part);
}
