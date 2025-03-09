namespace CrossCutting.Utilities.Parsers;

internal sealed class FunctionAndTypeDescriptor
{
    public FunctionAndTypeDescriptor(IFunction? function, IGenericFunction? genericFunction, Type? returnValueType)
    {
        // Null check not necessary because this class is internal
        ReturnValueType = returnValueType;
        Function = function;
        GenericFunction = genericFunction;
    }

    public Type? ReturnValueType { get; }
    public IFunction? Function { get; }
    public IGenericFunction? GenericFunction { get; }
}
