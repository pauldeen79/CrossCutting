using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.Repositories
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial interface IExtraFieldRepository : IRepository<ExtraField, ExtraFieldIdentity>
    {
        Task<Result<IReadOnlyCollection<ExtraField>>> FindExtraFieldsByEntityNameAsync(string entityName, CancellationToken token);
    }
}

