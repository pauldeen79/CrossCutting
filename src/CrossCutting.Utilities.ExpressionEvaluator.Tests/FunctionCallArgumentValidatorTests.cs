namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class FunctionCallArgumentValidatorTests : TestBase<MemberCallArgumentValidator>
{
    public class Validate : FunctionCallArgumentValidatorTests
    {
        [Fact]
        public async Task Returns_Non_Successful_Result_From_CallArgument_Parse()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking();
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(string));
            var sut = CreateSut();

            // Act
            var result = await sut.ValidateAsync(descriptorArgument, "error", context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Invalid_When_Type_Is_Not_Assignable()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking();
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(string));
            var sut = CreateSut();

            // Act
            var result = await sut.ValidateAsync(descriptorArgument, "1", context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Argument Argument is not of type System.String");
        }

        [Fact]
        public async Task Returns_Ok_When_ValidateArgumentTypes_Is_False()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes(false);
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(string));
            var sut = CreateSut();

            // Act
            var result = await sut.ValidateAsync(descriptorArgument, "1", context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Ok_When_DescriptorArgument_Type_Is_Object()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking();
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(object));
            var sut = CreateSut();

            // Act
            var result = await sut.ValidateAsync(descriptorArgument, "1", context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Ok_When_CallArgument_ResultType_Is_Null()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking();
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(object));
            var sut = CreateSut();

            // Act
            var result = await sut.ValidateAsync(descriptorArgument, "unknown", context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Ok_When_CallArgument_ResultType_Is_Object_And_StrictTypeChecking_Is_False()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking(false);
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(string));
            var sut = CreateSut();

            // Act
            var result = await sut.ValidateAsync(descriptorArgument, "object", context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
