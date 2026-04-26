using CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions;

namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class SmallerThanOrEqualConditionBuilderTests : TestBase<SmallerThanOrEqualConditionBuilder>
{
    public class BuildTyped : SmallerThanOrEqualConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = CreateSut();

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<SmallerThanOrEqualCondition>();
        }
    }
}