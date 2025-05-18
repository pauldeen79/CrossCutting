namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IFunction : INonGenericMember
{
}

public interface IFunction<T> : IFunction, INonGenericMember<T>
{
}
