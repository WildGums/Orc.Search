﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PublicApiFacts.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Search.Tests
{
    using System.Runtime.CompilerServices;
    using ApiApprover;
    using NUnit.Framework;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orc_Search_HasNoBreakingChanges()
        {
            var assembly = typeof(SearchServiceBase).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public void Orc_Search_Xaml_HasNoBreakingChanges()
        {
            var assembly = typeof(SearchView).Assembly;

            PublicApiApprover.ApprovePublicApi(assembly);
        }
    }
}