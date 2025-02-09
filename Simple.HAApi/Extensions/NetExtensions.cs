namespace Simple.HAApi.Extensions;

using System.Collections.Generic;
using System.Linq;

public static class NetExtensions
{
    public static IEnumerable<T[]> Chunk<T>(this T[] source, int count)
    {
        if (source.Length <= count)
        {
            yield return source;
            yield break;
        }

        for (int i = 0; i < source.Length; i += count)
        {
            yield return source.Skip(i).Take(count).ToArray();
        }
    }
}
