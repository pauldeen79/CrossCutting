namespace CrossCutting.Utilities.QueryEvaluator.Tests.QueryParsers;

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
    [InlineData("ISNOT", "NULL", typeof(NotNullConditionBuilder))]
    [InlineData("\"IS NOT\"", "NULL", typeof(NotNullConditionBuilder))]
    [InlineData("IS", "NULL", typeof(NullConditionBuilder))]
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
        var conditionField = (actual.Conditions[0] as ISingleExpressionContainerBuilder)?.SourceExpression as PropertyNameExpression;
        var conditionValue = ((actual.Conditions[0] as IDoubleExpressionContainerBuilder)?.CompareExpression as LiteralExpression)?.Value;
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
        var conditionField = (actual.Conditions[0] as ISingleExpressionContainerBuilder)?.SourceExpression as PropertyNameExpression;
        var conditionValue = ((actual.Conditions[0] as IDoubleExpressionContainerBuilder)?.CompareExpression as LiteralExpression)?.Value;
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
    public void Can_Parse_SimpleQuery_With_Plus_And_Minus()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, "-First +Second");

        // Assert
        actual.Conditions.Count.ShouldBe(2);
        actual.Conditions[0].ShouldBeOfType<StringNotContainsConditionBuilder>();
        actual.Conditions[1].ShouldBeOfType<StringContainsConditionBuilder>();
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

    [Fact]
    public void Can_Parse_Query_With_Brackets()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, $"(MyFieldName = \"SomeValue\")");

        // Assert
        actual.Conditions.Count.ShouldBe(1);
        actual.Conditions[0].StartGroup.ShouldBe(true);
        actual.Conditions[0].EndGroup.ShouldBe(true);
    }

    [Fact]
    public void Can_Parse_Query_With_Combination()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();

        // Act
        var actual = sut.Parse(builder, $"MyFieldName = \"SomeValue\" OR MyFieldname = \"SomeOtherValue\"");

        // Assert
        actual.Conditions.Count.ShouldBe(2);
        actual.Conditions[0].Combination.ShouldBe(Combination.And);
        actual.Conditions[1].Combination.ShouldBe(Combination.Or);
    }

    [Fact]
    public void Validation_Fails_With_Too_Many_Open_Bracket()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();
        var queryBuilder = sut.Parse(builder, $"(MyFieldName = \"SomeValue\" OR MyFieldname = \"SomeOtherValue\"");
        var validationResults = new List<ValidationResult>();

        // Act
        var validationResult = queryBuilder.TryValidate(validationResults);

        // Assert
        validationResult.ShouldBeFalse();
        validationResults.Count.ShouldBe(1);
        validationResults[0].ErrorMessage.ShouldBe("Missing EndGroup");
    }

    [Fact]
    public void Validation_Fails_With_Too_Many_Open_Brackets()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();
        var queryBuilder = sut.Parse(builder, $"(MyFieldName = \"SomeValue\" OR (MyFieldname = \"SomeOtherValue\"");
        var validationResults = new List<ValidationResult>();

        // Act
        var validationResult = queryBuilder.TryValidate(validationResults);

        // Assert
        validationResult.ShouldBeFalse();
        validationResults.Count.ShouldBe(1);
        validationResults[0].ErrorMessage.ShouldBe("2 missing EndGroups");
    }

    [Fact]
    public void Validation_Fails_With_Too_Many_Close_Brackets()
    {
        // Arrange
        var builder = new SingleEntityQueryBuilder();
        var sut = CreateSut();
        var queryBuilder = sut.Parse(builder, $"MyFieldName = \"SomeValue\") OR MyFieldname = \"SomeOtherValue\")");
        var validationResults = new List<ValidationResult>();

        // Act
        var validationResult = queryBuilder.TryValidate(validationResults);

        // Assert
        validationResult.ShouldBeFalse();
        validationResults.Count.ShouldBe(1);
        validationResults[0].ErrorMessage.ShouldBe("EndGroup not valid at index 0, because there is no corresponding StartGroup");
    }

    private static SingleEntityQueryParser<IQueryBuilder, PropertyNameExpression> CreateSut()
        => new(() => new PropertyNameExpression(new ContextExpression(), "PrefilledField"));
}
