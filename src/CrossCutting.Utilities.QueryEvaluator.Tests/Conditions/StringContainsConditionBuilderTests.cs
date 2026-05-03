namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringContainsConditionBuilderTests : TestBase<StringContainsConditionBuilder>
{
    public class BuildTyped : StringContainsConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<StringContainsCondition>();
        }
    }
}