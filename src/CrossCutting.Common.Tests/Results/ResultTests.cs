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
        actual.ShouldBe(sut.Value);
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Result_Is_Invalid()
    {
        // Arrange
        var sut = Result.Invalid<string>();

        // Act
        var act = new Action(() => _ = sut.GetValueOrThrow());

        // Assert
        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("Result: Invalid");
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Result_Is_Error_And_ErrorMessage_Is_Filled()
    {
        // Arrange
        var sut = Result.Error<string>("Kaboom");

        // Act
        var act = new Action(() => _ = sut.GetValueOrThrow());

        // Assert
        act.ShouldThrow<InvalidOperationException>().Message.ShouldBe("Result: Error, ErrorMessage: Kaboom");
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_InvalidResult()
    {
        // Arrange
        var voidResult = Result.Invalid("Failed", [new ValidationError("v1", ["m1"])]);

        // Act
        var actual = Result.FromExistingResult<string>(voidResult);

        // Assert
        actual.ErrorMessage.ShouldBe(voidResult.ErrorMessage);
        actual.Status.ShouldBe(voidResult.Status);
        actual.ValidationErrors.ShouldBeEquivalentTo(voidResult.ValidationErrors);
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_NotFoundResult()
    {
        // Arrange
        var voidResult = Result.NotFound("Failed");

        // Act
        var actual = Result.FromExistingResult<string>(voidResult);

        // Assert
        actual.ErrorMessage.ShouldBe(voidResult.ErrorMessage);
        actual.Status.ShouldBe(voidResult.Status);
        actual.ValidationErrors.ShouldBeEquivalentTo(voidResult.ValidationErrors);
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_ErrorResult()
    {
        // Arrange
        var voidResult = Result.Error("Failed");

        // Act
        var actual = Result.FromExistingResult<string>(voidResult);

        // Assert
        actual.ErrorMessage.ShouldBe(voidResult.ErrorMessage);
        actual.Status.ShouldBe(voidResult.Status);
        actual.ValidationErrors.ShouldBeEquivalentTo(voidResult.ValidationErrors);
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_SuccessResult()
    {
        // Arrange
        var voidResult = Result.Success("success value");

        // Act
        var actual = Result.FromExistingResult<string>(voidResult);

        // Assert
        actual.ErrorMessage.ShouldBe(voidResult.ErrorMessage);
        actual.Status.ShouldBe(voidResult.Status);
        actual.ValidationErrors.ShouldBeEquivalentTo(voidResult.ValidationErrors);
        actual.Value.ShouldBe("success value");
    }

    [Fact]
    public void FromExistingResult_Copies_Values_From_Void_SuccessResult_And_Adds_Value()
    {
        // Arrange
        var voidResult = Result.Success();

        // Act
        var actual = Result.FromExistingResult(voidResult, "yes");

        // Assert
        actual.ErrorMessage.ShouldBe(voidResult.ErrorMessage);
        actual.Status.ShouldBe(voidResult.Status);
        actual.ValidationErrors.ShouldBeEquivalentTo(voidResult.ValidationErrors);
        actual.Value.ShouldBe("yes");
    }

    [Fact]
    public void FromExistingResult_Copies_Error_From_Source_When_Not_Successful()
    {
        // Arrange
        var source = Result.Error<bool>("Kaboom");

        // Act
        var actual = Result.FromExistingResult<bool, object?>(source, x => x);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.Value.ShouldBe(default(bool));
    }

    [Fact]
    public void FromExistingResult_Copies_Value_From_Source_When_Successful()
    {
        // Arrange
        var source = Result.Success(true);

        // Act
        var actual = Result.FromExistingResult<bool, object?>(source, x => x);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Ok);
        actual.Value.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public void Can_Create_Success_With_Correct_Value_Using_ReferenceType()
    {
        // Act
        var actual = Result.Success("test");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Ok);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBe("test");
    }

    [Fact]
    public void Can_Create_Success_With_Correct_Value_Using_ValueType()
    {
        // Act
        var actual = Result.Success<(string Name, string Address)>(("name", "address"));

        // Assert
        actual.Status.ShouldBe(ResultStatus.Ok);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.Name.ShouldBe("name");
        actual.Value.Address.ShouldBe("address");
    }

    [Fact]
    public void Can_Create_Success_Result_From_NonNull_Instance_Without_ErrorMesssage_Provided()
    {
        // Act
        var actual = Result.FromInstance(this);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Ok);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeSameAs(this);
    }

    [Fact]
    public void Can_Create_Success_Result_From_NonNull_Instance_With_ErrorMessage_Provided()
    {
        // Act
        var actual = Result.FromInstance(this, "This gets ignored because the instance is not null");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Ok);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeSameAs(this);
    }

    [Fact]
    public void Can_Create_Success_Result_From_NonNull_Instance_With_ValidationErrors_Provided()
    {
        // Act
        var actual = Result.FromInstance(this, [new ValidationError("Ignored", ["Member1"])]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Ok);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeSameAs(this);
    }

    [Fact]
    public void Can_Create_Success_Result_From_NonNull_Instance_With_ErrorMessage_And_ValidationErrors_Provided()
    {
        // Act
        var actual = Result.FromInstance(this, "This gets ignored because the instance is not null", [new ValidationError("Ignored", ["Member1"])]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Ok);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeSameAs(this);
    }

    [Fact]
    public void Can_Create_Invalid_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid<string>("Error");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ErrorMessage_And_InnerResults()
    {
        // Act
        var actual = Result.Invalid<string>("Error", [Result.Error("Kaboom")]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.InnerResults.Count.ShouldBe(1);
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid("Error");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ErrorMessage_And_InnerResults()
    {
        // Act
        var actual = Result.Invalid("Error", [Result.Error("Kaboom")]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.InnerResults.Count.ShouldBe(1);
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ValidationErrors()
    {
        // Act
        var actual = Result.Invalid<string>([new ValidationError("x", ["m1", "m2"])]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ToArray().ShouldBeEquivalentTo(new[] { new ValidationError("x", ["m1", "m2"]) });
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ValidationErrors()
    {
        // Act
        var actual = Result.Invalid([new ValidationError("x", ["m1", "m2"])]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ToArray().ShouldBeEquivalentTo(new[] { new ValidationError("x", ["m1", "m2"]) });
    }

    [Fact]
    public void Can_Create_Invalid_Result_With_ValidationErrors_And_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid<string>("Error", [new ValidationError("x", ["m1", "m2"])]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ToArray().ShouldBeEquivalentTo(new[] { new ValidationError("x", ["m1", "m2"]) });
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Void_Result_With_ValidationErrors_And_ErrorMessage()
    {
        // Act
        var actual = Result.Invalid("Error", [new ValidationError("x", ["m1", "m2"])]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ToArray().ShouldBeEquivalentTo(new[] { new ValidationError("x", ["m1", "m2"]) });
    }

    [Fact]
    public void Can_Create_Invalid_Result_From_Null_Instance_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null, [new ValidationError("Error", ["Name"])]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.Count.ShouldBe(1);
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Invalid_Result_From_Null_Instance_With_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null, "My error message", [new ValidationError("Error", ["Name"])]);

        // Assert
        actual.Status.ShouldBe(ResultStatus.Invalid);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("My error message");
        actual.ValidationErrors.Count.ShouldBe(1);
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Error_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Error<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Error_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Error();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Error_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Error<string>("Error");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Error_Result_With_ErrorMessage_And_Exception()
    {
        // Act
        var actual = Result.Error<string>(new InvalidOperationException("Kaboom"), "Error");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
        actual.Exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe("Kaboom");
    }

    [Fact]
    public void Can_Create_Error_Result_With_ErrorMessage_And_InnerResults()
    {
        // Act
        var actual = Result.Error<string>([Result.Error("InnerError")], "Error");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
        actual.Exception.ShouldBeNull();
        actual.InnerResults.Count.ShouldBe(1);
    }

    [Fact]
    public void Can_Create_Error_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Error("Error");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Error_Void_Result_With_ErrorMessage_And_Exception()
    {
        // Act
        var actual = Result.Error(new InvalidOperationException("Kaboom"), "Error");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe("Kaboom");
    }

    [Fact]
    public void Can_Create_Error_Void_Result_With_ErrorMessage_And_InnerResults()
    {
        // Act
        var actual = Result.Error([Result.Error("InnerError")], "Error");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Error);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Error");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Exception.ShouldBeNull();
        actual.InnerResults.Count.ShouldBe(1);
    }

    [Fact]
    public void Can_Create_NotFound_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotFound<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotFound);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotFound_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotFound();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotFound);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotFound_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotFound<string>("NotFound");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotFound);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("NotFound");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotFound_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotFound("NotFound");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotFound);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("NotFound");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotFound_Result_From_Null_Instance_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null);

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotFound);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotFound_Result_From_Null_Instance_With_ErrorMessage()
    {
        // Act
        var actual = Result.FromInstance<ResultTests>(null, "My error message");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotFound);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("My error message");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Unauthorized_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Unauthorized<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Unauthorized);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Unauthorized_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Unauthorized();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Unauthorized);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Unauthorized_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Unauthorized<string>("Not authorized");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Unauthorized);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not authorized");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Unauthorized_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Unauthorized("Not authorized");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Unauthorized);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not authorized");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotAuthenticated_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotAuthenticated<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotAuthenticated);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotAuthenticated_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotAuthenticated();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotAuthenticated);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotAuthenticated_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotAuthenticated<string>("Not authenticated");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotAuthenticated);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not authenticated");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotAuthenticated_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotAuthenticated("Not authenticated");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotAuthenticated);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not authenticated");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotSupported_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotSupported<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotSupported);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotSupported_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotSupported();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotSupported);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotSupported_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotSupported<string>("Not supported");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotSupported);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not supported");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotSupported_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotSupported("Not supported");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotSupported);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not supported");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Unavailable_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Unavailable<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Unavailable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Unavailable_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Unavailable();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Unavailable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Unavailable_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Unavailable<string>("Not available");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Unavailable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not available");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Unavailable_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Unavailable("Not available");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Unavailable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not available");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotImplemented_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotImplemented<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotImplemented);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotImplemented_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotImplemented();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotImplemented);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotImplemented_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotImplemented<string>("Not implemented");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotImplemented);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not implemented");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotImplemented_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotImplemented("Not implemented");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotImplemented);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Not implemented");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NoContent_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NoContent<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NoContent);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NoContent_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NoContent();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NoContent);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NoContent_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NoContent<string>("No content");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NoContent);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBe("No content");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NoContent_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NoContent("No content");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NoContent);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBe("No content");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_ResetContent_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.ResetContent<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.ResetContent);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_ResetContent_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.ResetContent();

        // Assert
        actual.Status.ShouldBe(ResultStatus.ResetContent);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_ResetContent_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.ResetContent<string>("Reset");

        // Assert
        actual.Status.ShouldBe(ResultStatus.ResetContent);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Reset");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_ResetContent_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.ResetContent("Reset");

        // Assert
        actual.Status.ShouldBe(ResultStatus.ResetContent);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Reset");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Continue_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Continue<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Continue);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Continue_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Continue();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Continue);
        actual.IsSuccessful().ShouldBeTrue();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Conflict_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Conflict<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Conflict);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Conflict_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Conflict();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Conflict);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Conflict_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Conflict<string>("There is a huge conflict");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Conflict);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("There is a huge conflict");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Conflict_Result_With_ErrorMessage_And_InnerResult()
    {
        // Act
        var actual = Result.Conflict<string>([Result.Error("Kaboom")], "There is a huge conflict");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Conflict);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("There is a huge conflict");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
        actual.InnerResults.Count.ShouldBe(1);
    }

    [Fact]
    public void Can_Create_Conflict_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Conflict("There is a huge conflict");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Conflict);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("There is a huge conflict");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Conflict_Void_Result_With_ErrorMessage_And_InnerResult()
    {
        // Act
        var actual = Result.Conflict([Result.Error("Kaboom")], "There is a huge conflict");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Conflict);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("There is a huge conflict");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.InnerResults.Count.ShouldBe(1);
    }

    [Fact]
    public void Can_Create_Redirect_Result()
    {
        // Act
        var actual = Result.Redirect("redirect address");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Redirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBe("redirect address");
    }

    [Fact]
    public void Can_Create_Created_Result_With_Value()
    {
        // Act
        var result = Result.Created("Some value");

        // Assert
        result.Status.ShouldBe(ResultStatus.Created);
        result.Value.ShouldBe("Some value");
    }

    [Fact]
    public void Can_Create_Created_Result_Without_Value()
    {
        // Act
        var result = Result.Created();

        // Assert
        result.Status.ShouldBe(ResultStatus.Created);
        result.GetValue().ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Accepted_Result_With_Value()
    {
        // Act
        var result = Result.Accepted("Some value");

        // Assert
        result.Status.ShouldBe(ResultStatus.Accepted);
        result.Value.ShouldBe("Some value");
    }

    [Fact]
    public void Can_Create_Accepted_Result_Without_Value()
    {
        // Act
        var result = Result.Accepted();

        // Assert
        result.Status.ShouldBe(ResultStatus.Accepted);
        result.GetValue().ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Already_Reported_Result_With_Value()
    {
        // Act
        var result = Result.AlreadyReported("Some value");

        // Assert
        result.Status.ShouldBe(ResultStatus.AlreadyReported);
        result.Value.ShouldBe("Some value");
    }

    [Fact]
    public void Can_Create_Already_Reported_Result_Without_Value()
    {
        // Act
        var result = Result.AlreadyReported();

        // Assert
        result.Status.ShouldBe(ResultStatus.AlreadyReported);
        result.GetValue().ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Found_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Found<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Found);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Found_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Found();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Found);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Found_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Found<string>("Found");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Found);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Found");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Found_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Found("Found");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Found);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Found");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_MovedPermanently_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.MovedPermanently<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.MovedPermanently);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_MovedPermanently_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.MovedPermanently();

        // Assert
        actual.Status.ShouldBe(ResultStatus.MovedPermanently);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_MovedPermanently_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.MovedPermanently<string>("MovedPermanently");

        // Assert
        actual.Status.ShouldBe(ResultStatus.MovedPermanently);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("MovedPermanently");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_MovedPermanently_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.MovedPermanently("MovedPermanently");

        // Assert
        actual.Status.ShouldBe(ResultStatus.MovedPermanently);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("MovedPermanently");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Gone_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Gone<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Gone);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Gone_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Gone();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Gone);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Gone_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Gone<string>("Gone");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Gone);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Gone");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Gone_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Gone("Gone");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Gone);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Gone");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotModified_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotModified<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotModified);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotModified_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotModified();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotModified);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotModified_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotModified<string>("NotModified");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotModified);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("NotModified");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotModified_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotModified("NotModified");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotModified);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("NotModified");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_TemporaryRedirect_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TemporaryRedirect<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.TemporaryRedirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_TemporaryRedirect_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TemporaryRedirect();

        // Assert
        actual.Status.ShouldBe(ResultStatus.TemporaryRedirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_TemporaryRedirect_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TemporaryRedirect<string>("TemporaryRedirect");

        // Assert
        actual.Status.ShouldBe(ResultStatus.TemporaryRedirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("TemporaryRedirect");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_TemporaryRedirect_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TemporaryRedirect("TemporaryRedirect");

        // Assert
        actual.Status.ShouldBe(ResultStatus.TemporaryRedirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("TemporaryRedirect");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_PermanentRedirect_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PermanentRedirect<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.PermanentRedirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_PermanentRedirect_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PermanentRedirect();

        // Assert
        actual.Status.ShouldBe(ResultStatus.PermanentRedirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_PermanentRedirect_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PermanentRedirect<string>("PermanentRedirect");

        // Assert
        actual.Status.ShouldBe(ResultStatus.PermanentRedirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("PermanentRedirect");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_PermanentRedirect_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PermanentRedirect("PermanentRedirect");

        // Assert
        actual.Status.ShouldBe(ResultStatus.PermanentRedirect);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("PermanentRedirect");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Forbidden_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Forbidden<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Forbidden);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Forbidden_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Forbidden();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Forbidden);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Forbidden_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Forbidden<string>("Forbidden");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Forbidden);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Forbidden");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Forbidden_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Forbidden("Forbidden");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Forbidden);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Forbidden");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotAcceptable_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotAcceptable<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotAcceptable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotAcceptable_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.NotAcceptable();

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotAcceptable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_NotAcceptable_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotAcceptable<string>("NotAcceptable");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotAcceptable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("NotAcceptable");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_NotAcceptable_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.NotAcceptable("NotAcceptable");

        // Assert
        actual.Status.ShouldBe(ResultStatus.NotAcceptable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("NotAcceptable");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_TimeOut_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TimeOut<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.TimeOut);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_TimeOut_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TimeOut();

        // Assert
        actual.Status.ShouldBe(ResultStatus.TimeOut);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_TimeOut_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TimeOut<string>("TimeOut");

        // Assert
        actual.Status.ShouldBe(ResultStatus.TimeOut);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("TimeOut");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_TimeOut_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TimeOut("TimeOut");

        // Assert
        actual.Status.ShouldBe(ResultStatus.TimeOut);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("TimeOut");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Locked_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Locked<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Locked);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Locked_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.Locked();

        // Assert
        actual.Status.ShouldBe(ResultStatus.Locked);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_Locked_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Locked<string>("Locked");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Locked);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Locked");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Locked_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.Locked("Locked");

        // Assert
        actual.Status.ShouldBe(ResultStatus.Locked);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("Locked");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_ServiceUnavailable_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.ServiceUnavailable<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.ServiceUnavailable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_ServiceUnavailable_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.ServiceUnavailable();

        // Assert
        actual.Status.ShouldBe(ResultStatus.ServiceUnavailable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_ServiceUnavailable_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.ServiceUnavailable<string>("ServiceUnavailable");

        // Assert
        actual.Status.ShouldBe(ResultStatus.ServiceUnavailable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("ServiceUnavailable");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_ServiceUnavailable_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.ServiceUnavailable("ServiceUnavailable");

        // Assert
        actual.Status.ShouldBe(ResultStatus.ServiceUnavailable);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("ServiceUnavailable");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_GatewayTimeout_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.GatewayTimeout<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.GatewayTimeout);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_GatewayTimeout_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.GatewayTimeout();

        // Assert
        actual.Status.ShouldBe(ResultStatus.GatewayTimeout);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_GatewayTimeout_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.GatewayTimeout<string>("GatewayTimeout");

        // Assert
        actual.Status.ShouldBe(ResultStatus.GatewayTimeout);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("GatewayTimeout");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_GatewayTimeout_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.GatewayTimeout("GatewayTimeout");

        // Assert
        actual.Status.ShouldBe(ResultStatus.GatewayTimeout);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("GatewayTimeout");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_BadGateway_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.BadGateway<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.BadGateway);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_BadGateway_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.BadGateway();

        // Assert
        actual.Status.ShouldBe(ResultStatus.BadGateway);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_BadGateway_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.BadGateway<string>("BadGateway");

        // Assert
        actual.Status.ShouldBe(ResultStatus.BadGateway);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("BadGateway");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_BadGateway_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.BadGateway("BadGateway");

        // Assert
        actual.Status.ShouldBe(ResultStatus.BadGateway);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("BadGateway");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_InsuficientStorage_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.InsuficientStorage<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.InsuficientStorage);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_InsuficientStorage_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.InsuficientStorage();

        // Assert
        actual.Status.ShouldBe(ResultStatus.InsuficientStorage);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_InsuficientStorage_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.InsuficientStorage<string>("InsuficientStorage");

        // Assert
        actual.Status.ShouldBe(ResultStatus.InsuficientStorage);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("InsuficientStorage");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_InsuficientStorage_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.InsuficientStorage("InsuficientStorage");

        // Assert
        actual.Status.ShouldBe(ResultStatus.InsuficientStorage);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("InsuficientStorage");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_UnprocessableContent_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.UnprocessableContent<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.UnprocessableContent);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_UnprocessableContent_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.UnprocessableContent();

        // Assert
        actual.Status.ShouldBe(ResultStatus.UnprocessableContent);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_UnprocessableContent_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.UnprocessableContent<string>("UnprocessableContent");

        // Assert
        actual.Status.ShouldBe(ResultStatus.UnprocessableContent);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("UnprocessableContent");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_UnprocessableContent_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.UnprocessableContent("UnprocessableContent");

        // Assert
        actual.Status.ShouldBe(ResultStatus.UnprocessableContent);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("UnprocessableContent");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_FailedDependency_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FailedDependency<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.FailedDependency);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_FailedDependency_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.FailedDependency();

        // Assert
        actual.Status.ShouldBe(ResultStatus.FailedDependency);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_FailedDependency_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.FailedDependency<string>("FailedDependency");

        // Assert
        actual.Status.ShouldBe(ResultStatus.FailedDependency);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("FailedDependency");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_FailedDependency_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.FailedDependency("FailedDependency");

        // Assert
        actual.Status.ShouldBe(ResultStatus.FailedDependency);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("FailedDependency");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_PreconditionRequired_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionRequired<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.PreconditionRequired);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_PreconditionRequired_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionRequired();

        // Assert
        actual.Status.ShouldBe(ResultStatus.PreconditionRequired);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_PreconditionRequired_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionRequired<string>("PreconditionRequired");

        // Assert
        actual.Status.ShouldBe(ResultStatus.PreconditionRequired);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("PreconditionRequired");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_PreconditionRequired_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionRequired("PreconditionRequired");

        // Assert
        actual.Status.ShouldBe(ResultStatus.PreconditionRequired);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("PreconditionRequired");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_PreconditionFailed_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionFailed<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.PreconditionFailed);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_PreconditionFailed_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionFailed();

        // Assert
        actual.Status.ShouldBe(ResultStatus.PreconditionFailed);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_PreconditionFailed_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionFailed<string>("PreconditionFailed");

        // Assert
        actual.Status.ShouldBe(ResultStatus.PreconditionFailed);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("PreconditionFailed");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_PreconditionFailed_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.PreconditionFailed("PreconditionFailed");

        // Assert
        actual.Status.ShouldBe(ResultStatus.PreconditionFailed);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("PreconditionFailed");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_TooManyRequests_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TooManyRequests<string>();

        // Assert
        actual.Status.ShouldBe(ResultStatus.TooManyRequests);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_TooManyRequests_Void_Result_Without_ErrorMessage()
    {
        // Act
        var actual = Result.TooManyRequests();

        // Assert
        actual.Status.ShouldBe(ResultStatus.TooManyRequests);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBeNull();
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void Can_Create_TooManyRequests_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TooManyRequests<string>("TooManyRequests");

        // Assert
        actual.Status.ShouldBe(ResultStatus.TooManyRequests);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("TooManyRequests");
        actual.ValidationErrors.ShouldBeEmpty();
        actual.Value.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_TooManyRequests_Void_Result_With_ErrorMessage()
    {
        // Act
        var actual = Result.TooManyRequests("TooManyRequests");

        // Assert
        actual.Status.ShouldBe(ResultStatus.TooManyRequests);
        actual.IsSuccessful().ShouldBeFalse();
        actual.ErrorMessage.ShouldBe("TooManyRequests");
        actual.ValidationErrors.ShouldBeEmpty();
    }

    [Fact]
    public void GetValueOrThrow_Throws_When_Value_Is_Null()
    {
        // Arrange
        var sut = Result.Error<string>();

        // Act
        var act = new Action(() => sut.GetValueOrThrow());

        // Assert
        act.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void GetValueOrThrow_Returns_Value_When_Value_Is_Not_Null()
    {
        // Arrange
        var sut = Result.Success("yes!");

        // Act
        var value = sut.GetValueOrThrow();

        // Assert
        value.ShouldBe(sut.Value);
    }

    [Fact]
    public void TryCast_Returns_Invalid_With_Default_ErrorMessage_When_Value_Could_Not_Be_Cast()
    {
        // Arrange
        var sut = Result.Success("test");

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Could not cast System.String to System.Boolean");
    }

    [Fact]
    public void TryCast_Returns_Invalid_With_Custom_ErrorMessage_When_Value_Could_Not_Be_Cast()
    {
        // Arrange
        var sut = Result.Success<object?>("test");

        // Act
        var result = sut.TryCast<bool>("my custom error message");

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("my custom error message");
    }

    [Fact]
    public void TryCast_Returns_Same_Properties_When_Status_Is_Error()
    {
        // Arrange
        var sut = Result.Error<object?>("error");

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.ShouldBe(sut.Status);
        result.ErrorMessage.ShouldBe(sut.ErrorMessage);
    }

    [Fact]
    public void TryCast_Returns_Same_Properties_When_Status_Is_Invalid()
    {
        // Arrange
        var sut = Result.Invalid<object?>("error", [new ValidationError("validation error 1"), new ValidationError("validation error 2")]);

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.ShouldBe(sut.Status);
        result.ErrorMessage.ShouldBe(sut.ErrorMessage);
        result.ValidationErrors.ShouldBeEquivalentTo(sut.ValidationErrors);
    }

    [Fact]
    public void TryCast_Returns_Success_With_Cast_Value_When_Possible()
    {
        // Arrange
        var sut = Result.Success<object?>(true);

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeTrue();
    }

    [Fact]
    public void TryCast_Returns_Success_With_Default_Value_When_Value_Is_Null()
    {
        // Arrange
        var sut = Result.Success<object?>(null);

        // Act
        var result = sut.TryCastAllowNull<bool?>();

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void TryCast_Returns_Same_Status()
    {
        // Arrange
        var sut = Result.Continue<object?>(true);

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.ShouldBe(ResultStatus.Continue);
        result.Value.ShouldBeTrue();
    }

    [Fact]
    public void TryCast_Returns_Invalid_When_AllowNull_Is_False_And_Value_Is_Null()
    {
        // Arrange
        var sut = Result.Continue<object?>(null);

        // Act
        var result = sut.TryCast<bool>();

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
    }

    [Fact]
    public void TryCast_Returns_Success_When_AllowNull_Is_True_And_Value_Is_Null()
    {
        // Arrange
        var sut = Result.Continue<object?>(null);

        // Act
        var result = sut.TryCastAllowNull<bool>("some custom error message");

        // Assert
        result.Status.ShouldBe(sut.Status);
    }

    [Fact]
    public void Can_Create_ValidationResult_With_ErrorMessage_From_ValidationErrors()
    {
        // Arrange
        var validationErrors = new[] { new ValidationError("It's invalid, yo") };

        // Act
        var result = Result.FromValidationErrors(validationErrors, "Kaboom");

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Kaboom");
        result.ValidationErrors.Select(x => x.ErrorMessage).ToArray().ShouldBeEquivalentTo(new[] { "It's invalid, yo" });
    }

    [Fact]
    public void Can_Create_ValidationResult_Without_ErrorMessage_From_ValidationErrors()
    {
        // Arrange
        var validationErrors = new[] { new ValidationError("It's invalid, yo") };

        // Act
        var result = Result.FromValidationErrors(validationErrors);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBeNull();
        result.ValidationErrors.Select(x => x.ErrorMessage).ToArray().ShouldBeEquivalentTo(new[] { "It's invalid, yo" });
    }

    [Fact]
    public void Can_Create_ValidationResult_With_ErrorMessage_From_Empty_ValidationErrors()
    {
        // Arrange
        var validationErrors = Array.Empty<ValidationError>();

        // Act
        var result = Result.FromValidationErrors(validationErrors, "Kaboom");

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ErrorMessage.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_ValidationResult_Without_ErrorMessage_From_EmptyValidationErrors()
    {
        // Arrange
        var validationErrors = Array.Empty<ValidationError>();

        // Act
        var result = Result.FromValidationErrors(validationErrors);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.ErrorMessage.ShouldBeNull();
    }

    [Fact]
    public void Can_Create_Untyped_Result_From_Typed_Result_And_Then_Read_The_Value_Untyped()
    {
        // Arrange
        Result typed = Result.Success(13);

        // Act
        var result = typed.GetValue();

        // Assert
        result.ShouldBeEquivalentTo(13);
    }

    [Fact]
    public void Can_Cast_Any_Typed_Result_To_Result_Of_Object_Using_Implicit_Operator()
    {
        // Arrange
        var sut = Result.Success("Hello");

        // Act
        Result<object?> untyped = sut;

        // Assert
        untyped.Status.ShouldBe(ResultStatus.Ok);
        untyped.Value.ShouldBe("Hello");
    }

    [Fact]
    public void Can_Cast_Any_Typed_Result_To_Result_Of_Object_Using_ToResult_Instance_Method()
    {
        // Arrange
        var sut = Result.Success("Hello");

        // Act
        var untyped = sut.FromResult();

        // Assert
        untyped.Status.ShouldBe(ResultStatus.Ok);
        untyped.Value.ShouldBe("Hello");
    }

    [Fact]
    public void ThrowIfNotSuccessful_Does_Not_Throw_When_Result_Is_Success()
    {
        // Arrange
        var sut = Result.Success("ok");

        // Act & Assert
        Action a = sut.ThrowIfNotSuccessful;
        a.ShouldNotThrow();
    }

    [Fact]
    public void ThrowIfNotSuccessful_Throws_When_Result_Is_Invalid()
    {
        // Arrange
        var sut = Result.Invalid<string>();

        // Act
        var act = new Action(sut.ThrowIfNotSuccessful);

        // Assert
        act.ShouldThrow<InvalidOperationException>()
           .Message.ShouldBe("Result: Invalid");
    }

    [Fact]
    public void ThrowIfNotSuccessful_Throws_When_Result_Is_Error_And_ErrorMessage_Is_Filled()
    {
        // Arrange
        var sut = Result.Error<string>("Kaboom");

        // Act
        var act = new Action(sut.ThrowIfNotSuccessful);

        // Assert
        act.ShouldThrow<InvalidOperationException>()
           .Message.ShouldBe("Result: Error, ErrorMessage: Kaboom");
    }

    [Fact]
    public void Transform_Can_Transform_The_Value_On_Success_With_Value_Delegate()
    {
        // Arrange
        var source = Result.Success(1);

        // Act
        var result = source.Transform(x => x.ToString());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("1");
    }

    [Fact]
    public void Transform_Returns_Same_Error_On_Failure_With_Value_Delegate()
    {
        // Arrange
        var source = Result.Error<int>("Kaboom!");

        // Act
        var result = source.Transform(x => x.ToString());

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom!");
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void Transform_Can_Transform_The_Value_On_Success_With_Typed_Result_Delegate()
    {
        // Arrange
        var source = Result.Success(1);

        // Act
        var result = source.Transform(x => Result.Success(x.ToString()));

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe("1");
    }

    [Fact]
    public void Transform_Returns_Same_Error_On_Failure_With_Typed_Result_Delegate()
    {
        // Arrange
        var source = Result.Error<int>("Kaboom!");

        // Act
        var result = source.Transform(x => Result.Success(x.ToString()));

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom!");
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void Transform_Can_Transform_The_Value_On_Success_With_Untyped_Result_Delegate()
    {
        // Arrange
        var source = Result.Success(1);

        // Act
        var result = source.Transform(_ => Result.Success());

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
    }

    [Fact]
    public void Transform_Returns_Same_Error_On_Failure_With_Untyped_Result_Delegate()
    {
        // Arrange
        var source = Result.Error<int>("Kaboom!");

        // Act
        var result = source.Transform(_ => Result.Success());

        // Assert
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Kaboom!");
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
        result.Status.ShouldBe(ResultStatus.Ok);
        result.InnerResults.ShouldBeEmpty();
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
        result.Status.ShouldBe(ResultStatus.Ok);
        result.InnerResults.ShouldBeEmpty();
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
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Something went wrong. See inner results.");
        result.InnerResults.Count.ShouldBe(1);
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
        result.Status.ShouldBe(ResultStatus.Error);
        result.ErrorMessage.ShouldBe("Something went wrong. See inner results.");
        result.InnerResults.Count.ShouldBe(2);
    }

    [Fact]
    public void TryCastValueAs_Returns_Cast_Value_When_Cast_Is_Possible()
    {
        // Arrange
        Result sut = Result.Success("Hello world!");

        // Act
        var value = sut.TryCastValueAs<string>();

        // Assert
        value.ShouldBe("Hello world!");
    }

    [Fact]
    public void TryCastValueAs_Returns_Default_Value_From_ValueType_When_Cast_Is_Not_Possible()
    {
        // Arrange
        Result sut = Result.Success("Hello world!");

        // Act
        var value = sut.TryCastValueAs<int>();

        // Assert
        value.ShouldBe(0);
    }

    [Fact]
    public void TryCastValueAs_Returns_Default_Value_From_Nullable_ValueType_When_Cast_Is_Not_Possible()
    {
        // Arrange
        Result sut = Result.Success("Hello world!");

        // Act
        var value = sut.TryCastValueAs<int?>();

        // Assert
        value.ShouldBeNull();
    }

    [Fact]
    public void TryCastValueAs_Returns_Provided_Default_Value_From_Nullable_ValueType_When_Cast_Is_Not_Possible()
    {
        // Arrange
        Result sut = Result.Success("Hello world!");

        // Act
        var value = sut.TryCastValueAs<int?>(13);

        // Assert
        value.ShouldBe(13);
    }

    [Fact]
    public void CastValueAs_Returns_Cast_Value_When_Cast_Is_Possible()
    {
        // Arrange
        Result sut = Result.Success("Hello world!");

        // Act
        var value = sut.CastValueAs<string>();

        // Assert
        value.ShouldBe("Hello world!");
    }

    [Fact]
    public void CastValueAs_Throws_When_Cast_Is_Not_Possible()
    {
        // Arrange
        Result sut = Result.Success("Hello world!");

        // Act
        Action a = () => sut.CastValueAs<int>();
        a.ShouldThrow<InvalidCastException>();
    }

    [Fact]
    public void CastValueAs_Throws_When_Value_Is_Null()
    {
        // Arrange
        Result sut = Result.Success<string?>(default);

        // Act & Assert
        Action a = () => sut.CastValueAs<int>();
        a.ShouldThrow<InvalidOperationException>();
    }

    [Fact]
    public void CastValueAs_Does_Not_Throw_When_Value_Is_Null_But_Type_Is_Nullable_ValueType()
    {
        // Arrange
        Result sut = Result.Success<string?>(default);

        // Act & Assert
        Action a = () => sut.CastValueAs<int?>();
        a.ShouldNotThrow();
    }

    [Fact]
    public void CastValueAs_Throws_When_Value_Is_Null_But_Type_Is_Nullable_ReferenceType()
    {
        // Arrange
        var sut = Result.Success<string>(default!);

        // Act & Assert
        Action a = () => sut.CastValueAs<string?>();
        a.ShouldThrow<InvalidOperationException>();

        // Note that if you don't know if the value is null, you can simply use TryCastAllowNull<string>
    }

    public class WrapException_Func : ResultTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var resultDelegate = new Func<Result>(Result.Success);

            // Act
            var result = Result.WrapException(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Exception.ShouldBeNull();
        }

        [Fact]
        public void Returns_Error_When_Exception_Occurs()
        {
            // Arrange
            var resultDelegate = new Func<Result>(() => throw new InvalidOperationException("Kaboom"));

            // Act
            var result = Result.WrapException(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
        }
    }

    public class WrapExceptionAsync_Func : ResultTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var resultDelegate = new Func<Task<Result>>(() => Task.FromResult(Result.Success()));

            // Act
            var result = await Result.WrapExceptionAsync(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Exception.ShouldBeNull();
        }

        [Fact]
        public async Task Returns_Error_When_Exception_Occurs()
        {
            // Arrange
            var resultDelegate = new Func<Task<Result>>(() => Task.FromException<Result>(new InvalidOperationException("Kaboom")));

            // Act
            var result = await Result.WrapExceptionAsync(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
        }
    }

    public class WrapException_Typed_Func_Result : ResultTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var resultDelegate = new Func<Result<string>>(Result.NoContent<string>);

            // Act
            var result = Result.WrapException(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
            result.Exception.ShouldBeNull();
        }

        [Fact]
        public void Returns_Error_When_Exception_Occurs()
        {
            // Arrange
            var resultDelegate = new Func<Result<string>>(() => throw new InvalidOperationException("Kaboom"));

            // Act
            var result = Result.WrapException(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
        }
    }

    public class WrapException_Typed_Func : ResultTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var resultDelegate = new Func<string>(() => string.Empty);

            // Act
            var result = Result.WrapException(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Exception.ShouldBeNull();
        }

        [Fact]
        public void Returns_Error_When_Exception_Occurs()
        {
            // Arrange
            var resultDelegate = new Func<string>(() => throw new InvalidOperationException("Kaboom"));

            // Act
            var result = Result.WrapException(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
        }
    }

    public class WrapExceptionAsync_Typed_Func_Result : ResultTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var resultDelegate = new Func<Task<Result<string>>>(() => Task.FromResult(Result.NoContent<string>()));

            // Act
            var result = await Result.WrapExceptionAsync(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
            result.Exception.ShouldBeNull();
        }

        [Fact]
        public async Task Returns_Error_When_Exception_Occurs()
        {
            // Arrange
            var resultDelegate = new Func<Task<Result<string>>>(() => Task.FromException<Result<string>>(new InvalidOperationException("Kaboom")));

            // Act
            var result = await Result.WrapExceptionAsync(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
        }
    }

    public class WrapExceptionAsync_Typed_Func : ResultTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var resultDelegate = new Func<Task<string>>(() => Task.FromResult(string.Empty));

            // Act
            var result = await Result.WrapExceptionAsync(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Exception.ShouldBeNull();
        }

        [Fact]
        public async Task Returns_Error_When_Exception_Occurs()
        {
            // Arrange
            var resultDelegate = new Func<Task<string>>(() => Task.FromException<string>(new InvalidOperationException("Kaboom")));

            // Act
            var result = await Result.WrapExceptionAsync(resultDelegate);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
        }
    }
}
