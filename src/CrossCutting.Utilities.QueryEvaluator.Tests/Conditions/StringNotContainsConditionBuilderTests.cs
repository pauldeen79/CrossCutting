namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringNotContainsConditionBuilderTests : TestBase<StringNotContainsConditionBuilder>
{
    public class BuildTyped : StringNotContainsConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<StringNotContainsCondition>();
        }
    }
}