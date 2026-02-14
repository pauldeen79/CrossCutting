using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Core.Builders;
using CrossCutting.Utilities.ExpressionEvaluator.Builders.Evaluatables;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions.Builders.Extensions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Conditions;
using CrossCutting.Utilities.QueryEvaluator.Core.Builders.Evaluatables;
using DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettings;
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
                                    IQueryProcessor queryProcessor)
            : base(commandProcessor, entityRetriever, identitySelectCommandProvider, pagedEntitySelectCommandProvider, entitySelectCommandProvider, entityCommandProvider)
        {
            QueryProcessor = queryProcessor;
        }

        public IQueryProcessor QueryProcessor { get; }

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

            return QueryProcessor.FindManyAsync<ExtraField>(new ExtraFieldQueryBuilder()
                //.Where(nameof(ExtraField.EntityName)).IsEqualTo(entityName).OrderBy(settings.DefaultOrderBy)
                .AddConditions(new EqualConditionBuilder()
                    .WithSourceExpression(new PropertyNameEvaluatableBuilder(nameof(ExtraField.EntityName)))
                    .WithCompareExpression(new LiteralEvaluatableBuilder(entityName)))
                .AddSortOrders(new SortOrderBuilder(new PropertyNameEvaluatableBuilder(nameof(ExtraField.Name))))
                .Build(), null, token);
        }
    }
}
