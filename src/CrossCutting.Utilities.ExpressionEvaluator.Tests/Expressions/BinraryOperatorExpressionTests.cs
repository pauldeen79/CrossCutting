namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class BinaryOperatorExpressionTests : TestBase<BinaryOperatorExpression>
{
    public class Evaluate : BinaryOperatorExpressionTests
    {
        [Fact]
        public void Returns_Continue_When_Expression_Does_Not_Contain_Any_Comparison_Characters()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "Some expression not containing binary characters like 'and'";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Invalid_When_Expression_Contains_Comparison_Character_But_Is_Not_Complete()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "\"A\" &&";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Comparison expression has invalid number of parts");
        }

        [Fact]
        public void Returns_Continue_On_Malformed_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "&&";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Success_On_Valid_Single_Part_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "true && false";

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
            var expression = "true && true && \"string value\"";

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
            var expression = "(true && true) || false";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Non_Successful_Result_From_Inner_Expression_Simple_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 && error";

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
            var expression = "(2 && error)";

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Parse : BinaryOperatorExpressionTests
    {
        [Fact]
        public void Returns_Continue_When_Expression_Does_Not_Contain_Any_Comparison_Characters()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "Some expression not containing binary characters like 'and'";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Invalid_When_Expression_Contains_Comparison_Character_But_Is_Not_Complete()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "\"A\" &&";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Comparison expression has invalid number of parts");
        }

        [Fact]
        public void Returns_Continue_On_Malformed_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "&&";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Success_On_Valid_Single_Part_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "true && false";

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
            var expression = "true && true && \"string value\"";

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
            var expression = "(true && true) || false";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Non_Successful_Result_From_Inner_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var expression = "2 && error";

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Parsing of the expression failed, see inner results for details");
            result.PartResults.Count.ShouldBe(2);
            result.PartResults.First().Status.ShouldBe(ResultStatus.Ok);
            result.PartResults.Last().Status.ShouldBe(ResultStatus.Error);
            result.PartResults.Last().ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Evaluate_Comparison : BinaryOperatorExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Complex_Query_With_All_Types_Of_Combinations()
        {
            // Arrange
            var conditions = new BinaryConditionGroup(
            [
                new BinaryConditionBuilder().WithStartGroup().WithExpression("true"),
                new BinaryConditionBuilder().WithCombination(Combination.And).WithExpression("true").WithEndGroup(),
                new BinaryConditionBuilder().WithCombination(Combination.Or).WithExpression("false")
            ]);
            var context = CreateContext("Dummy"); // only needed for recursive calls

            // Act
            var result = BinaryOperatorExpression.Evaluate(context, conditions);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }
}
