namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class DateTimeDotExpressionComponent : DotExpressionComponentBase<DateTime>, IDynamicDescriptorsProvider
{
    private const string DaysToAdd = nameof(DaysToAdd);

    private static readonly MemberDescriptor _dateDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.Date))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(DateTime))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent));

    private static readonly MemberDescriptor _yearDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.Year))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(int))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent));

    private static readonly MemberDescriptor _monthDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.Month))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(int))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent));

    private static readonly MemberDescriptor _dayDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.Day))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(int))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent));

    private static readonly MemberDescriptor _hourDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.Hour))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(int))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent));

    private static readonly MemberDescriptor _minuteDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.Minute))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(int))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent));

    private static readonly MemberDescriptor _secondDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.Second))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Property)
        .WithReturnValueType(typeof(int))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent));

    private static readonly MemberDescriptor _addDaysDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.AddDays))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Method)
        .WithReturnValueType(typeof(DateTime))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent))
        .AddArguments(new MemberDescriptorArgumentBuilder().WithName(nameof(DaysToAdd)).WithIsRequired().WithType(typeof(int)));

    public DateTimeDotExpressionComponent(IMemberCallArgumentValidator validator) : base(new DotExpressionDescriptor<DateTime>(new Dictionary<string, DotExpressionDelegates<DateTime>>()
    {
        { nameof(DateTime.Date), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(DateTime)), (_, typedValue) => Result.Success<object?>(typedValue.Date)) },
        { nameof(DateTime.Year), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Year)) },
        { nameof(DateTime.Month), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Month)) },
        { nameof(DateTime.Day), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Day)) },
        { nameof(DateTime.Hour), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Hour)) },
        { nameof(DateTime.Minute), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Minute)) },
        { nameof(DateTime.Second), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Second)) },
        { nameof(DateTime.AddDays), new DotExpressionDelegates<DateTime>(0, x => MemberCallArgumentValidator.Validate(x, validator, _addDaysDescriptor), EvaluateAddDays) },
    }))
    {
    }

    public override int Order => 11;

    public IEnumerable<MemberDescriptor> GetDescriptors()
        => [_dateDescriptor, _yearDescriptor, _monthDescriptor, _dayDescriptor, _hourDescriptor, _minuteDescriptor, _secondDescriptor, _addDaysDescriptor];

    private static Result<object?> EvaluateAddDays(DotExpressionComponentState state, DateTime sourceValue)
    {
        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add(DaysToAdd, () => context.FunctionCall.GetArgumentValueResult<int>(1, DaysToAdd, context))
            .Build()
            .OnSuccess(results => Result.Success<object?>(sourceValue.AddDays(results.GetValue<int>(DaysToAdd))));
    }
}
