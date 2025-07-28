using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders;
using CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders.Extensions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Expressions;

namespace CrossCutting.Utilities.QueryEvaluator.QueryParsers;

public class SingleEntityQueryParser<TQueryBuilder, TQueryExpressionBuilder> : IQueryParser<TQueryBuilder>
    where TQueryBuilder : IQueryBuilder
    where TQueryExpressionBuilder : IExpressionBuilder, new()
{
    private readonly Func<TQueryExpressionBuilder> _defaultFieldExpressionBuilderFactory;

    public SingleEntityQueryParser(Func<TQueryExpressionBuilder> defaultFieldExpressionBuilderFactory)
    {
        ArgumentGuard.IsNotNull(defaultFieldExpressionBuilderFactory, nameof(defaultFieldExpressionBuilderFactory));
        _defaultFieldExpressionBuilderFactory = defaultFieldExpressionBuilderFactory;
    }

    public TQueryBuilder Parse(TQueryBuilder builder, string queryString)
    {
        builder = ArgumentGuard.IsNotNull(builder, nameof(builder));
        queryString = ArgumentGuard.IsNotNull(queryString, nameof(queryString));

        var items = queryString
            .Replace("\r\n", " ")
            .Replace("\n", " ")
            .Replace("\t", " ")
            .SplitDelimited(' ', '\"');

        return builder.AddConditions(PerformQuerySearch(items) ?? PerformSimpleSearch(items));
    }

    private static List<IConditionBuilder>? PerformQuerySearch(string[] items)
    {
        var itemCountIsCorrect = (items.Length - 3) % 4 == 0;
        if (!itemCountIsCorrect)
        {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            return default;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
        }
        var result = new List<IConditionBuilder>();
        for (int i = 0; i < items.Length; i += 4)
        {
            //verify that:
            //-items[i] needs to be a valid fieldname
            //-items[i + 1] needs to be a valid operator
            //-items[i + 3] needs to be a valid combination for the next condition
            var fieldName = items[i];
            var fieldValue = items[i + 2];
            var @operator = items[i + 1];

            //remove brackets and set bracket property values for this query item.
            if (fieldName.StartsWith("("))
            {
                fieldName = fieldName.Substring(1);
            }
            if (fieldValue.EndsWith(")"))
            {
                fieldValue = fieldValue.Substring(0, fieldValue.Length - 1);
            }

            var condition = GetCondition(@operator);

            if (condition is null)
            {
                return default;
            }

            if (condition is ISingleExpressionContainerBuilder singleExpressionContainerBuilder)
            {
                singleExpressionContainerBuilder.WithFirstExpression(GetField(fieldName));
            }

            if (condition is IDoubleExpressionContainerBuilder doubleExpressionContainerBuilder)
            {
                doubleExpressionContainerBuilder.WithSecondExpression(new LiteralExpressionBuilder(fieldValue));
            }

            result.Add(condition);
        }

        return result;
    }

    private static PropertyNameExpressionBuilder GetField(string fieldName)
        => new PropertyNameExpressionBuilder().WithPropertyName(fieldName);

    private List<IConditionBuilder> PerformSimpleSearch(string[] items)
        => items
            .Where(x => !string.IsNullOrEmpty(x))
            .Select((x, i) => new
            {
                Index = i,
                Value = x,
                StartsWithPlusOrMinus = x.StartsWith("+") || x.StartsWith("-"),
                StartsWithMinus = x.StartsWith("-")
            })
            .Select(item => CreateQueryCondition(item.Value,
                                                 item.StartsWithPlusOrMinus,
                                                 item.StartsWithMinus))
            .ToList();

    private IConditionBuilder CreateQueryCondition(string value, bool startsWithPlusOrMinus, bool startsWithMinus)
        => startsWithMinus
            ? new StringNotContainsConditionBuilder()
                .WithFirstExpression(_defaultFieldExpressionBuilderFactory())
                .WithSecondExpression(GetSecondExpression(value, startsWithPlusOrMinus))
            : new StringContainsConditionBuilder()
                .WithFirstExpression(_defaultFieldExpressionBuilderFactory())
                .WithSecondExpression(GetSecondExpression(value, startsWithPlusOrMinus));

    private static LiteralExpressionBuilder GetSecondExpression(string value, bool startsWithPlusOrMinus)
        => new LiteralExpressionBuilder(startsWithPlusOrMinus
            ? value.Substring(1)
            : value);

    private static IConditionBuilder? GetCondition(string @operator)
        // False positive
#pragma warning disable S2589 // Boolean expressions should not be gratuitous
        => @operator.ToUpper(CultureInfo.InvariantCulture) switch
        {
            var x when
                x == "=" ||
                x == "==" => new EqualConditionBuilder(),
            var x when
                x == "<>" ||
                x == "!=" ||
                x == "#" => new NotEqualConditionBuilder(),
            "<" => new SmallerThanConditionBuilder(),
            ">" => new GreaterThanConditionBuilder(),
            "<=" => new SmallerThanOrEqualConditionBuilder(),
            ">=" => new GreaterThanOrEqualConditionBuilder(),
            "CONTAINS" => new StringContainsConditionBuilder(),
            var x when
                x == "NOTCONTAINS" ||
                x == "NOT CONTAINS" => new StringNotContainsConditionBuilder(),
            "IS" => new IsNullConditionBuilder(),
            var x when
                x == "ISNOT" ||
                x == "IS NOT" => new IsNotNullConditionBuilder(),
            var x when
                x == "STARTS WITH" ||
                x == "STARTSWITH" => new StringStartsWithConditionBuilder(),
            var x when
                x == "ENDS WITH" ||
                x == "ENDSWITH" => new StringEndsWithConditionBuilder(),
            var x when
                x == "NOT STARTS WITH" ||
                x == "NOTSTARTSWITH" => new StringNotStartsWithConditionBuilder(),
            var x when
                x == "NOT ENDS WITH" ||
                x == "NOTENDSWITH" => new StringNotEndsWithConditionBuilder(),
            _ => null // Unknown operator
        };
#pragma warning restore S2589 // Boolean expressions should not be gratuitous
}

