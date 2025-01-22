namespace CrossCutting.Utilities.Parsers;

internal class FunctionAndTypeDescriptor
{
    public FunctionAndTypeDescriptor(IFunction function, Type? returnValueType)
    {
        ArgumentGuard.IsNotNull(function, nameof(function));

        ReturnValueType = returnValueType;
        Function = function;
    }

    public Type? ReturnValueType { get; }
    public IFunction Function { get; }
}
