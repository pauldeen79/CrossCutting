namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class StringEndsWithOperatorEvaluatableHandlerTests : TestBase<StringEndsWithOperatorEvaluatableHandler>
{
    public class GetExpressionAsync : StringEndsWithOperatorEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_SqlExpression_On_Right_Evaluatable()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new StringEndsWithOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder("Test"))
                .WithRightOperand(new LiteralEvaluatableBuilder("T"))
                .Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([new LiteralEvaluatableHandler()]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("@p0 LIKE @p1");
            parameterBag.Parameters.Count.ShouldBe(2);
            parameterBag.Parameters.First().Key.ShouldBe("@p0");
            parameterBag.Parameters.First().Value.ShouldBe("Test");
            parameterBag.Parameters.Last().Key.ShouldBe("@p1");
            parameterBag.Parameters.Last().Value.ShouldBe("%T");
        }

        [Fact]
        public async Task Returns_SqlExpression_On_Right_Evaluatable_With_Inversion()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new UnaryNegateOperatorEvaluatableBuilder()
                .WithOperand(
                        new StringEndsWithOperatorEvaluatableBuilder()
                            .WithLeftOperand(new LiteralEvaluatableBuilder("Test"))
                            .WithRightOperand(new LiteralEvaluatableBuilder("T")))
                .Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([new LiteralEvaluatableHandler()]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("@p0 NOT LIKE @p1");
            parameterBag.Parameters.Count.ShouldBe(2);
            parameterBag.Parameters.First().Key.ShouldBe("@p0");
            parameterBag.Parameters.First().Value.ShouldBe("Test");
            parameterBag.Parameters.Last().Key.ShouldBe("@p1");
            parameterBag.Parameters.Last().Value.ShouldBe("%T");
        }

        [Fact]
        public async Task Skips_When_Evaluatable_Is_Of_Wrong_Type()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new EmptyEvaluatable();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([new LiteralEvaluatableHandler()]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }
}