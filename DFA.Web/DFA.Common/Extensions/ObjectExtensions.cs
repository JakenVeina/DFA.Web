using System;

namespace DFA.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static T Cast<T>(this object @this)
            => (T)@this;

        public static T As<T>(this object @this) where T : class
            => @this as T;

        public static T? AsNullable<T>(this T @this) where T : struct
            => @this;
    }
}
