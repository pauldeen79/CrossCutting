namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class EvaluatableProcessorTests : TestBase
{
    protected IPagedDatabaseCommandProvider<IEvaluatableContext> PagedDatabaseCommandProvider => ClassFactories.GetOrCreate<IPagedDatabaseCommandProvider<IEvaluatableContext>>(ClassFactory);
    protected IDatabaseEntityRetrieverProvider DatabaseEntityRetrieverProvider => ClassFactories.GetOrCreate<IDatabaseEntityRetrieverProvider>(ClassFactory);
    protected IDatabaseEntityRetriever<MyEntity> DatabaseEntityRetriever => ClassFactories.GetOrCreate<IDatabaseEntityRetriever<MyEntity>>(ClassFactory);
    protected IPagedDatabaseCommand PagedDatabaseCommand => ClassFactories.GetOrCreate<IPagedDatabaseCommand>(ClassFactory);
    protected IDatabaseCommand DataCommand { get; } = new SelectCommandBuilder()
        .Select(nameof(MyEntity.MyProperty))
        .From(nameof(MyEntity))
        .WithSqlExpression(new SqlExpression(string.Empty, null))
        .Build();

    protected EvaluatableProcessor CreateSut() => ClassFactories.GetOrCreate<EvaluatableProcessor>(ClassFactory);

    public EvaluatableProcessorTests() : base()
    {
        DatabaseEntityRetrieverProvider
            .Create<MyEntity>(Arg.Any<object>())
            .Returns(Result.Success(DatabaseEntityRetriever));
        PagedDatabaseCommand
            .DataCommand
            .Returns(DataCommand);
        PagedDatabaseCommandProvider
            .CreatePagedAsync(Arg.Any<IEvaluatableContext>(), DatabaseOperation.Select, Arg.Any<int>(), Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Result.Success(PagedDatabaseCommand));
    }

    public class FindOne : EvaluatableProcessorTests
    {
        [Fact]
        public async Task Returns_Empty_Result_When_Not_Found()
        {
            // Arrange
            DatabaseEntityRetriever
                .FindOneAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>())
                .Returns(Result.NotFound<MyEntity>());
            var sut = CreateSut();
            var evaluatable = Evaluatable.OfPropertyName(nameof(MyEntity.MyProperty)).IsEqualTo("Something");

            // Act
            var result = await sut.FindOneAsync<MyEntity>(evaluatable, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
            result.Value.ShouldBeNull();
        }

        [Fact]
        public async Task Returns_Filled_Result_When_Found()
        {
            // Arrange
            var myEntity = new MyEntity();
            DatabaseEntityRetriever
                .FindOneAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success(myEntity));
            var sut = CreateSut();
            var evaluatable = Evaluatable.OfPropertyName(nameof(MyEntity.MyProperty)).IsEqualTo("Something");

            // Act
            var result = await sut.FindOneAsync<MyEntity>(evaluatable, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeSameAs(myEntity);
        }
    }

    public class FindMany : EvaluatableProcessorTests
    {
        [Fact]
        public async Task Returns_Filled_Result()
        {
            // Arrange
            var myEntity = new MyEntity();
            DatabaseEntityRetriever
                .FindManyAsync(Arg.Any<IDatabaseCommand>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<IReadOnlyCollection<MyEntity>>([ myEntity ]));
            var sut = CreateSut();
            var evaluatable = Evaluatable.OfPropertyName(nameof(MyEntity.MyProperty)).IsEqualTo("Something");

            // Act
            var result = await sut.FindManyAsync<MyEntity>(evaluatable, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(1);
            result.Value.First().ShouldBeSameAs(myEntity);
        }
    }

    public class FindPaged : EvaluatableProcessorTests
    {
        [Fact]
        public async Task Returns_Filled_Result()
        {
            // Arrange
            var myEntity = new MyEntity();
            DatabaseEntityRetriever
                .FindPagedAsync(Arg.Any<IPagedDatabaseCommand>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<IPagedResult<MyEntity>>(new PagedResult<MyEntity>([myEntity], 10, 1, 1)));
            var sut = CreateSut();
            var evaluatable = Evaluatable.OfPropertyName(nameof(MyEntity.MyProperty)).IsEqualTo("Something");

            // Act
            var result = await sut.FindPagedAsync<MyEntity>(evaluatable, 1, 1, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(1);
            result.Value.First().ShouldBeSameAs(myEntity);
        }
    }

    public sealed class MyEntity
    {
        public string MyProperty { get; set; } = "";
    }
}