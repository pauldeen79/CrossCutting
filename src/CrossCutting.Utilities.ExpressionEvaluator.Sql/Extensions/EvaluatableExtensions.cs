namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class EvaluatableExtensions
{
    public static IEvaluatableContext ToEvaluatableContext(this IEvaluatable<bool> evaluatable, Type entityType, IEvaluatable? orderByEvaluatable = null)
        => new EvaluatableWrapper(evaluatable, entityType, orderByEvaluatable);

    private sealed class EvaluatableWrapper : IEvaluatableContext
    {
        public EvaluatableWrapper(IEvaluatable<bool> evaluatable, Type entityType, IEvaluatable? orderByEvaluatable)
        {
            Evaluatable = evaluatable;
            EntityType = entityType;
            OrderByEvaluatable = orderByEvaluatable;
        }

        public IEvaluatable<bool> Evaluatable { get; }

        public Type EntityType { get; }

        public IEvaluatable? OrderByEvaluatable { get; }
    }
}
