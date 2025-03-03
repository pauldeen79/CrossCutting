﻿namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public class NotEqualOperator : OperatorExpressionProcessorBase
{
    protected override string Sign => "!=";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => NotEqual.Evaluate(leftValue, rightValue, StringComparison.CurrentCultureIgnoreCase);
}
