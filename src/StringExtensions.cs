using System;

namespace ScreenSaver
{
    internal static class StringExtensions
    {
        internal static bool HasSameText(this string value, string otherValue)
        {
            return string.Equals(value, otherValue, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
