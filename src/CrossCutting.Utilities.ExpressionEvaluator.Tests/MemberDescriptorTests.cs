namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class MemberDescriptorTests : TestBase
{
    public class Validate : MemberDescriptorTests
    {
        [Fact]
        public void Returns_ValidationError_When_MemberType_Is_Unknown()
        {
            // Arrange
            var sut = new MemberDescriptorBuilder().WithName("Need to fill this, it's required").WithMemberType(MemberType.Unknown);

            // Act
            var results = sut.Validate(new ValidationContext(sut)).ToArray();

            // Assert
            results.Length.ShouldBe(1);
            results[0].ErrorMessage.ShouldBe("MemberType cannot be Unknown");
        }

        [Fact]
        public void Returns_ValidationError_When_MemberType_Is_Method_And_InstanceType_Is_Null()
        {
            // Arrange
            var sut = new MemberDescriptorBuilder().WithName("Need to fill this, it's required").WithMemberType(MemberType.Method);

            // Act
            var results = sut.Validate(new ValidationContext(sut)).ToArray();

            // Assert
            results.Length.ShouldBe(1);
            results[0].ErrorMessage.ShouldBe("InstanceType is required when MemberType is Method or Property");
        }

        [Fact]
        public void Returns_ValidationError_When_MemberType_Is_Method_And_TypeArguments_Are_Filled()
        {
            // Arrange
            var sut = new MemberDescriptorBuilder().WithName("Need to fill this, it's required").WithMemberType(MemberType.Method).WithInstanceType(GetType()).AddTypeArguments(new MemberDescriptorTypeArgumentBuilder().WithName("T"));

            // Act
            var results = sut.Validate(new ValidationContext(sut)).ToArray();

            // Assert
            results.Length.ShouldBe(1);
            results[0].ErrorMessage.ShouldBe("TypeArguments are not allowed when MemberType is Method or Property");
        }

        [Fact]
        public void Returns_ValidationError_When_MemberType_Is_Function_And_InstanceType_Is_Not_Null()
        {
            // Arrange
            var sut = new MemberDescriptorBuilder().WithName("Need to fill this, it's required").WithMemberType(MemberType.Function).WithInstanceType(GetType());

            // Act
            var results = sut.Validate(new ValidationContext(sut)).ToArray();

            // Assert
            results.Length.ShouldBe(1);
            results[0].ErrorMessage.ShouldBe("InstanceType is not allowed when MemberType is not Method or Property");
        }

        [Fact]
        public void Returns_ValidationError_When_MemberType_Is_Property_And_Arguments_Are_Filled()
        {
            // Arrange
            var sut = new MemberDescriptorBuilder().WithName("Need to fill this, it's required").WithMemberType(MemberType.Property).WithInstanceType(GetType()).AddArguments(new MemberDescriptorArgumentBuilder().WithName("Argument").WithType(typeof(object)));

            // Act
            var results = sut.Validate(new ValidationContext(sut)).ToArray();

            // Assert
            results.Length.ShouldBe(1);
            results[0].ErrorMessage.ShouldBe("Arguments are not allowed when MemberType is Property");
        }
    }
}
