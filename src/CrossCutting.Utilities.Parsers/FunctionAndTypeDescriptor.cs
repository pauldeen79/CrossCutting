namespace CrossCutting.Utilities.Parsers;

internal sealed class FunctionAndTypeDescriptor
{
    public FunctionAndTypeDescriptor(IFunction? function, IGenericFunction? genericFunction, Type? returnValueType)
    {
        if(function is null && genericFunction is null)
        {
            throw new ArgumentException("Either function or genericFunction needs to be specified");
        }

        ReturnValueType = returnValueType;
        Function = function;
        GenericFunction = genericFunction;
    }

    public Type? ReturnValueType { get; }
    public IFunction? Function { get; }
    public IGenericFunction? GenericFunction { get; }
}
