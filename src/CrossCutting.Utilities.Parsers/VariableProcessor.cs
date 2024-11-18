namespace CrossCutting.Utilities.Parsers;

public class VariableProcessor : IVariableProcessor
{
    private readonly IEnumerable<IVariable> _variables;

    public VariableProcessor(IEnumerable<IVariable> variables)
    {
        ArgumentGuard.IsNotNull(variables, nameof(variables));

        _variables = variables;
    }

    public Result<object?> Process(string variableExpression, object? context)
    {
        if (string.IsNullOrEmpty(variableExpression))
        {
            return Result.Invalid<object?>("Variable is required");
        }

        return _variables
            .Select(x => x.Process(variableExpression, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown variable found: {variableExpression}");
    }
}
