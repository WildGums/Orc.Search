namespace Orc.Search
{
    using System;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string PrepareOrcSearchFilter(this string filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            if (filter.StartsWith("\"") && filter.EndsWith("\""))
            {
                return filter.Length == 2 
                    ? string.Empty
                    : filter;
            }

            if (!filter.Contains("*") &&
                !filter.Contains(":") &&
                !filter.Contains(" ") &&
                !filter.Contains("AND") &&
                !filter.Contains("OR"))
            {
                filter += "*";
            }

            return filter;
        }

        public static bool IsValidOrcSearchFilter(this string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return false;
            }

            filter = filter.Trim();
            if (filter.EndsWith(":"))
            {
                return false;
            }

            return true;
        }

        public static string ExtractRegexString(this string filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            if (!filter.StartsWith("/") || !filter.EndsWith("/"))
            {
                return string.Empty;
            }

            filter = filter.Substring(1, filter.Length - 2);
            if (!filter.IsValidRegexPattern())
            {
                return string.Empty;
            }

            return filter;
        }

        public static bool IsValidRegexPattern(this string pattern)
        {
            ArgumentNullException.ThrowIfNull(pattern);

            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new Regex(pattern);
                return true;
            }
            catch
            {
                // ignored
            }

            return false;
        }
    }
}
