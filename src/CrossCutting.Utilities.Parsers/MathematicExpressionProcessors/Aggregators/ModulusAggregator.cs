﻿namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors.Aggregators;

public class ModulusAggregator() : AggregatorBase('%', 2)
{
    public override Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider)
        => Modulus.Evaluate(value1, value2, formatProvider);
}
