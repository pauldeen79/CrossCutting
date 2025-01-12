namespace CrossCutting.Utilities.Parsers;

public class VariableProcessor : IVariableProcessor
{
    private readonly IEnumerable<IVariable> _variables;

    public VariableProcessor(IEnumerable<IVariable> variables)
    {
        ArgumentGuard.IsNotNull(variables, nameof(variables));

        _variables = variables;
    }

    public Result<object?> Evaluate(string expression, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Invalid<object?>("Variable is required");
        }

        return _variables
            .Select(x => x.Evaluate(expression, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<object?>($"Unknown variable found: {expression}");
    }

    public Result Validate(string expression, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Invalid("Variable is required");
        }

        return _variables
            .Select(x => x.Validate(expression, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid($"Unknown variable found: {expression}");
    }
}
