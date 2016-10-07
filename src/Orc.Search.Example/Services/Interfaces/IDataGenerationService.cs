// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGenerationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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