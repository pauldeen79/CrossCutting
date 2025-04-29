namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class DateTimeDotExpressionComponent : DotExpressionComponentBase<DateTime>
{
    public DateTimeDotExpressionComponent() : base(new DotExpressionDescriptor<DateTime>(new Dictionary<string, DotExpressionDelegates<DateTime>>()
    {
        { "Date", new DotExpressionDelegates<DateTime>(DotExpressionType.Property, _ => Result.Success(typeof(DateTime)), (_, typedValue) => Result.Success<object?>(typedValue.Date)) },
        { "Year", new DotExpressionDelegates<DateTime>(DotExpressionType.Property, _ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Year)) },
        { "Month", new DotExpressionDelegates<DateTime>(DotExpressionType.Property, _ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Month)) },
        { "Day", new DotExpressionDelegates<DateTime>(DotExpressionType.Property, _ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Day)) },
        { "Hour", new DotExpressionDelegates<DateTime>(DotExpressionType.Property, _ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Hour)) },
        { "Minute", new DotExpressionDelegates<DateTime>(DotExpressionType.Property, _ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Minute)) },
        { "Second", new DotExpressionDelegates<DateTime>(DotExpressionType.Property, _ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Second)) },
    }))
    {
    }

    public override int Order => 11;
}
