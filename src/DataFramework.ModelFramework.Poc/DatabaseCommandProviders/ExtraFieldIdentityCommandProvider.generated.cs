using System.CodeDom.Compiler;
using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.CommandProviders;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.DatabaseCommandProviders
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class ExtraFieldIdentityCommandProvider : IdentityDatabaseCommandProviderBase<ExtraFieldIdentity>
    {
        public ExtraFieldIdentityCommandProvider(IEnumerable<IPagedDatabaseEntityRetrieverSettingsProvider> settingsProviders) : base(settingsProviders)
        {
        }

        protected override IEnumerable<IdentityDatabaseCommandProviderField> GetFields()
        {
            yield return new IdentityDatabaseCommandProviderField("EntityName", "EntityName");
            yield return new IdentityDatabaseCommandProviderField("Name", "Name");
        }
    }
}
