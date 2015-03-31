// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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
        public const Version Version = Lucene.Net.Util.Version.LUCENE_30;
    }
}