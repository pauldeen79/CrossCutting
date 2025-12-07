namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class ResultDictionaryExtensions
{
    public static IEvaluatable GetEvaluatable(this IReadOnlyDictionary<string, Result> instance, string resultKey)
        =>  new LiteralEvaluatable(instance.GetValue(resultKey));
}
