namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class BinaryOperatorEvaluatableHandlerUtilTests : TestBase
{
    public class GetSqlExpression : BinaryOperatorEvaluatableHandlerUtilTests
    {
        [Fact]
        public void Returns_Non_Successful_Result_When_Left_Operand_Is_Not_Succesful()
        {
            // Arrange
            var left = Result.Error("Kaboom");
            var right = Result.Success("A = 1");
            var results = new ResultDictionaryBuilder<string>()
                .Add("LeftOperand", () => left)
                .Add("RightOperand", () => right)
                .Build();

            // Act
            var result = BinaryOperatorEvaluatableHandlerUtil.GetSqlExpression(results, "AND");

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Non_Successful_Result_When_Right_Operand_Is_Not_Succesful()
        {
            // Arrange
            var left = Result.Success("A = 1");
            var right = Result.Error("Kaboom");
            var results = new ResultDictionaryBuilder<string>()
                .Add("LeftOperand", () => left)
                .Add("RightOperand", () => right)
                .Build();

            // Act
            var result = BinaryOperatorEvaluatableHandlerUtil.GetSqlExpression(results, "AND");

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Empty_String_When_Left_And_Right_Operands_Are_Empty()
        {
            // Arrange
            var left = Result.Success((object?)null);
            var right = Result.Success((object?)null);
            var results = new ResultDictionaryBuilder<string>()
                .Add("LeftOperand", () => left)
                .Add("RightOperand", () => right)
                .Build();

            // Act
            var result = BinaryOperatorEvaluatableHandlerUtil.GetSqlExpression(results, "AND");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEmpty();
        }

        [Fact]
        public void Returns_Left_Operand_When_Only_Left_Operands_Is_Filled()
        {
            // Arrange
            var left = Result.Success("A = 1");
            var right = Result.Success((object?)null);
            var results = new ResultDictionaryBuilder<string>()
                .Add("LeftOperand", () => left)
                .Add("RightOperand", () => right)
                .Build();

            // Act
            var result = BinaryOperatorEvaluatableHandlerUtil.GetSqlExpression(results, "AND");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("A = 1");
        }

        [Fact]
        public void Returns_Right_Operand_When_Only_Left_Operands_Is_Filled()
        {
            // Arrange
            var left = Result.Success((object?)null);
            var right = Result.Success("A = 1");
            var results = new ResultDictionaryBuilder<string>()
                .Add("LeftOperand", () => left)
                .Add("RightOperand", () => right)
                .Build();

            // Act
            var result = BinaryOperatorEvaluatableHandlerUtil.GetSqlExpression(results, "AND");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("A = 1");
        }

        [Fact]
        public void Returns_Combined_Expression_When_All_Operands_Are_Filled()
        {
            // Arrange
            var left = Result.Success("A = 1");
            var right = Result.Success("B = 2");
            var results = new ResultDictionaryBuilder<string>()
                .Add("LeftOperand", () => left)
                .Add("RightOperand", () => right)
                .Build();

            // Act
            var result = BinaryOperatorEvaluatableHandlerUtil.GetSqlExpression(results, "AND");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("(A = 1 AND B = 2)");
        }
    }
}