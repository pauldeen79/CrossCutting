using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Data.Sql.Builders;
using CrossCutting.Utilities.ExpressionEvaluator;
using CrossCutting.Utilities.ExpressionEvaluator.Abstractions;
using CrossCutting.Utilities.ExpressionEvaluator.Builders.Abstractions;
using CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;
using CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;
using CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;
using CrossCutting.Utilities.ExpressionEvaluator.Extensions;
using CrossCutting.Utilities.ExpressionEvaluator.Sql;
using CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;
using CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders.Extensions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Extensions;
using DataFramework.ModelFramework.Poc.EntityFieldInfoProviderHandlers;
using DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettings;
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
                                 IQueryProcessor queryProcessor,
                                 IEvaluatableProcessor evaluatableProcessor)
            : base(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider)
        {
            QueryProcessor = queryProcessor;
            EvaluatableProcessor = evaluatableProcessor;
        }

        private IQueryProcessor QueryProcessor { get; }
        private IEvaluatableProcessor EvaluatableProcessor { get; }

        public async Task<Result<IReadOnlyCollection<Catalog>>> FindSomethingAsync(CancellationToken token)
        {
            // return QueryProcessor.FindManyAsync<Catalog>(new CatalogQueryBuilder()
            //     .Where(nameof(Catalog.Name)).IsEqualTo("Something")
            //     .Build(), null, token);

            IEvaluatable<bool> evaluatable = Evaluatable.Empty<bool>();
            evaluatable = evaluatable.And(Evaluatable.OfPropertyName(nameof(Catalog.Name)).IsEqualTo("Something"));

            return await EvaluatableProcessor.FindManyAsync<Catalog>(evaluatable, null, token);
        }

        public async Task<Result<IPagedResult<Catalog>>> FindSomethingPagedAsync(int offset, int pageSize, CancellationToken token)
        {
            var evaluatable = Evaluatable.OfPropertyName(nameof(Catalog.Name)).IsEqualTo("Something");

            return await EvaluatableProcessor.FindPagedAsync<Catalog>(evaluatable, offset, pageSize, null, token);
        }
    }
}
