using System;
namespace EStoreWebApi.Extensions;

public static class EnumerableExtensions
{
    private static Random random = new Random();

    public static T GetRandomElement<T>(this IEnumerable<T> list)
    {
        // If there are no elements in the collection, return the default value of T
        if (list.Count() == 0)
            return default(T);

        return list.ElementAt(random.Next(list.Count()));
    }
}

