namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallTypeArguments;

public class FunctionTypeArgumentTests
{
    public class IsDynamic : FunctionTypeArgumentTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new FunctionTypeArgumentBuilder().WithFunction(new FunctionCallBuilder().WithName("Dummy")).BuildTyped();

            // Act
            var result = sut.IsDynamic;

            // Assert
            result.ShouldBeTrue();
        }
    }

    public class Evaluate : FunctionTypeArgumentTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new FunctionTypeArgumentBuilder().WithFunction(new FunctionCallBuilder().WithName("Dummy")).BuildTyped();
            var functionEvaluator = Substitute.For<IFunctionEvaluator>();
            var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
            functionEvaluator
                .Evaluate(Arg.Any<FunctionCall>(), Arg.Any<FunctionEvaluatorSettings>(), Arg.Any<object?>())
                .Returns(Result.Success<object?>(typeof(short)));
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy"), functionEvaluator, expressionEvaluator, new FunctionEvaluatorSettingsBuilder(), null);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(short));
        }
    }

    public class Validate : FunctionTypeArgumentTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new FunctionTypeArgumentBuilder().WithFunction(new FunctionCallBuilder().WithName("Dummy")).BuildTyped();
            var functionEvaluator = Substitute.For<IFunctionEvaluator>();
            var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
            functionEvaluator
                .Validate(Arg.Any<FunctionCall>(), Arg.Any<FunctionEvaluatorSettings>(), Arg.Any<object?>())
                .Returns(Result.Success(typeof(short)));
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy"), functionEvaluator, expressionEvaluator, new FunctionEvaluatorSettingsBuilder(), null);

            // Act
            var result = sut.Validate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(short));
        }
    }
}
