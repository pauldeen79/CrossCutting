﻿namespace CrossCutting.Common.Tests.Results;

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
        var act = new Action(() => sut.ThrowIfInvalid());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Result: Invalid");
    }

    [Fact]
    public void ThrowIfInvalid_Throws_When_Result_Is_Error_And_ErrorMessage_Is_Filled()
    {
        // Arrange
        var sut = Result.Error<string>("Kaboom");

        // Act
        var act = new Action(() => sut.ThrowIfInvalid());

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>().WithMessage("Result: Error, ErrorMessage: Kaboom");
    }

    [Fact]
    public void TransformValue_Can_TransformValue_The_Value_On_Success()
    {
        // Arrange
        var source = Result.Success(1);

        // Act
        var result = source.TransformValue(x => x.ToString());

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be("1");
    }

    [Fact]
    public void TransformValue_Returns_Same_Error_On_Failure()
    {
        // Arrange
        var source = Result.Error<int>("Kaboom!");

        // Act
        var result = source.TransformValue(x => x.ToString());

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Kaboom!");
        result.Value.Should().BeNull();
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
}
