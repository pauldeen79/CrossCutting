namespace CrossCutting.Utilities.QueryEvaluator.QueryParsers;

public class SingleEntityQueryParser<TQueryBuilder, TQueryExpression> : IQueryParser<TQueryBuilder>
    where TQueryBuilder : IQueryBuilder
    where TQueryExpression : IEvaluatableBuilder
{
    private readonly Func<TQueryExpression> _defaultFieldExpressionFactory;

    private static readonly Dictionary<string, Func<IConditionBuilder>> _operatorMap = new(StringComparer.OrdinalIgnoreCase)
    {
        { "=", ()=> new EqualConditionBuilder() },
        { "==", () => new EqualConditionBuilder() },
        { "<>", () => new NotEqualConditionBuilder() },
        { "!=", () => new NotEqualConditionBuilder() },
        { "#", () => new NotEqualConditionBuilder() },
        { "<", () => new SmallerThanConditionBuilder() },
        { ">", () => new GreaterThanConditionBuilder() },
        { "<=", () => new SmallerThanOrEqualConditionBuilder() },
        { ">=", () => new GreaterThanOrEqualConditionBuilder() },
        { "CONTAINS", () => new StringContainsConditionBuilder() },
        { "NOTCONTAINS", () => new StringNotContainsConditionBuilder() },
        { "NOT CONTAINS", () => new StringNotContainsConditionBuilder() },
        { "IS", () => new NullConditionBuilder() },
        { "ISNOT", () => new NotNullConditionBuilder() },
        { "IS NOT", () => new NotNullConditionBuilder() },
        { "STARTS WITH", () => new StringStartsWithConditionBuilder() },
        { "STARTSWITH", () => new StringStartsWithConditionBuilder() },
        { "ENDS WITH", () =>  new StringEndsWithConditionBuilder() },
        { "ENDSWITH", () => new StringEndsWithConditionBuilder() },
        { "NOT STARTS WITH", () => new StringNotStartsWithConditionBuilder() },
        { "NOTSTARTSWITH", () => new StringNotStartsWithConditionBuilder() },
        { "NOT ENDS WITH", () => new StringNotEndsWithConditionBuilder() },
        { "NOTENDSWITH", () => new StringNotEndsWithConditionBuilder() },
    };

    public SingleEntityQueryParser(Func<TQueryExpression> defaultFieldExpressionFactory)
    {
        ArgumentGuard.IsNotNull(defaultFieldExpressionFactory, nameof(defaultFieldExpressionFactory));

        _defaultFieldExpressionFactory = defaultFieldExpressionFactory;
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

        return builder.AddConditions(PerformQuerySearch(items)
            ?? PerformSimpleSearch(items));
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

        var nextSearchCombination = Combination.And;
        var result = new List<IConditionBuilder>();

        for (int i = 0; i < items.Length; i += 4)
        {
            //verify that:
            //-items[i] needs to be a valid fieldname
            //-items[i + 1] needs to be a valid operator
            //-items[i + 3] needs to be a valid combination for the next condition
            var startGroup = false;
            var endGroup = false;
            var fieldName = items[i];
            var fieldValue = items[i + 2];
            var @operator = items[i + 1];

            //remove brackets and set bracket property values for this query item.
            if (fieldName.StartsWith("("))
            {
                startGroup = true;
                fieldName = fieldName.Substring(1);
            }
            if (fieldValue.EndsWith(")"))
            {
                endGroup = true;
                fieldValue = fieldValue.Substring(0, fieldValue.Length - 1);
            }

            var condition = CreateCondition(@operator);

            if (condition is null)
            {
                return default;
            }

            condition
                .WithStartGroup(startGroup)
                .WithEndGroup(endGroup)
                .WithCombination(nextSearchCombination);

            (condition as ISourceExpressionContainerBuilder)?.WithSourceExpression(GetField(fieldName));
            (condition as ICompareExpressionContainerBuilder)?.WithCompareExpression(new LiteralEvaluatableBuilder(fieldValue));

            if (items.Length > i + 3)
            {
                var combination = GetQueryCombination(items[i + 3]);
                if (combination is null)
                {
                    return default;
                }

                nextSearchCombination = combination.Value;
            }

            result.Add(condition);
        }

        return result;
    }

    private static Combination? GetQueryCombination(string combination)
        => combination.ToUpper(CultureInfo.InvariantCulture) switch
        {
            "AND" => Combination.And,
            "OR" => Combination.Or,
            _ => null,// Unknown search combination
        };

    private static PropertyNameEvaluatableBuilder GetField(string fieldName)
        => new PropertyNameEvaluatableBuilder(fieldName);

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
                .WithSourceExpression(_defaultFieldExpressionFactory())
                .WithCompareExpression(CreateLiteralExpression(value, startsWithPlusOrMinus))
            : new StringContainsConditionBuilder()
                .WithSourceExpression(_defaultFieldExpressionFactory())
                .WithCompareExpression(CreateLiteralExpression(value, startsWithPlusOrMinus));

    private static LiteralEvaluatableBuilder CreateLiteralExpression(string value, bool startsWithPlusOrMinus)
        => new LiteralEvaluatableBuilder(startsWithPlusOrMinus
            ? value.Substring(1)
            : value);

    private static IConditionBuilder? CreateCondition(string @operator)
        => _operatorMap.TryGetValue(@operator, out Func<IConditionBuilder> factory)
            ? factory()
            : null;
}

