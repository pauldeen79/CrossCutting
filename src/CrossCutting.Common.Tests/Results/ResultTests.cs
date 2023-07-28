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
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Result: Invalid");
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Result_Is_Error_And_ErrorMessage_Is_Filled()
    {
        // Arrange
        var sut = Result<string>.Error("Kaboom");

        // Act
        var act = new Action(() => _ = sut.GetValueOrThrow());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Result: Error, ErrorMessage: Kaboom");
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
        var voidResult = Result<string>.Success("success value");

        // Act
        var actual = Result<string>.FromExistingResult(voidResult);

        // Assert
        actual.ErrorMessage.Should().Be(voidResult.ErrorMessage);
        actual.Status.Should().Be(voidResult.Status);
        actual.ValidationErrors.Should().BeEquivalentTo(voidResult.ValidationErrors);
        actual.Value.Should().Be("success value");
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_SuccessResult_And_Adds_Value()
    {
        // Arrange
        var voidResult = Result.Success();

        // Act
        var actual = Result<string>.FromExistingResult(voidResult, "yes");

        // Assert
        actual.ErrorMessage.Should().Be(voidResult.ErrorMessage);
        actual.Status.Should().Be(voidResult.Status);
        actual.ValidationErrors.Should().BeEquivalentTo(voidResult.ValidationErrors);
        actual.Value.Should().Be("yes");
    }

    [Fact]
    public void FromExistingResult_Copies_Error_From_Source_When_Not_Successful()
    {
        // Arrange
        var source = Result<bool>.Error("Kaboom");

        // Act
        var actual = Result<object?>.FromExistingResult(source, x => x);

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.Value.Should().Be(default(bool));
    }

    [Fact]
    public void FromExistingResult_Copies_Value_From_Source_When_Successful()
    {
        // Arrange
        var source = Result<bool>.Success(true);

        // Act
        var actual = Result<object?>.FromExistingResult(source, x => x);

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.Value.Should().BeEquivalentTo(true);
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
    public void Can_Create_NotSupported_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NotSupported();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotSupported);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotSupported_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotSupported();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotSupported);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotSupported_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NotSupported("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotSupported);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotSupported_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotSupported("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotSupported);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Unavailable_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Unavailable();

        // Assert
        actual.Status.Should().Be(ResultStatus.Unavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Unavailable_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Unavailable();

        // Assert
        actual.Status.Should().Be(ResultStatus.Unavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Unavailable_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Unavailable("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.Unavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Unavailable_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Unavailable("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.Unavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotImplemented_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NotImplemented();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotImplemented);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotImplemented_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotImplemented();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotImplemented);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotImplemented_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NotImplemented("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotImplemented);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotImplemented_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotImplemented("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotImplemented);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NoContent_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NoContent();

        // Assert
        actual.Status.Should().Be(ResultStatus.NoContent);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NoContent_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NoContent();

        // Assert
        actual.Status.Should().Be(ResultStatus.NoContent);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NoContent_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.NoContent("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.NoContent);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NoContent_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NoContent("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.NoContent);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_ResetContent_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.ResetContent();

        // Assert
        actual.Status.Should().Be(ResultStatus.ResetContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_ResetContent_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.ResetContent();

        // Assert
        actual.Status.Should().Be(ResultStatus.ResetContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_ResetContent_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.ResetContent("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.ResetContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_ResetContent_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.ResetContent("Not authenticated");

        // Assert
        actual.Status.Should().Be(ResultStatus.ResetContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not authenticated");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Continue_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Continue();

        // Assert
        actual.Status.Should().Be(ResultStatus.Continue);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Continue_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Continue();

        // Assert
        actual.Status.Should().Be(ResultStatus.Continue);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Conflict_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Conflict();

        // Assert
        actual.Status.Should().Be(ResultStatus.Conflict);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Conflict_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Conflict();

        // Assert
        actual.Status.Should().Be(ResultStatus.Conflict);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Conflict_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result<string>.Conflict("There is a huge conflict");

        // Assert
        actual.Status.Should().Be(ResultStatus.Conflict);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("There is a huge conflict");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Conflict_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Conflict("There is a huge conflict");

        // Assert
        actual.Status.Should().Be(ResultStatus.Conflict);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("There is a huge conflict");
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

    [Fact]
    public void TryCast_Returns_Invalid_Without_ErrorMessage_When_Value_Could_Not_Be_Cast()
    {
        // Arrange
        var sut = Result<object?>.Success("test");

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void TryCast_Returns_Invalid_With_ErrorMessage_When_Value_Could_Not_Be_Cast()
    {
        // Arrange
        var sut = Result<object?>.Success("test");

        // Act
        var result = sut.TryCast<bool>("my custom error message");

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("my custom error message");
    }

    [Fact]
    public void TryCast_Returns_Same_Properties_When_Status_Is_Error()
    {
        // Arrange
        var sut = Result<object?>.Error("error");

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.Should().Be(sut.Status);
        result.ErrorMessage.Should().Be(sut.ErrorMessage);
    }

    [Fact]
    public void TryCast_Returns_Same_Properties_When_Status_Is_Invalid()
    {
        // Arrange
        var sut = Result<object?>.Invalid("error", new[] { new ValidationError("validation error 1"), new ValidationError("validation error 2") });

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.Should().Be(sut.Status);
        result.ErrorMessage.Should().Be(sut.ErrorMessage);
        result.ValidationErrors.Should().BeEquivalentTo(sut.ValidationErrors);
    }

    [Fact]
    public void TryCast_Returns_Success_With_Cast_Value_When_Possible()
    {
        // Arrange
        var sut = Result<object?>.Success(true);

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeTrue();
    }

    [Fact]
    public void Can_Create_ValidationResult_With_ErrorMessage_From_ValidationErrors()
    {
        // Arrange
        var validationErrors = new[] { new ValidationError("It's invalid, yo") };

        // Act
        var result = Result.FromValidationErrors(validationErrors, "Kaboom");

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Kaboom");
        result.ValidationErrors.Select(x => x.ErrorMessage).Should().BeEquivalentTo("It's invalid, yo");
    }

    [Fact]
    public void Can_Create_ValidationResult_Without_ErrorMessage_From_ValidationErrors()
    {
        // Arrange
        var validationErrors = new[] { new ValidationError("It's invalid, yo") };

        // Act
        var result = Result.FromValidationErrors(validationErrors);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().BeNull();
        result.ValidationErrors.Select(x => x.ErrorMessage).Should().BeEquivalentTo("It's invalid, yo");
    }

    [Fact]
    public void Can_Create_ValidationResult_With_ErrorMessage_From_Empty_ValidationErrors()
    {
        // Arrange
        var validationErrors = Array.Empty<ValidationError>();

        // Act
        var result = Result.FromValidationErrors(validationErrors, "Kaboom");

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Can_Create_ValidationResult_Without_ErrorMessage_From_EmptyValidationErrors()
    {
        // Arrange
        var validationErrors = Array.Empty<ValidationError>();

        // Act
        var result = Result.FromValidationErrors(validationErrors);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Untyped_Result_From_Typed_Result_And_Then_Read_The_Value_Untyped()
    {
        // Arrange
        Result typed = Result<int>.Success(13);

        // Act
        var result = typed.GetValue();

        // Assert
        result.Should().BeEquivalentTo(13);
    }
}
