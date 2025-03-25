namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class ComparisonOperatorExpressionTests : TestBase
{
    protected static ComparisonOperatorExpression CreateSut() => new ComparisonOperatorExpression([new EqualOperator(), new GreaterOrEqualThanOperator(), new GreaterThanOperator(), new NotEqualOperator(), new SmallerOrEqualThanOperator(), new SmallerThanOperator()]);

    public class Evaluate : ComparisonOperatorExpressionTests
    {
        [Fact]
        public void Returns_Continue_When_Expression_Does_Not_Contain_Any_Comparison_Characters()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "Some expression not containing comparison characters like equals";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Continue_When_Expression_Contains_Comparison_Character_But_Is_Not_Complete()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "A ==";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Invalid_On_Wrong_Number_Of_Expression_Parts()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "context == \"some value\" AND context.Length >";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Comparison expression has invalid number of parts");
        }

        [Fact]
        public void Returns_Invalid_On_Malformed_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "context && \"some value\" && B == C";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Comparison expression is malformed");
        }

        [Fact]
        public void Returns_Success_On_Valid_Single_Part_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "context == \"some value\"";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_On_Valid_Multiple_Part_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "context == \"some value\" && context.Length > 1";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_On_Valid_Expression_With_Brackets()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "(context >= 1 && context <= 5) || context == \"some other value\"";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Value_On_SmallerThanOrEqual_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "1 <= 2";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Correct_Value_On_SmallerThan_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "1 < 2";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Correct_Value_On_GreaterThanOrEqual_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 >= 1";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Correct_Value_On_GreaterThan_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 > 1";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Correct_Value_On_Equal_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "1 == 1";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Correct_Value_On_NotEqual_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 != 1";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Value.ShouldBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Non_Successful_Result_From_Inner_Expression_Simple_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 != error";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Non_Successful_Result_From_Inner_Expression_Complex_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "(2 != error)";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Parse : ComparisonOperatorExpressionTests
    {
        [Fact]
        public void Returns_Continue_When_Expression_Does_Not_Contain_Any_Comparison_Characters()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "Some expression not containing comparison characters like equals";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Continue_When_Expression_Contains_Comparison_Character_But_Is_Not_Complete()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "A ==";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Invalid_On_Wrong_Number_Of_Expression_Parts()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "context == \"some value\" AND context.Length >";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Comparison expression has invalid number of parts");
        }

        [Fact]
        public void Returns_Invalid_On_Malformed_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "context && \"some value\" && B == C";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Comparison expression is malformed");
        }

        [Fact]
        public void Returns_Success_On_Valid_Single_Part_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "context == \"some value\"";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_On_Valid_Multiple_Part_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "context == \"some value\" && context.Length > 1";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Success_On_Valid_Expression_With_Brackets()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "(context >= 1 && context <= 5) || context == \"some other value\"";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Value_On_SmallerThanOrEqual_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "1 <= 2";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Value.ShouldNotBeNull();
            result.Value.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Correct_Value_On_SmallerThan_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "1 < 2";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Value.ShouldNotBeNull();
            result.Value.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Correct_Value_On_GreaterThanOrEqual_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 >= 1";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Value.ShouldNotBeNull();
            result.Value.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Correct_Value_On_GreaterThan_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 > 1";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Value.ShouldNotBeNull();
            result.Value.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Correct_Value_On_Equal_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "1 == 1";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Value.ShouldNotBeNull();
            result.Value.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Correct_Value_On_NotEqual_Operator()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 != 1";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Value.ShouldNotBeNull();
            result.Value.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public void Returns_Non_Successful_Result_From_Inner_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 != error";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.InnerResults.Count.ShouldBe(2);
            result.InnerResults.First().Status.ShouldBe(ResultStatus.Ok);
            result.InnerResults.Last().Status.ShouldBe(ResultStatus.Error);
            result.InnerResults.Last().ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Evaluate_Comparison : ComparisonOperatorExpressionTests
    {
        [Fact]
        public void Returns_Non_Successful_Result_From_Comparison_Result()
        {
            // Arrange
            var context = CreateContext("Dummy"); // only needed for recursive calls

            // Act
            var result = ComparisonOperatorExpression.Evaluate(context, Result.Error<ComparisonConditionGroup>("Kaboom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Correct_Result_On_Complex_Query_With_All_Types_Of_Combinations()
        {
            // Arrange
            var conditionsResult = Result.Success(new ComparisonConditionGroup(
            [
                new ComparisonConditionBuilder().WithStartGroup().WithLeftExpression("1").WithOperator(new EqualOperator()).WithRightExpression("1"),
                new ComparisonConditionBuilder().WithCombination(Combination.And).WithLeftExpression("2").WithOperator(new NotEqualOperator()).WithRightExpression("1").WithEndGroup(),
                new ComparisonConditionBuilder().WithCombination(Combination.Or).WithLeftExpression("\"some text\"").WithOperator(new GreaterThanOperator()).WithRightExpression("\"zzz\"")
            ]));
            var context = CreateContext("Dummy"); // only needed for recursive calls

            // Act
            var result = ComparisonOperatorExpression.Evaluate(context, conditionsResult);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }

        [Fact]
        public void Can_Perform_Null_Check()
        {
            // Arrange
            var conditionsResult = Result.Success(new ComparisonConditionGroup(
            [
                new ComparisonConditionBuilder().WithLeftExpression("context").WithOperator(new EqualOperator()).WithRightExpression("null")
            ]));
            var context = CreateContext("Dummy", context: null); // only needed for recursive calls. explicitly setting context to null

            // Act
            var result = ComparisonOperatorExpression.Evaluate(context, conditionsResult);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }
}
