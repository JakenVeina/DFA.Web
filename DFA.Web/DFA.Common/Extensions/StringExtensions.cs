using System;

namespace DFA.Common.Extensions
{
    public static class StringExtensions
    {
        /**********************************************************************/
        #region Extension Methods

        public static bool IsNullOrEmpty(this string @this)
            => string.IsNullOrEmpty(@this);

        public static bool IsNullOrWhitespace(this string @this)
            => string.IsNullOrWhiteSpace(@this);

        #endregion Extension Methods
    }
}
