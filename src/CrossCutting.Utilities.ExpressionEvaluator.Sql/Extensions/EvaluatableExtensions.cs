namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class EvaluatableExtensions
{
    public static IEvaluatableContext ToEvaluatableContext(this IEvaluatable<bool> evaluatable, Type entityType, object? context = null, IEvaluatable? orderByEvaluatable = null)
        => new EvaluatableWrapper(evaluatable, entityType, context, orderByEvaluatable);

    private sealed class EvaluatableWrapper : IEvaluatableContext
    {
        public EvaluatableWrapper(IEvaluatable<bool> evaluatable, Type entityType, object? context, IEvaluatable? orderByEvaluatable)
        {
            Evaluatable = evaluatable;
            EntityType = entityType;
            Context = context;
            OrderByEvaluatable = orderByEvaluatable;
        }

        public IEvaluatable<bool> Evaluatable { get; }

        public Type EntityType { get; }

        public object? Context { get; }

        public IEvaluatable? OrderByEvaluatable { get; }
    }
}
