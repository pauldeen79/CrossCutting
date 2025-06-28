namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    private readonly DataProviderMock _dataProvider;
    private readonly ServiceProvider _serviceProvider;

    public IntegrationTests()
    {
        _dataProvider = new DataProviderMock();
        _serviceProvider = new ServiceCollection()
            .AddQueryEvaluatorInMemory()
            .AddSingleton<IDataProvider>(_dataProvider)
            .BuildServiceProvider();
    }

    public void Dispose()
        => _serviceProvider.Dispose();

    [Fact]
    public async Task Can_Query_Entity()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .With(x => x.Filter.Add(new ComposableConditionBuilder()
                .WithLeftExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property1)))
                .WithOperator(new NotEqualsOperatorBuilder().WithStringComparison(StringComparison.InvariantCulture))
                .WithRightExpression(new LiteralExpressionBuilder().WithValue("A"))))
            .With(x => x.OrderByFields.Add(new QuerySortOrderBuilder()
                .WithExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property2)))
                .WithOrder(Domains.QuerySortOrderDirection.Ascending)))
            .Build();

        InitializeMock([new MyEntity { Property1 = "B" }, new MyEntity { Property1 = "A", Property2 = "Z" }, new MyEntity { Property1 = "A", Property2 = "A" }]);

        var queryProcessor = _serviceProvider.GetRequiredService<IQueryProcessor>();

        // Act
        var result = await queryProcessor.FindManyAsync<MyEntity>(query, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(1);
        result.Value.First().Property1.ShouldBe("A");
    }

    private void InitializeMock(MyEntity[] items)
    {
        _dataProvider.ResultDelegate = new Func<Query, Task<Result<IEnumerable>>>
        (
            async query =>
            {
                return Result.Success<IEnumerable>(Enumerable.Empty<object?>());
            }
            //items.Where
            //(
            //    item =>
            //    {
            //        var satisfied = true;
            //        foreach (var condition in query.Filter)
            //        {
            //            var result = condition.EvaluateTypedAsync(CreateContext("Dummy", item), CancellationToken.None).Result;
            //            if (!result.EnsureValue().IsSuccessful())
            //            {
            //                return satisfied = false;
            //            }
            //            else if (!result.Value)
            //            {
            //                satisfied = false;
            //                break;
            //            }
            //        }
            //        return satisfied;
            //    }
            //)
        );
    }
}
