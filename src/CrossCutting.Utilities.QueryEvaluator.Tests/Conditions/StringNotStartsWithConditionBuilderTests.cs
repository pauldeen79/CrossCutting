namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class StringNotStartsWithConditionBuilderTests : TestBase<StringNotStartsWithConditionBuilder>
{
    public class BuildTyped : StringNotStartsWithConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<StringNotStartsWithCondition>();
        }
    }
}