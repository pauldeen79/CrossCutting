namespace CrossCutting.Utilities.QueryEvaluator.Tests.Builders.Extensions;

public class QueryBuilderExtensionsTests : TestBase
{
    [Fact]
    public void Where_Adds_Conditions_Correctly()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();
        var conditions = new[] { new NullConditionBuilder().WithSourceExpression(new PropertyNameEvaluatableBuilder("Property")) }.AsEnumerable();

        // Act
        var result = sut.Where(conditions);

        // Assert
        result.Conditions.Count.ShouldBe(1);
        result.Conditions[0].Combination.ShouldBeNull();
    }

    [Fact]
    public void And_Adds_Conditions_Correctly()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();
        var conditions = new[] { new NullConditionBuilder().WithSourceExpression(new PropertyNameEvaluatableBuilder("Property")) }.AsEnumerable();

        // Act
        var result = sut.And(conditions);

        // Assert
        result.Conditions.Count.ShouldBe(1);
        result.Conditions[0].Combination.ShouldBe(Combination.And);
    }

    [Fact]
    public void Or_Adds_Conditions_Correctly()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();
        var conditions = new[] { new NullConditionBuilder().WithSourceExpression(new PropertyNameEvaluatableBuilder("Property")) }.AsEnumerable();

        // Act
        var result = sut.Or(conditions);

        // Assert
        result.Conditions.Count.ShouldBe(1);
        result.Conditions[0].Combination.ShouldBe(Combination.Or);
    }

    [Fact]
    public void OrderBy_Adds_SortOrders_Correctly()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();
        var sortOrders = new[] { new SortOrderBuilder().WithPropertyName("Property") }.AsEnumerable();

        // Act
        var result = sut.OrderBy(sortOrders);

        // Assert
        result.SortOrders.Count.ShouldBe(1);
        result.SortOrders[0].Order.ShouldBe(SortOrderDirection.Ascending);
    }
    
    [Fact]
    public void ThenBy_Adds_SortOrders_Correctly()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();
        var sortOrders = new[] { new SortOrderBuilder().WithPropertyName("Property") }.AsEnumerable();

        // Act
        var result = sut.ThenBy(sortOrders);

        // Assert
        result.SortOrders.Count.ShouldBe(1);
        result.SortOrders[0].Order.ShouldBe(SortOrderDirection.Ascending);
    }
    
    [Fact]
    public void OrderByDescending_Adds_SortOrders_Correctly()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();
        var sortOrders = new[] { new SortOrderBuilder().WithPropertyName("Property") }.AsEnumerable();

        // Act
        var result = sut.OrderByDescending(sortOrders);

        // Assert
        result.SortOrders.Count.ShouldBe(1);
        result.SortOrders[0].Order.ShouldBe(SortOrderDirection.Descending);
    }
    
    [Fact]
    public void ThenByDescending_Adds_SortOrders_Correctly()
    {
        // Arrange
        var sut = new SingleEntityQueryBuilder();
        var sortOrders = new[] { new SortOrderBuilder().WithPropertyName("Property") }.AsEnumerable();

        // Act
        var result = sut.ThenByDescending(sortOrders);

        // Assert
        result.SortOrders.Count.ShouldBe(1);
        result.SortOrders[0].Order.ShouldBe(SortOrderDirection.Descending);
    }
}