﻿namespace CrossCutting.Utilities.Parsers.ExpressionStrings.Operators;

public class EqualOperator : OperatorExpressionProcessorBase
{
    protected override string Sign => "==";

    protected override Result<bool> PerformOperator(object? leftValue, object? rightValue)
        => Equal.Evaluate(leftValue, rightValue, StringComparison.CurrentCultureIgnoreCase);
}
