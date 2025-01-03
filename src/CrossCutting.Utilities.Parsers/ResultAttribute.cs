namespace CrossCutting.Utilities.Parsers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ResultAttribute : Attribute
{
    public ResultStatus Status { get; }
    public string Value { get; }
    public string ValueType { get; }
    public string Description { get; }

    public ResultAttribute(ResultStatus status, string valueType, string value, string description)
    {
        ArgumentGuard.IsNotNull(valueType, nameof(valueType));
        ArgumentGuard.IsNotNull(value, nameof(value));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Status = status;
        ValueType = valueType;
        Value = value;
        Description = description;
    }

    public ResultAttribute(ResultStatus status, Type valueType, string value, string description)
    {
        valueType = ArgumentGuard.IsNotNull(valueType, nameof(valueType));
        ArgumentGuard.IsNotNull(value, nameof(value));
        ArgumentGuard.IsNotNull(description, nameof(description));

        Status = status;
        ValueType = valueType.FullName;
        Value = value;
        Description = description;
    }

    public ResultAttribute(ResultStatus status, string description)
    {
        ArgumentGuard.IsNotNull(description, nameof(description));

        Status = status;
        Value = string.Empty;
        ValueType = string.Empty;
        Description = description;
    }

    public ResultAttribute(ResultStatus status)
    {
        Status = status;
        Value = string.Empty;
        ValueType = string.Empty;
        Description = string.Empty;
    }
}
