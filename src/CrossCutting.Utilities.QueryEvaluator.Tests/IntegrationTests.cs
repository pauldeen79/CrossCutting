namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public sealed class IntegrationTests : TestBase, IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public IntegrationTests()
    {
        _serviceProvider = new ServiceCollection()
            .AddQueryEvaluatorInMemory()
            .AddSingleton(DataProvider)
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
                .WithOperator(new EqualsOperatorBuilder().WithStringComparison(StringComparison.InvariantCulture))
                .WithRightExpression(new LiteralExpressionBuilder().WithValue("A"))))
            .With(x => x.OrderByFields.Add(new QuerySortOrderBuilder()
                .WithExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property2)))
                .WithOrder(QuerySortOrderDirection.Ascending)))
            .Build();

        InitializeMock(
            [
                new MyEntity("B", "C"),
                new MyEntity("A", "Z"),
                new MyEntity("B", "D"),
                new MyEntity("A", "A"),
                new MyEntity("B", "E"),
            ]);

        var queryProcessor = _serviceProvider.GetRequiredService<IQueryProcessor>();

        // Act
        var result = await queryProcessor.FindManyAsync<MyEntity>(query, CancellationToken.None);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(2);
        result.Value.First().Property2.ShouldBe("A");
        result.Value.Last().Property2.ShouldBe("Z");
    }
}
