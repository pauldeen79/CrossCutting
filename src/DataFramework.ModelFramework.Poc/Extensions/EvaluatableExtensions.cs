using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Utilities.ExpressionEvaluator.Abstractions;
using CrossCutting.Utilities.ExpressionEvaluator.Extensions;
using CrossCutting.Utilities.QueryEvaluator.Core.Evaluatables;

namespace DataFramework.ModelFramework.Poc.Extensions;

public static class EvaluatableExtensions
{
        public static bool IsValidExpression(this IEvaluatable evaluatable, IEnumerable<string> validFieldNames)
        {
            foreach (var childEvaluatable in evaluatable.GetContainedEvaluatables(true))
            {
                if (!IsValidExpressionCore(childEvaluatable, validFieldNames))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsValidExpressionCore(IEvaluatable evaluatable, IEnumerable<string> validFieldNames)
        {
            if (evaluatable is PropertyNameEvaluatable propertyNameEvaluatable)
            {
                var result = false;

                return result || validFieldNames.Any(s => s.Equals(propertyNameEvaluatable.PropertyName, StringComparison.Ordinal));
            }

            // You might want to validate the expression to prevent sql injection (unless you can only create query expressions in code)
            return evaluatable.GetType().Assembly.FullName.StartsWith("CrossCutting.Utilities.QueryEvaluator");
        }
}