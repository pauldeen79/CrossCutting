namespace CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;

public class DateTimeDotExpressionComponent : DotExpressionComponentBase<DateTime>, IDynamicDescriptorsProvider, IMember
{
    private const string DaysToAdd = nameof(DaysToAdd);
    private const string HoursToAdd = nameof(HoursToAdd);
    private const string MinutesToAdd = nameof(MinutesToAdd);
    private const string MonthsToAdd = nameof(MonthsToAdd);
    private const string SecondsToAdd = nameof(SecondsToAdd);
    private const string YearsToAdd = nameof(YearsToAdd);

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
        .AddArguments(
            new MemberDescriptorArgumentBuilder().WithName(Constants.DotArgument).WithType(typeof(DateTime)).WithIsRequired(),
            new MemberDescriptorArgumentBuilder().WithName(DaysToAdd).WithType(typeof(int)).WithIsRequired());

    private static readonly MemberDescriptor _addHoursDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.AddHours))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Method)
        .WithReturnValueType(typeof(DateTime))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent))
        .AddArguments(
            new MemberDescriptorArgumentBuilder().WithName(Constants.DotArgument).WithType(typeof(DateTime)).WithIsRequired(),
            new MemberDescriptorArgumentBuilder().WithName(HoursToAdd).WithType(typeof(int)).WithIsRequired());

    private static readonly MemberDescriptor _addMinutesDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.AddMinutes))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Method)
        .WithReturnValueType(typeof(DateTime))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent))
        .AddArguments(
            new MemberDescriptorArgumentBuilder().WithName(Constants.DotArgument).WithType(typeof(DateTime)).WithIsRequired(),
            new MemberDescriptorArgumentBuilder().WithName(MinutesToAdd).WithType(typeof(int)).WithIsRequired());

    private static readonly MemberDescriptor _addMonthsDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.AddMonths))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Method)
        .WithReturnValueType(typeof(DateTime))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent))
        .AddArguments(
            new MemberDescriptorArgumentBuilder().WithName(Constants.DotArgument).WithType(typeof(DateTime)).WithIsRequired(), 
            new MemberDescriptorArgumentBuilder().WithName(MonthsToAdd).WithType(typeof(int)).WithIsRequired());

    private static readonly MemberDescriptor _addSecondsDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.AddSeconds))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Method)
        .WithReturnValueType(typeof(DateTime))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent))
        .AddArguments(
            new MemberDescriptorArgumentBuilder().WithName(Constants.DotArgument).WithType(typeof(DateTime)).WithIsRequired(),
            new MemberDescriptorArgumentBuilder().WithName(SecondsToAdd).WithType(typeof(int)).WithIsRequired());

    private static readonly MemberDescriptor _addYearsDescriptor = new MemberDescriptorBuilder()
        .WithName(nameof(DateTime.AddYears))
        .WithInstanceType(typeof(DateTime))
        .WithMemberType(MemberType.Method)
        .WithReturnValueType(typeof(DateTime))
        .WithImplementationType(typeof(DateTimeDotExpressionComponent))
        .AddArguments(
            new MemberDescriptorArgumentBuilder().WithName(Constants.DotArgument).WithType(typeof(DateTime)).WithIsRequired(),
            new MemberDescriptorArgumentBuilder().WithName(YearsToAdd).WithType(typeof(int)).WithIsRequired());

    public DateTimeDotExpressionComponent(IMemberCallArgumentValidator validator)
    {
        ArgumentGuard.IsNotNull(validator, nameof(validator));

        Descriptor = new DotExpressionDescriptor<DateTime>(new Dictionary<string, DotExpressionDelegates<DateTime>>()
        {
            { nameof(DateTime.Date), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(DateTime)), (_, typedValue) => Result.Success<object?>(typedValue.Date)) },
            { nameof(DateTime.Year), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Year)) },
            { nameof(DateTime.Month), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Month)) },
            { nameof(DateTime.Day), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Day)) },
            { nameof(DateTime.Hour), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Hour)) },
            { nameof(DateTime.Minute), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Minute)) },
            { nameof(DateTime.Second), new DotExpressionDelegates<DateTime>(_ => Result.Success(typeof(int)), (_, typedValue) => Result.Success<object?>(typedValue.Second)) },
            { nameof(DateTime.AddDays), new DotExpressionDelegates<DateTime>(0, x => MemberCallArgumentValidator.Validate(x, validator, _addDaysDescriptor), EvaluateAddDays) },
            { nameof(DateTime.AddHours), new DotExpressionDelegates<DateTime>(0, x => MemberCallArgumentValidator.Validate(x, validator, _addHoursDescriptor), EvaluateAddHours) },
            { nameof(DateTime.AddMinutes), new DotExpressionDelegates<DateTime>(0, x => MemberCallArgumentValidator.Validate(x, validator, _addMinutesDescriptor), EvaluateAddMinutes) },
            { nameof(DateTime.AddMonths), new DotExpressionDelegates<DateTime>(0, x => MemberCallArgumentValidator.Validate(x, validator, _addMonthsDescriptor), EvaluateAddMonths) },
            { nameof(DateTime.AddSeconds), new DotExpressionDelegates<DateTime>(0, x => MemberCallArgumentValidator.Validate(x, validator, _addSecondsDescriptor), EvaluateAddSeconds) },
            { nameof(DateTime.AddYears), new DotExpressionDelegates<DateTime>(0, x => MemberCallArgumentValidator.Validate(x, validator, _addYearsDescriptor), EvaluateAddYears) },
            });
    }

    public override int Order => 11;

    public Result<IReadOnlyCollection<MemberDescriptor>> GetDescriptors(IMemberDescriptorCallback callback)
        => Result.Success<IReadOnlyCollection<MemberDescriptor>>([
            _dateDescriptor,
            _yearDescriptor,
            _monthDescriptor,
            _dayDescriptor,
            _hourDescriptor,
            _minuteDescriptor,
            _secondDescriptor,
            _addDaysDescriptor,
            _addHoursDescriptor,
            _addMinutesDescriptor,
            _addMonthsDescriptor,
            _addSecondsDescriptor,
            _addYearsDescriptor
           ]);

    private static Result<object?> EvaluateAddDays(DotExpressionComponentState state, DateTime sourceValue)
    {
        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add(DaysToAdd, () => context.FunctionCall.GetArgumentValueResult<int>(1, DaysToAdd, context))
            .Build()
            .OnSuccess(results => Result.Success<object?>(sourceValue.AddDays(results.GetValue<int>(DaysToAdd))));
    }

    private static Result<object?> EvaluateAddHours(DotExpressionComponentState state, DateTime sourceValue)
    {
        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add(HoursToAdd, () => context.FunctionCall.GetArgumentValueResult<int>(1, HoursToAdd, context))
            .Build()
            .OnSuccess(results => Result.Success<object?>(sourceValue.AddHours(results.GetValue<int>(HoursToAdd))));
    }

    private static Result<object?> EvaluateAddMinutes(DotExpressionComponentState state, DateTime sourceValue)
    {
        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add(MinutesToAdd, () => context.FunctionCall.GetArgumentValueResult<int>(1, MinutesToAdd, context))
            .Build()
            .OnSuccess(results => Result.Success<object?>(sourceValue.AddMinutes(results.GetValue<int>(MinutesToAdd))));
    }

    private static Result<object?> EvaluateAddMonths(DotExpressionComponentState state, DateTime sourceValue)
    {
        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add(MonthsToAdd, () => context.FunctionCall.GetArgumentValueResult<int>(1, MonthsToAdd, context))
            .Build()
            .OnSuccess(results => Result.Success<object?>(sourceValue.AddMonths(results.GetValue<int>(MonthsToAdd))));
    }

    private static Result<object?> EvaluateAddSeconds(DotExpressionComponentState state, DateTime sourceValue)
    {
        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add(SecondsToAdd, () => context.FunctionCall.GetArgumentValueResult<int>(1, SecondsToAdd, context))
            .Build()
            .OnSuccess(results => Result.Success<object?>(sourceValue.AddSeconds(results.GetValue<int>(SecondsToAdd))));
    }

    private static Result<object?> EvaluateAddYears(DotExpressionComponentState state, DateTime sourceValue)
    {
        var context = new FunctionCallContext(state);

        return new ResultDictionaryBuilder()
            .Add(YearsToAdd, () => context.FunctionCall.GetArgumentValueResult<int>(1, YearsToAdd, context))
            .Build()
            .OnSuccess(results => Result.Success<object?>(sourceValue.AddYears(results.GetValue<int>(YearsToAdd))));
    }
}
