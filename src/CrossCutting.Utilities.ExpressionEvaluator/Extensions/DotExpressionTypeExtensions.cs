namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class DotExpressionTypeExtensions
{
    public static MemberType ToMemberType(this DotExpressionType dotExpressionType)
        => dotExpressionType switch
        {
            DotExpressionType.Method => MemberType.Method,
            DotExpressionType.Property => MemberType.Property,
            _ => MemberType.Unknown
        };
}
