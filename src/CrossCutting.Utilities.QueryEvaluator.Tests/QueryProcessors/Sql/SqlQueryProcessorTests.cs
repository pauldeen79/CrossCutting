namespace CrossCutting.Utilities.QueryEvaluator.Tests.QueryProcessors.Sql;

public sealed class SqlQueryProcessorTests : TestBase
{
    [Fact]
    public async Task Can_Find_One_Item_With_BetweenCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new BetweenConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithLowerBoundExpression(new LiteralEvaluatableBuilder("A"))
                .WithUpperBoundExpression(new LiteralEvaluatableBuilder("B")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 BETWEEN @p0 AND @p1 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_EqualCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 = @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_GreaterThanCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new GreaterThanConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 > @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_GreaterThanOrEqualCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new GreaterThanOrEqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 >= @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_InCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new InConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .AddCompareExpressions(new LiteralEvaluatableBuilder("A"))
                .AddCompareExpressions(new DelegateEvaluatableBuilder(() => "B")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 IN (@p0, @p1) ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_NotEqualCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new NotEqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 <> @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_NotInCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new NotInConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .AddCompareExpressions(new LiteralEvaluatableBuilder("A"))
                .AddCompareExpressions(new DelegateEvaluatableBuilder(() => "B")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 NOT IN (@p0, @p1) ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_NotNullCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new NotNullConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1))))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData().Where(x => x.Property1 != null));

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 IS NOT NULL ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_NullCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new NullConditionBuilder()
                .WithStartGroup()
                .WithEndGroup()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1))))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData().Where(x => x.Property1 == null));

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.NotFound);
        result.Value.ShouldBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE (Property1 IS NULL) ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_SmallerThanCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new SmallerThanConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 < @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_SmallerThanOrEqualCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new SmallerThanOrEqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 <= @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_StringContainsCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new StringContainsConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 LIKE @p0 ORDER BY Property2 ASC");
        IsValidParameters(LastDatabaseCommand.CommandParameters, "%A%").ShouldBeTrue();
    }

    [Fact]
    public async Task Can_Find_One_Item_With_StringEndsWithCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new StringEndsWithConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 LIKE @p0 ORDER BY Property2 ASC");
        IsValidParameters(LastDatabaseCommand.CommandParameters, "%A").ShouldBeTrue();
    }

    [Fact]
    public async Task Can_Find_One_Item_With_StringEqualsCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new StringEqualsConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 = @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_StringNotContainsCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new StringNotContainsConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 NOT LIKE @p0 ORDER BY Property2 ASC");
        IsValidParameters(LastDatabaseCommand.CommandParameters, "%A%").ShouldBeTrue();
    }

    [Fact]
    public async Task Can_Find_One_Item_With_StringNotEndsWithCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new StringNotEndsWithConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 NOT LIKE @p0 ORDER BY Property2 ASC");
        IsValidParameters(LastDatabaseCommand.CommandParameters, "%A").ShouldBeTrue();
    }

    [Fact]
    public async Task Can_Find_One_Item_With_StringNotEqualsCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new StringNotEqualsConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 <> @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_One_Item_With_StringNotStartsWithCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new StringNotStartsWithConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 NOT LIKE @p0 ORDER BY Property2 ASC");
        IsValidParameters(LastDatabaseCommand.CommandParameters, "A%").ShouldBeTrue();
    }

    [Fact]
    public async Task Can_Find_One_Item_With_StringStartsWithCondition()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new StringStartsWithConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new LiteralEvaluatableBuilder("A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
            .Build();

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindOneAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT TOP 1 * FROM MyEntity WHERE Property1 LIKE @p0 ORDER BY Property2 ASC");
        IsValidParameters(LastDatabaseCommand.CommandParameters, "A%").ShouldBeTrue();
    }

    [Fact]
    public async Task Can_Find_Many_Items()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new ContextEvaluatableBuilder()))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
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
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT * FROM MyEntity WHERE Property1 = @p0 ORDER BY Property2 ASC");
    }

    [Fact]
    public async Task Can_Find_Many_Items_Using_DefaultWhere()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder().Build();

        InitializeMock(CreateData().Where(x => x.Property1 == "A"));

        DatabaseEntityRetrieverSettings.DefaultWhere.Returns("Property1 = 'A'");

        // Act
        var result = await SqlQueryProcessor.FindManyAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT * FROM MyEntity WHERE Property1 = 'A'");
    }

    [Fact]
    public async Task Can_Find_Many_Items_Using_DefaultOrderBy()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder().Build();

        InitializeMock(CreateData().OrderBy(x => x.Property1));

        DatabaseEntityRetrieverSettings.DefaultOrderBy.Returns("Property1 ASC");

        // Act
        var result = await SqlQueryProcessor.FindManyAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT * FROM MyEntity ORDER BY Property1 ASC");
    }

    [Fact]
    public async Task Can_Find_Many_Items_Using_DataObjectNameQuery()
    {
        // Arrange
        var query = new DataObjectNameQuery { DataObjectName = "CustomTable" };

        InitializeMock(CreateData());

        // Act
        var result = await SqlQueryProcessor.FindManyAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastDatabaseCommand.ShouldNotBeNull();
        LastDatabaseCommand.CommandText.ShouldBe("SELECT * FROM CustomTable");
    }

    [Fact]
    public async Task Can_Find_Items_Paged()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddConditions(new EqualConditionBuilder()
                .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                .WithCompareExpression(new DelegateEvaluatableBuilder(() => "A")))
            .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending))
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
        LastPagedDatabaseCommand.ShouldNotBeNull();
        LastPagedDatabaseCommand.DataCommand.CommandText.ShouldBe("SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY Property2 ASC) as sq_row_number FROM MyEntity WHERE Property1 = @p0) sq WHERE sq.sq_row_number BETWEEN 2 and 2;");
        LastPagedDatabaseCommand.RecordCountCommand.CommandText.ShouldBe("SELECT COUNT(*) FROM MyEntity WHERE Property1 = @p0");
    }

    [Fact]
    public async Task Can_Find_Items_Paged_With_FieldSelectionQuery_Custom_Fields()
    {
        // Arrange
        var query = new FieldSelectionQuery
        {
            Distinct = false,
            GetAllFields = false,
            FieldNames = new List<string> { "Property1", "Property2" },
            Conditions =  new List<ICondition>
            {
                new EqualConditionBuilder()
                    .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                    .WithCompareExpression(new DelegateEvaluatableBuilder(() => "A"))
                    .Build()
            },
            SortOrders = new List<ISortOrder>
            {
                new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending).Build()
            }
        };

        InitializeMock(CreateData().Where(x => x.Property1 == "A").OrderBy(x => x.Property2));

        // Act
        var result = await SqlQueryProcessor.FindPagedAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastPagedDatabaseCommand.ShouldNotBeNull();
        LastPagedDatabaseCommand.DataCommand.CommandText.ShouldBe("SELECT Property1, Property2 FROM MyEntity WHERE Property1 = @p0 ORDER BY Property2 ASC");
        LastPagedDatabaseCommand.RecordCountCommand.CommandText.ShouldBe("SELECT COUNT(*) FROM MyEntity WHERE Property1 = @p0");
    }

    [Fact]
    public async Task Can_Find_Items_Paged_With_FieldSelectionQuery_Custom_Fields_But_Empty()
    {
        // Arrange
        var query = new FieldSelectionQuery
        {
            Distinct = false,
            GetAllFields = true,
            Conditions = new List<ICondition>
            {
                new EqualConditionBuilder()
                    .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                    .WithCompareExpression(new DelegateEvaluatableBuilder(() => "A"))
                    .Build()
            }
        };

        InitializeMock(CreateData().Where(x => x.Property1 == "A"));
        DatabaseEntityRetrieverSettings.Fields.Returns("Property1, Property2");

        // Act
        var result = await SqlQueryProcessor.FindPagedAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastPagedDatabaseCommand.ShouldNotBeNull();
        LastPagedDatabaseCommand.DataCommand.CommandText.ShouldBe("SELECT Property1, Property2 FROM MyEntity WHERE Property1 = @p0");
        LastPagedDatabaseCommand.RecordCountCommand.CommandText.ShouldBe("SELECT COUNT(*) FROM MyEntity WHERE Property1 = @p0");
    }

    [Fact]
    public async Task Can_Find_Items_Paged_With_FieldSelectionQuery_All_Fields_Distinct()
    {
        // Arrange
        var query = new FieldSelectionQuery
        {
            Distinct = true,
            GetAllFields = true,
            Conditions = new List<ICondition>
            {
                new EqualConditionBuilder()
                    .WithSourceExpression(new PropertyNameEvaluatableBuilder().WithSourceExpression(new ContextEvaluatableBuilder()).WithPropertyName(nameof(MyEntity.Property1)))
                    .WithCompareExpression(new DelegateEvaluatableBuilder(() => "A"))
                    .Build()
            },
            SortOrders = new List<ISortOrder>
            {
                new SortOrderBuilder(new PropertyNameEvaluatableBuilder().WithPropertyName(nameof(MyEntity.Property2)), SortOrderDirection.Ascending).Build()
            }
        };

        InitializeMock(CreateData().Where(x => x.Property1 == "A").OrderBy(x => x.Property2));

        // Act
        var result = await SqlQueryProcessor.FindPagedAsync<MyEntity>(query);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldNotBeNull();
        LastPagedDatabaseCommand.ShouldNotBeNull();
        LastPagedDatabaseCommand.DataCommand.CommandText.ShouldBe("SELECT DISTINCT * FROM MyEntity WHERE Property1 = @p0 ORDER BY Property2 ASC");
        LastPagedDatabaseCommand.RecordCountCommand.CommandText.ShouldBe("SELECT COUNT(*) FROM MyEntity WHERE Property1 = @p0");
    }

    private sealed class FieldSelectionQuery : IFieldSelectionQuery
    {
        public bool Distinct { get; set; }

        public bool GetAllFields { get; set; }

        public IReadOnlyCollection<string> FieldNames { get; set; } = new List<string>();

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public IReadOnlyCollection<ICondition> Conditions { get; set; } = new List<ICondition>();

        public IReadOnlyCollection<ISortOrder> SortOrders { get; set; } = new List<ISortOrder>();

        public IFieldSelectionQueryBuilder ToBuilder() => throw new NotImplementedException();

        IQueryBuilder IQuery.ToBuilder() => ToBuilder();
    }

    private sealed class DataObjectNameQuery : IDataObjectNameQuery
    {
        public string DataObjectName { get; set; } = string.Empty;

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public IReadOnlyCollection<ICondition> Conditions => Array.Empty<ICondition>();

        public IReadOnlyCollection<ISortOrder> SortOrders => Array.Empty<ISortOrder>();

        public IDataObjectNameQueryBuilder ToBuilder() => throw new NotImplementedException();

        IQueryBuilder IQuery.ToBuilder() => ToBuilder();
    }
}
