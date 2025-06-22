namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

internal interface IOperator
{
    Task<Result<bool>> EvaluateAsync(object? leftValue, object? rightValue, StringComparison stringComparison, CancellationToken token);
}

internal interface IQuery
{
    int? Limit { get; set; }
    int? Offset { get; set; }
    [ValidGroups]
    IReadOnlyCollection<ICondition> Conditions { get; set; }
    IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }
}

internal interface IQuerySortOrder
{
    IEvaluatable Expression { get; set; }
    QuerySortOrderDirection Order { get; set; }
}

internal interface ICondition
{
    StringComparison StringComparison { get; set; }

    IEvaluatable LeftExpression { get; set; }
    IOperator Operator { get; set; }
    IEvaluatable RightExpression { get; set; }

    Combination? Combination { get; set; }
    bool StartGroup { get; set; }
    bool EndGroup { get; set; }
}

internal enum QuerySortOrderDirection
{
    Ascending,
    Descending
}

internal enum Combination
{
    And,
    Or
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
internal sealed class ValidGroupsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IEnumerable collection)
        {
            return ValidationResult.Success;
        }

        var groupCounter = 0;
        var index = 0;

        foreach (var item in collection)
        {
            if (item == null) continue;

            var type = item.GetType();
            var prop1 = type.GetProperty(nameof(ICondition.StartGroup), BindingFlags.Public | BindingFlags.Instance);
            var prop2 = type.GetProperty(nameof(ICondition.EndGroup), BindingFlags.Public | BindingFlags.Instance);

            if (prop1 is null || prop2 is null)
            {
                return new ValidationResult($"Properties '{nameof(ICondition.StartGroup)}' or '{nameof(ICondition.EndGroup)}' not found on {type.Name}");
            }

#pragma warning disable CS8605 // Unboxing a possibly null value.
            var startGroup = (bool)prop1.GetValue(item);
            var endGroup = (bool)prop2.GetValue(item);
#pragma warning restore CS8605 // Unboxing a possibly null value.

            if (startGroup)
            {
                groupCounter++;
            }

            if (endGroup)
            {
                groupCounter--;
            }

            if (groupCounter < 0)
            {
                return new ValidationResult($"EndGroup not valid at index {index}, because there is no corresponding StartGroup");
            }

            index++;
        }

        if (groupCounter == 1)
        {
            return new ValidationResult("Missing EndGroup");
        }
#pragma warning disable S2583 // false positive!
        else if (groupCounter > 1)
#pragma warning restore S2583 // false positive!
        {
            return new ValidationResult($"{groupCounter} missing EndGroups");
        }

        return ValidationResult.Success;
    }
}

internal sealed class MyQuery : IQuery
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    [ValidGroups]
    public IReadOnlyCollection<ICondition> Conditions { get; set; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }
}

internal sealed class QuerySortOrder : IQuerySortOrder
{
    public IEvaluatable Expression { get; set; }
    public QuerySortOrderDirection Order { get; set; }
}

internal sealed class Condition : ICondition
{
    public StringComparison StringComparison { get; set; }

    public IEvaluatable LeftExpression { get; set; }
    public IOperator Operator { get; set; }
    public IEvaluatable RightExpression { get; set; }

    public Combination? Combination { get; set; }
    public bool StartGroup { get; set; }
    public bool EndGroup { get; set; }
}

internal sealed class InMemoryQueryProcessor : IEvaluatable<bool>
{
    public IQuery Query { get; set; }

    public async Task<Result<object?>> EvaluateAsync(CancellationToken token)
        => await EvaluateTypedAsync(token).ConfigureAwait(false);

    public async Task<Result<bool>> EvaluateTypedAsync(CancellationToken token)
    {
        if (CanEvaluateSimpleConditions(Query.Conditions))
        {
            return await EvaluateSimpleConditions(Query.Conditions, token).ConfigureAwait(false);
        }

        return await EvaluateComplexConditions(Query.Conditions, token).ConfigureAwait(false);
    }

    private static bool CanEvaluateSimpleConditions(IEnumerable<ICondition> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup
        );

    private static async Task<Result<bool>> EvaluateSimpleConditions(IEnumerable<ICondition> conditions, CancellationToken token)
    {
        foreach (var evaluatable in conditions)
        {
            var itemResult = await IsItemValid(evaluatable, token).ConfigureAwait(false);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }

            if (!itemResult.Value)
            {
                return itemResult;
            }
        }

        return Result.Success(true);
    }

    private static async Task<Result<bool>> EvaluateComplexConditions(IEnumerable<ICondition> conditions, CancellationToken token)
    {
        var builder = new StringBuilder();
        foreach (var evaluatable in conditions)
        {
            if (builder.Length > 0)
            {
                builder.Append(evaluatable.Combination == Combination.Or
                    ? "|"
                    : "&");
            }

            var prefix = evaluatable.StartGroup ? "(" : string.Empty;
            var suffix = evaluatable.EndGroup ? ")" : string.Empty;
            var itemResult = await IsItemValid(evaluatable, token).ConfigureAwait(false);
            if (!itemResult.IsSuccessful())
            {
                return itemResult;
            }
            builder.Append(prefix)
                   .Append(itemResult.Value ? "T" : "F")
                   .Append(suffix);
        }

        return Result.Success(EvaluateBooleanExpression(builder.ToString()));
    }

    private static async Task<Result<bool>> IsItemValid(ICondition condition, CancellationToken token)
        => await(await new AsyncResultDictionaryBuilder()
            .Add(Constants.LeftExpression, condition.LeftExpression.EvaluateAsync(token))
            .Add(Constants.RightExpression, condition.RightExpression.EvaluateAsync(token))
            .Build()
            .ConfigureAwait(false))
            .OnSuccess(async results => await condition.Operator
                .EvaluateAsync(results.GetValue<object?>(Constants.LeftExpression), results.GetValue<object?>(Constants.RightExpression), condition.StringComparison, token)
                .ConfigureAwait(false))
            .ConfigureAwait(false);

    private static bool EvaluateBooleanExpression(string expression)
    {
        var result = ProcessRecursive(ref expression);

        var @operator = "&";
        foreach (var character in expression)
        {
            bool currentResult;
            switch (character)
            {
                case '&':
                    @operator = "&";
                    break;
                case '|':
                    @operator = "|";
                    break;
                case 'T':
                case 'F':
                    currentResult = character == 'T';
                    result = @operator == "&"
                        ? result && currentResult
                        : result || currentResult;
                    break;
            }
        }

        return result;
    }

    private static bool ProcessRecursive(ref string expression)
    {
        var result = true;
        var openIndex = -1;
        int closeIndex;
        do
        {
            closeIndex = expression.IndexOf(')', StringComparison.Ordinal);
            if (closeIndex > -1)
            {
                openIndex = expression.LastIndexOf('(', closeIndex);
                if (openIndex > -1)
                {
                    result = EvaluateBooleanExpression(expression.Substring(openIndex + 1, closeIndex - openIndex - 1));
                    expression = string.Concat(GetPrefix(expression, openIndex),
                                               GetCurrent(result),
                                               GetSuffix(expression, closeIndex));
                }
            }
        } while (closeIndex > -1 && openIndex > -1);
        return result;
    }

    private static string GetPrefix(string expression, int openIndex)
        => openIndex == 0
            ? string.Empty
            : expression.Substring(0, openIndex - 2);

    private static string GetCurrent(bool result)
        => result
            ? "T"
            : "F";

    private static string GetSuffix(string expression, int closeIndex)
        => closeIndex == expression.Length
            ? string.Empty
            : expression.Substring(closeIndex + 1);
}

internal sealed class EqualsOperator : IOperator
{
    public Task<Result<bool>> EvaluateAsync(object? leftValue, object? rightValue, StringComparison stringComparison, CancellationToken token)
        => Task.Run(() => Equal.Evaluate(leftValue, rightValue, stringComparison), token);
}

internal sealed class ConstantEvaluatable : IEvaluatable
{
    public ConstantEvaluatable(object? value)
    {
        Value = value;
    }

    public object? Value { get; }

    public Task<Result<object?>> EvaluateAsync(CancellationToken token)
        => Task.FromResult(Result.Success(Value));
}

internal sealed class ConstantEvaluatable<T> : IEvaluatable<T>
{
    public ConstantEvaluatable(T value)
    {
        Value = value;
    }

    public T Value { get; }

    public Task<Result<object?>> EvaluateAsync(CancellationToken token)
        => Task.FromResult(Result.Success<object?>(Value));

    public Task<Result<T>> EvaluateTypedAsync(CancellationToken token)
        => Task.FromResult(Result.Success(Value));
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class QueryFrameworkTests : TestBase
{
    [Fact]
    public async Task Can_Evaluate_Query()
    {
        // Arrange
        var query = new MyQuery
        {
            Limit = 10,
            Offset = 100,
            Conditions = 
            [
                new Condition
                {
                    LeftExpression = new ConstantEvaluatable<string>("A"),
                    Operator = new EqualsOperator(),
                    RightExpression = new ConstantEvaluatable<string>("A")
                }
            ]
        };
        var processor = new InMemoryQueryProcessor { Query = query };

        // Act
        var result = await processor.EvaluateTypedAsync(CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Can_Validate_Query()
    {
        // Arrange
        var query = new MyQuery
        {
            Limit = 10,
            Offset = 100,
            Conditions =
            [
                new Condition
                {
                    LeftExpression = new ConstantEvaluatable<string>("A"),
                    Operator = new EqualsOperator(),
                    RightExpression = new ConstantEvaluatable<string>("A"),
                    StartGroup = true
                }
            ]
        };
        var validationContext = new ValidationContext(query);
        var validationResults = new List<ValidationResult>();

        // Act
        var success = Validator.TryValidateObject(query, validationContext, validationResults, true);

        // Assert
        success.ShouldBe(false);
        validationResults.Count.ShouldBe(1);
        validationResults[0].ErrorMessage.ShouldBe("Missing EndGroup");
    }
}
