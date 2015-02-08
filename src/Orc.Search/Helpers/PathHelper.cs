// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathHelper.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using System.IO;
    using Path = Catel.IO.Path;

    public static class PathHelper
    {
        public static string GetRootDirectory()
        {
            var directory = Path.GetApplicationDataDirectory();

            directory = Path.Combine(directory, "search");

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }
    }
}