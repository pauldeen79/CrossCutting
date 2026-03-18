namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public class ComposableEvaluatableBuilderHelperTests : TestBase
{
    public class Create_FieldName_Value : ComposableEvaluatableBuilderHelperTests
    {
        [Fact]
        public void Fills_ConditionBuilder_Correctly()
        {
            // Arrange
            var condition = new EqualConditionBuilder();

            // Act
            var result = ComposableEvaluatableBuilderHelper.Create("MyProperty", condition, "literal value");

            // Assert
            result.ShouldBeOfType<EqualConditionBuilder>();
            var equalConditionBuilderResult = (EqualConditionBuilder)result;
            equalConditionBuilderResult.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)equalConditionBuilderResult.SourceExpression).PropertyName.ShouldBe("MyProperty");
            equalConditionBuilderResult.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)equalConditionBuilderResult.CompareExpression).Value.ShouldBe("literal value");
        }
    }

    public class Create_FieldName_EvaluatableBuilder : ComposableEvaluatableBuilderHelperTests
    {
        [Fact]
        public void Fills_ConditionBuilder_Correctly()
        {
            // Arrange
            var condition = new EqualConditionBuilder();

            // Act
            var result = ComposableEvaluatableBuilderHelper.Create("MyProperty", condition, new LiteralEvaluatableBuilder("literal value"));

            // Assert
            result.ShouldBeOfType<EqualConditionBuilder>();
            var equalConditionBuilderResult = (EqualConditionBuilder)result;
            equalConditionBuilderResult.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)equalConditionBuilderResult.SourceExpression).PropertyName.ShouldBe("MyProperty");
            equalConditionBuilderResult.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)equalConditionBuilderResult.CompareExpression).Value.ShouldBe("literal value");
        }
    }

    public class Create_FieldName_EmptyRightExpression : ComposableEvaluatableBuilderHelperTests
    {
        [Fact]
        public void Fills_ConditionBuilder_Correctly()
        {
            // Arrange
            var condition = new NullConditionBuilder();

            // Act
            var result = ComposableEvaluatableBuilderHelper.Create("MyProperty", condition);

            // Assert
            result.ShouldBeOfType<NullConditionBuilder>();
            var nullConditionBuilderResult = (NullConditionBuilder)result;
            nullConditionBuilderResult.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)nullConditionBuilderResult.SourceExpression).PropertyName.ShouldBe("MyProperty");
        }
    }

    public class Create_EvaluatableBuilder_Value : ComposableEvaluatableBuilderHelperTests
    {
        [Fact]
        public void Fills_ConditionBuilder_Correctly()
        {
            // Arrange
            var condition = new EqualConditionBuilder();

            // Act
            var result = ComposableEvaluatableBuilderHelper.Create(new PropertyNameEvaluatableBuilder("MyProperty"), condition, "literal value");

            // Assert
            result.ShouldBeOfType<EqualConditionBuilder>();
            var equalConditionBuilderResult = (EqualConditionBuilder)result;
            equalConditionBuilderResult.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)equalConditionBuilderResult.SourceExpression).PropertyName.ShouldBe("MyProperty");
            equalConditionBuilderResult.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)equalConditionBuilderResult.CompareExpression).Value.ShouldBe("literal value");
        }
    }

    public class Create_EvaluatableBuilder_EvaluatableBuilder : ComposableEvaluatableBuilderHelperTests
    {
        [Fact]
        public void Fills_ConditionBuilder_Correctly()
        {
            // Arrange
            var condition = new EqualConditionBuilder();

            // Act
            var result = ComposableEvaluatableBuilderHelper.Create(new PropertyNameEvaluatableBuilder("MyProperty"), condition, new LiteralEvaluatableBuilder("literal value"));

            // Assert
            result.ShouldBeOfType<EqualConditionBuilder>();
            var equalConditionBuilderResult = (EqualConditionBuilder)result;
            equalConditionBuilderResult.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)equalConditionBuilderResult.SourceExpression).PropertyName.ShouldBe("MyProperty");
            equalConditionBuilderResult.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)equalConditionBuilderResult.CompareExpression).Value.ShouldBe("literal value");
        }
    }

    public class Create_EvaluatableBuilder_EmptyRightExpression : ComposableEvaluatableBuilderHelperTests
    {
        [Fact]
        public void Fills_ConditionBuilder_Correctly()
        {
            // Arrange
            var condition = new NullConditionBuilder();

            // Act
            var result = ComposableEvaluatableBuilderHelper.Create(new PropertyNameEvaluatableBuilder("MyProperty"), condition);

            // Assert
            result.ShouldBeOfType<NullConditionBuilder>();
            var nullConditionBuilderResult = (NullConditionBuilder)result;
            nullConditionBuilderResult.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)nullConditionBuilderResult.SourceExpression).PropertyName.ShouldBe("MyProperty");
        }
    }
}