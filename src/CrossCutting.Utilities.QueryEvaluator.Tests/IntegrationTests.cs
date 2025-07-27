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
            .AddConditions(new EqualConditionBuilder()
                .WithFirstExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
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
            .AddConditions(new EqualConditionBuilder()
                .WithFirstExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
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
            .AddConditions(new EqualConditionBuilder()
                .WithFirstExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithSecondExpression(new DelegateExpressionBuilder(() => "A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
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
    public void CanParseQuery()
    {
        // Arrange
        var sut = new SingleEntityQueryParser<SingleEntityQueryBuilder, PropertyNameExpressionBuilder>(() => new PropertyNameExpressionBuilder("MyProperty"));
        var builder = new SingleEntityQueryBuilder();

        // Act
        var result = sut.Parse(builder, "Field = \"A\"");

        // Assert
        result.Conditions.Count.ShouldBe(1);
        result.Conditions[0].ShouldBeOfType<EqualConditionBuilder>();
        var equalConditionBuilder = (EqualConditionBuilder)result.Conditions[0];
        equalConditionBuilder.FirstExpression.ShouldBeOfType<PropertyNameExpressionBuilder>();
        ((PropertyNameExpressionBuilder)equalConditionBuilder.FirstExpression).PropertyName.ShouldBe("Field");
        equalConditionBuilder.SecondExpression.ShouldBeOfType<LiteralExpressionBuilder>();
        ((LiteralExpressionBuilder)equalConditionBuilder.SecondExpression).Value.ShouldBe("A");
    }
}
