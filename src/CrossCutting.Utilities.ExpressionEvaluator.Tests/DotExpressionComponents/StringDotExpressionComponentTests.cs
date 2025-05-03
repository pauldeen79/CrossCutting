namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.DotExpressionComponents;

public class StringDotExpressionComponentTests : TestBase
{
    protected StringDotExpressionComponent CreateSut()
        => new StringDotExpressionComponent(new MemberCallArgumentValidator(), new MemberDescriptorMapper());

    protected IFunctionParser FunctionParser { get; }

    public StringDotExpressionComponentTests()
    {
        FunctionParser = Substitute.For<IFunctionParser>();
    }

    public class Evaluate : StringDotExpressionComponentTests
    {
        public class Substring : Evaluate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid_One_Argument()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("Substring").AddArguments("6")));
                var state = new DotExpressionComponentState(CreateContext("\"hello world\".Substring(6)"), FunctionParser, Result.Success<object?>("hello world"), "hello world", typeof(string));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe("hello world".Substring(6));
            }

            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid_Two_Arguments()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("Substring").AddArguments("0", "5")));
                var state = new DotExpressionComponentState(CreateContext("\"hello world\".Substring(0, 5)"), FunctionParser, Result.Success<object?>("hello world"), "hello world", typeof(string));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe("hello world".Substring(0, 5));
            }

            [Fact]
            public void Returns_Invalid_On_Missing_Arguments()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("Substring")));
                var state = new DotExpressionComponentState(CreateContext("\"hello world\".Substring()"), FunctionParser, Result.Success<object?>("hello world"), "hello world", typeof(string));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Invalid);
                result.ErrorMessage.ShouldBe("Missing argument: Index");
            }

            [Fact]
            public void Returns_Invalid_On_Index_Out_Of_Range()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("Substring").AddArguments("100")));
                var state = new DotExpressionComponentState(CreateContext("\"hello world\".Substring(100)"), FunctionParser, Result.Success<object?>("hello world"), "hello world", typeof(string));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Invalid);
                result.ErrorMessage.ShouldBe("Index must refer to a location within the string");
            }

            [Fact]
            public void Returns_Invalid_On_Length_Out_Of_Range()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("Substring").AddArguments("0", "100")));
                var state = new DotExpressionComponentState(CreateContext("\"hello world\".Substring(0, 100)"), FunctionParser, Result.Success<object?>("hello world"), "hello world", typeof(string));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Invalid);
                result.ErrorMessage.ShouldBe("Index and length must refer to a location within the string");
            }
        }
    }

    public class Validate : StringDotExpressionComponentTests
    {
        public class Substring : Validate
        {
            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid_One_Argument()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("Substring").AddArguments("6")));
                var state = new DotExpressionComponentState(CreateContext("\"hello world\".Substring(6)"), FunctionParser, Result.Success<object?>("hello world"), "hello world", typeof(string));

                // Act
                var result = sut.Validate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(typeof(string));
            }

            [Fact]
            public void Returns_Success_When_All_Arguments_Are_Valid_Two_Arguments()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("Substring").AddArguments("0", "5")));
                var state = new DotExpressionComponentState(CreateContext("\"hello world\".Substring(0, 5)"), FunctionParser, Result.Success<object?>("hello world"), "hello world", typeof(string));

                // Act
                var result = sut.Validate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Ok);
                result.Value.ShouldBe(typeof(string));
            }

            [Fact]
            public void Returns_Invalid_On_Missing_Arguments()
            {
                // Arrange
                var sut = CreateSut();
                FunctionParser
                    .Parse(Arg.Any<ExpressionEvaluatorContext>())
                    .Returns(Result.Success<FunctionCall>(new FunctionCallBuilder().WithName("Substring")));
                var state = new DotExpressionComponentState(CreateContext("\"hello world\".Substring()"), FunctionParser, Result.Success<object?>("hello world"), "hello world", typeof(string));

                // Act
                var result = sut.Evaluate(state);

                // Assert
                result.Status.ShouldBe(ResultStatus.Invalid);
                result.ErrorMessage.ShouldBe("Missing argument: Index");
            }
        }
    }
}
