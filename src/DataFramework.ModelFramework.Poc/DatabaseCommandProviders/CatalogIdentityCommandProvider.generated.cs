using System.CodeDom.Compiler;
using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.CommandProviders;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.DatabaseCommandProviders
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class CatalogIdentityCommandProvider : IdentityDatabaseCommandProviderBase<CatalogIdentity>
    {
        public CatalogIdentityCommandProvider(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : base(settingsProviders)
        {
        }

        protected override IEnumerable<IdentityDatabaseCommandProviderField> GetFields()
        {
            yield return new IdentityDatabaseCommandProviderField("Id", "Id");
        }
    }
}
