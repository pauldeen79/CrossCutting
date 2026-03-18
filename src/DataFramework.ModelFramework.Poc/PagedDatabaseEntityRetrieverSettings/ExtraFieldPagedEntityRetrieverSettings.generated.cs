using System.CodeDom.Compiler;
using CrossCutting.Data.Abstractions;

namespace DataFramework.ModelFramework.Poc.PagedDatabaseEntityRetrieverSettings
{
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class ExtraFieldPagedEntityRetrieverSettings : IPagedDatabaseEntityRetrieverSettings
    {
        public string TableName => @"[ExtraField]";
        public string Fields => @"[EntityName], [Name], [Description], [FieldNumber], [FieldType]";
        public string DefaultOrderBy => "[Name]";
        public string DefaultWhere => string.Empty;
        public int? OverridePageSize => null;
    }
}
