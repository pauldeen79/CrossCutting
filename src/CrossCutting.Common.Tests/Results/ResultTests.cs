namespace CrossCutting.Common.Tests.Results;

public class ResultTests
{
    [Fact]
    public void GetValueOrThrow_Gets_Value_When_Result_Is_Success()
    {
        // Arrange
        var sut = Result<string>.Success("ok");

        // Act
        var actual = sut.GetValueOrThrow();

        // Assert
        actual.Should().Be(sut.Value);
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Result_Is_Invalid()
    {
        // Arrange
        var sut = Result<string>.Invalid();

        // Act
        var act = new Action(() => _ = sut.GetValueOrThrow());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_InvalidResult()
    {
        // Arrange
        var voidResult = Result.Invalid("Failed", new[] { new ValidationError("v1", new[] { "m1" }) });

        // Act
        var actual = Result<string>.FromExistingResult(voidResult);

        // Assert
        actual.ErrorMessage.Should().Be(voidResult.ErrorMessage);
        actual.Status.Should().Be(voidResult.Status);
        actual.ValidationErrors.Should().BeEquivalentTo(voidResult.ValidationErrors);
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_NotFoundResult()
    {
        // Arrange
        var voidResult = Result.NotFound("Failed");

        // Act
        var actual = Result<string>.FromExistingResult(voidResult);

        // Assert
        actual.ErrorMessage.Should().Be(voidResult.ErrorMessage);
        actual.Status.Should().Be(voidResult.Status);
        actual.ValidationErrors.Should().BeEquivalentTo(voidResult.ValidationErrors);
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_ErrorResult()
    {
        // Arrange
        var voidResult = Result.Error("Failed");

        // Act
        var actual = Result<string>.FromExistingResult(voidResult);

        // Assert
        actual.ErrorMessage.Should().Be(voidResult.ErrorMessage);
        actual.Status.Should().Be(voidResult.Status);
        actual.ValidationErrors.Should().BeEquivalentTo(voidResult.ValidationErrors);
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_SuccessResult()
    {
        // Arrange
        var voidResult = Result.Success();
        // Act
        var actual = Result<string>.FromExistingResult(voidResult);

        // Assert
        actual.ErrorMessage.Should().Be(voidResult.ErrorMessage);
        actual.Status.Should().Be(voidResult.Status);
        actual.ValidationErrors.Should().BeEquivalentTo(voidResult.ValidationErrors);
    }

    [Fact]
    public void Can_Create_Success_With_Correct_Value_Using_ReferenceType()
    {
        // Act
        var actual = Result<string>.Success("test");

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().Be("test");
    }

    [Fact]
    public void Can_Create_Success_With_Correct_Value_Using_ValueType()
    {
        // Act
        var actual = Result<(string Name, string Address)>.Success(("name", "address"));

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Name.Should().Be("name");
        actual.Value.Address.Should().Be("address");
    }

    [Fact]
    public void Can_Create_Success_Result_From_NonNull_Instance_Without_ErrorMesssage_Provided()
    {
        // Act
        var actual = Result.FromInstance(this);

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeSameAs(this);
    }

    [Fact]
    public void Can_Create_Success_Result_From_NonNull_Instance_With_ErrorMessage_Provided()
    {
        // Act
        var actual = Result.FromInstance(this, "This gets ignored because the instance is not null");

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeSameAs(this);
    }

    [Fact]
    public void Can_Create_Success_Result_From_NonNull_Instance_With_ValidationErrors_Provided()
    {
        // Act
        var actual = Result.FromInstance(this, new[] { new ValidationError("Ignored", new[] { "Member1" }) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeSameAs(this);
    }

    [Fact]
    public void Can_Create_Success_Result_From_NonNull_Instance_With_ErrorMessage_And_ValidationErrors_Provided()
    {
        // Act
        var actual = Result.FromInstance(this, "This gets ignored because the instance is not null", new[] { new ValidationError("Ignored", new[] { "Member1" }) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeSameAs(this);
    }

    [Fact]
    public void Can_Create_Invalid_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Invalid();

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid();

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Invalid("Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid("Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ValidationErrors()
    {
        // Act
        var actual = Result<string>.Invalid(new[] { new ValidationError("x", new[] { "m1", "m2" }) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEquivalentTo(new[] { new ValidationError("x", new[] { "m1", "m2" }) });
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ValidationErrors()
    {
        // Act
        var actual = Result.Invalid(new[] { new ValidationError("x", new[] { "m1", "m2" }) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEquivalentTo(new[] { new ValidationError("x", new[] { "m1", "m2" }) });
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ValidationErrors_And_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Invalid("Error", new[] { new ValidationError("x", new[] { "m1", "m2" }) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEquivalentTo(new[] { new ValidationError("x", new[] { "m1", "m2" }) });
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ValidationErrors_And_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid("Error", new[] { new ValidationError("x", new[] { "m1", "m2" }) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEquivalentTo(new[] { new ValidationError("x", new[] { "m1", "m2" }) });
    }

    [Fact]
    public void Can_Create_Invalid_Result_From_Null_Instance_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null, new[] { new ValidationError("Error", new[] { "Name" }) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().ContainSingle();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Result_From_Null_Instance_With_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null, "My error message", new[] { new ValidationError("Error", new[] { "Name" }) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("My error message");
        actual.ValidationErrors.Should().ContainSingle();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Error_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Error();

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Error_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Error();

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Error_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Error("Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Error_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Error("Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotFound_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NotFound();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotFound_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotFound();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotFound_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NotFound("NotFound");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("NotFound");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotFound_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotFound("NotFound");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("NotFound");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotFound_Result_From_Null_Instance_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null);

        // Assert
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotFound_Result_From_Null_Instance_With_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null, "My error message");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotFound);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("My error message");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Unauthorized_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Unauthorized();

        // Assert
        actual.Status.Should().Be(ResultStatus.Unauthorized);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Unauthorized_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Unauthorized();

        // Assert
        actual.Status.Should().Be(ResultStatus.Unauthorized);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Unauthorized_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Unauthorized("Not authorized");

        // Assert
        actual.Status.Should().Be(ResultStatus.Unauthorized);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authorized");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Unauthorized_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Unauthorized("Not authorized");

        // Assert
        actual.Status.Should().Be(ResultStatus.Unauthorized);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authorized");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotAuthenticated_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NotAuthenticated();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotAuthenticated);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotAuthenticated_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotAuthenticated();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotAuthenticated);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotAuthenticated_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NotAuthenticated("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotAuthenticated);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotAuthenticated_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotAuthenticated("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotAuthenticated);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Redirect_Result()
    {
        // Act
        var actual = Result<string>.Redirect("redirect address");

        // Assert
        actual.Status.Should().Be(ResultStatus.Redirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().Be("redirect address");
    }

    [Fact]
    public void Can_Chain_Multiple_Typed_Steps_That_Get_Executed_Entirely()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var result = Result<SomeResultValue>.Chain(request, OkStep, OkStep, OkStep);

        // Assert
        result.IsSuccessful().Should().BeTrue();
        request.OkCount.Should().Be(3);
        request.FailedCount.Should().Be(0);
        result.HasValue.Should().BeTrue();
        result.Value.Should().BeOfType<SomeResultValue>();
    }

    [Fact]
    public void Can_Chain_Multiple_Typed_Steps_That_Dont_Get_Executed_Entirely()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var result = Result<SomeResultValue>.Chain(request, OkStep, FailedStep, OkStep);

        // Assert
        result.IsSuccessful().Should().BeFalse();
        request.OkCount.Should().Be(1);
        request.FailedCount.Should().Be(1);
        result.HasValue.Should().BeFalse();
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Chain_Multiple_Untyped_Steps_That_Get_Executed_Entirely()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var result = Result.Chain(request, OkStep, OkStep, OkStep);

        // Assert
        result.IsSuccessful().Should().BeTrue();
        request.OkCount.Should().Be(3);
        request.FailedCount.Should().Be(0);
    }

    [Fact]
    public void Can_Chain_Multiple_Untyped_Steps_That_Dont_Get_Executed_Entirely()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var result = Result.Chain(request, OkStep, FailedStep, OkStep);

        // Assert
        result.IsSuccessful().Should().BeFalse();
        request.OkCount.Should().Be(1);
        request.FailedCount.Should().Be(1);
    }

    [Fact]
    public void Can_Chain_Multiple_Untyped_Steps_With_A_Writable_Context_To_Build_A_Result_From_A_Request_Using_HappyFlow()
    {
        // Arrange
        var request = new SomeRequest();
        var context = new SomeContext(request);

        // Act
        var result = Result.Chain(context, OkStep, OkStep, OkStep);
        var entity = context.Builder.Build(); // just an example how to get a composable entity from an untyped result

        // Assert
        result.IsSuccessful().Should().BeTrue();
        result.Status.Should().Be(ResultStatus.Ok);
        context.Builder.Count.Should().Be(3);
        entity.Should().NotBeNull();
    }

    [Fact]
    public void Can_Chain_Multiple_Untyped_Steps_With_A_Writable_Context_To_Build_A_Result_From_A_Request_Using_UnhappyFlow()
    {
        // Arrange
        var request = new SomeRequest();
        var context = new SomeContext(request);

        // Act
        var result = Result.Chain(context, OkStep, FailedStep, OkStep);

        // Assert
        result.IsSuccessful().Should().BeFalse();
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Something went very wrong!");
        context.Builder.Count.Should().Be(1);
    }

    [Fact]
    public void Chain_Untyped_Without_Steps_Returns_Error_No_Accumulator()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var actual = Result.Chain(request, Array.Empty<Func<SomeRequest, Result>>());

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Could not determine result because there are no steps defined");
    }

    [Fact]
    public void Chain_Typed_Without_Steps_Returns_Error_No_Accumulator()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var actual = Result<SomeResultValue>.Chain(request, Array.Empty<Func<SomeRequest, Result<SomeResultValue>>>());

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Could not determine result because there are no steps defined");
    }

    [Fact]
    public void Chain_Without_Steps_Returns_Error_Accumulator()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var actual = Result.Chain(request, Array.Empty<Func<SomeRequest, Result, Result>>());

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Could not determine result because there are no steps defined");
    }

    [Fact]
    public void Chain_With_Successful_Steps_Returns_Result_From_Last_Step()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var result = Result<SomeResultValue>.Chain(request, AnotherOkStep, OkStep, AnotherOkStep);

        // Assert
        result.IsSuccessful().Should().BeTrue();
        request.OkCount.Should().Be(3);
        request.FailedCount.Should().Be(0);
        result.HasValue.Should().BeTrue();
        result.Value.Should().BeOfType<SomeDerivedResultValue>();
    }

    [Fact]
    public void Can_Chain_Multiple_Untyped_Steps_With_Accumulator()
    {
        // Arrange
        var request = new SomeRequest();

        // Act
        var result = Result.Chain(request, OkStepWithAccumulator, OkStepWithAccumulator, OkStepWithAccumulator);

        // Assert
        result.IsSuccessful().Should().BeTrue();
        request.OkCount.Should().Be(3);
        request.FailedCount.Should().Be(0);
    }

    [Fact]
    public void Can_Chain_Multiple_Typed_Steps_With_Accumulator()
    {
        // Arrange
        var context = new SomeContext(new SomeRequest());

        // Act
        var result = Result<SomeResultValue>.Chain(context, OkStepWithAccumulator, OkStepWithAccumulator, OkStepWithAccumulator);

        // Assert
        result.IsSuccessful().Should().BeTrue();
        context.Request.OkCount.Should().Be(3);
        context.Request.FailedCount.Should().Be(0);
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Value_Is_Null()
    {
        // Arrange
        var sut = Result<string>.Error();

        // Act
        var act = new Action(() => sut.GetValueOrThrow());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void GetValueOrThrow_Returns_Value_When_Value_Is_Not_Null()
    {
        // Arrange
        var sut = Result<string>.Success("yes!");

        // Act
        var value = sut.GetValueOrThrow();

        // Assert
        value.Should().Be(sut.Value);
    }

    private Result<SomeResultValue> OkStep(SomeRequest request)
    {
        request.OkCount++;
        return Result<SomeResultValue>.Success(new ());
    }

    private Result OkStep(SomeContext context)
    {
        context.Builder.Add();
        return Result.Success();
    }

    private Result<SomeResultValue> OkStepWithAccumulator(SomeContext context, Result<SomeResultValue> previousResult)
    {
        context.Request.OkCount++;
        return Result<SomeResultValue>.Success(new());
    }

    private Result OkStepWithAccumulator(SomeRequest request, Result previousResult)
    {
        request.OkCount++;
        return Result.Success();
    }

    private Result<SomeResultValue> AnotherOkStep(SomeRequest request)
    {
        request.OkCount++;
        return Result<SomeResultValue>.Success(new SomeDerivedResultValue());
    }

    private Result<SomeResultValue> FailedStep(SomeRequest request)
    {
        request.FailedCount++;
        return Result<SomeResultValue>.Error("Something went very wrong!");
    }

    private Result FailedStep(SomeContext context) => Result.Error("Something went very wrong!");

    private sealed class SomeRequest
    {
        public int OkCount { get; set; }
        public int FailedCount { get; set; }
    }

    private sealed class SomeContext
    {
        public SomeRequest Request { get; }
        public SomeEntityBuilder Builder { get; }

        public SomeContext(SomeRequest request)
        {
            Request = request;
            Builder = new();
        }
    }

    private class SomeResultValue
    {
    }

    private sealed class SomeDerivedResultValue : SomeResultValue
    {
    }

    private sealed class SomeEntityBuilder
    {
        public int Count { get; private set; }
        public void Add() => Count++;
        public SomeResultValue Build() => new SomeResultValue();
    }
}
