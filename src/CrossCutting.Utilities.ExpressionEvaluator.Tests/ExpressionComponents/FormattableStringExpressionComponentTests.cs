namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class FormattableStringExpressionComponentTests : TestBase<FormattableStringExpressionComponent>
{
    public class Evaluate : FormattableStringExpressionComponentTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("\"normal string \"")]
        [InlineData("$\"")]
        public void Returns_Continue_On_Non_FormattableString_Expression(string expression)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_End_Character()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("$\"some non terminated string"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_Start_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("$\"some } string\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_End_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("$\"some { string\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_Expression_In_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("$\"some {} value\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_First_Error_On_Non_Successful_Expressions()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("$\"some {error} {1} {error} value\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Ok_On_Valid_And_Successful_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("$\"some {context}\"", "value"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("some value");
        }

        [Fact]
        public void Can_Use_Recursive_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(CreateContext("$\"{recursiveplaceholder}\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("recursive value");
        }

        [Fact]
        public void Can_Use_Different_Placeholder_Signs()
        {
            // Arrange
            var sut = CreateSut();
            // just for fun, let's use classic ASP expression syntax ;-)
            var context = CreateContext("$\"<#= context #>\"", context: "Hello world!", settings: new ExpressionEvaluatorSettingsBuilder().WithPlaceholderStart("<#=").WithPlaceholderEnd("#>"));

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("Hello world!");
        }
    }

    public class Parse : FormattableStringExpressionComponentTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("\"normal string \"")]
        [InlineData("$\"")]
        public void Returns_Continue_On_Non_FormattableString_Expression(string expression)
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext(expression));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_End_Character()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("$\"some non terminated string"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_Start_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("$\"some } string\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_End_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("$\"some { string\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_Expression_In_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("$\"some {} value\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_All_Errors_On_Non_Successful_Expressions()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("$\"some {error} {1} {error} value\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Validation failed, see part results for more details");
            result.PartResults.Count.ShouldBe(3);
        }

        [Fact]
        public void Returns_Ok_On_Valid_And_Successful_Expression()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("$\"some {context}\"", "value"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Can_Use_Recursive_Placeholder()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Parse(CreateContext("$\"{recursiveplaceholder}\""));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
