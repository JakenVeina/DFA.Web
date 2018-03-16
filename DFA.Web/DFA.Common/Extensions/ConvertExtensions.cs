using System;
using System.Diagnostics.Contracts;
using System.Xml;

namespace DFA.Common.Extensions
{
    public static class ConvertExtensions
    {
        /**********************************************************************/
        #region Extension Methods

        public static int ToInt32(this string @this)
        {
            Contract.Requires(@this != null);

            return int.Parse(@this);
        }

        public static int ToInt32(this byte[] @this)
        {
            Contract.Requires(@this != null);
            Contract.Requires(@this.Length <= 4);

            return BitConverter.ToInt32(@this, 0);
        }

        public static TimeSpan ToTimeSpan(this string @this)
        {
            Contract.Requires(@this != null);

            return XmlConvert.ToTimeSpan(@this);
        }

        public static T ToEnum<T>(this string @this)
        {
            Contract.Requires(@this != null);

            return (T)Enum.Parse(typeof(T), @this);
        }

        public static byte[] ToBase64ByteArray(this string @this)
        {
            Contract.Requires(@this != null);

            return Convert.FromBase64String(@this);
        }

        public static string ToBase64String(this byte[] @this)
        {
            Contract.Requires(@this != null);

            return Convert.ToBase64String(@this);
        }

        #endregion Extension Methods
    }
}
