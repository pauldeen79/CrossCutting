using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common.Results;
using CrossCutting.Utilities.QueryEvaluator.Abstractions;
using CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;
using DataFramework.ModelFramework.Poc.Repositories;
using PDC.Net.Core.Entities;
using PDC.Net.Core.Queries;

namespace DataFramework.ModelFramework.Poc.QueryFieldInfoProviderHandlers
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class CatalogQueryFieldInfoProviderHandler : IQueryFieldInfoProviderHandler
    {
        //private readonly IExtraFieldRepository _extraFieldRepository;

        //public CatalogQueryFieldInfoProvider(IExtraFieldRepository extraFieldRepository)
        //    => _extraFieldRepository = extraFieldRepository;

        public Result<IQueryFieldInfo> Create(IQuery query)
        {
            if (query is CatalogQuery)
            {
                return Result.Success<IQueryFieldInfo>(new CatalogQueryFieldInfo(new[] { new ExtraFieldBuilder().WithName("MyField").WithEntityName("Catalog").WithFieldType("varchar(512)").WithFieldNumber(1).Build() }/*_extraFieldRepository.FindExtraFieldsByEntityName(nameof(Catalog))*/));
            }

            return Result.Continue<IQueryFieldInfo>();
        }
    }
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class CatalogQueryFieldInfo : IQueryFieldInfo
    {
        private readonly IEnumerable<ExtraField> _extraFields;

        public CatalogQueryFieldInfo(IEnumerable<ExtraField> extraFields)
        {
            _extraFields = extraFields;
        }

        public IEnumerable<string> GetAllFields()
        {
            yield return @"Id";
            yield return @"Name";
            yield return @"DateCreated";
            yield return @"DateLastModified";
            yield return @"DateSynchronized";
            yield return @"DriveSerialNumber";
            yield return @"DriveTypeCodeType";
            yield return @"DriveTypeCode";
            yield return @"DriveTypeDescription";
            yield return @"DriveTotalSize";
            yield return @"DriveFreeSpace";
            yield return @"Recursive";
            yield return @"Sorted";
            yield return @"StartDirectory";
            // injected by ExtraField transformation
            yield return @"ExtraField1";
            yield return @"ExtraField2";
            yield return @"ExtraField3";
            yield return @"ExtraField4";
            yield return @"ExtraField5";
            yield return @"ExtraField6";
            yield return @"ExtraField7";
            yield return @"ExtraField8";
            yield return @"ExtraField9";
            yield return @"ExtraField10";
            yield return @"ExtraField11";
            yield return @"ExtraField12";
            yield return @"ExtraField13";
            yield return @"ExtraField14";
            yield return @"ExtraField15";
            yield return @"ExtraField16";
            // injected by computed field
            yield return @"AllFields";
        }

        public string? GetDatabaseFieldName(string queryFieldName)
        {
            // default: return GetAllFields().FirstOrDefault(x => x.Equals(queryFieldName, StringComparison.OrdinalIgnoreCase));
            var extraField = _extraFields.FirstOrDefault(x => x.Name == queryFieldName);
            if (extraField is not null)
            {
                return string.Format("ExtraField{0}", extraField.FieldNumber);
            }

            if (queryFieldName == "AllFields")
            {
                return "[Name] + ' ' + [StartDirectory] + ' ' + COALESCE([ExtraField1], '') + ' ' + COALESCE([ExtraField2], '') + ' ' + COALESCE([ExtraField3], '') + ' ' + COALESCE([ExtraField4], '') + ' ' + COALESCE([ExtraField5], '') + ' ' + COALESCE([ExtraField6], '') + ' ' + COALESCE([ExtraField7], '') + ' ' + COALESCE([ExtraField8], '') + ' ' + COALESCE([ExtraField9], '') + ' ' + COALESCE([ExtraField10], '') + ' ' + COALESCE([ExtraField11], '') + ' ' + COALESCE([ExtraField12], '') + ' ' + COALESCE([ExtraField13], '') + ' ' + COALESCE([ExtraField14], '') + ' ' + COALESCE([ExtraField15], '') + ' ' + COALESCE([ExtraField16], '')";
            }

            return GetAllFields().FirstOrDefault(x => x.Equals(queryFieldName, StringComparison.OrdinalIgnoreCase));
        }
    }
#nullable restore
}
