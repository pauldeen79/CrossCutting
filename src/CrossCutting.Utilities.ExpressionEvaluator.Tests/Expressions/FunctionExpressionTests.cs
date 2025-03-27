namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class FunctionExpressionTests : TestBase<FunctionExpression>
{
    public class Evaluate : FunctionExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_Without_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("MyFunction()");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("MyFunction(argument1, argument2, argument3)");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Generics_Without_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("MyFunction<System.String>()");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Generics_And_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("MyFunction<System.String>(argument1, argument2, argument3)");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Correct_Result_On_FunctionCall_With_Nested_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("MyFunction(argument1, MySubFunction(argument2), argument3)");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
