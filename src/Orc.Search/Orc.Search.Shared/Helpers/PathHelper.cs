// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PathHelper.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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