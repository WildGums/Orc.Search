// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISearchable.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    using Metadata;

    public interface ISearchable
    {
        #region Properties
        object Instance { get; }
        IMetadataCollection MetadataCollection { get; }
        #endregion
    }
}