﻿namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.InstanceConstructors;

public class DateConstructorTests : TestBase<DateConstructor>
{
    public class EvaluateAsync : DateConstructorTests
    {
        [Fact]
        public async Task Returns_Success_On_Correct_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("Date").WithMemberType(MemberType.Constructor).AddArguments("2025", "1", "1");
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Unspecified));
        }

        [Fact]
        public async Task Returns_Error_When_Date_Could_Not_Be_Created_Due_To_Argument_Values()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("Date").WithMemberType(MemberType.Constructor).AddArguments("2025", "13", "1"); // month 13 is not okay!
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
            result.Exception.Message.ShouldBe("Year, Month, and Day parameters describe an un-representable DateTime.");
        }
    }
}
