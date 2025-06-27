namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public class Tests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var query = new SingleEntityQueryBuilder()
            .With(x => x.Filter.Add(new ComposableConditionBuilder()
                .WithLeftExpression(new FieldNameExpressionBuilder().WithFieldName("MyField"))
                .WithOperator(new EqualsOperatorBuilder())
                .WithRightExpression(new LiteralExpressionBuilder().WithValue("A"))))
            .With(x => x.Limit = 50)
            .With(x => x.Offset = 0)
            .With(x => x.OrderByFields.Add(new QuerySortOrderBuilder()
                .WithExpression(new FieldNameExpressionBuilder().WithFieldName("MyField"))
                .WithOrder(Domains.QuerySortOrderDirection.Ascending)))
            .Build();

        // Act
        //var result = queryProcessor.Execute(query);

        // Assert
        query.ShouldNotBeNull();
    }
}
