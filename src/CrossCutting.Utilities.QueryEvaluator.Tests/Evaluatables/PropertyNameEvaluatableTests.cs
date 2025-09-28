namespace CrossCutting.Utilities.QueryEvaluator.Tests.Evaluatables;

public class PropertyNameEvaluatableTests : TestBase<PropertyNameEvaluatable>
{
    public class EvaluateAsync : PropertyNameEvaluatableTests
    {
        [Fact]
        public async Task Returns_Error_When_Evaluation_Returns_Null_Result()
        {
            // Arrange
            var sut = new PropertyNameEvaluatableBuilder()
                .WithPropertyName("MyProperty")
                .WithSourceExpression(new ContextEvaluatableBuilder())
                .Build();
            var context = CreateContext();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Expression evaluation result value is null");
        }

        [Fact]
        public async Task Returns_Error_When_Evaluation_Returns_Null_Value()
        {
            // Arrange
            var sut = new PropertyNameEvaluatableBuilder()
                .WithPropertyName("MyProperty")
                .WithSourceExpression(new EmptyEvaluatableBuilder())
                .Build();
            var context = CreateContext(/*state: new AsyncResultDictionaryBuilder<object?>()
                .Add(Constants.Context, () => null!)
                .BuildDeferred()*/);

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Expression evaluation result value is null");
        }

        [Fact]
        public async Task Returns_Error_When_PropertyName_Is_Not_Found()
        {
            // Arrange
            var sut = new PropertyNameEvaluatableBuilder()
                .WithPropertyName("MyProperty")
                .WithSourceExpression(new ContextEvaluatableBuilder())
                .Build();
            var context = CreateContext(state: new AsyncResultDictionaryBuilder<object?>()
                .Add(Constants.Context, () => this)
                .BuildDeferred());

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Type CrossCutting.Utilities.QueryEvaluator.Tests.Evaluatables.PropertyNameEvaluatableTests+EvaluateAsync does not contain property MyProperty");
        }

        [Fact]
        public async Task Returns_Success_When_PropertyName_Is_Found()
        {
            // Arrange
            var sut = new PropertyNameEvaluatableBuilder()
                .WithPropertyName(nameof(MyClass.MyProperty))
                .WithSourceExpression(new ContextEvaluatableBuilder())
                .Build();
            var context = CreateContext(state: new AsyncResultDictionaryBuilder<object?>()
                .Add(Constants.Context, () => new MyClass())
                .BuildDeferred());

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(new MyClass().MyProperty);
        }

        private sealed class MyClass
        {
            public string MyProperty => "Hello world!";
        }
    }
}
