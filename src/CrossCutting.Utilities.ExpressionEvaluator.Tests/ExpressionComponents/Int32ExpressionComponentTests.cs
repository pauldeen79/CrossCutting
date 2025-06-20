﻿namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class Int32ExpressionComponentTests : TestBase<Int32ExpressionComponent>
{
    public class EvaluateAsync : Int32ExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Correct_Result_For_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Negative_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("-13");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(-13);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Forced_Positive_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("+13");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(13);
        }
    }

    public class ParseAsync : Int32ExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Correct_Result_For_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("13");

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Negative_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("-13");

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Correct_Result_For_Forced_Positive_Int32()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("+13");

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }
    }
}
