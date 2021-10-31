using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository
    {
        public TestEntity Add(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _addProcessor.InvokeCommand(instance);
        }

        public TestEntity Update(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _updateProcessor.InvokeCommand(instance);
        }

        public TestEntity Delete(TestEntity instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _deleteProcessor.InvokeCommand(instance);
        }

        public TestRepository(IDatabaseCommandProcessor<TestEntity> addProcessor,
                              IDatabaseCommandProcessor<TestEntity> updateProcessor,
                              IDatabaseCommandProcessor<TestEntity> deleteProcessor)
        {
            if (addProcessor == null)
            {
                throw new ArgumentNullException(nameof(addProcessor));
            }
            if (updateProcessor == null)
            {
                throw new ArgumentNullException(nameof(updateProcessor));
            }
            if (deleteProcessor == null)
            {
                throw new ArgumentNullException(nameof(deleteProcessor));
            }
            _addProcessor = addProcessor;
            _updateProcessor = updateProcessor;
            _deleteProcessor = deleteProcessor;
        }

        private readonly IDatabaseCommandProcessor<TestEntity> _addProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _updateProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _deleteProcessor;
    }
}
