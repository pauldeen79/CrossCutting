using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class DeleteDatabaseCommandEntityProvider : IDatabaseCommandEntityProvider<TestEntity>
    {
        public Func<TestEntity, IDatabaseCommand> CommandDelegate => x => new StoredProcedureCommand<TestEntity>(@"[DeleteCode]", x, DeleteParameters);

        public Func<TestEntity, TestEntity>? ResultEntityDelegate => DeleteResultEntity;

        public Func<TestEntity, IDataReader, TestEntity>? AfterReadDelegate => null;

        private object DeleteParameters(TestEntity resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object>("@Code", resultEntity.Code),
                new KeyValuePair<string, object>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object>("@Description", resultEntity.Description),
            };
        }

        private TestEntity DeleteResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }
    }
}
