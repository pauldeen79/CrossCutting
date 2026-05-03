namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringNotEqualsConditionBuilderTests : TestBase<StringNotEqualsConditionBuilder>
{
    public class BuildTyped : StringNotEqualsConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<StringNotEqualsCondition>();
        }
    }
}