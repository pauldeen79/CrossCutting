using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.CommandProviders;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestEntityIdentityDatabaseCommandProvider : IdentityDatabaseCommandProviderBase<TestEntityIdentity>
    {
        public TestEntityIdentityDatabaseCommandProvider(TestEntityDatabaseEntityRetrieverSettings settings) : base(settings)
        {
        }

        protected override IEnumerable<IdentityDatabaseCommandProviderField> GetFields()
        {
            yield return new IdentityDatabaseCommandProviderField("Code");
            yield return new IdentityDatabaseCommandProviderField("CodeType");
        }
    }
}
