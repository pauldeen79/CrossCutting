namespace CrossCutting.Utilities.Parsers.Tests.ExpressionFrameworkTests.HowItShouldBe;

public class ExpressionFrameworkHowItShouldBeTests
{

}

[FunctionName(@"ToUpperCase")]
[Description("Converts the expression to upper case")]
[FunctionArgument("Expression", typeof(string), "String to get the upper case for", true)]
[FunctionArgument("Culture", typeof(CultureInfo), "Optional CultureInfo to use", false)]
[FunctionResult(ResultStatus.Ok, typeof(string), "The value of the expression converted to upper case", "This result will be returned when the expression is of type string")]
[FunctionResult(ResultStatus.Invalid, "Expression must be of type string")]
[FunctionResult(ResultStatus.Invalid, "CultureInfo must be of type CultureInfo")]
public class ToUpperCaseFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        //example for OnFailure that has a custom result, with an inner result that contains the details.
        //if you don't want an error message stating that this is the source, then simply remove the OnFailure line.
        return new ResultDictionaryBuilder()
            .Add("Expression", () => context.GetArgumentValueResult<string>(0, "Expression"))
            .Add("Culture", () => context.GetArgumentValueResult<CultureInfo?>(1, "Culture", null))
            .Build()
            .OnFailure(error => Result.Error<object?>([error], "ToLowerCase evaluation failed, see inner results for details"))
            .OnSuccess(results =>
                Result.Success<object?>(results["Culture"].GetValue() is null
                    ? results["Expression"].CastValueAs<string>().ToUpperInvariant()
                    : results["Expression"].CastValueAs<string>().ToUpper(results["Culture"].CastValueAs<CultureInfo>())));
    }

    public Result Validate(FunctionCallContext context)
    {
        // No additional validation needed
        return Result.Success();
    }
}

public class ToLowerCaseFunctionCallBuilder : IBuilder<FunctionCall>
{
    public string Expression { get; set; }
    public CultureInfo? CultureInfo { get; set; }

    public FunctionCall Build()
    {
        return new FunctionCallBuilder()
            .WithName(@"ToUpperCase")
            .AddArguments(
                //TODO: Convert to DelegateArgument, when available
                new ConstantArgumentBuilder().WithValue(Expression)
                //new ConstantArgumentBuilder().WithValue(CultureInfo)
            )
            .Build();
    }
}
