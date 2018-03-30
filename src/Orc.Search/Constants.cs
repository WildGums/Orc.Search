// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Lucene.Net.Util;

    public static class SearchDefaults
    {
        public const int DefaultResults = 50;
    }

    internal static class LuceneDefaults
    {
        public const LuceneVersion Version = LuceneVersion.LUCENE_48;
    }
}