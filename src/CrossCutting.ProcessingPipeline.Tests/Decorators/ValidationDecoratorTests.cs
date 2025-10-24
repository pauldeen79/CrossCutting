namespace CrossCutting.ProcessingPipeline.Tests.Decorators;

public class ValidationDecoratorTests : TestBase
{
    public class ProcessAsync : ValidationDecoratorTests
    {
        [Fact]
        public async Task Returns_Same_Result_On_Execution_With_Valid_Context()
        {
            // Arrange
            var pipelineComponent = Fixture.Create<IPipelineComponent<MyValidatableContext>>();
            pipelineComponent.ProcessAsync(Arg.Any<PipelineContext<MyValidatableContext>>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var sut = new ValidationDecorator<MyValidatableContext>(pipelineComponent);
            var context = new PipelineContext<MyValidatableContext>(new MyValidatableContext("hello world"));

            // Act
            var result = await sut.ProcessAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Invalid_On_Execution_With_Invalid_Context()
        {
            // Arrange
            var pipelineComponent = Fixture.Create<IPipelineComponent<MyValidatableContext>>();
            pipelineComponent.ProcessAsync(Arg.Any<PipelineContext<MyValidatableContext>>(), Arg.Any<CancellationToken>()).Returns(Result.Success());
            var sut = new ValidationDecorator<MyValidatableContext>(pipelineComponent);
            var context = new PipelineContext<MyValidatableContext>(new MyValidatableContext(string.Empty));

            // Act
            var result = await sut.ProcessAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ValidationErrors.Count.ShouldBe(1);
            result.ValidationErrors.First().ErrorMessage.ShouldBe("The content is invalid");
        }

        public sealed class MyValidatableContext : IValidatableObject
        {
            private readonly string _contents;

            public MyValidatableContext(string contents)
                => _contents = contents;

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (string.IsNullOrEmpty(_contents))
                {
                    yield return new ValidationResult("The content is invalid");
                }
            }
        }
    }
}
