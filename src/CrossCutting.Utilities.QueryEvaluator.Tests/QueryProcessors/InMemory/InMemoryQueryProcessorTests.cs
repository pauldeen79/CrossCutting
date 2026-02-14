namespace CrossCutting.Utilities.QueryEvaluator.Tests.QueryProcessors.InMemory;

public sealed class InMemoryQueryProcessorTests : TestBase
{
    [Fact]
    public async Task Can_Find_One_Item()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await InMemoryQueryProcessor.FindOneAsync<MyEntity>(query, CancellationToken.None);

        // Assert
        result.ThrowIfNotSuccessful();
        result.Value.ShouldNotBeNull();
        result.Value.Property2.ShouldBe("A");
    }

    [Fact]
    public async Task Can_Find_Many_Items()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await InMemoryQueryProcessor.FindManyAsync<MyEntity>(query, CancellationToken.None);

        // Assert
        result.ThrowIfNotSuccessful();
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
                .WithSourceExpression(new PropertyNameEvaluatableBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new DelegateEvaluatableBuilder(() => "A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .WithLimit(1)
            .WithOffset(1)
            .Build();
        
        InitializeMock(CreateData());

        // Act
        var result = await InMemoryQueryProcessor.FindPagedAsync<MyEntity>(query, CancellationToken.None);

        // Assert
        result.ThrowIfNotSuccessful();
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
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new PropertyNameEvaluatableBuilder(nameof(MyNestedEntity.Property))).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .Build();

        InitializeMock([new MyNestedEntity(new MyEntity("A", "B"))]);

        // Act
        var result = await InMemoryQueryProcessor.FindOneAsync<MyNestedEntity>(query, null);

        // Assert
        result.ThrowIfNotSuccessful();
        result.Value.ShouldNotBeNull();
        result.Value.Property.Property2.ShouldBe("B");
    }

    [Fact]
    public async Task Can_Use_Brackets_And_Multiple_Operators_In_Query()
    {
        // Arrange
        var parser = new SingleEntityQueryParser<SingleEntityQueryBuilder, PropertyNameEvaluatableBuilder>(() => new PropertyNameEvaluatableBuilder("MyProperty"));
        var builder = new SingleEntityQueryBuilder();
        var query = parser.Parse(builder, "(Property1 = \"A\" AND Property1 <> \"B\") OR Property1 = \"Z\"").Build();
        InitializeMock(CreateData());

        // Act
        var result = await InMemoryQueryProcessor.FindOneAsync<MyEntity>(query, CancellationToken.None);

        // Assert
        result.ThrowIfNotSuccessful();
        result.Value.ShouldNotBeNull();
        result.Value.Property1.ShouldBe("A");
    }
}
