namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionAndTypeDescriptor
{
    public FunctionAndTypeDescriptor(IMember? member, Type? returnValueType)
    {
        ReturnValueType = returnValueType;
        Member = member;
    }

    public Type? ReturnValueType { get; }
    public IMember? Member { get; }
}
