using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common.Results;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;
using PDC.Net.Core.Queries;

namespace DataFramework.ModelFramework.Poc.QueryFieldInfoProviderHandlers
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class ExtraFieldQueryFieldInfoProviderHandler : IQueryFieldInfoProviderHandler
    {
        public Result<IQueryFieldInfo> Create(IQuery query)
        {
            if (query is ExtraFieldQuery)
            {
                return Result.Success<IQueryFieldInfo>(new ExtraFieldQueryFieldInfo());
            }

            return Result.Continue<IQueryFieldInfo>();
        }
    }
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class ExtraFieldQueryFieldInfo : IQueryFieldInfo
    {
        public IEnumerable<string> GetAllFields()
        {
            yield return "[EntityName]";
            yield return "[Name]";
            yield return "[Description]";
            yield return "[FieldNumber]";
            yield return "[FieldType]";
        }

        public string? GetDatabaseFieldName(string queryFieldName)
        {
            // default: return GetAllFields().FirstOrDefault(x => x.Equals(queryFieldName, StringComparison.OrdinalIgnoreCase));
            return GetAllFields().FirstOrDefault(x => x.Equals(queryFieldName, StringComparison.OrdinalIgnoreCase));
        }
    }
#nullable restore
}
