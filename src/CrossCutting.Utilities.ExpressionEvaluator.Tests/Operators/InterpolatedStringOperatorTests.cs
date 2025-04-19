namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Operators;

public class InterpolatedStringOperatorTests : TestBase
{
    public class Evaluate : InterpolatedStringOperatorTests
    {
        [Fact]
        public void Returns_Invalid_On_Missing_Expression_In_Placeholder()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some {} value");

            // Act
            var result = sut.Evaluate(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_Start_Placeholder()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some } string");

            // Act
            var result = sut.Evaluate(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_End_Placeholder()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some { string");

            // Act
            var result = sut.Evaluate(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_First_Error_On_Non_Successful_Expressions()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some {error} {1} {error} value");

            // Act
            var result = sut.Evaluate(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Ok_On_Valid_And_Successful_Expression()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some {context}");

            // Act
            var result = sut.Evaluate(CreateContext("dummy", "value"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("some value");
        }

        [Fact]
        public void Can_Use_Recursive_Placeholder()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("{recursiveplaceholder}");

            // Act
            var result = sut.Evaluate(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("recursive value");
        }

        [Fact]
        public void Can_Use_Different_Placeholder_Signs()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("<#= context #>");
            // just for fun, let's use classic ASP expression syntax ;-)
            var context = CreateContext("dummy", context: "Hello world!", settings: new ExpressionEvaluatorSettingsBuilder().WithPlaceholderStart("<#=").WithPlaceholderEnd("#>"));

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ToStringWithDefault().ShouldBe("Hello world!");
        }
    }

    public class Parse : InterpolatedStringOperatorTests
    {
        [Fact]
        public void Returns_Invalid_On_Missing_Start_Placeholder()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some } string");

            // Act
            var result = sut.Parse(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_End_Placeholder()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some { string");

            // Act
            var result = sut.Parse(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Invalid_On_Missing_Expression_In_Placeholder()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some {} value");

            // Act
            var result = sut.Parse(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_All_Errors_On_Non_Successful_Expressions()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some {error} {1} {error} value");

            // Act
            var result = sut.Parse(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Validation failed, see part results for more details");
            result.PartResults.Count.ShouldBe(3);
        }

        [Fact]
        public void Returns_Ok_On_Valid_And_Successful_Expression()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("some {context}");

            // Act
            var result = sut.Parse(CreateContext("dummy", "value"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Can_Use_Recursive_Placeholder()
        {
            // Arrange
            var sut = new InterpolatedStringOperator("{recursiveplaceholder}");

            // Act
            var result = sut.Parse(CreateContext("dummy"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
