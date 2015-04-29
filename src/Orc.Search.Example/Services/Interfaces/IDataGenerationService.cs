// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGenerationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Example.Services
{
    using System.Collections.Generic;

    public interface IDataGenerationService
    {
        #region Methods
        IEnumerable<ISearchable> GenerateSearchables(int objectCount = ExampleDefaults.GeneratedObjectCount);
        ISearchable GenerateSearchable();
        #endregion
    }
}