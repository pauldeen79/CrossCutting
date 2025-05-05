namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Extensions;

public class DotExpressionTypeExtensionsTests
{
    public class ToMemberType : DotExpressionTypeExtensionsTests
    {
        [Theory]
        [InlineData(DotExpressionType.Method, MemberType.Method)]
        [InlineData(DotExpressionType.Property, MemberType.Property)]
        [InlineData(DotExpressionType.Unknown, MemberType.Unknown)]
        public void Returns_Correct_Result(DotExpressionType input, MemberType expectedResult)
        {
            // Act
            var result = input.ToMemberType();

            // Assert
            result.ShouldBe(expectedResult);
        }
    }
}
