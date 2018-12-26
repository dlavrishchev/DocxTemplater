using System.Collections.Generic;

namespace DocxTemplater.Extensions
{
    internal static class StringExtensions
    {
        public static string CreateCommaSeparatedString(this IEnumerable<string> strings)
        {
            return string.Join(", ", strings);
        }
    }
}
