namespace CrossCutting.Common.Tests.Extensions;

public class ResultExtensionsTests
{
    public class EnsureValue : ResultExtensionsTests
    {
        [Fact]
        public void Returns_Non_Successful_Result_Untyped()
        {
            // Arrange
            var sut = Result.Error("Kaboom");

            // Act
            var result = sut.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
        }

        [Fact]
        public void Returns_Successful_Result_With_Value_Untyped()
        {
            // Arrange
            var sut = new MyResult("Value");

            // Act
            var result = sut.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Error_On_Successful_Result_Without_Value_Untyped()
        {
            // Arrange
            var sut = new MyResult(null);

            // Act
            var result = sut.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Result value is required");
        }

        [Fact]
        public void Returns_Successful_Result_With_No_Value_Untyped_On_Status_Continue()
        {
            // Arrange
            var sut = Result.Continue();

            // Act
            var result = sut.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Non_Successful_Result_Typed()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");

            // Act
            var result = sut.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
        }

        [Fact]
        public void Returns_Successful_Result_With_Value_Typed()
        {
            // Arrange
            var sut = Result.Success("Value");

            // Act
            var result = sut.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_Successful_Result_With_No_Value_Typed_On_Status_Continue()
        {
            // Arrange
            var sut = Result.Continue<string>();

            // Act
            var result = sut.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Error_On_Successful_Result_Without_Value_Typed()
        {
            // Arrange
            var sut = Result.NoContent<string>();

            // Act
            var result = sut.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Result value is required");
        }

        [Fact]
        public void Returns_Error_On_Null_Result_Untyped()
        {
            // Arrange
            var sut = default(Result);

            // Act
            var result = sut!.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Result is null");
        }

        [Fact]
        public void Returns_Error_On_Null_Result_Typed()
        {
            // Arrange
            var sut = default(Result<string>);

            // Act
            var result = sut!.EnsureValue();

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Result is null");
        }
    }

    public class IgnoreNotFound : ResultExtensionsTests
    {
        [Fact]
        public void Untyped_Returns_Continue_When_Status_Is_NotFound()
        {
            // Arrange
            var sut = Result.NotFound();

            // Act
            var result = sut.IgnoreNotFound();

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Untyped_Returns_Ok_When_Status_Is_Ok()
        {
            // Arrange
            var sut = Result.Success();

            // Act
            var result = sut.IgnoreNotFound();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Typed_Returns_Continue_When_Status_Is_NotFound()
        {
            // Arrange
            var sut = Result.NotFound<int>();

            // Act
            var result = sut.IgnoreNotFound();

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Typed_Returns_Ok_When_Status_Is_Ok()
        {
            // Arrange
            var sut = Result.Success(1);

            // Act
            var result = sut.IgnoreNotFound();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }

    public class Either : ResultExtensionsTests
    {
        [Fact]
        public void Void_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;
            var success = false;

            // Act
            sut.Either(_ => error = true, _ => success = true);

            // Assert
            success.ShouldBeTrue();
            error.ShouldBeFalse();
        }

        [Fact]
        public async Task Void_Async_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;
            var success = false;

            // Act
            await sut.Either(_ => error = true, x => Task.FromResult(x.Chain(() => success = true)));

            // Assert
            success.ShouldBeTrue();
            error.ShouldBeFalse();
        }

        [Fact]
        public void Void_Runs_Failure_Action_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;
            var success = false;

            // Act
            sut.Either(_ => error = true, _ => success = true);

            // Assert
            success.ShouldBeFalse();
            error.ShouldBeTrue();
        }

        [Fact]
        public async Task Void_Async_Runs_Failure_Action_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;
            var success = false;

            // Act
            await sut.Either(_ => error = true, x => Task.FromResult(x.Chain(() => success = true)));

            // Assert
            success.ShouldBeFalse();
            error.ShouldBeTrue();
        }

        [Fact]
        public void Void_Parameterless_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;
            var success = false;

            // Act
            sut.Either(_ => error = true, () => success = true);

            // Assert
            success.ShouldBeTrue();
            error.ShouldBeFalse();
        }

        [Fact]
        public async Task Void_Parameterless_Async_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;
            var success = false;

            // Act
            await sut.Either(_ => error = true, () => Task.FromResult(Result.Success().Chain(() => success = true)));

            // Assert
            success.ShouldBeTrue();
            error.ShouldBeFalse();
        }

        [Fact]
        public void Void_Parameterless_Runs_Failure_Action_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;
            var success = false;

            // Act
            sut.Either(_ => error = true, () => success = true);

            // Assert
            success.ShouldBeFalse();
            error.ShouldBeTrue();
        }

        [Fact]
        public async Task Void_Parameterless_Async_Runs_Failure_Action_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;
            var success = false;

            // Act
            await sut.Either(_ => error = true, () => Task.FromResult(Result.Success().Chain(() => success = true)));

            // Assert
            success.ShouldBeFalse();
            error.ShouldBeTrue();
        }

        [Fact]
        public async Task Void_Func_Task_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;
            var success = false;

            // Act
            var result = await sut.Either(x => x.Chain(() => error = true), _ => Task.FromResult(Result.Success().Chain(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            success.ShouldBeTrue();
            error.ShouldBeFalse();
        }

        [Fact]
        public async Task Void_Func_Async_Runs_Failure_Action_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;
            var success = false;

            // Act
            var result = await sut.Either(x => x.Chain(() => error = true), _ => Task.FromResult(Result.Success().Chain(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
            error.ShouldBeTrue();
        }

        [Fact]
        public void Result_Returns_Success_Delegate_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success();

            // Act
            var result = sut.Either(_ => Result.Error("Custom"), _ => Result.Continue());

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Result_Returns_Failure_Delegate_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");

            // Act
            var result = sut.Either(_ => Result.Error("Custom"), _ => Result.Continue());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Custom");
        }

        [Fact]
        public void Result_Returns_Failure_Delegate_On_Non_Successful_Result_No_Alternate_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");

            // Act
            var result = sut.Either(_ => Result.Error<string>("Custom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Custom");
        }

        [Fact]
        public async Task Result_Async_Parameterless_Returns_Success_Delegate_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success("OK");

            // Act
            var result = await sut.Either(_ => Result.Error<string>("Custom"), () => Task.FromResult(Result.Continue<string>()));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Result_Returns_Same_Instance_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success("OK");

            // Act
            var result = sut.Either(_ => Result.Error<string>("Custom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("OK");
        }

        [Fact]
        public void Result_Parameterless_Returns_Success_Delegate_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success("Succes value");

            // Act
            var result = sut.Either(_ => Result.Error<string>("Custom"), Result.Continue<string>);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public void Result_Parameterless_Returns_Failure_Delegate_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");

            // Act
            var result = sut.Either(_ => Result.Error<string>("Custom"), Result.Continue<string>);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Custom");
        }

        [Fact]
        public async Task Result_Async_Parameterless_Returns_Failure_Delegate_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");

            // Act
            var result = await sut.Either(_ => Result.Error<string>("Custom"), () => Task.FromResult(Result.Continue<string>()));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Custom");
        }
    }

    public class OnFailure : ResultExtensionsTests
    {
        [Fact]
        public void Does_Nothing_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;

            // Act
            var result = sut.OnFailure(_ => error = true);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            error.ShouldBeFalse();
        }

        [Fact]
        public void Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;

            // Act
            var result = sut.OnFailure(_ => error = true);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            error.ShouldBeTrue();
        }

        [Fact]
        public void Parameterless_Does_Nothing_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;

            // Act
            var result = sut.OnFailure(() => error = true);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            error.ShouldBeFalse();
        }

        [Fact]
        public void Parameterless_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;

            // Act
            var result = sut.OnFailure(() => error = true);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            error.ShouldBeTrue();
        }

        [Fact]
        public void Func_No_Arguments_Does_Nothing_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;

            // Act
            var result = sut.OnFailure(() => Result.Continue().Then(() => error = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            error.ShouldBeFalse();
        }

        [Fact]
        public void Func_No_Arguments_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;

            // Act
            var result = sut.OnFailure(() => Result.Invalid().Then(() => error = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            error.ShouldBeTrue();
        }

        [Fact]
        public void Func_With_Arguments_Does_Nothing_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;

            // Act
            var result = ResultExtensions.OnFailure(sut, _ => Result.Continue().Then(() => error = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            error.ShouldBeFalse();
        }

        [Fact]
        public void Func_With_Arguments_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;

            // Act
            var result = ResultExtensions.OnFailure(sut, _ => Result.Invalid().Then(() => error = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            error.ShouldBeTrue();
        }

        [Fact]
        public async Task Func_No_Arguments_Async_Does_Nothing_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;

            // Act
            var result = await sut.OnFailure(() => Task.FromResult(Result.Continue().Then(() => error = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            error.ShouldBeFalse();
        }

        [Fact]
        public async Task Func_No_Arguments_Async_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;

            // Act
            var result = await sut.OnFailure(() => Task.FromResult(Result.Invalid().Then(() => error = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            error.ShouldBeTrue();
        }

        [Fact]
        public async Task Func_With_Arguments_Async_Does_Nothing_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var error = false;

            // Act
            var result = await sut.OnFailure(_ => Task.FromResult(Result.Continue().Then(() => error = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            error.ShouldBeFalse();
        }

        [Fact]
        public async Task Func_With_Arguments_Async_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var error = false;

            // Act
            var result = await sut.OnFailure(_ => Task.FromResult(Result.Invalid().Then(() => error = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            error.ShouldBeTrue();
        }
    }

    public class OnSuccess : ResultExtensionsTests
    {
        [Fact]
        public void Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => success = true);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            success.ShouldBeTrue();
        }

        [Fact]
        public void Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => success = true);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public void Parameterless_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var success = false;

            // Act
            var result = sut.OnSuccess(() => success = true);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            success.ShouldBeTrue();
        }

        [Fact]
        public void Parameterless_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var success = false;

            // Act
            var result = sut.OnSuccess(() => success = true);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public void Func_No_Arguments_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var success = false;

            // Act
            var result = sut.OnSuccess(() => Result.Continue().Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            success.ShouldBeTrue();
        }

        [Fact]
        public void Func_No_Arguments_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var success = false;

            // Act
            var result = sut.OnSuccess(() => Result.Continue().Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public void Func_With_Arguments_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Success();
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => Result.Continue().Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            success.ShouldBeTrue();
        }

        [Fact]
        public void Func_With_Arguments_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => Result.Continue().Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public void Func_With_Arguments_To_Different_Type_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Continue();
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => Result.Continue<int>().Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            success.ShouldBeTrue();
        }

        [Fact]
        public void Func_With_Arguments_To_Different_Type_Does_Nothing_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => Result.Continue<int>().Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public void Func_With_Arguments_Different_Type_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success(string.Empty);
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => Result.Success(0).Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            success.ShouldBeTrue();
        }

        [Fact]
        public void Func_With_Arguments_Different_Type_Does_Nothing_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => Result.Continue<int>().Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public void Func_With_Arguments_Different_Type_Value_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success(string.Empty);
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => 0.Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            success.ShouldBeTrue();
        }

        [Fact]
        public void Func_With_Arguments_Different_Type_Value_Does_Nothing_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");
            var success = false;

            // Act
            var result = sut.OnSuccess(_ => 0.Then(() => success = true));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }
    }

    public class OnSuccessAsync : ResultExtensionsTests
    {
        [Fact]
        public async Task Func_No_Arguments_Runs_Success_Action_On_Successful_Resul()
        {
            // Arrange
            var sut = Result.Success();
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(() => Task.FromResult(Result.Continue().Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            success.ShouldBeTrue();
        }

        [Fact]
        public async Task Func_No_Arguments_Does_Nothing_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(() => Task.FromResult(Result.Continue().Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public async Task Func_With_Arguments_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success();
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(_ => Task.FromResult(Result.Continue().Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            success.ShouldBeTrue();
        }

        [Fact]
        public async Task Func_With_Arguments_Does_Nothing_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(_ => Task.FromResult(Result.Continue().Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public async Task Func_With_Arguments_To_Different_Type_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Continue();
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(_ => Task.FromResult(Result.Continue<int>().Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
            success.ShouldBeTrue();
        }

        [Fact]
        public async Task Func_With_Arguments_To_Different_Type_Does_Nothing_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error("Kaboom");
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(_ => Task.FromResult(Result.Continue<int>().Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public async Task Func_With_Arguments_Different_Type_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success(string.Empty);
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(_ => Task.FromResult(Result.Success(0).Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            success.ShouldBeTrue();
        }

        [Fact]
        public async Task Func_With_Arguments_Different_Type_Does_Nothing_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(_ => Task.FromResult(Result.Continue<int>().Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }

        [Fact]
        public async Task Func_With_Arguments_Different_Type_Value_Runs_Success_Action_On_Successful_Result()
        {
            // Arrange
            var sut = Result.Success(string.Empty);
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(_ => Task.FromResult(0.Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            success.ShouldBeTrue();
        }

        [Fact]
        public async Task Func_With_Arguments_Different_Type_Value_Does_Nothing_On_Non_Successful_Result()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");
            var success = false;

            // Act
            var result = await sut.OnSuccessAsync(_ => Task.FromResult(0.Then(() => success = true)));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            success.ShouldBeFalse();
        }
    }

    public class Wrap_Untyped : ResultExtensionsTests
    {
        [Fact]
        public void Returns_Same_Instance_When_Successful()
        {
            // Arrange
            var sut = Result.Success();

            // Act
            var result = sut.Wrap("Something went wrong");

            // Assert
            result.ShouldBeSameAs(sut);
        }

        [Fact]
        public void Returns_Wrapped_Instane_WheN_Not_Successful()
        {
            // Arrange
            var sut = Result.Error("Kaboom");

            // Act
            var result = sut.Wrap("Something went wrong");

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBeSameAs("Something went wrong");
            result.InnerResults.Count.ShouldBe(1);
            result.InnerResults.First().ErrorMessage.ShouldBe("Kaboom");
            result.InnerResults.First().Status.ShouldBe(ResultStatus.Error);
        }
    }

    public class Wrap_Typed : ResultExtensionsTests
    {
        [Fact]
        public void Returns_Same_Instance_When_Successful()
        {
            // Arrange
            var sut = Result.Continue<string>();

            // Act
            var result = sut.Wrap("Something went wrong");

            // Assert
            result.ShouldBeSameAs(sut);
        }

        [Fact]
        public void Returns_Wrapped_Instane_WheN_Not_Successful()
        {
            // Arrange
            var sut = Result.Error<string>("Kaboom");

            // Act
            var result = sut.Wrap("Something went wrong");

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBeSameAs("Something went wrong");
            result.InnerResults.Count.ShouldBe(1);
            result.InnerResults.First().ErrorMessage.ShouldBe("Kaboom");
            result.InnerResults.First().Status.ShouldBe(ResultStatus.Error);
        }
    }

    public class EnsureNotNull_Untyped : ResultExtensionsTests
    {
        [Fact]
        public void Returns_New_Result_When_Result_Is_Null_Default_ErrorMessage()
        {
            // Arrange
            var sut = default(Result)!;

            // Act
            var result = sut.EnsureNotNull();

            // Arrange
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Result is null");
        }

        [Fact]
        public void Returns_New_Result_When_Result_Is_Null_Custom_ErrorMessage()
        {
            // Arrange
            var sut = default(Result)!;

            // Act
            var result = sut.EnsureNotNull("custom");

            // Arrange
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("custom");
        }

        [Fact]
        public void Returns_Same_Result_When_Result_Is_Not_Null()
        {
            // Arrange
            var sut = Result.Continue();

            // Act
            var result = sut.EnsureNotNull();

            // Arrange
            result.ShouldBeSameAs(sut);
        }
    }

    public class EnsureNotNull_Typed : ResultExtensionsTests
    {
        [Fact]
        public void Returns_New_Result_When_Result_Is_Null_Default_ErrorMessage()
        {
            // Arrange
            var sut = default(Result<string>)!;

            // Act
            var result = sut.EnsureNotNull();

            // Arrange
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Result is null");
        }

        [Fact]
        public void Returns_New_Result_When_Result_Is_Null_Custom_ErrorMessage()
        {
            // Arrange
            var sut = default(Result<string>)!;

            // Act
            var result = sut.EnsureNotNull("custom");

            // Arrange
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("custom");
        }

        [Fact]
        public void Returns_Same_Result_When_Result_Is_Not_Null()
        {
            // Arrange
            var sut = Result.Continue<string>();

            // Act
            var result = sut.EnsureNotNull();

            // Arrange
            result.ShouldBeSameAs(sut);
        }
    }

    public class WhenNull_Untyped : ResultExtensionsTests
    {
        [Fact]
        public void Returns_New_Result_When_Result_Is_Null_Default_ErrorMessage()
        {
            // Arrange
            var sut = default(Result)!;

            // Act
            var result = sut.WhenNull(ResultStatus.Invalid);

            // Arrange
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Result is null");
        }

        [Fact]
        public void Returns_New_Result_When_Result_Is_Null_Custom_ErrorMessage()
        {
            // Arrange
            var sut = default(Result)!;

            // Act
            var result = sut.WhenNull(ResultStatus.Invalid, "custom");

            // Arrange
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("custom");
        }

        [Fact]
        public void Returns_Same_Result_When_Result_Is_Not_Null()
        {
            // Arrange
            var sut = Result.Continue();

            // Act
            var result = sut.WhenNull(ResultStatus.Invalid);

            // Arrange
            result.ShouldBeSameAs(sut);
        }
    }

    public class WhenNull_Typed : ResultExtensionsTests
    {
        [Fact]
        public void Returns_New_Result_When_Result_Is_Null_Default_ErrorMessage()
        {
            // Arrange
            var sut = default(Result<string>)!;

            // Act
            var result = sut.WhenNull(ResultStatus.Invalid);

            // Arrange
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Result is null");
        }

        [Fact]
        public void Returns_New_Result_When_Result_Is_Null_Custom_ErrorMessage()
        {
            // Arrange
            var sut = default(Result<string>)!;

            // Act
            var result = sut.WhenNull(ResultStatus.Invalid, "custom");

            // Arrange
            result.ShouldNotBeNull();
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("custom");
        }

        [Fact]
        public void Returns_Same_Result_When_Result_Is_Not_Null()
        {
            // Arrange
            var sut = Result.Continue<string>();

            // Act
            var result = sut.WhenNull(ResultStatus.Invalid);

            // Arrange
            result.ShouldBeSameAs(sut);
        }
    }

    public class WhenNotContinue : ResultExtensionsTests
    {
        [Fact]
        public void Returns_Result_With_Status_Not_Equal_To_Continue_When_Found()
        {
            // Arrange
            var sut = new Result[] { Result.Continue(), Result.Success() };

            // Act
            var result = sut.WhenNotContinue(() => Result.Error("Kaboom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public void Returns_ErrorResult_When_Status_Not_Equal_To_Continue_Is_Not_Found()
        {
            // Arrange
            var sut = new Result[] { Result.Continue(), Result.Continue() };

            // Act
            var result = sut.WhenNotContinue(() => Result.Error("Kaboom"));

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
