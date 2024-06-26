﻿namespace CrossCutting.Data.Sql.Tests.Repositories;

public class TestEntityDatabaseCommandEntityProvider : IDatabaseCommandEntityProvider<TestEntity, TestEntityBuilder>
{
    public CreateCommandHandler<TestEntityBuilder> CreateCommand
        => (entity, operation) =>
        {
            return operation switch
            {
                DatabaseOperation.Insert => new StoredProcedureCommand<TestEntityBuilder>(@"[InsertEntity]", entity, DatabaseOperation.Insert, AddParameters),
                DatabaseOperation.Update => new StoredProcedureCommand<TestEntityBuilder>(@"[UpdateEntity]", entity, DatabaseOperation.Update, UpdateParameters),
                DatabaseOperation.Delete => new StoredProcedureCommand<TestEntityBuilder>(@"[DeleteEntity]", entity, DatabaseOperation.Delete, DeleteParameters),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}"),
            };
        };

    public CreateResultEntityHandler<TestEntityBuilder>? CreateResultEntity
        => (entity, operation) =>
        {
            return operation switch
            {
                DatabaseOperation.Insert => AddResultEntity(entity),
                DatabaseOperation.Update => UpdateResultEntity(entity),
                DatabaseOperation.Delete => DeleteResultEntity(entity),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}"),
            };
        };

    public AfterReadHandler<TestEntityBuilder>? AfterRead
        => (entity, operation, reader) =>
        {
            return operation switch
            {
                DatabaseOperation.Insert => AddAfterRead(entity, reader),
                DatabaseOperation.Update => UpdateAfterRead(entity, reader),
                DatabaseOperation.Delete => entity,
                _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Unsupported operation: {operation}"),
            };
        };

    public CreateBuilderHandler<TestEntity, TestEntityBuilder>? CreateBuilder => entity => new TestEntityBuilder(entity);

    public CreateEntityHandler<TestEntityBuilder, TestEntity>? CreateEntity => builder => builder.Build();

    private KeyValuePair<string, object?>[] AddParameters(TestEntityBuilder resultEntity)
        =>
        [
            new KeyValuePair<string, object?>("@Code", resultEntity.Code),
            new KeyValuePair<string, object?>("@CodeType", resultEntity.CodeType),
            new KeyValuePair<string, object?>("@Description", resultEntity.Description),
        ];

    private TestEntityBuilder AddResultEntity(TestEntityBuilder resultEntity) => resultEntity;

    private TestEntityBuilder AddAfterRead(TestEntityBuilder resultEntity, IDataReader reader)
    {
        // removed default(string) argument on required field
        resultEntity.Code = reader.GetString("Code");
        resultEntity.CodeType = reader.GetString("CodeType");
        resultEntity.Description = reader.GetString("Description");
        // Moved from AddFinalize!
        resultEntity.IsExistingEntity = true;
        return resultEntity;
    }

    private KeyValuePair<string, object?>[] UpdateParameters(TestEntityBuilder resultEntity)
        =>
        [
            new KeyValuePair<string, object?>("@Code", resultEntity.Code),
            new KeyValuePair<string, object?>("@CodeType", resultEntity.CodeType),
            new KeyValuePair<string, object?>("@Description", resultEntity.Description),
        ];

    private TestEntityBuilder UpdateResultEntity(TestEntityBuilder resultEntity) => resultEntity;

    private TestEntityBuilder UpdateAfterRead(TestEntityBuilder resultEntity, IDataReader reader)
    {
        // replace default(string) with string.Empty
        resultEntity.Code = reader.GetString("Code", string.Empty);
        resultEntity.CodeType = reader.GetString("CodeType", string.Empty);
        resultEntity.Description = reader.GetString("Description", string.Empty);

        return resultEntity;
    }

    private KeyValuePair<string, object?>[] DeleteParameters(TestEntityBuilder resultEntity)
        =>
        [
            new KeyValuePair<string, object?>("@Code", resultEntity.Code),
            new KeyValuePair<string, object?>("@CodeType", resultEntity.CodeType),
            new KeyValuePair<string, object?>("@Description", resultEntity.Description),
        ];

    private TestEntityBuilder DeleteResultEntity(TestEntityBuilder resultEntity)
        => resultEntity;
}
