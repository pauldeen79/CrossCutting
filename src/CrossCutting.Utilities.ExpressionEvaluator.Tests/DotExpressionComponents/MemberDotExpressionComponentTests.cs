namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.DotExpressionComponents;

public class MemberDotExpressionComponentTests : TestBase<MemberDotExpressionComponent>
{
    protected IFunctionParser FunctionParser => Mocks.GetOrCreate<IFunctionParser>(ClassFactory);

    public class EvaluateAsync : MemberDotExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Non_Successful_Result_From_MemberResolver()
        {
            // Arrange
            MemberResolver
                .ResolveAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Error<MemberAndTypeDescriptor>("Kaboom"));
            FunctionParser
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build()));
            var sut = CreateSut();
            var context = CreateContext("Dummy");
            var state = new DotExpressionComponentState(context, FunctionParser, Result.NoContent<object?>(), "Dummy");

            // Act
            var result = await sut.EvaluateAsync(state, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Result_From_MemberResolver_When_MemberResolver_Returns_Successful_Result_Without_Value()
        {
            // Arrange
            MemberResolver
                .ResolveAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.NoContent<MemberAndTypeDescriptor>());
            FunctionParser
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build()));
            var sut = CreateSut();
            var context = CreateContext("Dummy");
            var state = new DotExpressionComponentState(context, FunctionParser, Result.NoContent<object?>(), "Dummy");

            // Act
            var result = await sut.EvaluateAsync(state, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public async Task Returns_Continue_When_MemberResolver_Returns_NotFound()
        {
            // Arrange
            MemberResolver
                .ResolveAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.NotFound<MemberAndTypeDescriptor>());
            FunctionParser
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build()));
            var sut = CreateSut();
            var context = CreateContext("Dummy");
            var state = new DotExpressionComponentState(context, FunctionParser, Result.NoContent<object?>(), "Dummy");

            // Act
            var result = await sut.EvaluateAsync(state, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Return_Not_Supported_When_MemberResolver_Returns_Wrong_Type()
        {
            // Arrange
            MemberResolver
                .ResolveAsync(Arg.Any<FunctionCallContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success(new MemberAndTypeDescriptor(Substitute.For<IMember>(), null)));
            FunctionParser
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success(new FunctionCallBuilder().WithName("Dummy").WithMemberType(MemberType.Function).Build()));
            var sut = CreateSut();
            var context = CreateContext("Dummy");
            var state = new DotExpressionComponentState(context, FunctionParser, Result.NoContent<object?>(), "Dummy");

            // Act
            var result = await sut.EvaluateAsync(state, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            result.ErrorMessage.ShouldBe("Resolved member should be of type Method or Property");
        }
    }
}
