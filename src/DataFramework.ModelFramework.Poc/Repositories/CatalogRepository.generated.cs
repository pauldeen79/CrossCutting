using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrossCutting.Common.Extensions;
using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;
using CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;
using CrossCutting.Utilities.ExpressionEvaluator.Sql;
using CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders.Extensions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Extensions;
using DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettings;
using DataFramework.ModelFramework.Poc.QueryFieldInfoProviderHandlers;
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
                                 IEvaluatableSqlExpressionProvider evaluatableSqlExpressionProvider)
            : base(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider)
        {
            QueryProcessor = queryProcessor;
            EvaluatableSqlExpressionProvider = evaluatableSqlExpressionProvider;
        }

        private IQueryProcessor QueryProcessor { get; }
        private IEvaluatableSqlExpressionProvider EvaluatableSqlExpressionProvider { get; }

        public async Task<Result<IReadOnlyCollection<Catalog>>> FindSomethingAsync(CancellationToken token)
        {
            // return QueryProcessor.FindManyAsync<Catalog>(new CatalogQueryBuilder()
            //     .Where(nameof(Catalog.Name)).IsEqualTo("Something")
            //     // .AddConditions(new EqualConditionBuilder()
            //     //     .WithSourceExpression(new PropertyNameEvaluatableBuilder(nameof(Catalog.Name)))
            //     //     .WithCompareExpression(new LiteralEvaluatableBuilder("Something")))
            //     .Build(), null, token);

            var settings = new CatalogPagedEntityRetrieverSettings();
            var builder = new SelectCommandBuilder()
                .Select(settings.Fields)
                .From(settings.TableName);
            var evaluatable = new EqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new PropertyNameEvaluatableBuilder(nameof(Catalog.Name)))
                .WithRightOperand(new LiteralEvaluatableBuilder("Something"))
                .BuildTyped();
            var fieldNameProvider = new CatalogQueryFieldInfo([]);

            return await (await EvaluatableSqlExpressionProvider.GetExpressionAsync(builder, null, evaluatable, fieldNameProvider, token).ConfigureAwait(false))
                .OnSuccessAsync(_ => EntityRetriever.FindManyAsync(builder.Build(), token)).ConfigureAwait(false);
        }
    }
}
