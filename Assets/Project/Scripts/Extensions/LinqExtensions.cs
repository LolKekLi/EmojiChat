using System;
using System.Collections.Generic;
using System.Linq;

namespace EmojiChat
{
    public static class LinqExtensions
    {
        public static void Do<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
            {
                UnityEngine.Debug.LogError("enumerable is null");
                return;
            }

            foreach (var item in enumerable)
            {
                action(item);
            }
        }

        public static T RandomElement<T>(this IList<T> enumerable)
        {
            return enumerable[UnityEngine.Random.Range(0, enumerable.Count)];
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null)
            {
                UnityEngine.Debug.LogException(new Exception("source is null"));
            }

            if (rng == null)
            {
                UnityEngine.Debug.LogException(new Exception("rng is null"));
            }

            return source.ShuffleIterator(rng);
        }

        public static IEnumerable<T> DuplicateIntersect<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T> comparer = null)
        {
            var dict = new Dictionary<T, int>(comparer);

            foreach (T item in second)
            {
                int hits;
                dict.TryGetValue(item, out hits);
                dict[item] = hits + 1;
            }

            foreach (T item in first)
            {
                int hits;
                dict.TryGetValue(item, out hits);
                if (hits > 0)
                {
                    yield return item;
                    dict[item] = hits - 1;
                }
            }
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}