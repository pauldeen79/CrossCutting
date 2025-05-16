namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.QueryFrameworkTests;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

internal interface IOperator
{
    Task<Result<bool>> EvaluateAsync(object? leftValue, object? rightValue, StringComparison stringComparison);
}

internal interface IQuery
{
    int? Limit { get; set; }
    int? Offset { get; set; }
    IComposedEvaluatable Filter { get; set; }
    IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }
}

internal interface IQuerySortOrder
{
    IExpression FieldNameExpression { get; set; }
    QuerySortOrderDirection Order { get; set; }
}

interface IComposedEvaluatable : IEvaluatable<bool>
{
    IReadOnlyCollection<IComposableEvaluatable> Conditions { get; set; }
}

internal interface IComposableEvaluatable : IEvaluatable<bool>
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

internal sealed class MyQuery : IQuery
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public IComposedEvaluatable Filter { get; set; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; set; }
}

internal sealed class QuerySortOrder : IQuerySortOrder
{
    public IExpression FieldNameExpression { get; set; }
    public QuerySortOrderDirection Order { get; set; }
}

internal sealed class ComposableEvaluatable : IComposableEvaluatable
{
    public StringComparison StringComparison { get; set; }

    public IEvaluatable LeftExpression { get; set; }
    public IOperator Operator { get; set; }
    public IEvaluatable RightExpression { get; set; }

    public Combination? Combination { get; set; }
    public bool StartGroup { get; set; }
    public bool EndGroup { get; set; }

    public async Task<Result<object?>> EvaluateAsync()
        => (await EvaluateTypedAsync().ConfigureAwait(false)).TryCastAllowNull<object?>();

    public async Task<Result<bool>> EvaluateTypedAsync()
        => await (await new AsyncResultDictionaryBuilder()
            .Add(Constants.LeftExpression, LeftExpression.EvaluateAsync())
            .Add(Constants.RightExpression, RightExpression.EvaluateAsync())
            .Build()
            .ConfigureAwait(false))
            .OnSuccess(async results => await Operator
                .EvaluateAsync(results.GetValue<object?>(Constants.LeftExpression), results.GetValue<object?>(Constants.RightExpression), StringComparison)
                .ConfigureAwait(false))
            .ConfigureAwait(false);
}

internal sealed class ComposedEvaluatable : IComposedEvaluatable, IValidatableObject
{
    public IReadOnlyCollection<IComposableEvaluatable> Conditions { get; set; }
    public string SourceExpression { get; set; }

    public async Task<Result<object?>> EvaluateAsync()
        => (await EvaluateTypedAsync().ConfigureAwait(false)).TryCastAllowNull<object?>();

    public async Task<Result<bool>> EvaluateTypedAsync()
    {
        if (CanEvaluateSimpleConditions(Conditions))
        {
            return await EvaluateSimpleConditions(Conditions).ConfigureAwait(false);
        }

        return await EvaluateComplexConditions(Conditions).ConfigureAwait(false);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var groupCounter = 0;
        var index = 0;
        foreach (var evaluatable in Conditions)
        {
            if (evaluatable.StartGroup)
            {
                groupCounter++;
            }
            if (evaluatable.EndGroup)
            {
                groupCounter--;
            }
            if (groupCounter < 0)
            {
                yield return new ValidationResult($"EndGroup not valid at index {index}, because there is no corresponding StartGroup", [nameof(Conditions)]);
                break;
            }

            index++;
        }

        if (groupCounter == 1)
        {
            yield return new ValidationResult("Missing EndGroup", [nameof(Conditions)]);
        }
#pragma warning disable S2583 // false positive!
        else if (groupCounter > 1)
#pragma warning restore S2583 // false positive!
        {
            yield return new ValidationResult($"{groupCounter} missing EndGroups", [nameof(Conditions)]);
        }
    }

    private static bool CanEvaluateSimpleConditions(IEnumerable<IComposableEvaluatable> conditions)
        => !conditions.Any(x =>
            (x.Combination ?? Combination.And) == Combination.Or
            || x.StartGroup
            || x.EndGroup
        );

    private static async Task<Result<bool>> EvaluateSimpleConditions(IEnumerable<IComposableEvaluatable> conditions)
    {
        foreach (var evaluatable in conditions)
        {
            var itemResult = await IsItemValid(evaluatable).ConfigureAwait(false);
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

    private static async Task<Result<bool>> EvaluateComplexConditions(IEnumerable<IComposableEvaluatable> conditions)
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
            var itemResult = await IsItemValid(evaluatable).ConfigureAwait(false);
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

    private static Task<Result<bool>> IsItemValid(IComposableEvaluatable condition)
        => condition.EvaluateTypedAsync();

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
    public Task<Result<bool>> EvaluateAsync(object? leftValue, object? rightValue, StringComparison stringComparison)
        => Task.FromResult(Equal.Evaluate(leftValue, rightValue, stringComparison));
}

internal sealed class ConstantExpression : IEvaluatable
{
    public ConstantExpression(object? value)
    {
        Value = value;
    }

    public object? Value { get; }

    public Task<Result<object?>> EvaluateAsync()
        => Task.FromResult(Result.Success(Value));
}

internal sealed class ConstantExpression<T> : IEvaluatable<T>
{
    public ConstantExpression(T value)
    {
        Value = value;
    }

    public T Value { get; }

    public Task<Result<object?>> EvaluateAsync()
        => Task.FromResult(Result.Success<object?>(Value));

    public Task<Result<T>> EvaluateTypedAsync()
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
            Filter = new ComposedEvaluatable
            {
                SourceExpression = "Some source expression",
                Conditions =
                [
                    new ComposableEvaluatable { LeftExpression = new ConstantExpression<string>("A"), Operator = new EqualsOperator(), RightExpression = new ConstantExpression<string>("A") }
                ]
            }
        };

        // Act
        var result = await query.Filter.EvaluateTypedAsync();

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }
}
