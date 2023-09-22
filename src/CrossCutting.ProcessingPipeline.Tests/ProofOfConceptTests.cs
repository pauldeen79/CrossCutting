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
                .AddFeature(new MyFeatureWithContextBuilder())
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
                .AddFeatures(new MyFeatureWithContextBuilder(), new MyFeatureWithContextBuilder())
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
                .AddFeatures(new[] { new MyFeatureWithContextBuilder(), new MyFeatureWithContextBuilder() }.AsEnumerable())
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
                .AddFeature(new MyFeatureWithContextBuilder());

            // Act
            var pipeline = builder
                .ReplaceFeature<MyFeatureWithContextBuilder>(new MyReplacedFeatureWithContextBuilder())
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
                .AddFeature(new MyFeatureWithContextBuilder());

            // Act
            var pipeline = builder
                .RemoveFeature<MyFeatureWithContextBuilder>()
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
            builder.Features.Select(x => x.GetType()).Should().BeEquivalentTo(existingInstance.Features.Select(x => x.ToBuilder().GetType()));
        }

        [Fact]
        public void Can_Process_Pipeline_With_Feature()
        {
            // Arrange
            PipelineContext<object?, object?>? context = null;
            Action<PipelineContext<object?, object?>> processCallback = new(ctx => { context = ctx; });
            var sut = new PipelineBuilder<object?, object?>()
                .AddFeature(new MyFeatureWithContextBuilder { ProcessCallback = processCallback })
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
                .AddFeature(new MyContextlessFeatureBuilder())
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
                .AddFeatures(new MyContextlessFeatureBuilder(), new MyContextlessFeatureBuilder())
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
                .AddFeatures(new[] { new MyContextlessFeatureBuilder(), new MyContextlessFeatureBuilder() }.AsEnumerable())
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
                .AddFeature(new MyContextlessFeatureBuilder());

            // Act
            var pipeline = builder
                .ReplaceFeature<MyContextlessFeatureBuilder>(new MyReplacedContextlessFeatureBuilder())
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
                .AddFeature(new MyContextlessFeatureBuilder());

            // Act
            var pipeline = builder
                .RemoveFeature<MyContextlessFeatureBuilder>()
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
            builder.Features.Select(x => x.GetType()).Should().BeEquivalentTo(existingInstance.Features.Select(x => x.ToBuilder().GetType()));
        }

        [Fact]
        public void Can_Process_Pipeline_With_Feature()
        {
            // Arrange
            PipelineContext<object?>? context = null;
            Action<PipelineContext<object?>> processCallback = new(ctx => { context = ctx; });
            var sut = new PipelineBuilder<object?>()
                .AddFeature(new MyContextlessFeatureBuilder { ProcessCallback = processCallback })
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
        public Action<PipelineContext<object?>> ProcessCallback { get; }

        public MyContextlessFeature()
            => ProcessCallback = new Action<PipelineContext<object?>>(_ => { });

        public MyContextlessFeature(Action<PipelineContext<object?>> processCallback)
            => ProcessCallback = processCallback;

        public void Process(PipelineContext<object?> context)
            => ProcessCallback.Invoke(context);

        public IBuilder<IPipelineFeature<object?>> ToBuilder()
            => new MyContextlessFeatureBuilder { ProcessCallback = ProcessCallback };
    }

    private sealed class MyContextlessFeatureBuilder : IPipelineFeatureBuilder<object?>
    {
        public Action<PipelineContext<object?>> ProcessCallback { get; set; } = new(_ => { });

        public IPipelineFeature<object?> Build()
            => new MyContextlessFeature(ProcessCallback);
    }

    private sealed class MyReplacedContextlessFeature : IPipelineFeature<object?>
    {
        public void Process(PipelineContext<object?> context)
        {
            throw new NotImplementedException();
        }

        public IBuilder<IPipelineFeature<object?>> ToBuilder()
            => new MyReplacedContextlessFeatureBuilder();
    }

    private sealed class MyReplacedContextlessFeatureBuilder : IPipelineFeatureBuilder<object?>
    {
        public IPipelineFeature<object?> Build()
            => new MyReplacedContextlessFeature();
    }

    private sealed class MyFeatureWithContext : IPipelineFeature<object?, object?>
    {
        public Action<PipelineContext<object?, object?>> ProcessCallback { get; }

        public MyFeatureWithContext()
            => ProcessCallback = new Action<PipelineContext<object?, object?>>(_ => { });

        public MyFeatureWithContext(Action<PipelineContext<object?, object?>> processCallback)
            => ProcessCallback = processCallback;

        public void Process(PipelineContext<object?, object?> context)
            => ProcessCallback.Invoke(context);

        public IBuilder<IPipelineFeature<object?, object?>> ToBuilder()
            => new MyFeatureWithContextBuilder { ProcessCallback = ProcessCallback };
    }

    private sealed class MyFeatureWithContextBuilder : IPipelineFeatureBuilder<object?, object?>
    {
        public Action<PipelineContext<object?, object?>> ProcessCallback { get; set; } = new(_ => { });

        public IPipelineFeature<object?, object?> Build()
            => new MyFeatureWithContext(ProcessCallback);
    }

    private sealed class MyReplacedFeatureWithContext : IPipelineFeature<object?, object?>
    {
        public void Process(PipelineContext<object?, object?> context)
        {
            throw new NotImplementedException();
        }

        public IBuilder<IPipelineFeature<object?, object?>> ToBuilder()
            => new MyReplacedFeatureWithContextBuilder();
    }

    private sealed class MyReplacedFeatureWithContextBuilder : IPipelineFeatureBuilder<object?, object?>
    {
        public IPipelineFeature<object?, object?> Build()
            => new MyReplacedFeatureWithContext();
    }
}
