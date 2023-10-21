using CrossCutting.Common.Results;

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
        public void Can_Validate_PipelineBuilder_With_Null_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?> { Features = null! };

            // Act
            var validationResults = builder.Validate(new(builder));

            // Assert
            validationResults.Should().ContainSingle();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_Empty_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?> { Features = new List<IBuilder<IPipelineFeature<object?, object?>>>() };

            // Act
            var validationResults = builder.Validate(new(builder));

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_NonEmpty_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?, object?> { Features = new List<IBuilder<IPipelineFeature<object?, object?>>>(new[] { new MyFeatureWithContextBuilder() }) };

            // Act
            var validationResults = builder.Validate(new(builder));

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Can_Construct_Builder_From_Existing_Pipeline_Instance()
        {
            // Arrange
            var existingInstance = new Pipeline<object?, object?>((_, _) => { }, new[] { new MyFeatureWithContext() });

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
            Func<PipelineContext<object?, object?>, Result<object?>> processCallback = new(ctx => { context = ctx; return Result.Continue<object?>(); });
            var sut = new PipelineBuilder<object?, object?>()
                .AddFeature(new MyFeatureWithContextBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = sut.Process(model: 1, context: 2);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            context.Should().NotBeNull();
            context!.Model.Should().BeEquivalentTo(1);
            context.Context.Should().BeEquivalentTo(2);
        }

        [Fact]
        public void Can_Abort_Pipeline_With_Feature_Using_Non_Success_Status()
        {
            // Arrange
            Func<PipelineContext<object?, object?>, Result<object?>> processCallback = new(_ => Result.Error<object?>("Kaboom"));
            var sut = new PipelineBuilder<object?, object?>()
                .AddFeature(new MyFeatureWithContextBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = sut.Process(model: 1, context: 2);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_ValidationDelegate_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?>(validationDelegate: null!, features: Enumerable.Empty<IPipelineFeature<object?>>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("validationDelegate");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Features_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?>((_, _) => { }, features: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("features");
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
        public void Can_Validate_PipelineBuilder_With_Null_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?> { Features = null! };

            // Act
            var validationResults = builder.Validate(new(builder));

            // Assert
            validationResults.Should().ContainSingle();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_Empty_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?> { Features = new List<IBuilder<IPipelineFeature<object?>>>() };

            // Act
            var validationResults = builder.Validate(new(builder));

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Can_Validate_PipelineBuilder_With_NonEmpty_Features_List()
        {
            // Arrange
            var builder = new PipelineBuilder<object?> { Features = new List<IBuilder<IPipelineFeature<object?>>>(new[] { new MyContextlessFeatureBuilder() }) };

            // Act
            var validationResults = builder.Validate(new(builder));

            // Assert
            validationResults.Should().BeEmpty();
        }

        [Fact]
        public void Can_Construct_Builder_From_Existing_Pipeline_Instance()
        {
            // Arrange
            var existingInstance = new Pipeline<object?>((_, _) => { }, new[] { new MyContextlessFeature() });

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
            Func<PipelineContext<object?>, Result<object?>> processCallback = new(ctx => { context = ctx; return Result.Continue<object?>(); });
            var sut = new PipelineBuilder<object?>()
                .AddFeature(new MyContextlessFeatureBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = sut.Process(model: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            context.Should().NotBeNull();
            context!.Model.Should().BeEquivalentTo(1);
        }

        [Fact]
        public void Can_Abort_Pipeline_With_Feature_Using_Non_Success_Status()
        {
            // Arrange
            Func<PipelineContext<object?>, Result<object?>> processCallback = new(_ => Result.Error<object?>("Kaboom"));
            var sut = new PipelineBuilder<object?>()
                .AddFeature(new MyContextlessFeatureBuilder().WithProcessCallback(processCallback))
                .Build();

            // Act
            var result = sut.Process(model: 1);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_ValidationDelegate_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?, object?>(validationDelegate: null!, features: Enumerable.Empty<IPipelineFeature<object?, object?>>()))
                .Should().Throw<ArgumentNullException>().WithParameterName("validationDelegate");
        }

        [Fact]
        public void Constructing_Pipeline_Using_Null_Features_Throws_ArgumentNullException()
        {
            // Act & Assert
            this.Invoking(_ => new Pipeline<object?, object?>((_, _) => { }, features: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("features");
        }
    }

    public class Pipeline_With_Dependencies
    {
        [Fact]
        public void Can_Use_Dependency_Injection_To_Allow_Features_To_Have_Dependencies()
        {
            // Arrange
            using var provider = new ServiceCollection()
                .AddScoped<IMyService, MyService>()
                .AddScoped<IPipelineFeatureBuilderWithDependencies, PipelineFeatureBuilderWithDependencies>()
                .BuildServiceProvider();
            using var scope = provider.CreateScope();

            // Act
            var sut = scope.ServiceProvider.GetRequiredService<IPipelineFeatureBuilderWithDependencies>();

            // Assert
            sut.MyService.Should().NotBeNull();
        }
    }

    private sealed class MyContextlessFeature : IPipelineFeature<object?>
    {
        public Func<PipelineContext<object?>, Result> ProcessCallback { get; }

        public MyContextlessFeature()
            => ProcessCallback = new Func<PipelineContext<object?>, Result>(_ => Result.NoContent());

        public MyContextlessFeature(Func<PipelineContext<object?>, Result> processCallback)
            => ProcessCallback = processCallback;

        public Result Process(PipelineContext<object?> context)
            => ProcessCallback.Invoke(context);

        public IBuilder<IPipelineFeature<object?>> ToBuilder()
            => new MyContextlessFeatureBuilder { ProcessCallback = ProcessCallback };
    }

    private sealed class MyContextlessFeatureBuilder : IPipelineFeatureBuilder<object?>
    {
        public Func<PipelineContext<object?>, Result> ProcessCallback { get; set; } = new(_ => Result.Success());

        public MyContextlessFeatureBuilder WithProcessCallback(Func<PipelineContext<object?>, Result> processCallback)
        {
            ProcessCallback = processCallback;
            return this;
        }

        public IPipelineFeature<object?> Build()
            => new MyContextlessFeature(ProcessCallback);
    }

    private sealed class MyReplacedContextlessFeature : IPipelineFeature<object?>
    {
        public Result Process(PipelineContext<object?> context)
        {
            return Result.NotImplemented();
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
        public Func<PipelineContext<object?, object?>, Result<object?>> ProcessCallback { get; }

        public MyFeatureWithContext()
            => ProcessCallback = new Func<PipelineContext<object?, object?>, Result<object?>>(_ => Result.NoContent<object?>());

        public MyFeatureWithContext(Func<PipelineContext<object?, object?>, Result<object?>> processCallback)
            => ProcessCallback = processCallback;

        public Result<object?> Process(PipelineContext<object?, object?> context)
            => ProcessCallback.Invoke(context);

        public IBuilder<IPipelineFeature<object?, object?>> ToBuilder()
            => new MyFeatureWithContextBuilder { ProcessCallback = ProcessCallback };
    }

    private sealed class MyFeatureWithContextBuilder : IPipelineFeatureBuilder<object?, object?>
    {
        public Func<PipelineContext<object?, object?>, Result<object?>> ProcessCallback { get; set; } = new(_ => Result.NoContent<object?>());

        public MyFeatureWithContextBuilder WithProcessCallback(Func<PipelineContext<object?, object?>, Result<object?>> processCallback)
        {
            ProcessCallback = processCallback;
            return this;
        }

        public IPipelineFeature<object?, object?> Build()
            => new MyFeatureWithContext(ProcessCallback);
    }

    private sealed class MyReplacedFeatureWithContext : IPipelineFeature<object?, object?>
    {
        public Result<object?> Process(PipelineContext<object?, object?> context)
        {
            return Result.NotImplemented<object?>();
        }

        public IBuilder<IPipelineFeature<object?, object?>> ToBuilder()
            => new MyReplacedFeatureWithContextBuilder();
    }

    private sealed class MyReplacedFeatureWithContextBuilder : IPipelineFeatureBuilder<object?, object?>
    {
        public IPipelineFeature<object?, object?> Build()
            => new MyReplacedFeatureWithContext();
    }

    private interface IPipelineFeatureBuilderWithDependencies : IPipelineFeatureBuilder<object?>
    {
        IMyService MyService { get; }
    }

    public sealed class PipelineFeatureBuilderWithDependencies : IPipelineFeatureBuilderWithDependencies
    {
        private readonly IMyService _myService;

        public PipelineFeatureBuilderWithDependencies(IMyService myService)
        {
            _myService = myService;
        }

        public IPipelineFeature<object?> Build()
        {
            throw new NotImplementedException();
        }

        public IMyService MyService => _myService;
    }

    public interface IMyService
    {
        void DoSomething();
    }

    private sealed class MyService : IMyService
    {
        public void DoSomething() => throw new NotImplementedException();
    }
}
