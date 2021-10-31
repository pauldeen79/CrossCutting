using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class UpdateDatabaseCommandProcessorSettings : IDatabaseCommandProcessorSettings
    {
        public string? ExceptionMessage => typeof(TestEntity).Name + " entity was not updated";
    }
}
