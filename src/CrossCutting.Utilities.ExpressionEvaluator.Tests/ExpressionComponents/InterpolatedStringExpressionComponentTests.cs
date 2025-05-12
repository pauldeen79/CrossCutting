namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class InterpolatedStringExpressionComponentTests : TestBase<InterpolatedStringExpressionComponent>
{
    public class Evaluate : InterpolatedStringExpressionComponentTests
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
            var result = await sut.EvaluateAsync(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_End_Character()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some non terminated string"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_Start_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some } string\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_End_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some { string\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_Expression_In_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some {} value\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_First_Error_On_Non_Successful_Expressions()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(CreateContext("$\"some {error} {1} {error} value\""));

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
            var result = await sut.EvaluateAsync(CreateContext("$\"some {state}\"", "value"));

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
            var result = await sut.EvaluateAsync(CreateContext("$\"{recursiveplaceholder}\""));

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
            var context = CreateContext("$\"<#= state #>\"", state: "Hello world!", settings: new ExpressionEvaluatorSettingsBuilder().WithPlaceholderStart("<#=").WithPlaceholderEnd("#>"));

            // Act
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("Hello world!");
        }
    }

    public class Parse : InterpolatedStringExpressionComponentTests
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
            var result = await sut.ParseAsync(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_End_Character()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some non terminated string"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_Start_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some } string\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_End_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some { string\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_Invalid_On_Missing_Expression_In_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some {} value\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_All_Errors_On_Non_Successful_Expressions()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some {error} {1} {error} value\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Validation failed, see part results for more details");
            result.PartResults.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Returns_Ok_On_Valid_And_Successful_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"some {context}\"", "value"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Can_Use_Recursive_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(CreateContext("$\"{recursiveplaceholder}\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
