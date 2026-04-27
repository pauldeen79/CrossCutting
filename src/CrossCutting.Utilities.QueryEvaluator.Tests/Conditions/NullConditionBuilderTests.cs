namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NullConditionBuilderTests : TestBase<NullConditionBuilder>
{
    public class BuildTyped : NullConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<NullCondition>();
        }
    }
}