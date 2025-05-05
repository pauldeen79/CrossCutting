namespace CrossCutting.Utilities.ExpressionEvaluator;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class MemberResultAttribute : Attribute
{
    public ResultStatus Status { get; }
    public string Value { get; }
    public Type? ValueType { get; }
    public string Description { get; }

    public MemberResultAttribute(ResultStatus status, Type valueType, string value, string description)
    {
        ArgumentGuard.IsNotNull(valueType, nameof(valueType));
        ArgumentGuard.IsNotNull(value, nameof(value));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Status = status;
        ValueType = valueType;
        Value = value;
        Description = description;
    }

    public MemberResultAttribute(ResultStatus status, string description)
    {
        ArgumentGuard.IsNotNull(description, nameof(description));

        Status = status;
        Value = string.Empty;
        ValueType = null;
        Description = description;
    }

    public MemberResultAttribute(ResultStatus status)
    {
        Status = status;
        Value = string.Empty;
        ValueType = null;
        Description = string.Empty;
    }
}
