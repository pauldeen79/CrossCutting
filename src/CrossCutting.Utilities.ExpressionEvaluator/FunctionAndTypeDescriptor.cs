namespace CrossCutting.Utilities.ExpressionEvaluator;

public class FunctionAndTypeDescriptor
{
    public FunctionAndTypeDescriptor(IFunction? function, IGenericFunction? genericFunction, Type? returnValueType)
    {
        ReturnValueType = returnValueType;
        Function = function;
        GenericFunction = genericFunction;
    }

    public Type? ReturnValueType { get; }
    public IFunction? Function { get; }
    public IGenericFunction? GenericFunction { get; }
}
