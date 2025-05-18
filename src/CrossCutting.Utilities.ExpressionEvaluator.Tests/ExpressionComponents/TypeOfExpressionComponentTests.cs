namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class TypeOfExpressionComponentTests  : TestBase<TypeOfExpressionComponent>
{
    public class Evaluate : TypeOfExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Continue_When_Expression_Is_Not_A_TypeOf_Expression()
        {
            // Arrange
            var context = CreateContext("Some expression not being typeof");
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Invalid_When_Expression_A_TypeOf_Expression_With_Unknown_Type()
        {
            // Arrange
            var context = CreateContext("typeof(unknowntype)");
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }

        [Fact]
        public async Task Returns_Ok_When_Expression_A_TypeOf_Expression_With_Known_Type()
        {
            // Arrange
            var context = CreateContext("typeof(System.String)");
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(string));
        }
    }

    public class Parse : TypeOfExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Continue_When_Expression_Is_Not_A_TypeOf_Expression()
        {
            // Arrange
            var context = CreateContext("Some expression not being typeof");
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Invalid_When_Expression_A_TypeOf_Expression_With_Unknown_Type()
        {
            // Arrange
            var context = CreateContext("typeof(unknowntype)");
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown type: unknowntype");
        }

        [Fact]
        public async Task Returns_Ok_When_Expression_A_TypeOf_Expression_With_Known_Type()
        {
            // Arrange
            var context = CreateContext("typeof(System.String)");
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(Type));
        }
    }
}
