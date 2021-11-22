using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Abstractions.Extensions;

namespace CrossCutting.Data.Core
{
    public class Repository<TEntity, TIdentity> : IRepository<TEntity, TIdentity>
        where TEntity : class
    {
        public TEntity Add(TEntity instance)
            => CommandProcessor.InvokeCommand(instance, DatabaseOperation.Insert).HandleResult($"{typeof(TEntity).Name} has not been added");

        public TEntity Update(TEntity instance)
            => CommandProcessor.InvokeCommand(instance, DatabaseOperation.Update).HandleResult($"{typeof(TEntity).Name} has not been updated");

        public TEntity Delete(TEntity instance)
            => CommandProcessor.InvokeCommand(instance, DatabaseOperation.Delete).HandleResult($"{typeof(TEntity).Name} has not been deleted");

        public TEntity? Find(TIdentity identity)
            => EntityRetriever.FindOne(CommandProvider.Create(identity, DatabaseOperation.Select));

        public IReadOnlyCollection<TEntity> FindAll()
            => EntityRetriever.FindMany(CommandProvider.Create(DatabaseOperation.Select));

        public Repository(IDatabaseCommandProcessor<TEntity> databaseCommandProcessor,
                          IDatabaseEntityRetriever<TEntity> entityRetriever,
                          IDatabaseCommandProvider<TIdentity> databaseCommandProvider)
        {
            CommandProcessor = databaseCommandProcessor;
            EntityRetriever = entityRetriever;
            CommandProvider = databaseCommandProvider;
        }

        protected IDatabaseCommandProcessor<TEntity> CommandProcessor;
        protected IDatabaseEntityRetriever<TEntity> EntityRetriever;
        protected IDatabaseCommandProvider<TIdentity> CommandProvider;
    }
}
