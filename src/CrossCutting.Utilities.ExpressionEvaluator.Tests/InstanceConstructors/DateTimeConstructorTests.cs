namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.InstanceConstructors;

public class DateTimeConstructorTests : TestBase<DateTimeConstructor>
{
    public class Evaluate : DateTimeConstructorTests
    {
        [Fact]
        public async Task Returns_Success_On_Correct_Arguments()
        {
            // Arrange
            var sut = CreateSut();
            var functionCall = new FunctionCallBuilder().WithName("DateTime").WithMemberType(MemberType.Constructor).AddArguments("2025", "1", "1", "0", "0", "0");
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
            var functionCall = new FunctionCallBuilder().WithName("DateTime").WithMemberType(MemberType.Constructor).AddArguments("2025", "1", "1", "25", "0", "0"); // hour 25 is not okay!
            var context = new FunctionCallContext(functionCall, CreateContext("Dummy"));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
            result.Exception.Message.ShouldBe("Hour, Minute, and Second parameters describe an un-representable DateTime.");
        }
    }
}
