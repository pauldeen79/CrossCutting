namespace CrossCutting.Utilities.Parsers.Tests.FunctionCallTypeArguments;

public class ExpressionTypeArgumentTests
{
    public class IsDynamic : ExpressionTypeArgumentTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ExpressionTypeArgumentBuilder().WithExpression(typeof(string).FullName!).BuildTyped();

            // Act
            var result = sut.IsDynamic;

            // Assert
            result.ShouldBeFalse();
        }
    }

    public class Evaluate : ExpressionTypeArgumentTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Success_Expression()
        {
            // Arrange
            var sut = new ExpressionTypeArgumentBuilder().WithExpression("Some expression").BuildTyped();
            var functionEvaluator = Substitute.For<IFunctionEvaluator>();
            var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
            expressionEvaluator
                .Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                .Returns(Result.Success<object?>(typeof(short)));
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy"), functionEvaluator, expressionEvaluator, new FunctionEvaluatorSettingsBuilder(), null);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(short));
        }

        [Fact]
        public void Returns_Correct_Result_On_NotSupported_Expression()
        {
            // Arrange
            var sut = new ExpressionTypeArgumentBuilder().WithExpression(typeof(short).FullName!).BuildTyped();
            var functionEvaluator = Substitute.For<IFunctionEvaluator>();
            var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
            expressionEvaluator
                .Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                .Returns(Result.NotSupported<object?>());
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy"), functionEvaluator, expressionEvaluator, new FunctionEvaluatorSettingsBuilder(), null);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(short));
        }

        [Fact]
        public void Returns_Correct_Result_On_NotSupported_Expression_With_Unknown_TypeName()
        {
            // Arrange
            var sut = new ExpressionTypeArgumentBuilder().WithExpression("Unknown").BuildTyped();
            var functionEvaluator = Substitute.For<IFunctionEvaluator>();
            var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
            expressionEvaluator
                .Evaluate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                .Returns(Result.NotSupported<object?>());
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy"), functionEvaluator, expressionEvaluator, new FunctionEvaluatorSettingsBuilder(), null);

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: Unknown");
        }
    }

    public class Validate : ExpressionTypeArgumentTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Success_Expression()
        {
            // Arrange
            var sut = new ExpressionTypeArgumentBuilder().WithExpression("Some expression").BuildTyped();
            var functionEvaluator = Substitute.For<IFunctionEvaluator>();
            var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
            expressionEvaluator
                .Validate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                .Returns(Result.Success(typeof(short)));
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy"), functionEvaluator, expressionEvaluator, new FunctionEvaluatorSettingsBuilder(), null);

            // Act
            var result = sut.Validate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(short));
        }

        [Fact]
        public void Returns_Correct_Result_On_Invalid_Expression()
        {
            // Arrange
            var sut = new ExpressionTypeArgumentBuilder().WithExpression("Some expression").BuildTyped();
            var functionEvaluator = Substitute.For<IFunctionEvaluator>();
            var expressionEvaluator = Substitute.For<IExpressionEvaluator>();
            expressionEvaluator
                .Validate(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                .Returns(Result.Invalid<Type>("Unknown expression type found in fragment: Some expression"));
            var context = new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy"), functionEvaluator, expressionEvaluator, new FunctionEvaluatorSettingsBuilder(), null);

            // Act
            var result = sut.Validate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            result.Value.ShouldBeNull();
        }
    }
}
