// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionSearchable.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search
{
    public class ReflectionSearchable : Searchable
    {
        public ReflectionSearchable(object instance) 
            : base(instance, new AttributeMetadataCollection(instance.GetType()))
        {
        }
    }
}