using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IEnumerableExtenisons {

    public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
    {
        foreach (var item in self)
        {
            action(item);
        }
    }

    public static IEnumerable<Tuple<T1,T2>> CoverTuple<T1,T2>(this IEnumerable<T1> self, IEnumerable<T2> other)
    {
        var enumerable = other as T2[] ?? other.ToArray();
        
        foreach (var item1 in self)
        {
            foreach (var item2 in enumerable)
            {
                yield return new Tuple<T1, T2>(item1, item2);
            }
        }
    }
}