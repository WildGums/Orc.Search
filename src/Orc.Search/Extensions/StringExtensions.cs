// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
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

            if (filter.EndsWith(":"))
            {
                return false;
            }

            return true;
        }
    }
}