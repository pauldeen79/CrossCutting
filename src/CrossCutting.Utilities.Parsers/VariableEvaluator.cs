﻿namespace CrossCutting.Utilities.Parsers;

public class VariableEvaluator : IVariableEvaluator
{
    private readonly IEnumerable<IVariable> _variables;

    public VariableEvaluator(IEnumerable<IVariable> variables)
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
            .WhenNull(ResultStatus.Invalid, $"Unknown variable found: {expression}");
    }

    public Result<Type> Validate(string expression, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Invalid<Type>("Variable is required");
        }

        return _variables
            .Select(x => x.Validate(expression, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
            .WhenNull(ResultStatus.Invalid, $"Unknown variable found: {expression}");
    }
}
