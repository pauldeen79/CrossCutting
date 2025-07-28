namespace CrossCutting.Utilities.QueryEvaluator.Tests;
public class SingleEntityQueryParserTests
{
    [Theory]
    [InlineData("CONTAINS", "MyValue", typeof(StringContainsConditionBuilder))]
    [InlineData("ENDSWITH", "MyValue", typeof(StringEndsWithConditionBuilder))]
    [InlineData("\"ENDS WITH\"", "MyValue", typeof(StringEndsWithConditionBuilder))]
    [InlineData("=", "MyValue", typeof(EqualConditionBuilder))]
    [InlineData("==", "MyValue", typeof(EqualConditionBuilder))]
    [InlineData(">=", "MyValue", typeof(GreaterThanOrEqualConditionBuilder))]
    [InlineData(">", "MyValue", typeof(GreaterThanConditionBuilder))]
    [InlineData("\"IS NOT\"", "NULL", typeof(IsNotNullConditionBuilder))]
    [InlineData("IS", "NULL", typeof(IsNullConditionBuilder))]
    [InlineData("<=", "MyValue", typeof(SmallerThanOrEqualConditionBuilder))]
    [InlineData("<", "MyValue", typeof(SmallerThanConditionBuilder))]
    [InlineData("NOTCONTAINS", "MyValue", typeof(StringNotContainsConditionBuilder))]
    [InlineData("\"NOT CONTAINS\"", "MyValue", typeof(StringNotContainsConditionBuilder))]
    [InlineData("NOTENDSWITH", "MyValue", typeof(StringNotEndsWithConditionBuilder))]
    [InlineData("\"NOT ENDS WITH\"", "MyValue", typeof(StringNotEndsWithConditionBuilder))]
    [InlineData("<>", "MyValue", typeof(NotEqualConditionBuilder))]
    [InlineData("!=", "MyValue", typeof(NotEqualConditionBuilder))]
    [InlineData("#", "MyValue", typeof(NotEqualConditionBuilder))]
    [InlineData("NOTSTARTSWITH", "MyValue", typeof(StringNotStartsWithConditionBuilder))]
    [InlineData("\"NOT STARTS WITH\"", "MyValue", typeof(StringNotStartsWithConditionBuilder))]
    [InlineData("STARTSWITH", "MyValue", typeof(StringStartsWithConditionBuilder))]
    [InlineData("\"STARTS WITH\"", "MyValue", typeof(StringStartsWithConditionBuilder))]
    public void Can_Parse_EntityQuery_With_Operator(string @operator, string value, Type expectedConditionBuilder)
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, $"MyFieldName {@operator} {value}");

        // Assert
        actual.Conditions.Count.ShouldBe(1);
        var conditionField = (actual.Conditions[0] as ISingleExpressionContainerBuilder)?.FirstExpression as PropertyNameExpressionBuilder;
        var conditionValue = ((actual.Conditions[0] as IDoubleExpressionContainerBuilder)?.SecondExpression as LiteralExpressionBuilder)?.Value;
        conditionField.ShouldNotBeNull();
        conditionField.PropertyName.ShouldBe("MyFieldName");
        actual.Conditions[0].ShouldBeOfType(expectedConditionBuilder);
        conditionValue.ShouldBe(value == "NULL" ? null : value);
    }

    [Fact]
    public void Can_Parse_EntityQuery_With_Space_In_Value()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, $"MyFieldName = \"My Value\"");

        // Assert
        actual.Conditions.Count.ShouldBe(1);
        var conditionField = (actual.Conditions[0] as ISingleExpressionContainerBuilder)?.FirstExpression as PropertyNameExpressionBuilder;
        var conditionValue = ((actual.Conditions[0] as IDoubleExpressionContainerBuilder)?.SecondExpression as LiteralExpressionBuilder)?.Value;
        conditionField.ShouldNotBeNull();
        conditionField.PropertyName.ShouldBe("MyFieldName");
        actual.Conditions[0].ShouldBeOfType<EqualConditionBuilder>();
        conditionValue.ShouldBe("My Value");
    }

    [Fact]
    public void Can_Parse_SimpleQuery_With_Two_Words()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, "First Second");

        // Assert
        actual.Conditions.Count.ShouldBe(2);
    }

    [Fact]
    public void Can_Parse_SimpleQuery_With_Invalid_Operator()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, "MyFieldName = MyFirstValue AND MyOtherFieldName ? MySecondValue");

        // Assert
        actual.Conditions.Count.ShouldBe(7);
    }

    [Fact]
    public void Can_Parse_Empty_Query()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, string.Empty);

        // Assert
        actual.Conditions.ShouldBeEmpty();
    }

    private static SingleEntityQueryParser<IQueryBuilder, PropertyNameExpressionBuilder> CreateSut()
        => new(() => new PropertyNameExpressionBuilder().WithPropertyName("PrefilledField"));
}
