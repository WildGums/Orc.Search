// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGenerationServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class IDataGenerationServiceExtensions
    {
        public static async Task<IEnumerable<object>> GenerateObjectsAsync(this IDataGenerationService dataGenerationService, int objectCount = 10000)
        {
            return await Task.Factory.StartNew(() => dataGenerationService.GenerateObjects(objectCount));
        }
    }
}