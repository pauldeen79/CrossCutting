namespace CrossCutting.Common.Tests.Results;

public class ResultTests
{
    [Fact]
    public void GetValueOrThrow_Gets_Value_When_Result_Is_Success()
    {
        // Arrange
        var sut = Result.Success("ok");

        // Act
        var actual = sut.GetValueOrThrow();

        // Assert
        actual.Should().Be(sut.Value);
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Result_Is_Invalid()
    {
        // Arrange
        var sut = Result.Invalid<string>();

        // Act
        var act = new Action(() => _ = sut.GetValueOrThrow());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Result: Invalid");
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Result_Is_Error_And_ErrorMessage_Is_Filled()
    {
        // Arrange
        var sut = Result.Error<string>("Kaboom");

        // Act
        var act = new Action(() => _ = sut.GetValueOrThrow());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Result: Error, ErrorMessage: Kaboom");
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_InvalidResult()
    {
        // Arrange
        var voidResult = Result.Invalid("Failed", [new ValidationError("v1", ["m1"])]);

        // Act
        var actual = Result.FromExistingResult<string>(voidResult);

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
        var actual = Result.FromExistingResult<string>(voidResult);

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
        var actual = Result.FromExistingResult<string>(voidResult);

        // Assert
        actual.ErrorMessage.Should().Be(voidResult.ErrorMessage);
        actual.Status.Should().Be(voidResult.Status);
        actual.ValidationErrors.Should().BeEquivalentTo(voidResult.ValidationErrors);
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_SuccessResult()
    {
        // Arrange
        var voidResult = Result.Success("success value");

        // Act
        var actual = Result.FromExistingResult<string>(voidResult);

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
        var actual = Result.FromExistingResult(voidResult, "yes");

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
        var source = Result.Error<bool>("Kaboom");

        // Act
        var actual = Result.FromExistingResult<bool, object?>(source, x => x);

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.Value.Should().Be(default(bool));
    }

    [Fact]
    public void FromExistingResult_Copies_Value_From_Source_When_Successful()
    {
        // Arrange
        var source = Result.Success(true);

        // Act
        var actual = Result.FromExistingResult<bool, object?>(source, x => x);

        // Assert
        actual.Status.Should().Be(ResultStatus.Ok);
        actual.Value.Should().BeEquivalentTo(true);
    }

    [Fact]
    public void Can_Create_Success_With_Correct_Value_Using_ReferenceType()
    {
        // Act
        var actual = Result.Success("test");

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
        var actual = Result.Success<(string Name, string Address)>(("name", "address"));

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
        var actual = Result.FromInstance(this, new[] { new ValidationError("Ignored", ["Member1"]) });

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
        var actual = Result.FromInstance(this, "This gets ignored because the instance is not null", [new ValidationError("Ignored", ["Member1"])]);

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
        var actual = Result.Invalid<string>();

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
        var actual = Result.Invalid<string>("Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ErrorMessage_And_InnerResults()
    {
        // Act
        var actual = Result.Invalid<string>("Error", new[] { Result.Error("Kaboom") });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.InnerResults.Should().ContainSingle();
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
    public void Can_Create_Invalid_Void_Result_With_ErrorMessage_And_InnerResults()
    {
        // Act
        var actual = Result.Invalid("Error", new[] { Result.Error("Kaboom") });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.InnerResults.Should().ContainSingle();
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ValidationErrors()
    {
        // Act
        var actual = Result.Invalid<string>(new[] { new ValidationError("x", ["m1", "m2"]) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEquivalentTo(new[] { new ValidationError("x", ["m1", "m2"]) });
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ValidationErrors()
    {
        // Act
        var actual = Result.Invalid(new[] { new ValidationError("x", ["m1", "m2"]) });

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEquivalentTo(new[] { new ValidationError("x", ["m1", "m2"]) });
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ValidationErrors_And_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid<string>("Error", [new ValidationError("x", ["m1", "m2"])]);

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEquivalentTo(new[] { new ValidationError("x", ["m1", "m2"]) });
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ValidationErrors_And_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid("Error", [new ValidationError("x", ["m1", "m2"])]);

        // Assert
        actual.Status.Should().Be(ResultStatus.Invalid);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEquivalentTo(new[] { new ValidationError("x", ["m1", "m2"]) });
    }

    [Fact]
    public void Can_Create_Invalid_Result_From_Null_Instance_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null, new[] { new ValidationError("Error", ["Name"]) });

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
        var actual = Result.FromInstance<ResultTests>(null, "My error message", [new ValidationError("Error", ["Name"])]);

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
        var actual = Result.Error<string>();

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
        var actual = Result.Error<string>("Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Error_Result_With_ErrorMessage_And_Exception()
    {
        // Act
        var actual = Result.Error<string>(new InvalidOperationException("Kaboom"), "Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
        actual.Exception.Should().BeOfType<InvalidOperationException>().And.Match<InvalidOperationException>(x => x.Message == "Kaboom");
    }

    [Fact]
    public void Can_Create_Error_Result_With_ErrorMessage_And_InnerResults()
    {
        // Act
        var actual = Result.Error<string>([Result.Error("InnerError")], "Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
        actual.Exception.Should().BeNull();
        actual.InnerResults.Should().ContainSingle();
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
    public void Can_Create_Error_Void_Result_With_ErrorMessage_And_Exception()
    {
        // Act
        var actual = Result.Error(new InvalidOperationException("Kaboom"), "Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Exception.Should().BeOfType<InvalidOperationException>().And.Match<InvalidOperationException>(x => x.Message == "Kaboom");
    }

    [Fact]
    public void Can_Create_Error_Void_Result_With_ErrorMessage_And_InnerResults()
    {
        // Act
        var actual = Result.Error([Result.Error("InnerError")], "Error");

        // Assert
        actual.Status.Should().Be(ResultStatus.Error);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Error");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Exception.Should().BeNull();
        actual.InnerResults.Should().ContainSingle();
    }

    [Fact]
    public void Can_Create_NotFound_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotFound<string>();

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
        var actual = Result.NotFound<string>("NotFound");

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
        var actual = Result.Unauthorized<string>();

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
        var actual = Result.Unauthorized<string>("Not authorized");

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
        var actual = Result.NotAuthenticated<string>();

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
        var actual = Result.NotAuthenticated<string>("Not authenticated");

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
        var actual = Result.NotSupported<string>();

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
        var actual = Result.NotSupported<string>("Not supported");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotSupported);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not supported");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotSupported_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotSupported("Not supported");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotSupported);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not supported");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Unavailable_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Unavailable<string>();

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
        var actual = Result.Unavailable<string>("Not available");

        // Assert
        actual.Status.Should().Be(ResultStatus.Unavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not available");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Unavailable_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Unavailable("Not available");

        // Assert
        actual.Status.Should().Be(ResultStatus.Unavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not available");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotImplemented_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotImplemented<string>();

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
        var actual = Result.NotImplemented<string>("Not implemented");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotImplemented);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not implemented");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotImplemented_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotImplemented("Not implemented");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotImplemented);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Not implemented");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NoContent_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NoContent<string>();

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
        var actual = Result.NoContent<string>("No content");

        // Assert
        actual.Status.Should().Be(ResultStatus.NoContent);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().Be("No content");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NoContent_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NoContent("No content");

        // Assert
        actual.Status.Should().Be(ResultStatus.NoContent);
        actual.IsSuccessful().Should().BeTrue();
        actual.ErrorMessage.Should().Be("No content");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_ResetContent_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.ResetContent<string>();

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
        var actual = Result.ResetContent<string>("Reset");

        // Assert
        actual.Status.Should().Be(ResultStatus.ResetContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Reset");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_ResetContent_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.ResetContent("Reset");

        // Assert
        actual.Status.Should().Be(ResultStatus.ResetContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Reset");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Continue_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Continue<string>();

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
        var actual = Result.Conflict<string>();

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
        var actual = Result.Conflict<string>("There is a huge conflict");

        // Assert
        actual.Status.Should().Be(ResultStatus.Conflict);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("There is a huge conflict");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Conflict_Result_With_ErrorMessage_And_InnerResult()
    {
        // Act
        var actual = Result.Conflict<string>([Result.Error("Kaboom")], "There is a huge conflict");

        // Assert
        actual.Status.Should().Be(ResultStatus.Conflict);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("There is a huge conflict");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
        actual.InnerResults.Should().ContainSingle();
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
    public void Can_Create_Conflict_Void_Result_With_ErrorMessage_And_InnerResult()
    {
        // Act
        var actual = Result.Conflict([Result.Error("Kaboom")], "There is a huge conflict");

        // Assert
        actual.Status.Should().Be(ResultStatus.Conflict);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("There is a huge conflict");
        actual.ValidationErrors.Should().BeEmpty();
        actual.InnerResults.Should().ContainSingle();
    }

    [Fact]
    public void Can_Create_Redirect_Result()
    {
        // Act
        var actual = Result.Redirect("redirect address");

        // Assert
        actual.Status.Should().Be(ResultStatus.Redirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().Be("redirect address");
    }

    [Fact]
    public void Can_Create_Created_Result_With_Value()
    {
        // Act
        var result = Result.Created("Some value");

        // Assert
        result.Status.Should().Be(ResultStatus.Created);
        result.Value.Should().Be("Some value");
    }

    [Fact]
    public void Can_Create_Created_Result_Without_Value()
    {
        // Act
        var result = Result.Created();

        // Assert
        result.Status.Should().Be(ResultStatus.Created);
        result.GetValue().Should().BeNull();
    }

    [Fact]
    public void Can_Create_Accepted_Result_With_Value()
    {
        // Act
        var result = Result.Accepted("Some value");

        // Assert
        result.Status.Should().Be(ResultStatus.Accepted);
        result.Value.Should().Be("Some value");
    }

    [Fact]
    public void Can_Create_Accepted_Result_Without_Value()
    {
        // Act
        var result = Result.Accepted();

        // Assert
        result.Status.Should().Be(ResultStatus.Accepted);
        result.GetValue().Should().BeNull();
    }

    [Fact]
    public void Can_Create_Already_Reported_Result_With_Value()
    {
        // Act
        var result = Result.AlreadyReported("Some value");

        // Assert
        result.Status.Should().Be(ResultStatus.AlreadyReported);
        result.Value.Should().Be("Some value");
    }

    [Fact]
    public void Can_Create_Already_Reported_Result_Without_Value()
    {
        // Act
        var result = Result.AlreadyReported();

        // Assert
        result.Status.Should().Be(ResultStatus.AlreadyReported);
        result.GetValue().Should().BeNull();
    }

    [Fact]
    public void Can_Create_Found_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Found<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.Found);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Found_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Found();

        // Assert
        actual.Status.Should().Be(ResultStatus.Found);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Found_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Found<string>("Found");

        // Assert
        actual.Status.Should().Be(ResultStatus.Found);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Found");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Found_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Found("Found");

        // Assert
        actual.Status.Should().Be(ResultStatus.Found);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Found");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_MovedPermanently_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.MovedPermanently<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.MovedPermanently);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_MovedPermanently_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.MovedPermanently();

        // Assert
        actual.Status.Should().Be(ResultStatus.MovedPermanently);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_MovedPermanently_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.MovedPermanently<string>("MovedPermanently");

        // Assert
        actual.Status.Should().Be(ResultStatus.MovedPermanently);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("MovedPermanently");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_MovedPermanently_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.MovedPermanently("MovedPermanently");

        // Assert
        actual.Status.Should().Be(ResultStatus.MovedPermanently);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("MovedPermanently");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Gone_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Gone<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.Gone);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Gone_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Gone();

        // Assert
        actual.Status.Should().Be(ResultStatus.Gone);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Gone_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Gone<string>("Gone");

        // Assert
        actual.Status.Should().Be(ResultStatus.Gone);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Gone");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Gone_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Gone("Gone");

        // Assert
        actual.Status.Should().Be(ResultStatus.Gone);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Gone");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotModified_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotModified<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotModified);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotModified_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotModified();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotModified);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotModified_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotModified<string>("NotModified");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotModified);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("NotModified");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotModified_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotModified("NotModified");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotModified);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("NotModified");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_TemporaryRedirect_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TemporaryRedirect<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.TemporaryRedirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_TemporaryRedirect_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TemporaryRedirect();

        // Assert
        actual.Status.Should().Be(ResultStatus.TemporaryRedirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_TemporaryRedirect_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TemporaryRedirect<string>("TemporaryRedirect");

        // Assert
        actual.Status.Should().Be(ResultStatus.TemporaryRedirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("TemporaryRedirect");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_TemporaryRedirect_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TemporaryRedirect("TemporaryRedirect");

        // Assert
        actual.Status.Should().Be(ResultStatus.TemporaryRedirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("TemporaryRedirect");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_PermanentRedirect_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PermanentRedirect<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.PermanentRedirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_PermanentRedirect_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PermanentRedirect();

        // Assert
        actual.Status.Should().Be(ResultStatus.PermanentRedirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_PermanentRedirect_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PermanentRedirect<string>("PermanentRedirect");

        // Assert
        actual.Status.Should().Be(ResultStatus.PermanentRedirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("PermanentRedirect");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_PermanentRedirect_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PermanentRedirect("PermanentRedirect");

        // Assert
        actual.Status.Should().Be(ResultStatus.PermanentRedirect);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("PermanentRedirect");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Forbidden_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Forbidden<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.Forbidden);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Forbidden_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Forbidden();

        // Assert
        actual.Status.Should().Be(ResultStatus.Forbidden);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Forbidden_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Forbidden<string>("Forbidden");

        // Assert
        actual.Status.Should().Be(ResultStatus.Forbidden);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Forbidden");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Forbidden_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Forbidden("Forbidden");

        // Assert
        actual.Status.Should().Be(ResultStatus.Forbidden);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Forbidden");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotAcceptable_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotAcceptable<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotAcceptable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotAcceptable_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotAcceptable();

        // Assert
        actual.Status.Should().Be(ResultStatus.NotAcceptable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_NotAcceptable_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotAcceptable<string>("NotAcceptable");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotAcceptable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("NotAcceptable");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_NotAcceptable_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotAcceptable("NotAcceptable");

        // Assert
        actual.Status.Should().Be(ResultStatus.NotAcceptable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("NotAcceptable");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_TimeOut_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TimeOut<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.TimeOut);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_TimeOut_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TimeOut();

        // Assert
        actual.Status.Should().Be(ResultStatus.TimeOut);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_TimeOut_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TimeOut<string>("TimeOut");

        // Assert
        actual.Status.Should().Be(ResultStatus.TimeOut);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("TimeOut");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_TimeOut_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TimeOut("TimeOut");

        // Assert
        actual.Status.Should().Be(ResultStatus.TimeOut);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("TimeOut");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Locked_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Locked<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.Locked);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Locked_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Locked();

        // Assert
        actual.Status.Should().Be(ResultStatus.Locked);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_Locked_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Locked<string>("Locked");

        // Assert
        actual.Status.Should().Be(ResultStatus.Locked);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Locked");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_Locked_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Locked("Locked");

        // Assert
        actual.Status.Should().Be(ResultStatus.Locked);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("Locked");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_ServiceUnavailable_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.ServiceUnavailable<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.ServiceUnavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_ServiceUnavailable_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.ServiceUnavailable();

        // Assert
        actual.Status.Should().Be(ResultStatus.ServiceUnavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_ServiceUnavailable_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.ServiceUnavailable<string>("ServiceUnavailable");

        // Assert
        actual.Status.Should().Be(ResultStatus.ServiceUnavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("ServiceUnavailable");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_ServiceUnavailable_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.ServiceUnavailable("ServiceUnavailable");

        // Assert
        actual.Status.Should().Be(ResultStatus.ServiceUnavailable);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("ServiceUnavailable");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_GatewayTimeout_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.GatewayTimeout<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.GatewayTimeout);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_GatewayTimeout_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.GatewayTimeout();

        // Assert
        actual.Status.Should().Be(ResultStatus.GatewayTimeout);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_GatewayTimeout_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.GatewayTimeout<string>("GatewayTimeout");

        // Assert
        actual.Status.Should().Be(ResultStatus.GatewayTimeout);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("GatewayTimeout");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_GatewayTimeout_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.GatewayTimeout("GatewayTimeout");

        // Assert
        actual.Status.Should().Be(ResultStatus.GatewayTimeout);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("GatewayTimeout");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_BadGateway_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.BadGateway<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.BadGateway);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_BadGateway_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.BadGateway();

        // Assert
        actual.Status.Should().Be(ResultStatus.BadGateway);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_BadGateway_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.BadGateway<string>("BadGateway");

        // Assert
        actual.Status.Should().Be(ResultStatus.BadGateway);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("BadGateway");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_BadGateway_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.BadGateway("BadGateway");

        // Assert
        actual.Status.Should().Be(ResultStatus.BadGateway);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("BadGateway");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_InsuficientStorage_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.InsuficientStorage<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.InsuficientStorage);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_InsuficientStorage_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.InsuficientStorage();

        // Assert
        actual.Status.Should().Be(ResultStatus.InsuficientStorage);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_InsuficientStorage_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.InsuficientStorage<string>("InsuficientStorage");

        // Assert
        actual.Status.Should().Be(ResultStatus.InsuficientStorage);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("InsuficientStorage");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_InsuficientStorage_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.InsuficientStorage("InsuficientStorage");

        // Assert
        actual.Status.Should().Be(ResultStatus.InsuficientStorage);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("InsuficientStorage");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_UnprocessableContent_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.UnprocessableContent<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.UnprocessableContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_UnprocessableContent_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.UnprocessableContent();

        // Assert
        actual.Status.Should().Be(ResultStatus.UnprocessableContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_UnprocessableContent_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.UnprocessableContent<string>("UnprocessableContent");

        // Assert
        actual.Status.Should().Be(ResultStatus.UnprocessableContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("UnprocessableContent");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_UnprocessableContent_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.UnprocessableContent("UnprocessableContent");

        // Assert
        actual.Status.Should().Be(ResultStatus.UnprocessableContent);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("UnprocessableContent");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_FailedDependency_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FailedDependency<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.FailedDependency);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_FailedDependency_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FailedDependency();

        // Assert
        actual.Status.Should().Be(ResultStatus.FailedDependency);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_FailedDependency_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.FailedDependency<string>("FailedDependency");

        // Assert
        actual.Status.Should().Be(ResultStatus.FailedDependency);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("FailedDependency");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_FailedDependency_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.FailedDependency("FailedDependency");

        // Assert
        actual.Status.Should().Be(ResultStatus.FailedDependency);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("FailedDependency");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_PreconditionRequired_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionRequired<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.PreconditionRequired);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_PreconditionRequired_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionRequired();

        // Assert
        actual.Status.Should().Be(ResultStatus.PreconditionRequired);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_PreconditionRequired_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionRequired<string>("PreconditionRequired");

        // Assert
        actual.Status.Should().Be(ResultStatus.PreconditionRequired);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("PreconditionRequired");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_PreconditionRequired_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionRequired("PreconditionRequired");

        // Assert
        actual.Status.Should().Be(ResultStatus.PreconditionRequired);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("PreconditionRequired");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_PreconditionFailed_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionFailed<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.PreconditionFailed);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_PreconditionFailed_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionFailed();

        // Assert
        actual.Status.Should().Be(ResultStatus.PreconditionFailed);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_PreconditionFailed_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionFailed<string>("PreconditionFailed");

        // Assert
        actual.Status.Should().Be(ResultStatus.PreconditionFailed);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("PreconditionFailed");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_PreconditionFailed_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionFailed("PreconditionFailed");

        // Assert
        actual.Status.Should().Be(ResultStatus.PreconditionFailed);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("PreconditionFailed");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_TooManyRequests_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TooManyRequests<string>();

        // Assert
        actual.Status.Should().Be(ResultStatus.TooManyRequests);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_TooManyRequests_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TooManyRequests();

        // Assert
        actual.Status.Should().Be(ResultStatus.TooManyRequests);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().BeNull();
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void Can_Create_TooManyRequests_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TooManyRequests<string>("TooManyRequests");

        // Assert
        actual.Status.Should().Be(ResultStatus.TooManyRequests);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("TooManyRequests");
        actual.ValidationErrors.Should().BeEmpty();
        actual.Value.Should().BeNull();
    }

    [Fact]
    public void Can_Create_TooManyRequests_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TooManyRequests("TooManyRequests");

        // Assert
        actual.Status.Should().Be(ResultStatus.TooManyRequests);
        actual.IsSuccessful().Should().BeFalse();
        actual.ErrorMessage.Should().Be("TooManyRequests");
        actual.ValidationErrors.Should().BeEmpty();
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Value_Is_Null()
    {
        // Arrange
        var sut = Result.Error<string>();

        // Act
        var act = new Action(() => sut.GetValueOrThrow());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>();
    }

    [Fact]
    public void GetValueOrThrow_Returns_Value_When_Value_Is_Not_Null()
    {
        // Arrange
        var sut = Result.Success("yes!");

        // Act
        var value = sut.GetValueOrThrow();

        // Assert
        value.Should().Be(sut.Value);
    }

    [Fact]
    public void TryCast_Returns_Invalid_With_Default_ErrorMessage_When_Value_Could_Not_Be_Cast()
    {
        // Arrange
        var sut = Result.Success<object?>("test");

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        result.ErrorMessage.Should().Be("Could not cast System.Object to System.Boolean");
    }

    [Fact]
    public void TryCast_Returns_Invalid_With_Custom_ErrorMessage_When_Value_Could_Not_Be_Cast()
    {
        // Arrange
        var sut = Result.Success<object?>("test");

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
        var sut = Result.Error<object?>("error");

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
        var sut = Result.Invalid<object?>("error", [new ValidationError("validation error 1"), new ValidationError("validation error 2")]);

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
        var sut = Result.Success<object?>(true);

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeTrue();
    }

    [Fact]
    public void TryCast_Returns_Success_With_Default_Value_When_Value_Is_Null()
    {
        // Arrange
        var sut = Result.Success<object?>(null);

        // Act
        var result = sut.TryCast<bool?>();

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void TryCast_Returns_Same_Status()
    {
        // Arrange
        var sut = Result.Continue<object?>(true);

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
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
        Result typed = Result.Success(13);

        // Act
        var result = typed.GetValue();

        // Assert
        result.Should().BeEquivalentTo(13);
    }

    [Fact]
    public void ThrowIfInvalid_Does_Not_Throw_When_Result_Is_Success()
    {
        // Arrange
        var sut = Result.Success("ok");

        // Act & Assert
        sut.Invoking(x => x.ThrowIfInvalid()).Should().NotThrow();
    }

    [Fact]
    public void ThrowIfInvalid_Throws_When_Result_Is_Invalid()
    {
        // Arrange
        var sut = Result.Invalid<string>();

        // Act
        var act = new Action(sut.ThrowIfInvalid);

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Result: Invalid");
    }

    [Fact]
    public void ThrowIfInvalid_Throws_When_Result_Is_Error_And_ErrorMessage_Is_Filled()
    {
        // Arrange
        var sut = Result.Error<string>("Kaboom");

        // Act
        var act = new Action(sut.ThrowIfInvalid);

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Result: Error, ErrorMessage: Kaboom");
    }

    [Fact]
    public void Transform_Can_Transform_The_Value_On_Success_With_Value_Delegate()
    {
        // Arrange
        var source = Result.Success(1);

        // Act
        var result = source.Transform(x => x.ToString());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("1");
    }

    [Fact]
    public void Transform_Returns_Same_Error_On_Failure_With_Value_Delegate()
    {
        // Arrange
        var source = Result.Error<int>("Kaboom!");

        // Act
        var result = source.Transform(x => x.ToString());

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom!");
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Transform_Can_Transform_The_Value_On_Success_With_Typed_Result_Delegate()
    {
        // Arrange
        var source = Result.Success(1);

        // Act
        var result = source.Transform(x => Result.Success(x.ToString()));

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("1");
    }

    [Fact]
    public void Transform_Returns_Same_Error_On_Failure_With_Typed_Result_Delegate()
    {
        // Arrange
        var source = Result.Error<int>("Kaboom!");

        // Act
        var result = source.Transform(x => Result.Success(x.ToString()));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom!");
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Transform_Can_Transform_The_Value_On_Success_With_Untyped_Result_Delegate()
    {
        // Arrange
        var source = Result.Success(1);

        // Act
        var result = source.Transform(_ => Result.Success());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
    }

    [Fact]
    public void Transform_Returns_Same_Error_On_Failure_With_Untyped_Result_Delegate()
    {
        // Arrange
        var source = Result.Error<int>("Kaboom!");

        // Act
        var result = source.Transform(_ => Result.Success());

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom!");
    }

    [Fact]
    public void Aggregate_Returns_Success_When_No_Items_Are_Present_No_Value()
    {
        // Arrange
        var innerResults = Array.Empty<Result>();
        var successResult = Result.Success();
        var errorResultDelegate = new Func<Result[], Result>(errors => Result.Error(errors, "Something went wrong. See inner results."));

        // Act
        var result = Result.Aggregate(innerResults, successResult, errorResultDelegate);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.InnerResults.Should().BeEmpty();
    }

    [Fact]
    public void Aggregate_Returns_Success_When_No_Errors_Are_Present_No_Value()
    {
        // Arrange
        var innerResults = new Result[] { Result.Success(), Result.Success() };
        var successResult = Result.Success();
        var errorResultDelegate = new Func<Result[], Result>(errors => Result.Error(errors, "Something went wrong. See inner results."));

        // Act
        var result = Result.Aggregate(innerResults, successResult, errorResultDelegate);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.InnerResults.Should().BeEmpty();
    }

    [Fact]
    public void Aggregate_Returns_Aggregate_Error_When_One_Item_Is_Not_Succesful_No_Value()
    {
        // Arrange
        var innerResults = new Result[] { Result.Success(), Result.Error("Kaboom") };
        var successResult = Result.Success();
        var errorResultDelegate = new Func<Result[], Result>(errors => Result.Error(errors, "Something went wrong. See inner results."));

        // Act
        var result = Result.Aggregate(innerResults, successResult, errorResultDelegate);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Something went wrong. See inner results.");
        result.InnerResults.Should().ContainSingle();
    }

    [Fact]
    public void Aggregate_Returns_Aggregate_Error_With_InnerResults_When_Two_Items_Are_Not_Succesful_No_Value()
    {
        // Arrange
        var innerResults = new Result[] { Result.Error("Kaboom 1"), Result.Error("Kaboom 2") };
        var successResult = Result.Success();
        var errorResultDelegate = new Func<Result[], Result>(errors => Result.Error(errors, "Something went wrong. See inner results."));

        // Act
        var result = Result.Aggregate(innerResults, successResult, errorResultDelegate);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Something went wrong. See inner results.");
        result.InnerResults.Should().HaveCount(2);
    }

    [Fact]
    public void Either_Void_Runs_Success_Action_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;
        var success = false;

        // Act
        sut.Either(_ => error = true, _ => success = true);

        // Assert
        success.Should().BeTrue();
        error.Should().BeFalse();
    }

    [Fact]
    public async Task Either_Void_Async_Runs_Success_Action_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;
        var success = false;

        // Act
        await sut.Either(_ => error = true, x => Task.FromResult(x.Chain(() => success = true)));

        // Assert
        success.Should().BeTrue();
        error.Should().BeFalse();
    }

    [Fact]
    public void Either_Void_Runs_Failure_Action_On_Non_Successful_Result()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;
        var success = false;

        // Act
        sut.Either(_ => error = true, _ => success = true);

        // Assert
        success.Should().BeFalse();
        error.Should().BeTrue();
    }

    [Fact]
    public async Task Either_Void_Async_Runs_Failure_Action_On_Non_Successful_Result()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;
        var success = false;

        // Act
        await sut.Either(_ => error = true, x => Task.FromResult(x.Chain(() => success = true)));

        // Assert
        success.Should().BeFalse();
        error.Should().BeTrue();
    }

    [Fact]
    public void Either_Void_Parameterless_Runs_Success_Action_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;
        var success = false;

        // Act
        sut.Either(_ => error = true, () => success = true);

        // Assert
        success.Should().BeTrue();
        error.Should().BeFalse();
    }

    [Fact]
    public async Task Either_Void_Parameterless_Async_Runs_Success_Action_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;
        var success = false;

        // Act
        await sut.Either(_ => error = true, () => Task.FromResult(Result.Success().Chain(() => success = true)));

        // Assert
        success.Should().BeTrue();
        error.Should().BeFalse();
    }

    [Fact]
    public void Either_Void_Parameterless_Runs_Failure_Action_On_Non_Successful_Result()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;
        var success = false;

        // Act
        sut.Either(_ => error = true, () => success = true);

        // Assert
        success.Should().BeFalse();
        error.Should().BeTrue();
    }

    [Fact]
    public async Task Either_Void_Parameterless_Async_Runs_Failure_Action_On_Non_Successful_Result()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;
        var success = false;

        // Act
        await sut.Either(_ => error = true, () => Task.FromResult(Result.Success().Chain(() => success = true)));

        // Assert
        success.Should().BeFalse();
        error.Should().BeTrue();
    }

    [Fact]
    public async Task Either_Void_Func_Task_Runs_Success_Action_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;
        var success = false;

        // Act
        var result = await sut.Either(x => x.Chain(() => error = true), _ => Task.FromResult(Result.Success().Chain(() => success = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        success.Should().BeTrue();
        error.Should().BeFalse();
    }

    [Fact]
    public async Task Either_Void_Func_Async_Runs_Failure_Action_On_Non_Successful_Result()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;
        var success = false;

        // Act
        var result = await sut.Either(x => x.Chain(() => error = true), _ => Task.FromResult(Result.Success().Chain(() => success = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        success.Should().BeFalse();
        error.Should().BeTrue();
    }

    [Fact]
    public void Either_Void_Func_Does_Nothing_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;

        // Act
        var result = sut.Either(x => x.Chain(() => error = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        error.Should().BeFalse();
    }

    [Fact]
    public void Either_Void_Func_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;

        // Act
        var result = sut.Either(x => x.Chain(() => error = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom");
        error.Should().BeTrue();
    }

    [Fact]
    public void Either_Result_Returns_Success_Delegate_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.Either(_ => Result.Error("Custom"), _ => Result.Continue());

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Either_Result_Returns_Failure_Delegate_On_Non_Successful_Result()
    {
        // Arrange
        var sut = Result.Error("Kaboom");

        // Act
        var result = sut.Either(_ => Result.Error("Custom"), _ => Result.Continue());

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Custom");
    }

    [Fact]
    public void Either_Result_Returns_Failure_Delegate_On_Non_Successful_Result_No_Alternate_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error<string>("Kaboom");

        // Act
        var result = sut.Either(_ => Result.Error<string>("Custom"));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Custom");
    }

    [Fact]
    public async Task Either_Result_Async_Parameterless_Returns_Success_Delegate_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success("OK");

        // Act
        var result = await sut.Either(_ => Result.Error<string>("Custom"), () => Task.FromResult(Result.Continue<string>()));

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Either_Result_Returns_Same_Instance_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success("OK");

        // Act
        var result = sut.Either(_ => Result.Error<string>("Custom"));

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("OK");
    }

    [Fact]
    public void Either_Result_Parameterless_Returns_Success_Delegate_On_Successful_Result()
    {
        // Arrange
        var sut = Result.Success("Succes value");

        // Act
        var result = sut.Either(_ => Result.Error<string>("Custom"), Result.Continue<string>);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
        result.Value.Should().BeNull();
    }

    [Fact]
    public void Either_Result_Parameterless_Returns_Failure_Delegate_On_Non_Successful_Result()
    {
        // Arrange
        var sut = Result.Error<string>("Kaboom");

        // Act
        var result = sut.Either(_ => Result.Error<string>("Custom"), Result.Continue<string>);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Custom");
    }

    [Fact]
    public async Task Either_Result_Async_Parameterless_Returns_Failure_Delegate_On_Non_Successful_Result()
    {
        // Arrange
        var sut = Result.Error<string>("Kaboom");

        // Act
        var result = await sut.Either(_ => Result.Error<string>("Custom"), () => Task.FromResult(Result.Continue<string>()));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Custom");
    }

    [Fact]
    public void OnFailure_Does_Nothing_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;

        // Act
        var result = sut.OnFailure(_ => error = true);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        error.Should().BeFalse();
    }

    [Fact]
    public void OnFailure_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;

        // Act
        var result = sut.OnFailure(_ => error = true);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        error.Should().BeTrue();
    }

    [Fact]
    public void OnFailure_Parameterless_Does_Nothing_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;

        // Act
        var result = sut.OnFailure(() => error = true);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        error.Should().BeFalse();
    }

    [Fact]
    public void OnFailure_Parameterless_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;

        // Act
        var result = sut.OnFailure(() => error = true);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        error.Should().BeTrue();
    }

    [Fact]
    public void OnFailure_Func_No_Arguments_Does_Nothing_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;

        // Act
        var result = sut.OnFailure(() => Result.Continue().Then(() => error = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        error.Should().BeFalse();
    }

    [Fact]
    public void OnFailure_Func_No_Arguments_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;

        // Act
        var result = sut.OnFailure(() => Result.Invalid().Then(() => error = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        error.Should().BeTrue();
    }

    [Fact]
    public void OnFailure_Func_With_Arguments_Does_Nothing_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;

        // Act
        var result = sut.OnFailure(_ => Result.Continue().Then(() => error = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        error.Should().BeFalse();
    }

    [Fact]
    public void OnFailure_Func_With_Arguments_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;

        // Act
        var result = sut.OnFailure(_ => Result.Invalid().Then(() => error = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        error.Should().BeTrue();
    }

    [Fact]
    public async Task OnFailure_Func_No_Arguments_Async_Does_Nothing_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;

        // Act
        var result = await sut.OnFailure(() => Task.FromResult(Result.Continue().Then(() => error = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        error.Should().BeFalse();
    }

    [Fact]
    public async Task OnFailure_Func_No_Arguments_Async_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;

        // Act
        var result = await sut.OnFailure(() => Task.FromResult(Result.Invalid().Then(() => error = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        error.Should().BeTrue();
    }

    [Fact]
    public async Task OnFailure_Func_With_Arguments_Async_Does_Nothing_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var error = false;

        // Act
        var result = await sut.OnFailure(_ => Task.FromResult(Result.Continue().Then(() => error = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        error.Should().BeFalse();
    }

    [Fact]
    public async Task OnFailure_Func_With_Arguments_Async_Runs_Failure_Action_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var error = false;

        // Act
        var result = await sut.OnFailure(_ => Task.FromResult(Result.Invalid().Then(() => error = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
        error.Should().BeTrue();
    }

    [Fact]
    public void OnSuccess_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var success = false;

        // Act
        var result = sut.OnSuccess(_ => success = true);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        success.Should().BeTrue();
    }

    [Fact]
    public void OnSuccess_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var success = false;

        // Act
        var result = sut.OnSuccess(_ => success = true);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        success.Should().BeFalse();
    }

    [Fact]
    public void OnSuccess_Parameterless_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var success = false;

        // Act
        var result = sut.OnSuccess(() => success = true);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        success.Should().BeTrue();
    }

    [Fact]
    public void OnSuccess_Parameterless_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var success = false;

        // Act
        var result = sut.OnSuccess(() => success = true);

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        success.Should().BeFalse();
    }

    [Fact]
    public void OnSuccess_Func_No_Arguments_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var success = false;

        // Act
        var result = sut.OnSuccess(() => Result.Continue().Then(() => success = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
        success.Should().BeTrue();
    }

    [Fact]
    public void OnSuccess_Func_No_Arguments_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var success = false;

        // Act
        var result = sut.OnSuccess(() => Result.Continue().Then(() => success = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        success.Should().BeFalse();
    }

    [Fact]
    public void OnSuccess_Func_With_Arguments_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var success = false;

        // Act
        var result = sut.OnSuccess(_ => Result.Continue().Then(() => success = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
        success.Should().BeTrue();
    }

    [Fact]
    public void OnSuccess_Func_With_Arguments_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var success = false;

        // Act
        var result = sut.OnSuccess(_ => Result.Continue().Then(() => success = true));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        success.Should().BeFalse();
    }

    [Fact]
    public async Task OnSuccess_Func_No_Arguments_Async_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var success = false;

        // Act
        var result = await sut.OnSuccess(() => Task.FromResult(Result.Continue().Then(() => success = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
        success.Should().BeTrue();
    }

    [Fact]
    public async Task OnSuccess_Func_No_Arguments_Async_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var success = false;

        // Act
        var result = await sut.OnSuccess(() => Task.FromResult(Result.Continue().Then(() => success = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        success.Should().BeFalse();
    }

    [Fact]
    public async Task OnSuccess_Func_With_Arguments_Async_Runs_Success_Action_On_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Success();
        var success = false;

        // Act
        var result = await sut.OnSuccess(_ => Task.FromResult(Result.Continue().Then(() => success = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
        success.Should().BeTrue();
    }

    [Fact]
    public async Task OnSuccess_Func_With_Arguments_Async_Does_Nothing_On_Non_Successful_Result_No_Success_Delegate()
    {
        // Arrange
        var sut = Result.Error("Kaboom");
        var success = false;

        // Act
        var result = await sut.OnSuccess(_ => Task.FromResult(Result.Continue().Then(() => success = true)));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        success.Should().BeFalse();
    }
}
