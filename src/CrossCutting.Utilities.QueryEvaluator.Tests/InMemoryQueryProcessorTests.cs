namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public sealed class InMemoryQueryProcessorTests : TestBase
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
        var result = await InMemoryQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.ThrowIfInvalid();
        result.Value.ShouldNotBeNull();
        result.Value.Property2.ShouldBe("A");
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

        InitializeMock(CreateData());

        // Act
        var result = await InMemoryQueryProcessor.FindManyAsync<MyEntity>(query);

        // Assert
        result.ThrowIfInvalid();
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
        
        InitializeMock(CreateData());

        // Act
        var result = await InMemoryQueryProcessor.FindPagedAsync<MyEntity>(query);

        // Assert
        result.ThrowIfInvalid();
        result.Value.ShouldNotBeNull();
        result.Value.Count.ShouldBe(1);
        result.Value.First().Property2.ShouldBe("Z");
    }

    [Fact]
    public async Task Can_Use_Nested_Property_In_Query()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithFirstExpression(new PropertyNameExpressionBuilder(new PropertyNameExpressionBuilder(nameof(MyNestedEntity.Property)), nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder("A")))
            .Build();

        InitializeMock([new MyNestedEntity(new MyEntity("A", "B"))]);

        // Act
        var result = await InMemoryQueryProcessor.FindOneAsync<MyNestedEntity>(query);

        // Assert
        result.ThrowIfInvalid();
        result.Value.ShouldNotBeNull();
        result.Value.Property.Property2.ShouldBe("B");
    }

    [Fact]
    public async Task Can_Use_Brackets_And_Multiple_Operators_In_Query()
    {
        // Arrange
        var parser = new SingleEntityQueryParser<SingleEntityQueryBuilder, PropertyNameExpressionBuilder>(() => new PropertyNameExpressionBuilder("MyProperty"));
        var builder = new SingleEntityQueryBuilder();
        var query = parser.Parse(builder, "(Property1 = \"A\" AND Property1 <> \"B\") OR Property1 = \"Z\"").Build();
        InitializeMock(CreateData());

        // Act
        var result = await InMemoryQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.ThrowIfInvalid();
        result.Value.ShouldNotBeNull();
        result.Value.Property1.ShouldBe("A");
    }
}
