﻿namespace CrossCutting.Utilities.Parsers.MathematicExpressions.Validators;

internal sealed class NullOrEmptyValidator : IMathematicExpressionValidator
{
    public Result<MathematicExpressionState> Validate(MathematicExpressionState state)
    {
        if (string.IsNullOrEmpty(state.Input))
        {
            return Result.Invalid<MathematicExpressionState>("Input cannot be null or empty");
        }

        return Result.Success(state);
    }
}
