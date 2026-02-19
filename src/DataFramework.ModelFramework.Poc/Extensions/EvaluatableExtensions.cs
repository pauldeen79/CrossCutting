using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Utilities.ExpressionEvaluator.Abstractions;
using CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;
using CrossCutting.Utilities.ExpressionEvaluator.Extensions;

namespace DataFramework.ModelFramework.Poc.Extensions;

public static class EvaluatableExtensions
{
    public static bool IsValidExpression(this IEvaluatable evaluatable, IEnumerable<string> validFieldNames)
         => evaluatable.GetContainedEvaluatables(true).All(x => IsValidExpressionRecursive(x, validFieldNames));

    private static bool IsValidExpressionRecursive(IEvaluatable evaluatable, IEnumerable<string> validFieldNames)
    {
        if (evaluatable is PropertyNameEvaluatable propertyNameEvaluatable)
        {
            return validFieldNames.Any(s => s.Equals(propertyNameEvaluatable.PropertyName, StringComparison.Ordinal));
        }

        // You might want to validate the expression to prevent sql injection (unless you can only create query expressions in code)
        return evaluatable.GetType().Assembly.FullName.StartsWith("CrossCutting.Utilities.QueryEvaluator");
    }
}