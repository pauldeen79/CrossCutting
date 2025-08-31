namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public sealed class SqlQueryProcessorTests : TestBase
{
    [Fact]
    public async Task Can_Find_One_Item_With_BetweenCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new BetweenConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithLowerBoundExpression(new LiteralExpressionBuilder("A"))
                .WithUpperBoundExpression(new LiteralExpressionBuilder("B")))
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
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 BETWEEN @p0 AND @p1 ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Can_Find_One_Item_With_EqualCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralExpressionBuilder("A")))
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
    public async Task Can_Find_One_Item_With_GreaterThanCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new GreaterThanConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralExpressionBuilder("A")))
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
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 > @p0 ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Can_Find_One_Item_With_GreaterThanOrEqualCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new GreaterThanOrEqualConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralExpressionBuilder("A")))
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
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 >= @p0 ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Can_Find_One_Item_With_InCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new InConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .AddCompareExpressions(new LiteralExpressionBuilder("A"))
                .AddCompareExpressions(new DelegateExpressionBuilder(() => "B")))
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
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 IN (@p0, @p1) ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Can_Find_One_Item_With_NotEqualCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new NotEqualConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralExpressionBuilder("A")))
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
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 <> @p0 ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Can_Find_One_Item_With_NotInCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new NotInConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .AddCompareExpressions(new LiteralExpressionBuilder("A"))
                .AddCompareExpressions(new DelegateExpressionBuilder(() => "B")))
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
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 NOT IN (@p0, @p1) ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Can_Find_One_Item_With_NotNullCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new NotNullConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1))))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData().Where(x => x.Property1 != null));

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        await DatabaseEntityRetriever
            .Received()
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 IS NOT NULL ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Can_Find_One_Item_With_NullCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new NullConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1))))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData().Where(x => x.Property1 == null));

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Value.ShouldBeNull();
        await DatabaseEntityRetriever
            .Received()
            .FindOneAsync(Arg.Is<IDatabaseCommand>(x => x.CommandText == "SELECT TOP 1 * FROM MyEntity WHERE Property1 IS NULL ORDER BY Property2 ASC"), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Can_Find_Many_Items()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new ContextExpressionBuilder()))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameExpressionBuilder(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData().Where(x => x.Property1 == "A").OrderBy(x => x.Property2));

        // Act
        var result = await SqlQueryProcessor.FindManyAsync<MyEntity>(query, "A");

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
                .WithSourceExpression(new PropertyNameExpressionBuilder(nameof(MyEntity.Property1)))
                .WithCompareExpression(new DelegateExpressionBuilder(() => "A")))
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
                x.DataCommand.CommandText == "SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY Property2 ASC) as sq_row_number FROM MyEntity WHERE Property1 = @p0) sq WHERE sq.sq_row_number BETWEEN 2 and 2;"
                && x.RecordCountCommand.CommandText == "SELECT COUNT(*) FROM MyEntity WHERE Property1 = @p0"), Arg.Any<CancellationToken>());
    }
}
