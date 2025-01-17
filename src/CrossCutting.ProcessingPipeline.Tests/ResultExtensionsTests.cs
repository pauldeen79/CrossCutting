namespace CrossCutting.ProcessingPipeline.Tests;

public class ResultExtensionsTests : TestBase
{
    public class ProcessResult : ResultExtensionsTests
    {
        [Fact]
        public async Task Returns_Invalid_When_Pipeline_Returns_Builder_With_ValidationErrors()
        {
            // Arrange
            var pipeline = Fixture.Freeze<IPipeline<ClassContext>>();
            // note that by doing nothing on the received builder in the builder context, the name will be empty, and this is a required field.
            // thus, we are creating an invalid result 8-)
            pipeline.ProcessAsync(Arg.Any<ClassContext>(), Arg.Any<CancellationToken>()).Returns(_ => Result.Success());
            var sourceModel = new Model(null!);
            var context = new ClassContext(sourceModel);

            // Act
            var result = (await pipeline.ProcessAsync(context)).ProcessResult(context.Builder, context.Builder.Build);

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_InnerResult_When_Pipeline_Returns_NonSuccesful_Result()
        {
            // Arrange
            var pipeline = Fixture.Freeze<IPipeline<ClassContext>>();
            pipeline.ProcessAsync(Arg.Any<ClassContext>(), Arg.Any<CancellationToken>()).Returns(x => Result.Error("Kaboom!"));
            var sourceModel = new Model("Ok");
            var context = new ClassContext(sourceModel);

            // Act
            var result = (await pipeline.ProcessAsync(context)).ProcessResult(context.Builder, context.Builder.Build);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom!");
            result.Value.Should().BeNull();
        }

        [Fact]
        public async Task Returns_InnerResult_With_Value_When_Pipeline_Returns_Succesful_Result()
        {
            // Arrange
            var pipeline = Fixture.Freeze<IPipeline<ClassContext>>();
            pipeline.ProcessAsync(Arg.Any<ClassContext>(), Arg.Any<CancellationToken>()).Returns(_ => Result.Success());
            var sourceModel = new Model("Ok");
            var context = new ClassContext(sourceModel);

            // Act
            var result = (await pipeline.ProcessAsync(context)).ProcessResult(context.Builder, context.Builder.Build);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().NotBeNull();
            result.Value.Should().BeSameAs(sourceModel);
        }
    }

    public sealed class ClassContext
    {
        public ClassContext(Model sourceModel)
        {
            SourceModel = sourceModel;
            Builder = new MyBuilder(SourceModel);
        }

        public Model SourceModel { get; }

        public MyBuilder Builder { get; }
    }

    public sealed class MyBuilder(Model sourceModel)
    {
        [ValidateObject]
        public Model SourceModel { get; } = sourceModel;

        public Model Build()
        {
            return SourceModel;
        }
    }

    public class Model(string value)
    {
        [Required]
        public string Value { get; } = value;
    }
}
