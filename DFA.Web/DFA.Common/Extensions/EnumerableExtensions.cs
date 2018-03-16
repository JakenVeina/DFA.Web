using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace DFA.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> MakeEnumerable<T>(this T item)
        {
            yield return item;
        }

        public static IEnumerable<T> ToEnumerable<T>(this IEnumerable<T> sequence)
        {
            Contract.Requires(sequence != null);

            return sequence.YieldToEnumerable();
        }

        public static IEnumerable<T> YieldToEnumerable<T>(this IEnumerable<T> sequence)
        {
            foreach (var item in sequence)
                yield return item;
        }

        public static IEnumerable<(T item, int index)> SelectWithIndex<T>(this IEnumerable<T> sequence)
        {
            Contract.Requires(sequence != null);

            return sequence.Select((item, index) => (item, index));
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            Contract.Requires(sequence != null);
            Contract.Requires(action != null);

            return sequence.YieldDo(action);
        }

        private static IEnumerable<T> YieldDo<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach(var item in sequence)
            {
                action.Invoke(item);
                yield return item;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            Contract.Requires(sequence != null);
            Contract.Requires(action != null);

            foreach (var item in sequence)
                action.Invoke(item);
        }

        public static void ForEach<T>(this IEnumerable<T> sequence)
        {
            Contract.Requires(sequence != null);

            foreach (var item in sequence) ;
        }
    }
}
