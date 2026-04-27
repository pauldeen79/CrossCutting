namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringStartsWithConditionBuilderTests : TestBase<StringStartsWithConditionBuilder>
{
    public class BuildTyped : StringStartsWithConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<StringStartsWithCondition>();
        }
    }
}