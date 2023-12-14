namespace Orc.Search.Tests
{
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using PublicApiGenerator;
    using VerifyNUnit;

    [TestFixture]
    public class PublicApiFacts
    {
        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public async Task Orc_Search_HasNoBreakingChanges_Async()
        {
            var assembly = typeof(SearchServiceBase).Assembly;

            await PublicApiApprover.ApprovePublicApiAsync(assembly);
        }

        [Test, MethodImpl(MethodImplOptions.NoInlining)]
        public async Task Orc_Search_Xaml_HasNoBreakingChanges_Async()
        {
            var assembly = typeof(SearchView).Assembly;

            await PublicApiApprover.ApprovePublicApiAsync(assembly);
        }

        internal static class PublicApiApprover
        {
            public static async Task ApprovePublicApiAsync(Assembly assembly)
            {
                var publicApi = ApiGenerator.GeneratePublicApi(assembly, new ApiGeneratorOptions());
                await Verifier.Verify(publicApi);
            }
        }
    }
}
