namespace CrossCutting.Common.Tests.Extensions;

public class ResultDictionaryContainerExtensionsTests
{
    protected static Result NonGenericDelegate() => Result.Success();
    protected static Result<string> GenericDelegate() => Result.Success("My value");
    protected static Result NonGenericErrorDelegate() => Result.Error("Kaboom");
    protected static Result<string> GenericErrorDelegate() => Result.Error<string>("Kaboom");

    protected static Task<Result> NonGenericTask => Task.FromResult(Result.Success());
    protected static Task<Result<string>> GenericTask => Task.FromResult(Result.Success("My value"));
    protected static Task<Result> NonGenericErrorTask => Task.FromResult(Result.Error("Kaboom"));
    protected static Task<Result<string>> GenericErrorTask => Task.FromResult(Result.Error<string>("Kaboom"));

    protected static IEnumerable<Result> NonGenericRangeDelegate() => [Result.Success(), Result.Success()];
    protected static IEnumerable<Result<string>> GenericRangeDelegate() => [Result.Success(string.Empty), Result.Success(string.Empty)];
    protected static IEnumerable<Result> NonGenericErrorRangeDelegate() => [Result.Success(), Result.Error("Kaboom"), Result.Success()];
    protected static IEnumerable<Result<string>> GenericErrorRangeDelegate() => [Result.Success(string.Empty), Result.Error<string>("Kaboom"), Result.Success(string.Empty)];

    protected static IResultDictionaryContainer<T> CreateSut<T>(IReadOnlyDictionary<string, Result<T>> resultDictionary)
    {
        var sut = Substitute.For<IResultDictionaryContainer<T>>();
        sut.Results.Returns(resultDictionary);
        return sut;
    }

    protected static IResultDictionaryContainer CreateSut(IReadOnlyDictionary<string, Result> resultDictionary)
    {
        var sut = Substitute.For<IResultDictionaryContainer>();
        sut.Results.Returns(resultDictionary);
        return sut;
    }

    public class GetErrorGeneric : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder<string>()
                .Add(GenericDelegate)
                .Add(GenericErrorDelegate)
                .Add(NonGenericDelegate)
                .Build());

            // Act
            var result = sut.GetError();

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class GetErrorNonGeneric : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.GetError();

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class OnSuccessGenericToNonGeneric : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder<string>()
                .Add(GenericDelegate)
                .Add(GenericErrorDelegate)
                .Add(NonGenericDelegate)
                .Build());

            // Act
            var result = sut.OnSuccess(results => Result.Success());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class OnSuccessNonGeneric : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.OnSuccess(results => Result.Success());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class OnSuccessNonGenericAsync : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public async Task Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(await new AsyncResultDictionaryBuilder()
                .Add("Step1", () => NonGenericTask)
                .Add("Step2", () => NonGenericErrorTask)
                .Add("Step3", () => NonGenericTask)
                .BuildAsync(CancellationToken.None));

            // Act
            var result = await sut.OnSuccessAsync(results => Task.FromResult(Result.Success()));

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class OnSuccessNonGenericToGeneric : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.OnSuccess(results => Result.Continue<string>());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class OnSuccessNonGenericToGenericAsync : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public async Task Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(await new AsyncResultDictionaryBuilder()
                .Add("Step1", () => NonGenericTask)
                .Add("Step2", () => NonGenericErrorTask)
                .Add("Step3", () => NonGenericTask)
                .BuildAsync(CancellationToken.None));

            // Act
            var result = await sut.OnSuccessAsync(results => Task.FromResult(Result.Continue<string>()));

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class OnSuccessGenericAction : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());
            var counter = 0;

            // Act
            var result = sut.OnSuccess(results => { counter++; });

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
            counter.ShouldBe(0);
        }
    }

    public class OnSuccessNonGenericAction : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());
            var counter = 0;

            // Act
            var result = sut.OnSuccess(results => { counter++; });

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
            counter.ShouldBe(0);
        }
    }

    public class OnFailureGeneric : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Performs_Action_When_Non_Successful_Result_Is_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());
            var counter = 0;

            // Act
            _ = sut.OnFailure(results => { counter++; });

            // Assert
            counter.ShouldBe(1);
        }

        [Fact]
        public void Returns_Delegate_Result_When_Non_Successful_Result_Is_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.OnFailure(results => Result.Error<string>("Kaboom"));

            // Assert
            result.GetError().ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class OnFailureNonGeneric : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Performs_Action_When_Non_Successful_Result_Is_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());
            var counter = 0;

            // Act
            _ = sut.OnFailure(results => { counter++; });

            // Assert
            counter.ShouldBe(1);
        }

        [Fact]
        public void Returns_Delegate_Result_When_Non_Successful_Result_Is_Present()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.OnFailure(results => Result.Error("Kaboom"));

            // Assert
            result.GetError().ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class GetValue : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Gets_Value_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.GetValue<string>("Step1");

            // Assert
            result.ShouldBe("My value");
        }
    }

    public class GetValue_Untyped : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Gets_Value_When_ResultKey_Is_Valid()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.GetValue("Step1");

            // Assert
            result.ShouldBe("My value");
        }
    }
    public class TryGetValueNoDefaultValue : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Gets_Value_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.TryGetValue<string>("Step1");

            // Assert
            result.ShouldBe("My value");
        }
    }

    public class ResultDictionaryContainerExtensionsTestsDefaultValue : ResultDictionaryContainerExtensionsTests
    {
        [Fact]
        public void Gets_Value_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = CreateSut(new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build());

            // Act
            var result = sut.TryGetValue("Step1", "some default value");

            // Assert
            result.ShouldBe("My value");
        }
    }
}
