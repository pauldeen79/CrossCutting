﻿namespace CrossCutting.Utilities.Operators;

public static class Equal
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue, StringComparison stringComparison)
    {
        if (leftValue is null && rightValue is null)
        {
            return Result.Success(true);
        }

        if (leftValue is null || rightValue is null)
        {
            return Result.Success(false);
        }

        if (leftValue is string leftString && rightValue is string rightString)
        {
            return Result.Success(leftString.Equals(rightString, stringComparison));
        }

        return Result.Success(leftValue.Equals(rightValue));
    }
}
