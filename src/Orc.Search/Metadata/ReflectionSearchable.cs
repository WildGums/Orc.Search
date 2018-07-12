// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionSearchable.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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