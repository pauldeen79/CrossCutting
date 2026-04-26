using CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions;

namespace CrossCutting.Utilities.QueryEvaluator.Tests.Conditions;

public class NotInConditionBuilderTests : TestBase<NotInConditionBuilder>
{
    public class BuildTyped : NotInConditionBuilderTests
    {
        [Fact]
        public void Returns_Correct_Condition()
        {
            // Arrange
            IEvaluatableBuilder<bool> sut = new NotInConditionBuilder().WithSourceExpression(new EmptyEvaluatableBuilder());

            // Act
            var actual = sut.BuildTyped();

            // Assert
            actual.ShouldBeOfType<NotInCondition>();
        }
    }
}