namespace CrossCutting.Common.Tests.Extensions;

public class ResultDictionaryExtensionsTests
{
    protected static Result NonGenericDelegate() => Result.Success();
    protected static Result<string> GenericDelegate() => Result.Success(string.Empty);
    protected static Result NonGenericErrorDelegate() => Result.Error("Kaboom");
    protected static Result<string> GenericErrorDelegate() => Result.Error<string>("Kaboom");

    protected static IEnumerable<Result> NonGenericRangeDelegate() => [Result.Success(), Result.Success()];
    protected static IEnumerable<Result<string>> GenericRangeDelegate() => [Result.Success(string.Empty), Result.Success(string.Empty)];
    protected static IEnumerable<Result> NonGenericErrorRangeDelegate() => [Result.Success(), Result.Error("Kaboom"), Result.Success()];
    protected static IEnumerable<Result<string>> GenericErrorRangeDelegate() => [Result.Success(string.Empty), Result.Error<string>("Kaboom"), Result.Success(string.Empty)];

    public class GetErrorGeneric : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.GetError();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Null_When_Non_Successful_Result_Is_Not_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.GetError();

            // Assert
            result.Should().BeNull();
        }
    }

    public class GetErrorNonGeneric : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.GetError();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Null_When_Non_Successful_Result_Is_Not_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.GetError();

            // Assert
            result.Should().BeNull();
        }
    }

    public class OnSuccessGenericToNonGeneric : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnSuccess(results => Result.Success());

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Result_From_Delegate_When_All_Results_Are_Successful()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnSuccess(results => Result.Continue());

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ResultStatus.Continue);
        }
    }

    public class OnSuccessNonGeneric : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnSuccess(results => Result.Success());

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Result_From_Delegate_When_All_Results_Are_Successful()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnSuccess(results => Result.Continue());

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ResultStatus.Continue);
        }
    }

    public class OnSuccessNonGenericToGeneric : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnSuccess(results => Result.Continue<string>());

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Result_From_Delegate_When_All_Results_Are_Successful()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnSuccess(results => Result.Continue<string>());

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ResultStatus.Continue);
        }
    }

    public class EitherGenericToNonGeneric : ResultDictionaryExtensionsTests
    {
    }

    public class EitherNonGeneric : ResultDictionaryExtensionsTests
    {
    }

    public class EitherGenericToGeneric : ResultDictionaryExtensionsTests
    {
    }

    public class OnFailureGeneric : ResultDictionaryExtensionsTests
    {
    }

    public class OnFailureNonGeneric : ResultDictionaryExtensionsTests
    {
    }

    public class GetValue : ResultDictionaryExtensionsTests
    {
    }

    public class TryGetValueNoDefaultValue : ResultDictionaryExtensionsTests
    {
    }

    public class ResultDictionaryExtensionsTestsDefaultValue : ResultDictionaryExtensionsTests
    {
    }
}
