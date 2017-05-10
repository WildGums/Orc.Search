// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string PrepareOrcSearchFilter(this string filter)
        {
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
            if (!filter.StartsWith("/"))
            {
                return string.Empty;
            }

            filter = filter.Substring(1);
            if (!filter.IsValidRegexPattern())
            {
                return string.Empty;
            }

            return filter;
        }

        public static bool IsValidRegexPattern(this string pattern)
        {
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