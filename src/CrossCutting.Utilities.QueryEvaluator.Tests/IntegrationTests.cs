namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public sealed class IntegrationTests : TestBase
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
    public async Task CanFindOneItem()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualsConditionBuilder()
                .WithFirstExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder().WithValue("A")))
            .AddSortOrders(new SortOrderBuilder()
                .WithExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property2)))
                .WithOrder(SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await QueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.Property2.ShouldBe("A");
    }

    [Fact]
    public async Task CanFindManyItems()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualsConditionBuilder()
                .WithFirstExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder().WithValue("A")))
            .AddSortOrders(new SortOrderBuilder()
                .WithExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property2)))
                .WithOrder(SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await QueryProcessor.FindManyAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(2);
        result.Value.First().Property2.ShouldBe("A");
        result.Value.Last().Property2.ShouldBe("Z");
    }

    [Fact]
    public async Task CanFindItemsPaged()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualsConditionBuilder()
                .WithFirstExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder().WithValue("A")))
            .AddSortOrders(new SortOrderBuilder()
                .WithExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property2)))
                .WithOrder(SortOrderDirection.Ascending))
            .WithLimit(1)
            .WithOffset(1)
            .Build();
        
        InitializeMock(CreateData());

        // Act
        var result = await QueryProcessor.FindPagedAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(1);
        result.Value.First().Property2.ShouldBe("Z");
    }

    [Fact]
    public async Task Can_Create_ExpressionEvaluator_And_Evaluate_Expression()
    {
        // Act
        var now = await Evaluator.EvaluateAsync(CreateContext("DateTime.Now"));

        // Assert
        now.Status.ShouldBe(ResultStatus.Ok);
        now.Value.ShouldBe(CurrentDateTime);
    }
}
