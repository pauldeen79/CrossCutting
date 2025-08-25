namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public sealed class SqlQueryProcessorTests : TestBase
{
    private static MyEntity[] CreateData()=>
    [
        new MyEntity("B", "C"),
        new MyEntity("A", "Z"),
        new MyEntity("B", "D"),
        new MyEntity("A", "A"),
        new MyEntity("B", "E"),
    ];

    [Fact]
    public async Task Can_Find_One_Item()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithFirstExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
    }

    [Fact]
    public async Task Can_Find_Many_Items()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithFirstExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData().Where(x => x.Property1 == "A").OrderBy(x => x.Property2));

        // Act
        var result = await SqlQueryProcessor.FindManyAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(2);
        result.Value.First().Property2.ShouldBe("A");
        result.Value.Last().Property2.ShouldBe("Z");
    }

    [Fact]
    public async Task Can_Find_Items_Paged()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithFirstExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithSecondExpression(new DelegateExpressionBuilder(() => "A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .WithLimit(1)
            .WithOffset(1)
            .Build();
        
        InitializeMock(CreateData().Where(x => x.Property1 == "A").OrderBy(x => x.Property2).Skip(1).Take(1));

        // Act
        var result = await SqlQueryProcessor.FindPagedAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(1);
        result.Value.First().Property2.ShouldBe("Z");
    }
}
