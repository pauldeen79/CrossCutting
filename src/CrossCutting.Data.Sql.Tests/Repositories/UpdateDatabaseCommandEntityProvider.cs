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
    public class UpdateDatabaseCommandEntityProvider : IDatabaseCommandEntityProvider<TestEntity>
    {
        public Func<TestEntity, IDatabaseCommand> CommandDelegate => x => new StoredProcedureCommand<TestEntity>(@"[UpdateCode]", x, UpdateParameters);

        public Func<TestEntity, TestEntity>? ResultEntityDelegate => UpdateResultEntity;

        public Func<TestEntity, IDataReader, TestEntity>? AfterReadDelegate => UpdateAfterRead;

        private object UpdateParameters(TestEntity resultEntity)
        {
            return new[]
            {
                new KeyValuePair<string, object>("@Code", resultEntity.Code),
                new KeyValuePair<string, object>("@CodeType", resultEntity.CodeType),
                new KeyValuePair<string, object>("@Description", resultEntity.Description),
            };
        }

        private TestEntity UpdateResultEntity(TestEntity resultEntity)
        {

            return resultEntity;
        }

        private TestEntity UpdateAfterRead(TestEntity resultEntity, IDataReader reader)
        {
            resultEntity.Code = reader.GetString("Code", default(string));
            resultEntity.CodeType = reader.GetString("CodeType", default(string));
            resultEntity.Description = reader.GetString("Description", default(string));

            return resultEntity;
        }
    }
}
