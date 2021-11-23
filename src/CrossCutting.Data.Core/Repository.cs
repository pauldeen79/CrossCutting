using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Abstractions.Extensions;

namespace CrossCutting.Data.Core
{
    public class Repository<TEntity, TIdentity> : IRepository<TEntity, TIdentity>
        where TEntity : class
    {
        public TEntity Add(TEntity instance)
            => CommandProcessor.ExecuteCommand(EntityCommandProvider.Create(instance, DatabaseOperation.Insert), instance).HandleResult($"{typeof(TEntity).Name} has not been added");

        public TEntity Update(TEntity instance)
            => CommandProcessor.ExecuteCommand(EntityCommandProvider.Create(instance, DatabaseOperation.Update), instance).HandleResult($"{typeof(TEntity).Name} has not been updated");

        public TEntity Delete(TEntity instance)
            => CommandProcessor.ExecuteCommand(EntityCommandProvider.Create(instance, DatabaseOperation.Delete), instance).HandleResult($"{typeof(TEntity).Name} has not been deleted");

        public TEntity? Find(TIdentity identity)
            => EntityRetriever.FindOne(IdentityCommandProvider.Create(identity, DatabaseOperation.Select));

        public IReadOnlyCollection<TEntity> FindAll()
            => EntityRetriever.FindMany(IdentityCommandProvider.Create(DatabaseOperation.Select));

        public IPagedResult<TEntity> FindAllPaged(int offset, int pageSize)
            => EntityRetriever.FindPaged(IdentityCommandProvider.CreatePaged(DatabaseOperation.Select, offset, pageSize));

        public Repository(IDatabaseCommandProcessor<TEntity> databaseCommandProcessor,
                          IDatabaseEntityRetriever<TEntity> entityRetriever,
                          IPagedDatabaseCommandProvider<TIdentity> identityDatabaseCommandProvider,
                          IDatabaseCommandProvider<TEntity> entityDatabaseCommandProvider)
        {
            CommandProcessor = databaseCommandProcessor;
            EntityRetriever = entityRetriever;
            IdentityCommandProvider = identityDatabaseCommandProvider;
            EntityCommandProvider = entityDatabaseCommandProvider;
        }

        protected IDatabaseCommandProcessor<TEntity> CommandProcessor;
        protected IDatabaseEntityRetriever<TEntity> EntityRetriever;
        protected IPagedDatabaseCommandProvider<TIdentity> IdentityCommandProvider;
        protected IDatabaseCommandProvider<TEntity> EntityCommandProvider;
    }
}
