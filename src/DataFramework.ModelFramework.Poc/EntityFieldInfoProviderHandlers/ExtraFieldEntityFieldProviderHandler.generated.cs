using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common.Results;
using CrossCutting.Data.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;
using PDC.Net.Core.Entities;
using PDC.Net.Core.Queries;

namespace DataFramework.ModelFramework.Poc.EntityFieldInfoProviderHandlers
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class ExtraFieldEntityFieldInfoProviderHandler : IEntityFieldInfoProviderHandler
    {
        public Result<IEntityFieldInfo> Create(object query)
        {
            if (query is ExtraFieldQuery || query.Equals(typeof(ExtraField)))
            {
                return Result.Success<IEntityFieldInfo>(new ExtraFieldQueryFieldInfo());
            }

            return Result.Continue<IEntityFieldInfo>();
        }
    }
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class ExtraFieldQueryFieldInfo : IEntityFieldInfo
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
