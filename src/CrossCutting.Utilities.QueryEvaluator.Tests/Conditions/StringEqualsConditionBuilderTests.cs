namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringEqualsConditionBuilderTests : TestBase<StringEqualsConditionBuilder>
{
    public class BuildTyped : StringEqualsConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<StringEqualsCondition>();
        }
    }
}