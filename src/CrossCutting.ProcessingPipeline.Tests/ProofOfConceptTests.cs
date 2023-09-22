namespace CrossCutting.ProcessingPipeline.Tests;

public class ProofOfConceptTests
{
    public class Pipeline_With_Context
    {
        [Fact]
        public void Can_Create_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>();

            // Act
            var pipeline = builder
                .AddFeature(new MyFeatureWithContext())
                .Build();

            // Assert
            pipeline.Features.Should().ContainSingle();
            pipeline.Features.Single().Should().BeOfType<MyFeatureWithContext>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Array()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>();

            // Act
            var pipeline = builder
                .AddFeatures(new MyFeatureWithContext(), new MyFeatureWithContext())
                .Build();

            // Assert
            pipeline.Features.Should().HaveCount(2);
            pipeline.Features.Should().AllBeOfType<MyFeatureWithContext>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Enumerable()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>();

            // Act
            var pipeline = builder
                .AddFeatures(new[] { new MyFeatureWithContext(), new MyFeatureWithContext() }.AsEnumerable())
                .Build();

            // Assert
            pipeline.Features.Should().HaveCount(2);
            pipeline.Features.Should().AllBeOfType<MyFeatureWithContext>();
        }

        [Fact]
        public void Can_Replace_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>()
                .AddFeature(new MyFeatureWithContext());

            // Act
            var pipeline = builder
                .ReplaceFeature<MyFeatureWithContext>(new MyReplacedFeatureWithContext())
                .Build();

            // Assert
            pipeline.Features.Should().ContainSingle();
            pipeline.Features.Single().Should().BeOfType<MyReplacedFeatureWithContext>();
        }

        [Fact]
        public void Can_Remove_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?>()
                .AddFeature(new MyFeatureWithContext());

            // Act
            var pipeline = builder
                .RemoveFeature<MyFeatureWithContext>()
                .Build();

            // Assert
            pipeline.Features.Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?> { Features = null! };

            // Act
            var validationResults = builder.Validate(new(builder));

            // Assert
            validationResults.Should().ContainSingle();
        }

        [Fact]
        public void Can_Construct_Builder_From_Existing_Pipeline_Instance()
        {
            // Arrange
            var existingInstance = new Pipeline<object?, object?>(new[] { new MyFeatureWithContext() });

            // Act
            var builder = new PipelineBuilder<object?, object?>(existingInstance);

            // Assert
            builder.Features.Should().BeEquivalentTo(existingInstance.Features);
        }

        [Fact]
        public void Can_Process_Pipeline_With_Feature()
        {
            // Arrange
            PipelineContext<object?, object?>? context = null;
            Action<PipelineContext<object?, object?>> processCallback = new(ctx => { context = ctx; });
            var sut = new PipelineBuilder<object?, object?>()
                .AddFeature(new MyFeatureWithContext(processCallback))
                .Build();

            // Act
            sut.Process(model: 1, context: 2);

            // Assert
            context.Should().NotBeNull();
            context!.Model.Should().BeEquivalentTo(1);
            context.Context.Should().BeEquivalentTo(2);
        }
    }

    public class Pipeline_Without_Context
    {
        [Fact]
        public void Can_Create_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddFeature(new MyContextlessFeature())
                .Build();

            // Assert
            pipeline.Features.Should().ContainSingle();
            pipeline.Features.Single().Should().BeOfType<MyContextlessFeature>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Array()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddFeatures(new MyContextlessFeature(), new MyContextlessFeature())
                .Build();

            // Assert
            pipeline.Features.Should().HaveCount(2);
            pipeline.Features.Should().AllBeOfType<MyContextlessFeature>();
        }

        [Fact]
        public void Can_Add_Multiple_Features_Using_Enumerable()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>();

            // Act
            var pipeline = builder
                .AddFeatures(new[] { new MyContextlessFeature(), new MyContextlessFeature() }.AsEnumerable())
                .Build();

            // Assert
            pipeline.Features.Should().HaveCount(2);
            pipeline.Features.Should().AllBeOfType<MyContextlessFeature>();
        }

        [Fact]
        public void Can_Replace_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>()
                .AddFeature(new MyContextlessFeature());

            // Act
            var pipeline = builder
                .ReplaceFeature<MyContextlessFeature>(new MyReplacedContextlessFeature())
                .Build();

            // Assert
            pipeline.Features.Should().ContainSingle();
            pipeline.Features.Single().Should().BeOfType<MyReplacedContextlessFeature>();
        }

        [Fact]
        public void Can_Remove_Feature_On_Pipeline()
        {
            // Arrange
            var builder = new PipelineBuilder<object?>()
                .AddFeature(new MyContextlessFeature());

            // Act
            var pipeline = builder
                .RemoveFeature<MyContextlessFeature>()
                .Build();

            // Assert
            pipeline.Features.Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder()
        {
            // Arrange
            var builder = new PipelineBuilder<object?> { Features = null! };

            // Act
            var validationResults = builder.Validate(new(builder));

            // Assert
            validationResults.Should().ContainSingle();
        }

        [Fact]
        public void Can_Construct_Builder_From_Existing_Pipeline_Instance()
        {
            // Arrange
            var existingInstance = new Pipeline<object?>(new[] { new MyContextlessFeature() });

            // Act
            var builder = new PipelineBuilder<object?>(existingInstance);

            // Assert
            builder.Features.Should().BeEquivalentTo(existingInstance.Features);
        }

        [Fact]
        public void Can_Process_Pipeline_With_Feature()
        {
            // Arrange
            PipelineContext<object?>? context = null;
            Action<PipelineContext<object?>> processCallback = new(ctx => { context = ctx; });
            var sut = new PipelineBuilder<object?>()
                .AddFeature(new MyContextlessFeature(processCallback))
                .Build();

            // Act
            sut.Process(model: 1);

            // Assert
            context.Should().NotBeNull();
            context!.Model.Should().BeEquivalentTo(1);
        }
    }

    private sealed class MyContextlessFeature : IPipelineFeature<object?>
    {
        private readonly Action<PipelineContext<object?>> _processCallback;

        public MyContextlessFeature()
            => _processCallback = new Action<PipelineContext<object?>>(_ => { });

        public MyContextlessFeature(Action<PipelineContext<object?>> processCallback)
            => _processCallback = processCallback;

        public void Process(PipelineContext<object?> context)
            => _processCallback.Invoke(context);
    }

    private sealed class MyReplacedContextlessFeature : IPipelineFeature<object?>
    {
        public void Process(PipelineContext<object?> context)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class MyFeatureWithContext : IPipelineFeature<object?, object?>
    {
        private readonly Action<PipelineContext<object?, object?>> _processCallback;

        public MyFeatureWithContext()
            => _processCallback = new Action<PipelineContext<object?, object?>>(_ => { });

        public MyFeatureWithContext(Action<PipelineContext<object?, object?>> processCallback)
            => _processCallback = processCallback;

        public void Process(PipelineContext<object?, object?> context)
            => _processCallback.Invoke(context);
    }

    private sealed class MyReplacedFeatureWithContext : IPipelineFeature<object?, object?>
    {
        public void Process(PipelineContext<object?, object?> context)
        {
            throw new NotImplementedException();
        }
    }
}
