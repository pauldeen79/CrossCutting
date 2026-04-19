namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class EvaluatableExtensions
{
    public static IEvaluatableContext WithTypeAndContext(this IEvaluatable<bool> evaluatable, Type entityType, object? context)
        => new EvaluatableWrapper(evaluatable, entityType, context);

    private sealed class EvaluatableWrapper : IEvaluatableContext
    {
        public EvaluatableWrapper(IEvaluatable<bool> evaluatable, Type entityType, object? context)
        {
            Evaluatable = evaluatable;
            EntityType = entityType;
            Context = context;
        }

        public IEvaluatable<bool> Evaluatable { get; }

        public Type EntityType { get; }

        public object? Context { get; }
    }
}
