namespace CrossCutting.Common.Tests.Extensions;

public class ResultDictionaryExtensionsTests
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
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
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
            result.ShouldBeNull();
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
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
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
            result.ShouldBeNull();
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
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_First_Non_Successful_Result_When_Present_Typed()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnSuccess(results => Result.Continue<string>());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
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
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Result_From_Delegate_When_All_Results_Are_Successful_Typed()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnSuccess(results => Result.Continue<string>());

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Continue);
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
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
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
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Continue);
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
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
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
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }

    public class OnSuccessNonGenericToGeneric_Task : ResultDictionaryExtensionsTests
    {
        [Fact]
        public async Task Returns_First_Non_Successful_Result_When_Present()
        {
            // Arrange
            var sut = await new AsyncResultDictionaryBuilder()
                .Add("Step1", GenericTask)
                .Add("Step2", GenericErrorTask)
                .Add("Step3", NonGenericTask)
                .Build();

            // Act
            var result = await sut.OnSuccess(results => Task.FromResult(Result.Continue<string>()));

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Result_From_Delegate_When_All_Results_Are_Successful()
        {
            // Arrange
            var sut = await new AsyncResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = await sut.OnSuccess(results => Task.FromResult(Result.Continue<string>()));

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Continue);
        }
    }
    public class OnSuccessGenericAction : ResultDictionaryExtensionsTests
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
            var counter = 0;

            // Act
            var result = sut.OnSuccess(results => { counter++; });

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
            counter.ShouldBe(0);
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
            var counter = 0;

            // Act
            var result = sut.OnSuccess(results => { counter++; });

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Ok);
            counter.ShouldBe(1);
        }
    }

    public class OnSuccessNonGenericAction : ResultDictionaryExtensionsTests
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
            var counter = 0;

            // Act
            var result = sut.OnSuccess(results => { counter++; });

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
            counter.ShouldBe(0);
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
            var counter = 0;

            // Act
            var result = sut.OnSuccess(results => { counter++; });

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Ok);
            counter.ShouldBe(1);
        }
    }

    public class OnFailureGeneric : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Performs_Action_When_Non_Successful_Result_Is_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();
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
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnFailure(results => Result.Error<string>("Kaboom"));

            // Assert
            result.GetError().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Does_Not_Perform_Action_When_Non_Successful_Result_Is_Not_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();
            var counter = 0;

            // Act
            _ = sut.OnFailure(results => { counter++; });

            // Assert
            counter.ShouldBe(0);
        }

        [Fact]
        public void Does_Not_Perform_Delegate_When_Non_Successful_Result_Is_Not_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();
            var counter = 0;

            // Act
            _ = sut.OnFailure(results => { counter++; return Result.Error<string>("Kaboom"); });

            // Assert
            counter.ShouldBe(0);
        }
    }

    public class OnFailureNonGeneric : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Performs_Action_When_Non_Successful_Result_Is_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();
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
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.OnFailure(results => Result.Error("Kaboom"));

            // Assert
            result.GetError().ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Does_Not_Perform_Action_When_Non_Successful_Result_Is_Not_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();
            var counter = 0;

            // Act
            _ = sut.OnFailure(results => { counter++; });

            // Assert
            counter.ShouldBe(0);
        }

        [Fact]
        public void Does_Not_Perform_Delegate_When_Non_Successful_Result_Is_Not_Present()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();
            var counter = 0;

            // Act
            _ = sut.OnFailure(results => { counter++; return Result.Error("Kaboom"); });

            // Assert
            counter.ShouldBe(0);
        }
    }

    public class GetValue : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Gets_Value_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.GetValue<string>("Step1");

            // Assert
            result.ShouldBe("My value");
        }

        [Fact]
        public void Gets_Value_When_Cast_Is_Possible_Typed()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.GetValue<string>("Step1");

            // Assert
            result.ShouldBe("My value");
        }

        [Fact]
        public void Throws_When_Cast_Is_Not_Possible_And_Value_Is_Null()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            Action a = () => sut.GetValue<string>("Step3");
            a.ShouldThrow<InvalidOperationException>()
             .Message.ShouldBe("Value is null");
        }

        [Fact]
        public void Throws_When_Cast_Is_Not_Possible_And_Value_Is_Not_Null()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            Action a = () => sut.GetValue<int>("Step1");
            a.ShouldThrow<InvalidCastException>()
             .Message.ShouldBe("Unable to cast object of type 'System.String' to type 'System.Int32'.");
        }

        [Fact]
        public void Throws_When_Result_Key_Does_Not_Exist()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            Action a = () => sut.GetValue<int>("typo");
            a.ShouldThrow<ArgumentOutOfRangeException>()
             .Message.ShouldBe("Unknown argument: typo (Parameter 'resultKey')");
        }
    }

    public class GetValue_Untyped : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Gets_Value_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.GetValue("Step1");

            // Assert
            result.ShouldBe("My value");
        }

        [Fact]
        public void Throws_When_Result_Key_Does_Not_Exist()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            Action a = () => sut.GetValue("typo");
            a.ShouldThrow<ArgumentOutOfRangeException>()
             .Message.ShouldBe("Unknown argument: typo (Parameter 'resultKey')");
        }
    }

    public class TryGetValueNoDefaultValue : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Gets_Value_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.TryGetValue<string>("Step1");

            // Assert
            result.ShouldBe("My value");
        }

        [Fact]
        public void Gets_Value_When_Cast_Is_Possible_Typed()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.TryGetValue<string>("Step1");

            // Assert
            result.ShouldBe("My value");
        }

        [Fact]
        public void Gets_Default_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.TryGetValue<int>("Step1");

            // Assert
            result.ShouldBe(0);
        }

        [Fact]
        public void Gets_Default_When_Key_Is_Not_Found()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.TryGetValue<int>("Step4");

            // Assert
            result.ShouldBe(0);
        }
    }

    public class ResultDictionaryExtensionsTestsDefaultValue : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Gets_Value_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.TryGetValue("Step1", "some default value");

            // Assert
            result.ShouldBe("My value");
        }

        [Fact]
        public void Gets_Value_When_Cast_Is_Possible_Typed()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.TryGetValue("Step1", "some default value");

            // Assert
            result.ShouldBe("My value");
        }

        [Fact]
        public void Gets_Default_When_Cast_Is_Possible()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.TryGetValue("Step1", 13);

            // Assert
            result.ShouldBe(13);
        }

        [Fact]
        public void Gets_Default_When_Key_Is_Not_Found()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", NonGenericDelegate)
                .Build();

            // Act
            var result = sut.TryGetValue("Step4", 13);

            // Assert
            result.ShouldBe(13);
        }
    }

    public class Aggregate_NonGeneric : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Returns_Last_Successful_Result_When_All_Is_Well()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", NonGenericDelegate)
                .Add("Step2", NonGenericDelegate)
                .Add("Step3", GenericDelegate)
                .Build();

            // Act
            var result = sut.Aggregate();

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Ok);
            result.GetValue().ShouldBe("My value");
        }

        [Fact]
        public void Returns_First_Non_Successful_Result_When_Error_Is_Found()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", NonGenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", GenericDelegate)
                .Build();

            // Act
            var result = sut.Aggregate();

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Aggregate_NonGeneric_Cast : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Returns_Last_Successful_Result_When_All_Is_Well()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", NonGenericDelegate)
                .Add("Step2", NonGenericDelegate)
                .Add("Step3", GenericDelegate)
                .Build();

            // Act
            var result = sut.Aggregate<string>();

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("My value");
        }

        [Fact]
        public void Returns_First_Non_Successful_Result_When_Error_Is_Found()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder()
                .Add("Step1", NonGenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", GenericDelegate)
                .Build();

            // Act
            var result = sut.Aggregate<string>();

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }

    public class Aggregate_Generic : ResultDictionaryExtensionsTests
    {
        [Fact]
        public void Returns_Last_Successful_Result_When_All_Is_Well()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", GenericDelegate)
                .Add("Step2", GenericDelegate)
                .Add("Step3", GenericDelegate)
                .Build();

            // Act
            var result = sut.Aggregate();

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("My value");
        }

        [Fact]
        public void Returns_First_Non_Successful_Result_When_Error_Is_Found()
        {
            // Arrange
            var sut = new ResultDictionaryBuilder<string>()
                .Add("Step1", NonGenericDelegate)
                .Add("Step2", GenericErrorDelegate)
                .Add("Step3", GenericDelegate)
                .Build();

            // Act
            var result = sut.Aggregate();

            // Assert
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
