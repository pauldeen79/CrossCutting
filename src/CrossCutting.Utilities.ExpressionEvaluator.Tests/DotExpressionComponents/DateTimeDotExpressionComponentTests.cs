namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.DotExpressionComponents;

public class DateTimeDotExpressionComponentTests : TestBase
{
    protected DateTimeDotExpressionComponent CreateSut()
        => new DateTimeDotExpressionComponent(new MemberCallArgumentValidator());

    protected IFunctionParser FunctionParser { get; }

    public DateTimeDotExpressionComponentTests()
    {
        FunctionParser = Substitute.For<IFunctionParser>();
    }

    public class Evaluate : DateTimeDotExpressionComponentTests
    {
        public class AddDays : Evaluate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddDays").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddDays(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddDays(1));
            }
        }

        public class AddHours : Evaluate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddHours").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddHours(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddHours(1));
            }
        }

        public class AddMinutes : Evaluate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddMinutes").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddMinutes(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddMinutes(1));
            }
        }

        public class AddMonths: Evaluate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddMonths").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddMonths(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddMonths(1));
            }
        }

        public class AddSeconds : Evaluate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddSeconds").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddSeconds(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(1));
            }
        }

        public class AddYears : Evaluate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddYears").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddYears(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddYears(1));
            }
        }
    }

    public class Validate : DateTimeDotExpressionComponentTests
    {
        public class AddDays : Validate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddDays").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddDays(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Validate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddDays(1).GetType());
            }
        }

        public class AddHours : Validate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddHours").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddHours(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Validate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddHours(1).GetType());
            }
        }

        public class AddMinutes : Validate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddMinutes").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddMinutes(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Validate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddMinutes(1).GetType());
            }
        }

        public class AddMonths : Validate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddMonths").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddMonths(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Validate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddMonths(1).GetType());
            }
        }

        public class AddSeconds : Validate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddSeconds").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddSeconds(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Validate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddSeconds(1).GetType());
            }
        }

        public class AddYears : Validate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("AddYears").AddArguments("1")));
                var state = new DotExpressionComponentState(CreateContext("DateTime(2025, 1, 1).AddYears(1)"), FunctionParser, Result.Success<object?>(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)), "DateTime(2025, 1, 1)", typeof(DateTime));

                // Act
                var result = sut.Validate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified).AddYears(1).GetType());
            }
        }
    }
}
