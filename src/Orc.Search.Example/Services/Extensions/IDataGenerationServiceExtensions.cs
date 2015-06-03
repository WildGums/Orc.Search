// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGenerationServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Catel.Threading;

    public static class IDataGenerationServiceExtensions
    {
        public static Task<IEnumerable<ISearchable>> GenerateSearchablesAsync(this IDataGenerationService dataGenerationService, int objectCount = ExampleDefaults.GeneratedObjectCount)
        {
            return TaskHelper.Run(() => dataGenerationService.GenerateSearchables(objectCount));
        }
    }
}