namespace CrossCutting.Utilities.Parsers;

public partial record FunctionCallArgument
{
    public abstract Result<object?> GetValueResult(FunctionCallRequest request);
}
