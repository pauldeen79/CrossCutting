namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NotNullConditionBuilderTests : TestBase<NotNullConditionBuilder>
{
    public class BuildTyped : NotNullConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<NotNullCondition>();
        }
    }
}