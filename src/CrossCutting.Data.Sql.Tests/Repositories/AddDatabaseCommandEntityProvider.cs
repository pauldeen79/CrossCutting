using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core;
using CrossCutting.Data.Sql.Extensions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AddDatabaseCommandEntityProvider : IDatabaseCommandEntityProvider<TestEntity>
    {
        public Func<TestEntity, IDatabaseCommand> CommandDelegate => x => new StoredProcedureCommand<TestEntity>(@"[InsertCode]", x, AddParameters);

        public Func<TestEntity, TestEntity>? ResultEntityDelegate => AddResultEntity;

        public Func<TestEntity, IDataReader, TestEntity>? AfterReadDelegate => AddAfterRead;

        private object AddParameters(TestEntity resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object>("@Code", resultEntity.Code),
                new KeyValuePair<string, object>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object>("@Description", resultEntity.Description),
            };
        }

        private TestEntity AddResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }

        private TestEntity AddAfterRead(TestEntity resultEntity, IDataReader reader)
        {
            resultEntity.Code = reader.GetString("Code", default(string));
            resultEntity.CodeType = reader.GetString("CodeType", default(string));
            resultEntity.Description = reader.GetString("Description", default(string));
            // Moved from AddFinalize!
            resultEntity.IsExistingEntity = true;
            return resultEntity;
        }
    }
}
