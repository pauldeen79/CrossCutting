﻿namespace CrossCutting.Utilities.Operators;

public static class GreaterOrEqualThan
{
    public static Result<bool> Evaluate(object? leftValue, object? rightValue)
        => Comparable.Evaluate(leftValue, rightValue, result => result >= 0);
}
