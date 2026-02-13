using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
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

            //return EntityRetriever.FindMany(new SelectCommandBuilder()
            //    .Select("*")
            //    .From(settings.TableName)
            //    .Where("EntityName = @entityName")
            //    .AppendParameter(nameof(entityName), entityName)
            //    .OrderBy(settings.DefaultOrderBy)
            //    .Build());

            return QueryProcessor.FindManyAsync<ExtraField>(new ExtraFieldQueryBuilder()/*.Where(nameof(ExtraField.EntityName)).IsEqualTo(entityName).OrderBy(settings.DefaultOrderBy)*/.Build(), null, token);
        }
    }
}
