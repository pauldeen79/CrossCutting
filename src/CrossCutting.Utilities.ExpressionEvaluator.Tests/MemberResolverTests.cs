namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class MemberResolverTests : TestBase<MemberResolver>
{
    public class ResolveAsync : MemberResolverTests
    {
        [Fact]
        public async Task Returns_Non_Successful_Result_From_MemberDescriptorProvider()
        {
            // Arrange
            var sut = CreateSut();
            var functionCallContext = new FunctionCallContext(new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function), CreateContext("MyFunction()"));
            MemberDescriptorProvider
                .GetAll()
                .Returns(Result.Error<IReadOnlyCollection<MemberDescriptor>>("Kaboom"));

            // Act
            var result = await sut.ResolveAsync(functionCallContext);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Error_When_MemberDescriptorProvider_Throws_Exception()
        {
            // Arrange
            var sut = CreateSut();
            var functionCallContext = new FunctionCallContext(new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function), CreateContext("MyFunction()"));
            MemberDescriptorProvider
                .GetAll()
                .Throws<InvalidOperationException>();

            // Act
            var result = await sut.ResolveAsync(functionCallContext);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldBeOfType<InvalidOperationException>();
        }
    }
}
