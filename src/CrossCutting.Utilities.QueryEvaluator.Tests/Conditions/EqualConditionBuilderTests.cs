namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class EqualConditionBuilderTests : TestBase<EqualConditionBuilder>
{
    public class BuildTyped : EqualConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<EqualCondition>();
        }
    }
}