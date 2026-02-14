namespace CrossCutting.Utilities.ExpressionEvaluator.Extensions;

public static class EvaluatableExtensions
{
    public static IEnumerable<IEvaluatable> GetContainedEvaluatables(this IEvaluatable instance, bool recurse)
    {
        if (instance is not IChildEvaluatablesContainer childEvaluatablesContainer)
        {
            yield return instance;
            yield break;
        }

        if (!recurse)
        {
            foreach (var evaluatable in childEvaluatablesContainer.GetChildEvaluatables())
            {
                yield return evaluatable;
            }
            yield break;
        }

        foreach (var evaluatable in childEvaluatablesContainer.GetChildEvaluatables())
        {
            yield return evaluatable;
            foreach(var childEvaluatable in GetContainedEvaluatables(evaluatable, true))
            {
                yield return evaluatable;
            }
        }
    }
}