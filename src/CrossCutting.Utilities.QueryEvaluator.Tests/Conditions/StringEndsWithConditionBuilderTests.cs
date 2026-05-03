namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringEndsWithConditionBuilderTests : TestBase<StringEndsWithConditionBuilder>
{
    public class BuildTyped : StringEndsWithConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<StringEndsWithCondition>();
        }
    }
}