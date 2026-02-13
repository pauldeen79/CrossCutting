using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using PDC.Net.Core.Entities;
using PDC.Net.Core.Queries;

namespace DataFramework.ModelFramework.Poc.Repositories
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class CatalogRepository : Repository<Catalog, CatalogIdentity>, ICatalogRepository
    {
        public CatalogRepository(IDatabaseCommandProcessor<Catalog> commandProcessor,
                                 IDatabaseEntityRetriever<Catalog> entityRetriever,
                                 IDatabaseCommandProvider<CatalogIdentity> identitySelectCommandProvider,
                                 IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
                                 IDatabaseCommandProvider entitySelectCommandProvider,
                                 IDatabaseCommandProvider<Catalog> entityCommandProvider,
                                 IQueryProcessor queryProcessor)
            : base(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider)
        {
            QueryProcessor = queryProcessor;
        }

        public IQueryProcessor QueryProcessor { get; }

        public Task<Result<IReadOnlyCollection<Catalog>>> FindSomethingAsync(CancellationToken token)
        {
            return QueryProcessor.FindManyAsync<Catalog>(new CatalogQueryBuilder()/*.Where(nameof(Catalog.Name)).IsEqualTo("Something")*/.Build(), null, token);
        }
    }
}
