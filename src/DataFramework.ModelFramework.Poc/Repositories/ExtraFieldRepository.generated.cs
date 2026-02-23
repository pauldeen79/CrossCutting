using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
using CrossCutting.Utilities.QueryEvaluator.Core.Builders;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Extensions;
using DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettings;
using DataFramework.ModelFramework.Poc.QueryFieldInfoProviderHandlers;
using PDC.Net.Core.Entities;
using PDC.Net.Core.Queries;

namespace DataFramework.ModelFramework.Poc.Repositories
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class ExtraFieldRepository : Repository<ExtraField, ExtraFieldIdentity>, IExtraFieldRepository
    {
        public ExtraFieldRepository(IDatabaseCommandProcessor<ExtraField> commandProcessor,
                                    IDatabaseEntityRetriever<ExtraField> entityRetriever,
                                    IDatabaseCommandProvider<ExtraFieldIdentity> identitySelectCommandProvider,
                                    IPagedDatabaseCommandProvider pagedEntitySelectCommandProvider,
                                    IDatabaseCommandProvider entitySelectCommandProvider,
                                    IDatabaseCommandProvider<ExtraField> entityCommandProvider,
                                    IQueryProcessor queryProcessor/*,
                                    IEvaluatableSqlExpressionProvider evaluatableSqlExpressionProvider*/)
            : base(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider)
        {
            QueryProcessor = queryProcessor;
            // EvaluatableSqlExpressionProvider = evaluatableSqlExpressionProvider;
        }

        private IQueryProcessor QueryProcessor { get; }
        // private IEvaluatableSqlExpressionProvider EvaluatableSqlExpressionProvider { get; }

        public Task<Result<IReadOnlyCollection<ExtraField>>> FindExtraFieldsByEntityNameAsync(string entityName, CancellationToken token)
        {
            var settings = new ExtraFieldPagedEntityRetrieverSettings();

            // return EntityRetriever.FindManyAsync(new SelectCommandBuilder()
            //    .Select("*")
            //    .From(settings.TableName)
            //    .Where("EntityName = @entityName")
            //    .AppendParameter(nameof(entityName), entityName)
            //    .OrderBy(settings.DefaultOrderBy)
            //    .Build(), token);

            // var parameterBag = new ParameterBag();
            // var builder = new SelectCommandBuilder()
            //     .Select("*")
            //     .From(settings.TableName);
            // var condition = new EqualOperatorEvaluatableBuilder()
            //     .WithLeftOperand(new PropertyNameEvaluatableBuilder(nameof(ExtraField.Name)))
            //     .WithRightOperand(new LiteralEvaluatableBuilder(entityName))
            //     .BuildTyped();
            // var fieldNameProvider = new ExtraFieldQueryFieldInfo();
            // var result = await EvaluatableSqlExpressionProvider.GetConditionExpressionAsync(builder, null, condition, fieldNameProvider, parameterBag, token).ConfigureAwait(false);
            // if (!result.IsSuccessful())
            // {
            //     return Result.FromExistingResult<IReadOnlyCollection<ExtraField>>(result);
            // }
            // builder.AppendParameters(parameterBag.Parameters);
            // return await EntityRetriever.FindManyAsync(builder.Build(), token).ConfigureAwait(false);

            return QueryProcessor.FindManyAsync<ExtraField>(new ExtraFieldQueryBuilder()
                .Where(nameof(ExtraField.EntityName)).IsEqualTo(entityName)
                .OrderBy(settings.DefaultOrderBy)
                // .AddConditions(new EqualConditionBuilder()
                //     .WithSourceExpression(new PropertyNameEvaluatableBuilder(nameof(ExtraField.EntityName)))
                //     .WithCompareExpression(new LiteralEvaluatableBuilder(entityName)))
                // .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder(nameof(ExtraField.Name))))
                .Build(), null, token);
        }
    }
}
