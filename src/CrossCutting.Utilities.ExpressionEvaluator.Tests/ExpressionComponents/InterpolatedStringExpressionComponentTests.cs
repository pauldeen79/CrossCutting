namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class InterpolatedStringExpressionComponentTests : TestBase<InterpolatedStringExpressionComponent>
{
    public class EvaluateAsync : InterpolatedStringExpressionComponentTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("\"normal string \"")]
        [InlineData("$\"")]
        public async Task Returns_Continue_On_Non_FormattableString_Expression(string expression)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext(expression), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_End_Character()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some non terminated string"), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_Start_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some } string\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_End_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some { string\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_Expression_In_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some {} value\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_First_Error_On_Non_Successful_Expressions()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some {error} {1} {error} value\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Ok_On_Valid_And_Successful_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some {Context}\"", "value"), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("some value");
        }

        [Fact]
        public async Task Can_Use_Recursive_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"{recursiveplaceholder}\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("recursive value");
        }

        [Fact]
        public async Task Can_Use_Different_Placeholder_Signs()
        {
            // Arrange
            var sut = CreateSut();
            // just for fun, let's use classic ASP expression syntax ;-)
            var context = CreateContext("$\"<#= Context #>\"", state: "Hello world!", settings: new ExpressionEvaluatorSettingsBuilder().WithPlaceholderStart("<#=").WithPlaceholderEnd("#>"));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("Hello world!");
        }
    }

    public class ParseAsync : InterpolatedStringExpressionComponentTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("\"normal string \"")]
        [InlineData("$\"")]
        public async Task Returns_Continue_On_Non_FormattableString_Expression(string expression)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext(expression), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_End_Character()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some non terminated string"), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_Start_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some } string\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_End_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some { string\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_Expression_In_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some {} value\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Breaks_On_First_Error_On_Non_Successful_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some {error} {1} {error} value\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Validation failed, see part results for more details");
            result.PartResults.Count.ShouldBe(1);
        }

        [Fact]
        public async Task Returns_Ok_On_Valid_And_Successful_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some {context}\"", "value"), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Can_Use_Recursive_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"{recursiveplaceholder}\""), CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
