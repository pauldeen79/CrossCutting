namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class FunctionCallArgumentValidatorTests : TestBase<MemberCallArgumentValidator>
{
    public class Validate : FunctionCallArgumentValidatorTests
    {
        [Fact]
        public void Returns_Non_Successful_Result_From_CallArgument_Parse()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking();
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(string));
            var sut = CreateSut();

            // Act
            var result = sut.Validate(descriptorArgument, "error", context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Invalid_When_Type_Is_Not_Assignable()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking();
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(string));
            var sut = CreateSut();

            // Act
            var result = sut.Validate(descriptorArgument, "1", context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Argument Argument is not of type System.String");
        }

        [Fact]
        public void Returns_Ok_When_ValidateArgumentTypes_Is_False()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes(false);
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(string));
            var sut = CreateSut();

            // Act
            var result = sut.Validate(descriptorArgument, "1", context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Ok_When_DescriptorArgument_Type_Is_Object()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking();
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(object));
            var sut = CreateSut();

            // Act
            var result = sut.Validate(descriptorArgument, "1", context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Ok_When_CallArgument_ResultType_Is_Null()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking();
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(object));
            var sut = CreateSut();

            // Act
            var result = sut.Validate(descriptorArgument, "unknown", context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Ok_When_CallArgument_ResultType_Is_Object_And_StrictTypeChecking_Is_False()
        {
            // Arrange
            var functionCall = new FunctionCallBuilder().WithName("MyFunction").WithMemberType(MemberType.Function);
            var settings = new ExpressionEvaluatorSettingsBuilder().WithValidateArgumentTypes().WithStrictTypeChecking(false);
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy", settings: settings));
            var descriptorArgument = new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(string));
            var sut = CreateSut();

            // Act
            var result = sut.Validate(descriptorArgument, "object", context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
