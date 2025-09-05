namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public class QueryExtensionsTests : TestBase
{
    public class WithContext : QueryExtensionsTests
    {
        [Fact]
        public void Updates_Context_When_Instance_Implements_IContextualQuery()
        {
            // Arrange
            var sut = new MyContextualQuery(null, null, null, Enumerable.Empty<ICondition>(), Enumerable.Empty<ISortOrder>());

            // Act
            var result = sut.WithContext(this);

            // Assert
            result.Context.ShouldBeSameAs(this);
        }

        [Fact]
        public void Wraps_Instance_And_Sets_Context_When_Instance_Does_Not_Implement_IContextualQuery()
        {
            // Arrange
            var sut = Substitute.For<IQuery>();

            // Act
            var result = sut.WithContext(this);

            // Assert
            result.Context.ShouldBeSameAs(this);
        }

        [Fact]
        public void Can_Convert_Result_To_Builder_And_Alter_It_When_Instance_Does_Not_Implement_IContextualQuery()
        {
            // Arrange
            var sut = Substitute.For<IQuery>().WithContext(this);

            // Act
            var result = sut.ToBuilder()
                .WithLimit(1)
                .WithOffset(2)
                .WithContext("altered context")
                .AddConditions(new EqualConditionBuilder()
                    .WithSourceExpression(new ContextExpressionBuilder())
                    .WithCompareExpression(new ContextExpressionBuilder()))
                .AddSortOrders(new SortOrderBuilder().WithExpression(new ContextExpressionBuilder()))
                .Build();

            // Assert
            result.Limit.ShouldBe(1);
            result.Offset.ShouldBe(2);
            result.Context.ShouldBe("altered context");
            result.Conditions.Count.ShouldBe(1);
            result.SortOrders.Count.ShouldBe(1);
        }
    }

    private sealed class MyContextualQuery : IContextualQuery
    {
        public MyContextualQuery(object? context, int? limit, int? offset, IEnumerable<ICondition> conditions, IEnumerable<ISortOrder> sortOrders)
        {
            Context = context;
            Limit = limit;
            Offset = offset;
            Conditions = conditions.ToList();
            SortOrders = sortOrders.ToList();
        }

        public object? Context { get; }

        public int? Limit { get; }

        public int? Offset { get; }

        public IReadOnlyCollection<ICondition> Conditions { get; }

        public IReadOnlyCollection<ISortOrder> SortOrders { get; }

        public IContextualQueryBuilder ToBuilder() => new MyContextualQueryBuilder(this);

        IQueryBuilder IQuery.ToBuilder() => ToBuilder();
    }

    private sealed class MyContextualQueryBuilder : IContextualQueryBuilder
    {
        public MyContextualQueryBuilder(MyContextualQuery myContextualQuery)
        {
            Context = myContextualQuery.Context;
            Limit = myContextualQuery.Limit;
            Offset = myContextualQuery.Offset;
            Conditions = myContextualQuery.Conditions.Select(x => x.ToBuilder()).ToList();
            SortOrders = myContextualQuery.SortOrders.Select(x => x.ToBuilder()).ToList();
        }

        public object? Context { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public List<IConditionBuilder> Conditions { get; set; }
        public List<ISortOrderBuilder> SortOrders { get; set; }

        public IContextualQuery Build() => new MyContextualQuery(Context, Limit, Offset, Conditions.Select(x => x.Build()), SortOrders.Select(x => x.Build()));

        IQuery IQueryBuilder.Build() => Build();
    }
}
