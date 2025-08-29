namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public sealed class SqlQueryProcessorTests : TestBase
{
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
        await DatabaseEntityRetriever
            .Received()
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 = @p0 ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
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
        await DatabaseEntityRetriever
            .Received()
            .FindManyAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT * FROM MyEntity WHERE Property1 = @p0 ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
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
        await DatabaseEntityRetriever
            .Received()
            .FindPagedAsync(Arg.Is<IPagedDatabaseCommand>(x =>
                x.DataCommand.CommandText == "SELECT * FROM (SELECT TOP 1 *, ROW_NUMBER() OVER (ORDER BY (SELECT 0)) as sq_row_number FROM MyEntity WHERE Property1 = @p0) sq WHERE sq.sq_row_number BETWEEN 2 and 2;"
                && x.RecordCountCommand.CommandText == "SELECT COUNT(*) FROM MyEntity WHERE Property1 = @p0"), Arg.Any<CancellationToken>());
    }
}
