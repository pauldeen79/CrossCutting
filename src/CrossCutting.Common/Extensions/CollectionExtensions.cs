namespace CrossCutting.Common.Extensions;

public static class CollectionExtensions
{
    public static ICollection<T> AddRange<T>(this ICollection<T> instance, IEnumerable<T> itemsToAdd)
    {
        ArgumentGuard.IsNotNull(itemsToAdd, nameof(itemsToAdd));

        foreach (var itemToAdd in itemsToAdd)
        {
            instance.Add(itemToAdd);
        }

        return instance;
    }
}
