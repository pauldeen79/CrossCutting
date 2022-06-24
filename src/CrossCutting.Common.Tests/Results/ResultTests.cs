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
    public void Can_Create_Success_Result_From_NonNull_Instance()
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
}
