namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public sealed class IntegrationTests : TestBase
{
    [Fact]
    public async Task Can_Query_Entity()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .AddFilter(new EqualsConditionBuilder()
                .WithFirstExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property1)))
                .WithSecondExpression(new LiteralExpressionBuilder().WithValue("A")))
            .AddOrderByFields(new QuerySortOrderBuilder()
                .WithExpression(new FieldNameExpressionBuilder().WithFieldName(nameof(MyEntity.Property2)))
                .WithOrder(QuerySortOrderDirection.Ascending))
            .Build();

        InitializeMock(
            [
                new MyEntity("B", "C"),
                new MyEntity("A", "Z"),
                new MyEntity("B", "D"),
                new MyEntity("A", "A"),
                new MyEntity("B", "E"),
            ]);

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
    public async Task Can_Create_ExpressionEvaluator_And_Evaluate_Expression()
    {
        // Act
        var now = await Evaluator.EvaluateAsync(CreateContext("DateTime.Now"));

        // Assert
        now.Status.ShouldBe(ResultStatus.Ok);
        now.Value.ShouldBe(CurrentDateTime);
    }
}
