namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class GreaterThanConditionBuilderTests : TestBase<GreaterThanConditionBuilder>
{
    public class BuildTyped : GreaterThanConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<GreaterThanCondition>();
        }
    }
}