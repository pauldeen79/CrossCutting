namespace CrossCutting.Utilities.Parsers;

public class VariableProcessor : IVariableProcessor
{
    private readonly IEnumerable<IVariable> _variables;

    public VariableProcessor(IEnumerable<IVariable> variables)
    {
        ArgumentGuard.IsNotNull(variables, nameof(variables));

        _variables = variables;
    }

    public Result<object?> Process(string variable, object? context)
        => _variables
            .Select(x => x.Process(variable, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotFound<object?>($"Unknown variable found: {variable}");
}
