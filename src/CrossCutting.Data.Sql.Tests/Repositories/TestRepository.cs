using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using Moq;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository
    {
        public TestRepository(IDatabaseCommandProcessor<TestEntity> addProcessor,
                              IDatabaseCommandProcessor<TestEntity> updateProcessor,
                              IDatabaseCommandProcessor<TestEntity> deleteProcessor,
                              IDatabaseEntityRetriever<TestEntity> retriever)
        {
            _addProcessor = addProcessor;
            _updateProcessor = updateProcessor;
            _deleteProcessor = deleteProcessor;
            _retriever = retriever;
        }

        private readonly IDatabaseCommandProcessor<TestEntity> _addProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _updateProcessor;
        private readonly IDatabaseCommandProcessor<TestEntity> _deleteProcessor;
        private readonly IDatabaseEntityRetriever<TestEntity> _retriever;

        public TestEntity Add(TestEntity instance)
            => _addProcessor.InvokeCommand(instance);

        public TestEntity Update(TestEntity instance)
            => _updateProcessor.InvokeCommand(instance);

        public TestEntity Delete(TestEntity instance)
            => _deleteProcessor.InvokeCommand(instance);

        // for test purposes only. normally you would add arguments here (request/query)
        public TestEntity? FindOne()
            => _retriever.FindOne(new Mock<IDatabaseCommand>().Object);

        // for test purposes only. normally you would add arguments here (request/query)
        public IReadOnlyCollection<TestEntity> FindMany()
            => _retriever.FindMany(new Mock<IDatabaseCommand>().Object);

        // for test purposes only. normally you would add arguments here (request/query)
        public IPagedResult<TestEntity> FindPaged()
            => _retriever.FindPaged(new Mock<IDatabaseCommand>().Object, new Mock<IDatabaseCommand>().Object, 0, 10);
    }
}
