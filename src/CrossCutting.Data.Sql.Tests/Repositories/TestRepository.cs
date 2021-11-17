using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Abstractions.Extensions;
using Moq;

namespace CrossCutting.Data.Sql.Tests.Repositories
{
    [ExcludeFromCodeCoverage]
    public class TestRepository
    {
        public TestRepository(IDatabaseCommandProcessor<TestEntity> commandProcessor,
                              IDatabaseEntityRetriever<TestEntity> retriever)
        {
            _commandProcessor = commandProcessor;
            _retriever = retriever;
        }

        private readonly IDatabaseCommandProcessor<TestEntity> _commandProcessor;
        private readonly IDatabaseEntityRetriever<TestEntity> _retriever;

        public TestEntity Add(TestEntity instance)
            => _commandProcessor.InvokeCommand(instance).HandleResult("TestEntity was not added");

        public TestEntity Update(TestEntity instance)
            => _commandProcessor.InvokeCommand(instance).HandleResult("TestEntity was not updated");

        public TestEntity Delete(TestEntity instance)
            => _commandProcessor.InvokeCommand(instance).HandleResult("TestEntity was not deleted");

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
