using System;
using System.CodeDom.Compiler;
using System.Data;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Sql.Extensions;
using PDC.Net.Core.Entities;

namespace DataFramework.ModelFramework.Poc.DatabaseCommandEntityProviders
{
#nullable enable
    [GeneratedCode(@"DataFramework.ModelFramework.Generators.Repositories.RepositoryGenerator", @"1.0.0.0")]
    public partial class ExtraFieldDatabaseCommandEntityProvider : IDatabaseCommandEntityProvider<ExtraField, ExtraFieldBuilder>
    {
        public CreateResultEntityHandler<ExtraFieldBuilder>? CreateResultEntity
            => (entity, operation) =>
            {
                switch (operation)
                {
                    case DatabaseOperation.Insert:
                        return AddResultEntity(entity);
                    case DatabaseOperation.Update:
                        return UpdateResultEntity(entity);
                    case DatabaseOperation.Delete:
                        return DeleteResultEntity(entity);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), string.Format("Unsupported operation: {0}", operation));
                }
            };

        public AfterReadHandler<ExtraFieldBuilder>? AfterRead
            => (entity, operation, reader) =>
            {
                switch (operation)
                {
                    case DatabaseOperation.Insert:
                        return AddAfterRead(entity, reader);
                    case DatabaseOperation.Update:
                        return UpdateAfterRead(entity, reader);
                    case DatabaseOperation.Delete:
                        return entity;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation), string.Format("Unsupported operation: {0}", operation));
                }
            };

        public CreateBuilderHandler<ExtraField, ExtraFieldBuilder>? CreateBuilder => entity => new ExtraFieldBuilder(entity);

        public CreateEntityHandler<ExtraFieldBuilder, ExtraField>? CreateEntity => builder => builder.Build();

        private ExtraFieldBuilder AddResultEntity(ExtraFieldBuilder resultEntity)
        {
            // Moved from Finalize
            resultEntity = resultEntity.WithIsExistingEntity(true);

            return resultEntity;
        }

        private ExtraFieldBuilder AddAfterRead(ExtraFieldBuilder resultEntity, IDataReader reader)
        {
            resultEntity = resultEntity.WithEntityName(reader.GetString("EntityName"));
            resultEntity = resultEntity.WithName(reader.GetString("Name"));
            resultEntity = resultEntity.WithDescription(reader.GetNullableString("Description"));
            resultEntity = resultEntity.WithFieldNumber(reader.GetByte("FieldNumber"));
            resultEntity = resultEntity.WithFieldType(reader.GetString("FieldType"));
            resultEntity = resultEntity.WithEntityNameOriginal(reader.GetString("EntityName"));
            resultEntity = resultEntity.WithNameOriginal(reader.GetString("Name"));
            resultEntity = resultEntity.WithDescriptionOriginal(reader.GetString("Description"));
            resultEntity = resultEntity.WithFieldNumberOriginal(reader.GetByte("FieldNumber"));
            resultEntity = resultEntity.WithFieldTypeOriginal(reader.GetString("FieldType"));

            return resultEntity;
        }

        private ExtraFieldBuilder UpdateResultEntity(ExtraFieldBuilder resultEntity)
        {

            return resultEntity;
        }

        private ExtraFieldBuilder UpdateAfterRead(ExtraFieldBuilder resultEntity, IDataReader reader)
        {
            resultEntity = resultEntity.WithEntityName(reader.GetString("EntityName"));
            resultEntity = resultEntity.WithName(reader.GetString("Name"));
            resultEntity = resultEntity.WithDescription(reader.GetNullableString("Description"));
            resultEntity = resultEntity.WithFieldNumber(reader.GetByte("FieldNumber"));
            resultEntity = resultEntity.WithFieldType(reader.GetString("FieldType"));
            resultEntity = resultEntity.WithEntityNameOriginal(reader.GetString("EntityName"));
            resultEntity = resultEntity.WithNameOriginal(reader.GetString("Name"));
            resultEntity = resultEntity.WithDescriptionOriginal(reader.GetNullableString("Description"));
            resultEntity = resultEntity.WithFieldNumberOriginal(reader.GetByte("FieldNumber"));
            resultEntity = resultEntity.WithFieldTypeOriginal(reader.GetString("FieldType"));

            return resultEntity;
        }

        private ExtraFieldBuilder DeleteResultEntity(ExtraFieldBuilder resultEntity)
        {

            return resultEntity;
        }
    }
#nullable restore
}
