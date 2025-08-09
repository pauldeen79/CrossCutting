namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class ExpressionEvaluatorTests : TestBase<ExpressionEvaluator>
{
    public class EvaluateAsync : ExpressionEvaluatorTests
    {
        [Fact]
        public async Task Returns_Invalid_When_Expression_Is_Null_Or_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(string.Empty, new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public async Task Returns_First_Understood_Result_When_Not_Equal_To_Continue()
        {
            // Arrange
            Expression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync("expression", new ExpressionEvaluatorSettingsBuilder(), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public async Task Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Continue<object?>());
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }

        [Fact]
        public async Task Returns_Invalid_When_Maximum_Recursion_Has_Been_Reached()
        {
            // Arrange
            Expression
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), sut, null, int.MaxValue), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Maximum recursion level has been reached");
        }

        [Fact]
        public async Task Wraps_Exception_Into_Error_Result()
        {
            // Arrange
            Expression
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Throws<InvalidOperationException>();
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), sut), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
        }
    }

    public class EvaluateTypedAsync : ExpressionEvaluatorTests
    {
        [Fact]
        public async Task Returns_Invalid_When_Expression_Is_Null_Or_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedAsync<string>(string.Empty, new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public async Task Returns_First_Understood_Result_When_Not_Equal_To_Continue()
        {
            // Arrange
            Expression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedAsync<string>("expression", new ExpressionEvaluatorSettingsBuilder(), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public async Task Returns_First_Understood_TypedResult_When_Not_Equal_To_Continue()
        {
            // Arrange
            Expression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedAsync<string>("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public async Task Returns_First_Understood_TypedResult_When_Not_Successful()
        {
            // Arrange
            Expression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Error<object?>("Kaboom"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedAsync<string>("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Continue<object?>());
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedAsync<string>("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }

        [Fact]
        public async Task Returns_Invalid_When_Maximum_Recursion_Has_Been_Reached()
        {
            // Arrange
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedAsync<string>(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), sut, null, int.MaxValue), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Maximum recursion level has been reached");
        }

        [Fact]
        public async Task Wraps_Exception_Into_Error_Result()
        {
            // Arrange
            Expression
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Throws<InvalidOperationException>();
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedAsync<string>(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), sut), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
        }
    }

    public class EvaluateTypedCallbackAsync : ExpressionEvaluatorTests
    {
        [Fact]
        public async Task Returns_Invalid_When_Expression_Is_Null_Or_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedCallbackAsync<string>(CreateContext(string.Empty), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public async Task Returns_First_Understood_Result_When_Not_Equal_To_Continue()
        {
            // Arrange
            Expression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedCallbackAsync<string>(CreateContext("expression"), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("result value");
        }

        [Fact]
        public async Task Returns_First_Understood_TypedResult_When_Not_Successful()
        {
            // Arrange
            var typedExpression = Substitute.For<IExpressionComponent<string>>();
            typedExpression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            typedExpression.EvaluateTypedAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Error<string>("Kaboom"));
            var sut = new ExpressionEvaluator(new ExpressionTokenizer(), new ExpressionParser([]), [typedExpression]);

            // Act
            var result = await sut.EvaluateTypedCallbackAsync<string>(CreateContext("expression"), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression.ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(new ExpressionParseResultBuilder().WithStatus(ResultStatus.Continue));
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Continue<object?>());
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedCallbackAsync<string>(CreateContext("expression"), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }

        [Fact]
        public async Task Returns_Invalid_When_Maximum_Recursion_Has_Been_Reached()
        {
            // Arrange
            Expression.EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>()).Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedCallbackAsync<string>(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), sut, null, int.MaxValue), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Maximum recursion level has been reached");
        }

        [Fact]
        public async Task Wraps_Exception_Into_Error_Result()
        {
            // Arrange
            Expression
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Throws<InvalidOperationException>();
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateTypedCallbackAsync<string>(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), sut), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
        }
    }

    public class ParseAsync : ExpressionEvaluatorTests
    {
        [Fact]
        public async Task Returns_Invalid_When_Expression_Is_Null_Or_Empty()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(string.Empty, new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Value is required");
        }

        [Fact]
        public async Task Returns_First_Understood_Result_When_Not_Equal_To_Continue()
        {
            // Arrange
            // Note that this setup simulates the implementation of ComparisonExpression
            Expression
                .ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(new ExpressionParseResultBuilder()
                    .WithSourceExpression("Dummy")
                    .WithStatus(ResultStatus.Ok)
                    .WithResultType(typeof(bool)));
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync("expression", new ExpressionEvaluatorSettingsBuilder(), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(bool));
        }

        [Fact]
        public async Task Returns_Invalid_When_Expression_Is_Not_Understood()
        {
            // Arrange
            Expression
                .ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(new ExpressionParseResultBuilder()
                    .WithSourceExpression("Dummy")
                    .WithStatus(ResultStatus.Continue));
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync("expression", new ExpressionEvaluatorSettingsBuilder());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression type found in fragment: expression");
        }

        [Fact]
        public async Task Returns_Invalid_When_Maximum_Recursion_Has_Been_Reached()
        {
            // Arrange
            Expression
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<object?>("result value"));
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(new ExpressionEvaluatorContext("expression", new ExpressionEvaluatorSettingsBuilder(), sut, null, int.MaxValue));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Maximum recursion level has been reached");
        }
    }
}
