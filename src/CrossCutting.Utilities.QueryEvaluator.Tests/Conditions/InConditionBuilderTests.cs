using CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions;

namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class InConditionBuilderTests : TestBase<InConditionBuilder>
{
    public class BuildTyped : InConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = new InConditionBuilder().WithSourceExpression(new EmptyEvaluatableBuilder());

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<InCondition>();
        }
    }
}