namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringNotEndsWithConditionBuilderTests : TestBase<StringNotEndsWithConditionBuilder>
{
    public class BuildTyped : StringNotEndsWithConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<StringNotEndsWithCondition>();
        }
    }
}