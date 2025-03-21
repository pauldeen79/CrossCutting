namespace CrossCutting.Utilities.Parsers.Tests.Expressions;

public class MathematicExpressionTests
{
    protected IMathematicExpression Expression { get; }
    protected IExpressionEvaluator Evaluator { get; }

    protected MathematicExpression CreateSut() => new MathematicExpression([Expression]);

    protected ExpressionEvaluatorContext CreateContext(string expression)
        => new ExpressionEvaluatorContext(expression, new ExpressionEvaluatorSettingsBuilder(), null, Evaluator);

    protected MathematicExpressionTests()
    {
        Expression = Substitute.For<IMathematicExpression>();
        Evaluator = Substitute.For<IExpressionEvaluator>();
    }

    public class Evaluate : MathematicExpressionTests
    {
        [Fact]
        public void Returns_Non_Successful_Result_From_Mathematic_Expression()
        {
            // Arrange
            Expression.Evaluate(Arg.Any<MathematicExpressionState>()).Returns(Result.Error<MathematicExpressionState>("Kaboom"));
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("some expression"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Validate : MathematicExpressionTests
    {
        [Fact]
        public void Returns_Non_Successful_Result_From_Mathematic_Expression()
        {
            // Arrange
            Expression.Evaluate(Arg.Any<MathematicExpressionState>()).Returns(Result.Error<MathematicExpressionState>("Kaboom"));
            var sut = CreateSut();

            // Act
            var result = sut.Validate(CreateContext("some expression"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
